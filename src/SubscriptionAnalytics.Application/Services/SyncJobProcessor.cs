using Microsoft.Extensions.Logging;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Application.Services;

public class SyncJobProcessor : ISyncJobProcessor
{
    private readonly ISyncJobService _syncJobService;
    private readonly IConnectorFactory _connectorFactory;
    private readonly ILogger<SyncJobProcessor> _logger;

    public SyncJobProcessor(
        ISyncJobService syncJobService,
        IConnectorFactory connectorFactory,
        ILogger<SyncJobProcessor> logger)
    {
        _syncJobService = syncJobService;
        _connectorFactory = connectorFactory;
        _logger = logger;
    }

    public async Task<SyncJob> ProcessFullSyncAsync(SyncJob syncJob)
    {
        const int maxRetries = 3;

        try
        {
            // Update status to Running
            await _syncJobService.UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Running, 0);

            _logger.LogInformation("Starting full sync for job {JobId} with provider {ProviderName}",
                syncJob.Id, syncJob.ProviderName);

            // Get the appropriate connector
            var connectorType = syncJob.ProviderName?.ToLower() switch
            {
                "stripe" => ConnectorType.Stripe,
                "paypal" => ConnectorType.PayPal,
                _ => throw new InvalidOperationException($"Unsupported provider: {syncJob.ProviderName}")
            };

            var connector = _connectorFactory.GetConnector(connectorType);
            if (connector == null)
            {
                throw new InvalidOperationException($"No connector found for provider: {syncJob.ProviderName}");
            }

            // TODO: Implement percentage-based progress tracking
            // For now, we'll use simple progress updates

            // Update progress to 25%
            await _syncJobService.UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Running, 25);

            // Perform the full sync
            await connector.SyncDataAsync(syncJob.TenantId, CancellationToken.None);

            // Update progress to 100% and mark as completed
            await _syncJobService.UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Completed, 100);

            _logger.LogInformation("Full sync completed successfully for job {JobId}", syncJob.Id);

            return await _syncJobService.GetJobAsync(syncJob.Id) ?? syncJob;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing sync job {JobId}", syncJob.Id);

            // Increment retry count
            await _syncJobService.IncrementRetryCountAsync(syncJob.Id);

            // Check if we should retry
            var updatedJob = await _syncJobService.GetJobAsync(syncJob.Id);
            if (updatedJob?.RetryCount < maxRetries)
            {
                // Mark as pending for retry
                await _syncJobService.UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Pending,
                    errorMessage: $"Retry {updatedJob.RetryCount}/{maxRetries}: {ex.Message}");

                _logger.LogInformation("Scheduling retry {RetryCount}/{MaxRetries} for job {JobId}",
                    updatedJob.RetryCount, maxRetries, syncJob.Id);
            }
            else
            {
                // Mark as failed after max retries
                await _syncJobService.UpdateJobStatusAsync(syncJob.Id, SyncJobStatus.Failed,
                    errorMessage: $"Failed after {maxRetries} retries: {ex.Message}");

                _logger.LogError("Job {JobId} failed after {MaxRetries} retries", syncJob.Id, maxRetries);
            }

            return await _syncJobService.GetJobAsync(syncJob.Id) ?? syncJob;
        }
    }
}
