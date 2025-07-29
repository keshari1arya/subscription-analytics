namespace SubscriptionAnalytics.Shared.DTOs;

public class SyncJobResponseDto
{
    public Guid JobId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
