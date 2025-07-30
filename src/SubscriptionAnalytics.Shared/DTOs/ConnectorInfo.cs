namespace SubscriptionAnalytics.Shared.DTOs;

public class ConnectorInfo
{
    public string ProviderName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool SupportsOAuth { get; set; }
}
