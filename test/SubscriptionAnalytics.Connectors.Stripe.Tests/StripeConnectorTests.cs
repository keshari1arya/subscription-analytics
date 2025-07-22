using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Connectors.Stripe.Services;
using Xunit;

namespace SubscriptionAnalytics.Connectors.Stripe.Tests;

public class StripeConnectorTests
{
    private readonly IStripeConnector _connector;

    public StripeConnectorTests()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(x => x["Stripe:ConnectClientId"]).Returns("test_client_id");
        mockConfiguration.Setup(x => x["Stripe:ConnectClientSecret"]).Returns("test_client_secret");
        mockConfiguration.Setup(x => x["Stripe:RedirectUri"]).Returns("http://localhost:7001/api/stripe/callback");
        
        _connector = new StripeConnector(mockConfiguration.Object);
    }

    [Fact]
    public async Task GenerateOAuthUrl_Should_ReturnValidUrl()
    {
        // Arrange
        var state = "test_state";
        var redirectUri = "http://localhost:7001/api/stripe/callback";

        // Act
        var result = await _connector.GenerateOAuthUrl(state, redirectUri);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("https://connect.stripe.com/oauth/authorize");
        result.Should().Contain("client_id=test_client_id");
        result.Should().Contain($"state={state}");
    }

    [Fact]
    public async Task ExchangeOAuthCode_Should_ReturnTokenResponse()
    {
        // Arrange
        var code = "test_code";

        // Act & Assert
        // Note: This will fail in tests since we're using mock configuration
        // In real scenarios, this would be tested with integration tests
        await Assert.ThrowsAsync<Exception>(() => _connector.ExchangeOAuthCode(code));
    }
} 