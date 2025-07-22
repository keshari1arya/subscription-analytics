using Microsoft.Extensions.Configuration;
using Stripe;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Connectors.Stripe.Services;

public class StripeConnector : IStripeConnector
{
    private readonly string _clientId;
    private readonly string _clientSecret;

    public StripeConnector(IConfiguration configuration)
    {
        _clientId = configuration["Stripe:ConnectClientId"] 
            ?? throw new InvalidOperationException("Stripe Connect Client ID is not configured");
        _clientSecret = configuration["Stripe:ConnectClientSecret"] 
            ?? throw new InvalidOperationException("Stripe Connect Client Secret is not configured");
    }

    public Task<string> GenerateOAuthUrl(string state, string redirectUri)
    {
        var oauthUrl = "https://connect.stripe.com/oauth/authorize" +
                      $"?response_type=code" +
                      $"&client_id={_clientId}" +
                      $"&scope=read_write" +
                      $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                      $"&state={Uri.EscapeDataString(state)}";
        
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
        catch (StripeException)
        {
            throw;
        }
        catch (Exception)
        {
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
        catch (StripeException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
} 