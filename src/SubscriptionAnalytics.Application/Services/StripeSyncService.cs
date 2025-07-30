using Microsoft.Extensions.Logging;
using Stripe;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Application.Services;

public class StripeSyncService : SubscriptionAnalytics.Shared.Interfaces.IStripeSyncService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IStripeCustomerRepository _stripeCustomerRepository;
    private readonly ILogger<StripeSyncService> _logger;

    public StripeSyncService(
        ICustomerRepository customerRepository,
        IStripeCustomerRepository stripeCustomerRepository,
        ILogger<StripeSyncService> logger)
    {
        _customerRepository = customerRepository;
        _stripeCustomerRepository = stripeCustomerRepository;
        _logger = logger;
    }

    public async Task<int> SyncCustomersAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting customer sync for tenant: {TenantId}", tenantId);

        var customerService = new CustomerService();
        var options = new CustomerListOptions
        {
            Limit = 100 // Process 100 customers at a time
        };

        var totalSynced = 0;
        var hasMore = true;

        while (hasMore && !cancellationToken.IsCancellationRequested)
        {
            var stripeCustomers = await customerService.ListAsync(options, new RequestOptions
            {
                StripeAccount = accessToken
            });

            foreach (var stripeCustomer in stripeCustomers.Data)
            {
                if (cancellationToken.IsCancellationRequested) break;

                try
                {
                    await SyncCustomerAsync(tenantId, stripeCustomer);
                    totalSynced++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to sync customer {CustomerId} for tenant {TenantId}",
                        stripeCustomer.Id, tenantId);
                }
            }

            hasMore = stripeCustomers.HasMore;
            if (hasMore)
            {
                options.StartingAfter = stripeCustomers.Data.Last().Id;
            }
        }

        _logger.LogInformation("Completed customer sync for tenant: {TenantId}. Total synced: {TotalSynced}",
            tenantId, totalSynced);

        return totalSynced;
    }

    public async Task<int> SyncSubscriptionsAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting subscription sync for tenant: {TenantId}", tenantId);

        // TODO: Implement subscription sync
        // This will be implemented in the next phase
        await Task.Delay(100, cancellationToken); // Placeholder

        _logger.LogInformation("Completed subscription sync for tenant: {TenantId}", tenantId);
        return 0;
    }

    public async Task<int> SyncPaymentsAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting payment sync for tenant: {TenantId}", tenantId);

        // TODO: Implement payment sync
        // This will be implemented in the next phase
        await Task.Delay(100, cancellationToken); // Placeholder

        _logger.LogInformation("Completed payment sync for tenant: {TenantId}", tenantId);
        return 0;
    }

    public async Task<int> SyncAllDataAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting full data sync for tenant: {TenantId}", tenantId);

        var totalCustomers = await SyncCustomersAsync(tenantId, accessToken, cancellationToken);
        var totalSubscriptions = await SyncSubscriptionsAsync(tenantId, accessToken, cancellationToken);
        var totalPayments = await SyncPaymentsAsync(tenantId, accessToken, cancellationToken);

        _logger.LogInformation("Completed full data sync for tenant: {TenantId}. Customers: {Customers}, Subscriptions: {Subscriptions}, Payments: {Payments}",
            tenantId, totalCustomers, totalSubscriptions, totalPayments);

        return totalCustomers + totalSubscriptions + totalPayments;
    }

    private async Task SyncCustomerAsync(Guid tenantId, Stripe.Customer stripeCustomer)
    {
        // Check if we already have this customer
        var existingStripeCustomer = await _stripeCustomerRepository.GetByStripeCustomerIdAndTenantAsync(stripeCustomer.Id, tenantId);

        if (existingStripeCustomer != null)
        {
            // Update existing customer
            await UpdateStripeCustomerAsync(existingStripeCustomer, stripeCustomer);
        }
        else
        {
            // Create new customer
            await CreateStripeCustomerAsync(tenantId, stripeCustomer);
        }
    }

    private async Task CreateStripeCustomerAsync(Guid tenantId, Stripe.Customer stripeCustomer)
    {
        // First, create or get the generic customer
        var customer = await _customerRepository.GetByCustomerIdAsync(stripeCustomer.Id);

        if (customer == null)
        {
            customer = new SubscriptionAnalytics.Shared.Entities.Customer
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
                CreatedBy = "stripe-sync",
                UpdatedBy = "stripe-sync"
            };

            customer = await _customerRepository.AddAsync(customer);
        }

        // Create the Stripe-specific customer data
        var stripeCustomerEntity = new StripeCustomer
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CustomerId = customer.Id,
            StripeCustomerId = stripeCustomer.Id,
            DefaultPaymentMethod = stripeCustomer.DefaultSource?.Id,
            TaxInfo = null, // TODO: Map when Stripe API is available
            TaxInfoVerification = null, // TODO: Map when Stripe API is available
            Delinquent = stripeCustomer.Delinquent ?? false,
            Discount = null, // TODO: Map when Stripe API is available
            InvoicePrefix = stripeCustomer.InvoicePrefix,
            InvoiceSettings = null, // TODO: Map when Stripe API is available
            NextInvoiceSequence = stripeCustomer.NextInvoiceSequence.ToString(),
            PreferredLocales = stripeCustomer.PreferredLocales != null ? string.Join(",", stripeCustomer.PreferredLocales) : null,
            Shipping = stripeCustomer.Shipping?.Address?.Line1,
            Source = stripeCustomer.DefaultSource?.Id,
            Subscriptions = null, // TODO: Map when Stripe API is available
            Tax = null, // TODO: Map when Stripe API is available
            TaxExempt = stripeCustomer.TaxExempt?.ToString(),
            TestClock = stripeCustomer.TestClock?.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = "stripe-sync",
            UpdatedBy = "stripe-sync"
        };

        await _stripeCustomerRepository.AddAsync(stripeCustomerEntity);

        _logger.LogDebug("Created new Stripe customer {StripeCustomerId} for tenant {TenantId}",
            stripeCustomer.Id, tenantId);
    }

    private async Task UpdateStripeCustomerAsync(StripeCustomer existingCustomer, Stripe.Customer stripeCustomer)
    {
        // Update the generic customer if needed
        var customer = existingCustomer.Customer;
        var customerUpdated = false;

        if (customer.Name != stripeCustomer.Name ||
            customer.Email != stripeCustomer.Email ||
            customer.Phone != stripeCustomer.Phone)
        {
            customer.Name = stripeCustomer.Name;
            customer.Email = stripeCustomer.Email;
            customer.Phone = stripeCustomer.Phone;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = "stripe-sync";
            customerUpdated = true;
        }

        // Update Stripe-specific data
        existingCustomer.DefaultPaymentMethod = stripeCustomer.DefaultSource?.Id;
        existingCustomer.TaxInfo = null; // TODO: Map when Stripe API is available
        existingCustomer.TaxInfoVerification = null; // TODO: Map when Stripe API is available
        existingCustomer.Delinquent = stripeCustomer.Delinquent ?? false;
        existingCustomer.Discount = null; // TODO: Map when Stripe API is available
        existingCustomer.InvoicePrefix = stripeCustomer.InvoicePrefix;
        existingCustomer.InvoiceSettings = null; // TODO: Map when Stripe API is available
        existingCustomer.NextInvoiceSequence = stripeCustomer.NextInvoiceSequence.ToString();
        existingCustomer.PreferredLocales = stripeCustomer.PreferredLocales != null ? string.Join(",", stripeCustomer.PreferredLocales) : null;
        existingCustomer.Shipping = stripeCustomer.Shipping?.Address?.Line1;
        existingCustomer.Source = stripeCustomer.DefaultSource?.Id;
        existingCustomer.Subscriptions = null; // TODO: Map when Stripe API is available
        existingCustomer.Tax = null; // TODO: Map when Stripe API is available
        existingCustomer.TaxExempt = stripeCustomer.TaxExempt?.ToString();
        existingCustomer.TestClock = stripeCustomer.TestClock?.Id;
        existingCustomer.UpdatedAt = DateTime.UtcNow;
        existingCustomer.UpdatedBy = "stripe-sync";

        // Save changes
        if (customerUpdated)
        {
            await _customerRepository.UpdateAsync(customer);
        }

        await _stripeCustomerRepository.UpdateAsync(existingCustomer);

        _logger.LogDebug("Updated existing Stripe customer {StripeCustomerId} for tenant {TenantId}",
            stripeCustomer.Id, existingCustomer.TenantId);
    }
}
