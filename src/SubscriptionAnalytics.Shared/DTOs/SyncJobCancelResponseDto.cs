namespace SubscriptionAnalytics.Shared.DTOs;

public class SyncJobCancelResponseDto
{
    public Guid JobId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
