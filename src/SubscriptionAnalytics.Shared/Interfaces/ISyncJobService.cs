using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Enums;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface ISyncJobService
{
    Task<SyncJob> CreateJobAsync(
        Guid tenantId,
        string providerName,
        SyncJobType jobType,
        string? accessToken = null,
        string? refreshToken = null,
        Dictionary<string, object>? additionalData = null);
    Task<SyncJob?> GetJobAsync(Guid jobId);
    Task<IEnumerable<SyncJob>> GetJobsByTenantAsync(Guid tenantId);
    Task<SyncJob> UpdateJobStatusAsync(Guid jobId, SyncJobStatus status, int? progress = null, string? errorMessage = null);
    Task<SyncJob> IncrementRetryCountAsync(Guid jobId);
}
