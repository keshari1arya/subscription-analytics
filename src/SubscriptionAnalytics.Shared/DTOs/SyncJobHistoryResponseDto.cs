namespace SubscriptionAnalytics.Shared.DTOs;

public class SyncJobHistoryResponseDto
{
    public Guid JobId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Progress { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int RetryCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
