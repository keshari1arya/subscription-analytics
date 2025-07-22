using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Connectors.PayPal.Abstractions;

public interface IPayPalConnector : IConnector
{
    Task<string> GenerateOAuthUrl(string state, string redirectUri);
    Task<PayPalOAuthTokenResponse> ExchangeOAuthCode(string code);
    Task<bool> ValidateConnection(string accessToken);
} 