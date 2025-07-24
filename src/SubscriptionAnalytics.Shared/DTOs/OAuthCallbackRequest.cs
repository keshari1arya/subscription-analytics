namespace SubscriptionAnalytics.Shared.DTOs;

public class OAuthCallbackRequest
{
    public string Provider { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
} 