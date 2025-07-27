using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Connectors.Stripe.Abstractions;

public interface IStripeConnector : IConnector
{
    Task<string> GenerateOAuthUrl(string state, string redirectUri);
    Task<StripeOAuthTokenResponse> ExchangeOAuthCode(string code);
    Task<bool> ValidateConnection(string accessToken);
} 