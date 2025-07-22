using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface IConnector
{
    string ProviderName { get; }
    string DisplayName { get; }
    bool SupportsOAuth { get; }
    
    Task SyncDataAsync(Guid tenantId, CancellationToken cancellationToken);
    Task<string> GenerateOAuthUrlAsync(string state, string redirectUri, Guid tenantId);
    Task<OAuthTokenResponse> ExchangeOAuthCodeAsync(string code, string state);
    Task<bool> ValidateConnectionAsync(string accessToken);
    Task<bool> DisconnectAsync(Guid tenantId);
} 