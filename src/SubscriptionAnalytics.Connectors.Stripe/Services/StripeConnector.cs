using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Connectors.Stripe.Services;

public class StripeConnector : IStripeConnector, IConnector
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _apiKey;
    private readonly ILogger<StripeConnector> _logger;
    private readonly IStripeSyncService? _stripeSyncService;

    public StripeConnector(IConfiguration configuration, ILogger<StripeConnector> logger, IStripeSyncService? stripeSyncService = null)
    {
        // Use configuration first, then fall back to environment variables
        _clientId = configuration["Stripe:ConnectClientId"]
            ?? Environment.GetEnvironmentVariable("STRIPE_CONNECT_CLIENT_ID")
            ?? throw new InvalidOperationException("Stripe Connect Client ID is not configured");
        _clientSecret = configuration["Stripe:ConnectClientSecret"]
            ?? Environment.GetEnvironmentVariable("STRIPE_CONNECT_CLIENT_SECRET")
            ?? throw new InvalidOperationException("Stripe Connect Client Secret is not configured");
        _apiKey = configuration["Stripe:ApiKey"]
            ?? Environment.GetEnvironmentVariable("STRIPE_API_KEY")
            ?? throw new InvalidOperationException("Stripe API Key is not configured");
        _logger = logger;
        _stripeSyncService = stripeSyncService;

        // Initialize Stripe configuration
        StripeConfiguration.ApiKey = _apiKey;
    }

    // IConnector implementation
    public string ProviderName => "Stripe";
    public string DisplayName => "Stripe";
    public bool SupportsOAuth => true;

    public async Task<string> GenerateOAuthUrlAsync(string state, string redirectUri, Guid tenantId)
    {
        _logger.LogInformation("Generating Stripe OAuth URL for tenant: {TenantId}", tenantId);
        return await GenerateOAuthUrl(state, redirectUri);
    }

    public async Task<OAuthTokenResponse> ExchangeOAuthCodeAsync(string code, string state)
    {
        _logger.LogInformation("Exchanging Stripe OAuth code for token");
        var stripeResponse = await ExchangeOAuthCode(code);
        return new OAuthTokenResponse
        {
            AccessToken = stripeResponse.AccessToken,
            RefreshToken = stripeResponse.RefreshToken,
            TokenType = stripeResponse.TokenType,
            Scope = stripeResponse.Scope,
            ProviderAccountId = stripeResponse.StripeAccountId,
            AdditionalData = new Dictionary<string, object>
            {
                ["StripeAccountId"] = stripeResponse.StripeAccountId
            }
        };
    }

    public async Task<bool> ValidateConnectionAsync(string accessToken)
    {
        return await ValidateConnection(accessToken);
    }

    public async Task<bool> DisconnectAsync(Guid tenantId)
    {
        // TODO: Implement Stripe disconnection logic
        _logger.LogInformation("Disconnecting Stripe for tenant: {TenantId}", tenantId);
        return true;
    }

    public async Task SyncDataAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Syncing Stripe data for tenant: {TenantId}", tenantId);

        if (_stripeSyncService == null)
        {
            _logger.LogWarning("Stripe sync service not available, using placeholder implementation");
            await Task.Delay(100, cancellationToken); // Fallback implementation
            return;
        }

        // Get the access token for this tenant
        // TODO: Get access token from tenant's provider connection
        var accessToken = "placeholder"; // This should come from the tenant's provider connection

        try
        {
            var totalSynced = await _stripeSyncService.SyncAllDataAsync(tenantId, accessToken, cancellationToken);
            _logger.LogInformation("Successfully synced {TotalSynced} items for tenant {TenantId}", totalSynced, tenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing Stripe data for tenant {TenantId}", tenantId);
            throw;
        }
    }

    // IStripeConnector implementation (existing methods)
    public Task<string> GenerateOAuthUrl(string state, string redirectUri)
    {
        var oauthUrl = "https://connect.stripe.com/oauth/authorize" +
                      $"?response_type=code" +
                      $"&client_id={_clientId}" +
                      $"&scope=read_write" +
                      $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                      $"&state={Uri.EscapeDataString(state)}";

        if (_apiKey.StartsWith("sk_test"))
        {
            oauthUrl += "&stripe_user[test]=true";
        }

        return Task.FromResult(oauthUrl);
    }

    public async Task<StripeOAuthTokenResponse> ExchangeOAuthCode(string code)
    {
        try
        {
            var service = new OAuthTokenService();
            var options = new OAuthTokenCreateOptions
            {
                GrantType = "authorization_code",
                Code = code,
                ClientSecret = _clientSecret
            };

            var response = await service.CreateAsync(options);

            return new StripeOAuthTokenResponse
            {
                AccessToken = response.StripeUserId, // Use StripeUserId as the access token
                RefreshToken = response.StripeUserId ?? "", // Use StripeUserId instead of obsolete RefreshToken
                StripeAccountId = response.StripeUserId,
                TokenType = response.TokenType,
                Scope = response.Scope
            };
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe OAuth token exchange failed");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during Stripe OAuth token exchange");
            throw;
        }
    }

    public async Task<bool> ValidateConnection(string accessToken)
    {
        try
        {
            var service = new AccountService();
            await service.GetAsync(accessToken);
            return true;
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(ex, "Stripe connection validation failed");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during Stripe connection validation");
            return false;
        }
    }
}
