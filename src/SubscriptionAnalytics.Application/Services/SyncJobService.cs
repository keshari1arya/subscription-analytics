using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using System.Text.Json;

namespace SubscriptionAnalytics.Application.Services;

public class SyncJobService : ISyncJobService
{
    private readonly ISyncJobRepository _syncJobRepository;

    public SyncJobService(ISyncJobRepository syncJobRepository)
    {
        _syncJobRepository = syncJobRepository;
    }

    public async Task<SyncJob> CreateJobAsync(
        Guid tenantId,
        string providerName,
        SyncJobType jobType,
        string? accessToken = null,
        string? refreshToken = null,
        Dictionary<string, object>? additionalData = null)
    {
        var syncJob = new SyncJob
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            JobType = jobType,
            Status = SyncJobStatus.Pending,
            Progress = 0,
            RetryCount = 0,
            ProviderName = providerName,
            StartedAt = null,
            CompletedAt = null,
            ErrorMessage = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = "system",
            UpdatedBy = "system"
        };

        // Store access token and additional data in the job (you might want to encrypt these)
        if (!string.IsNullOrEmpty(accessToken))
        {
            // TODO: Encrypt sensitive data before storing
            syncJob.ErrorMessage = $"AccessToken: {accessToken}"; // Temporary storage, should be encrypted
        }

        if (additionalData != null)
        {
            // Store additional data as JSON in a separate field or encrypted
            var additionalDataJson = JsonSerializer.Serialize(additionalData);
            // TODO: Store this in a separate field or encrypted
        }

        return await _syncJobRepository.AddAsync(syncJob);
    }

    public async Task<SyncJob?> GetJobAsync(Guid jobId)
    {
        return await _syncJobRepository.GetByIdAsync(jobId);
    }

    public async Task<IEnumerable<SyncJob>> GetJobsByTenantAsync(Guid tenantId)
    {
        return await _syncJobRepository.GetByTenantIdAsync(tenantId);
    }

    public async Task<SyncJob> UpdateJobStatusAsync(Guid jobId, SyncJobStatus status, int? progress = null, string? errorMessage = null)
    {
        var job = await _syncJobRepository.GetByIdAsync(jobId);
        if (job == null)
            throw new ArgumentException($"Sync job with ID {jobId} not found");

        job.Status = status;
        job.UpdatedAt = DateTime.UtcNow;
        job.UpdatedBy = "system";

        if (progress.HasValue)
            job.Progress = progress.Value;

        if (errorMessage != null)
            job.ErrorMessage = errorMessage;

        if (status == SyncJobStatus.Running && job.StartedAt == null)
            job.StartedAt = DateTime.UtcNow;

        if (status == SyncJobStatus.Completed || status == SyncJobStatus.Failed)
            job.CompletedAt = DateTime.UtcNow;

        return await _syncJobRepository.UpdateAsync(job);
    }

    public async Task<SyncJob> IncrementRetryCountAsync(Guid jobId)
    {
        var job = await _syncJobRepository.GetByIdAsync(jobId);
        if (job == null)
            throw new ArgumentException($"Sync job with ID {jobId} not found");

        job.RetryCount++;
        job.UpdatedAt = DateTime.UtcNow;
        job.UpdatedBy = "system";

        return await _syncJobRepository.UpdateAsync(job);
    }
}
