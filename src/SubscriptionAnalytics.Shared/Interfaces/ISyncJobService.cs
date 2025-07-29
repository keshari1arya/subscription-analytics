using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Enums;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface ISyncJobService
{
    Task<SyncJob> CreateJobAsync(Guid tenantId, SyncJobType jobType, string providerName);
    Task<SyncJob?> GetJobAsync(Guid jobId);
    Task<IEnumerable<SyncJob>> GetJobsByTenantAsync(Guid tenantId);
    Task<SyncJob> UpdateJobStatusAsync(Guid jobId, SyncJobStatus status, int? progress = null, string? errorMessage = null);
    Task<SyncJob> IncrementRetryCountAsync(Guid jobId);
}
