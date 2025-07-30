using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Stripe;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Application.Services;
using System.Text.Json;

namespace SubscriptionAnalytics.Worker.Services;

public class StandaloneSyncWorker
{
    private readonly ILogger<StandaloneSyncWorker> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public StandaloneSyncWorker(ILogger<StandaloneSyncWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection connection string not found");
    }

    /// <summary>
    /// Gets pending sync jobs from the database
    /// </summary>
    public async Task<List<SyncJob>> GetPendingJobsAsync()
    {
        using var context = CreateDbContext();

        var pendingJobs = await context.SyncJobs
            .Where(job => job.Status == SyncJobStatus.Pending && !job.IsDeleted)
            .OrderBy(job => job.CreatedAt)
            .Take(10) // Process up to 10 jobs at a time
            .ToListAsync();

        _logger.LogDebug("Found {Count} pending jobs", pendingJobs.Count);
        return pendingJobs;
    }

    /// <summary>
    /// Processes a sync job from the database
    /// </summary>
    public async Task<SyncJobResult> ProcessSyncJobAsync(SyncJob syncJob)
    {
        _logger.LogInformation("Processing sync job {JobId} for tenant {TenantId}, provider {ProviderName}",
            syncJob.Id, syncJob.TenantId, syncJob.ProviderName);

        var result = new SyncJobResult
        {
            JobId = syncJob.Id,
            TenantId = syncJob.TenantId,
            ProviderName = syncJob.ProviderName ?? string.Empty,
            Status = SyncJobStatus.Running,
            StartedAt = DateTime.UtcNow
        };

        try
        {
            // Update job status to Running
            await UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Running, 0);

            // Get provider connection from database
            var providerConnection = await GetProviderConnectionAsync(syncJob.TenantId, syncJob.ProviderName ?? string.Empty);
            if (providerConnection == null)
            {
                throw new InvalidOperationException($"No connection found for provider {syncJob.ProviderName}");
            }

            // Initialize Stripe if it's a Stripe sync
            if (syncJob.ProviderName?.Equals("stripe", StringComparison.OrdinalIgnoreCase) == true)
            {
                var apiKey = _configuration["Stripe:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new InvalidOperationException("Stripe API key not configured");
                }
                StripeConfiguration.ApiKey = apiKey;
            }

            // Process based on provider
            switch (syncJob.ProviderName?.ToLower())
            {
                case "stripe":
                    result = await ProcessStripeSyncAsync(syncJob, result, providerConnection);
                    break;
                case "paypal":
                    result = await ProcessPayPalSyncAsync(syncJob, result, providerConnection);
                    break;
                default:
                    throw new NotSupportedException($"Provider {syncJob.ProviderName} is not supported");
            }

            result.Status = SyncJobStatus.Completed;
            result.CompletedAt = DateTime.UtcNow;

            // Update job status to Completed
            await UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Completed, 100);

            _logger.LogInformation("Sync job {JobId} completed successfully", syncJob.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing sync job {JobId}", syncJob.Id);
            result.Status = SyncJobStatus.Failed;
            result.ErrorMessage = ex.Message;
            result.CompletedAt = DateTime.UtcNow;

            // Update job status to Failed
            await UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Failed, errorMessage: ex.Message);
        }

        return result;
    }

    private async Task<ProviderConnection?> GetProviderConnectionAsync(Guid tenantId, string providerName)
    {
        using var context = CreateDbContext();

        var connection = await context.ProviderConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId &&
                                     c.ProviderName.ToLower() == providerName.ToLower() &&
                                     c.Status == "Connected");

        return connection;
    }

    private async Task<SyncJobResult> ProcessStripeSyncAsync(SyncJob syncJob, SyncJobResult result, ProviderConnection connection)
    {
        _logger.LogInformation("Processing Stripe sync for tenant {TenantId}", syncJob.TenantId);

        // For connected accounts, we need to use the account ID as the StripeAccount parameter
        // The access token is used for authentication, but the account ID is used for the API calls
        var connectedAccountId = connection.ProviderAccountId;
        if (string.IsNullOrEmpty(connectedAccountId))
        {
            throw new InvalidOperationException("No connected account ID found for Stripe connection");
        }

        var totalSynced = 0;
        var customerService = new CustomerService();

        try
        {
            // Update progress to 25%
            result.Progress = 25;
            await UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Running, 25);
            _logger.LogInformation("Starting customer sync for tenant {TenantId} with account {AccountId}",
                syncJob.TenantId, connectedAccountId);

            var options = new CustomerListOptions
            {
                Limit = 100
            };

            var hasMore = true;
            var processedCount = 0;

            while (hasMore)
            {
                var stripeCustomers = await customerService.ListAsync(options, new RequestOptions
                {
                    StripeAccount = connectedAccountId // Use the connected account ID
                });

                foreach (var stripeCustomer in stripeCustomers.Data)
                {
                    // Process each customer
                    await ProcessStripeCustomerAsync(syncJob.TenantId, stripeCustomer);
                    processedCount++;
                }

                // Update progress
                var progress = Math.Min(25 + (processedCount * 50 / 100), 75);
                result.Progress = progress;
                await UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Running, progress);

                hasMore = stripeCustomers.HasMore;
                if (hasMore)
                {
                    options.StartingAfter = stripeCustomers.Data.Last().Id;
                }
            }

            totalSynced = processedCount;
            result.Progress = 100;
            _logger.LogInformation("Completed Stripe sync for tenant {TenantId}. Total synced: {TotalSynced}",
                syncJob.TenantId, totalSynced);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe API error during sync for tenant {TenantId}", syncJob.TenantId);
            throw new InvalidOperationException($"Stripe API error: {ex.Message}", ex);
        }

        return result;
    }

    private async Task ProcessStripeCustomerAsync(Guid tenantId, Stripe.Customer stripeCustomer)
    {
        // Create or update customer in database
        using var context = CreateDbContext();

        // Check if customer already exists
        var existingCustomer = await context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == stripeCustomer.Id && c.TenantId == tenantId);

        if (existingCustomer == null)
        {
            // Create new customer
            var customer = new SubscriptionAnalytics.Shared.Entities.Customer
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CustomerId = stripeCustomer.Id,
                Name = stripeCustomer.Name,
                Email = stripeCustomer.Email,
                Phone = stripeCustomer.Phone,
                CustomerCreatedAt = stripeCustomer.Created,
                Livemode = stripeCustomer.Livemode,
                SyncedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "standalone-sync-worker",
                UpdatedBy = "standalone-sync-worker"
            };

            context.Customers.Add(customer);

            // Create Stripe-specific customer data
            var stripeCustomerEntity = new StripeCustomer
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CustomerId = customer.Id,
                StripeCustomerId = stripeCustomer.Id,
                DefaultPaymentMethod = stripeCustomer.DefaultSource?.Id,
                Delinquent = stripeCustomer.Delinquent ?? false,
                InvoicePrefix = stripeCustomer.InvoicePrefix,
                NextInvoiceSequence = stripeCustomer.NextInvoiceSequence.ToString(),
                PreferredLocales = stripeCustomer.PreferredLocales != null ? string.Join(",", stripeCustomer.PreferredLocales) : null,
                Shipping = stripeCustomer.Shipping?.Address?.Line1,
                Source = stripeCustomer.DefaultSource?.Id,
                TaxExempt = stripeCustomer.TaxExempt?.ToString(),
                TestClock = stripeCustomer.TestClock?.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = "standalone-sync-worker",
                UpdatedBy = "standalone-sync-worker"
            };

            context.StripeCustomers.Add(stripeCustomerEntity);
        }
        else
        {
            // Update existing customer
            existingCustomer.Name = stripeCustomer.Name;
            existingCustomer.Email = stripeCustomer.Email;
            existingCustomer.Phone = stripeCustomer.Phone;
            existingCustomer.UpdatedAt = DateTime.UtcNow;
            existingCustomer.UpdatedBy = "standalone-sync-worker";

            // Update Stripe-specific data
            var existingStripeCustomer = await context.StripeCustomers
                .FirstOrDefaultAsync(sc => sc.CustomerId == existingCustomer.Id);

            if (existingStripeCustomer != null)
            {
                existingStripeCustomer.DefaultPaymentMethod = stripeCustomer.DefaultSource?.Id;
                existingStripeCustomer.Delinquent = stripeCustomer.Delinquent ?? false;
                existingStripeCustomer.InvoicePrefix = stripeCustomer.InvoicePrefix;
                existingStripeCustomer.NextInvoiceSequence = stripeCustomer.NextInvoiceSequence.ToString();
                existingStripeCustomer.PreferredLocales = stripeCustomer.PreferredLocales != null ? string.Join(",", stripeCustomer.PreferredLocales) : null;
                existingStripeCustomer.Shipping = stripeCustomer.Shipping?.Address?.Line1;
                existingStripeCustomer.Source = stripeCustomer.DefaultSource?.Id;
                existingStripeCustomer.TaxExempt = stripeCustomer.TaxExempt?.ToString();
                existingStripeCustomer.TestClock = stripeCustomer.TestClock?.Id;
                existingStripeCustomer.UpdatedAt = DateTime.UtcNow;
                existingStripeCustomer.UpdatedBy = "standalone-sync-worker";
            }
        }

        await context.SaveChangesAsync();
    }

    private async Task<SyncJobResult> ProcessPayPalSyncAsync(SyncJob syncJob, SyncJobResult result, ProviderConnection connection)
    {
        _logger.LogInformation("Processing PayPal sync for tenant {TenantId}", syncJob.TenantId);

        // TODO: Implement PayPal sync logic
        // This is a placeholder for future PayPal integration

        // Simulate some async work to avoid the warning
        await Task.Delay(100);

        result.Progress = 100;
        _logger.LogInformation("PayPal sync completed for tenant {TenantId}", syncJob.TenantId);

        return result;
    }

    private async Task UpdateJobStatusAsync(Guid jobId, SyncJobStatus status, int? progress = null, string? errorMessage = null)
    {
        using var context = CreateDbContext();

        var job = await context.SyncJobs.FindAsync(jobId);
        if (job == null)
        {
            _logger.LogWarning("Sync job {JobId} not found for status update", jobId);
            return;
        }

        job.Status = status;
        job.UpdatedAt = DateTime.UtcNow;
        job.UpdatedBy = "standalone-sync-worker";

        if (progress.HasValue)
            job.Progress = progress.Value;

        if (errorMessage != null)
            job.ErrorMessage = errorMessage;

        if (status == SyncJobStatus.Running && job.StartedAt == null)
            job.StartedAt = DateTime.UtcNow;

        if (status == SyncJobStatus.Completed || status == SyncJobStatus.Failed)
            job.CompletedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    private AppDbContext CreateDbContext()
    {
        var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);
        return new AppDbContext(optionsBuilder.Options);
    }
}

public class SyncJobRequest
{
    public Guid JobId { get; set; }
    public Guid TenantId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public Dictionary<string, object>? AdditionalData { get; set; }
}

public class SyncJobResult
{
    public Guid JobId { get; set; }
    public Guid TenantId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public SyncJobStatus Status { get; set; }
    public int Progress { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int RetryCount { get; set; }
    public int TotalRecordsProcessed { get; set; }
}
