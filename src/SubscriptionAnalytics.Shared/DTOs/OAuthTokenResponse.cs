namespace SubscriptionAnalytics.Shared.DTOs;

public class OAuthTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public string? TokenType { get; set; }
    public int? ExpiresIn { get; set; }
    public string? Scope { get; set; }
    public string? ProviderAccountId { get; set; }
    public Dictionary<string, object>? AdditionalData { get; set; }
} 