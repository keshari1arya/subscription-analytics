namespace SubscriptionAnalytics.Shared.DTOs;

public class PayPalOAuthTokenResponse : OAuthTokenResponse
{
    public string? AppId { get; set; }
    public string? Nonce { get; set; }
} 