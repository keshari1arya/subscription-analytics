using SubscriptionAnalytics.Shared.Enums;

namespace SubscriptionAnalytics.Shared.Entities;

public class SyncJob : BaseTenantEntity
{
    public SyncJobType JobType { get; set; }
    public SyncJobStatus Status { get; set; }
    public int Progress { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int RetryCount { get; set; }
    public string? ProviderName { get; set; }
}
