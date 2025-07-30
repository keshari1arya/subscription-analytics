namespace SubscriptionAnalytics.Shared.DTOs;

public class InitiateConnectionResponse
{
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
}
