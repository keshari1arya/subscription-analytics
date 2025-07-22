using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Connectors.Stripe.Abstractions;

public interface IStripeConnector
{
    Task<string> GenerateOAuthUrl(string state, string redirectUri);
    Task<StripeOAuthTokenResponse> ExchangeOAuthCode(string code);
    Task<bool> ValidateConnection(string accessToken);
} 