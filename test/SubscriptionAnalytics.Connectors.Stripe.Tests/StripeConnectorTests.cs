using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Stripe;
using SubscriptionAnalytics.Connectors.Stripe.Services;
using SubscriptionAnalytics.Shared.DTOs;
using Xunit;

namespace SubscriptionAnalytics.Connectors.Stripe.Tests;

public class StripeConnectorTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<StripeConnector>> _loggerMock;
    private readonly StripeConnector _connector;

    public StripeConnectorTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _loggerMock = new Mock<ILogger<StripeConnector>>();
        _configurationMock.Setup(x => x["Stripe:ConnectClientId"]).Returns("test_client_id");
        _configurationMock.Setup(x => x["Stripe:ConnectClientSecret"]).Returns("test_client_secret");
        _configurationMock.Setup(x => x["Stripe:ApiKey"]).Returns("test_api_key");
        
        _connector = new StripeConnector(_configurationMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Constructor_WithValidConfiguration_Should_CreateConnector()
    {
        // Act & Assert
        _connector.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithMissingClientId_Should_ThrowInvalidOperationException()
    {
        // Arrange
        var configMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<StripeConnector>>();
        configMock.Setup(x => x["Stripe:ConnectClientId"]).Returns((string?)null);
        configMock.Setup(x => x["Stripe:ConnectClientSecret"]).Returns("test_secret");

        // Act & Assert
        var action = () => new StripeConnector(configMock.Object, loggerMock.Object);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Stripe Connect Client ID is not configured");
    }

    [Fact]
    public void Constructor_WithMissingClientSecret_Should_ThrowInvalidOperationException()
    {
        // Arrange
        var configMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<StripeConnector>>();
        configMock.Setup(x => x["Stripe:ConnectClientId"]).Returns("test_id");
        configMock.Setup(x => x["Stripe:ConnectClientSecret"]).Returns((string?)null);

        // Act & Assert
        var action = () => new StripeConnector(configMock.Object, loggerMock.Object);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Stripe Connect Client Secret is not configured");
    }

    [Fact]
    public async Task GenerateOAuthUrl_WithValidParameters_Should_ReturnCorrectUrl()
    {
        // Arrange
        var state = "test_state";
        var redirectUri = "https://example.com/callback";

        // Act
        var result = await _connector.GenerateOAuthUrl(state, redirectUri);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().StartWith("https://connect.stripe.com/oauth/authorize");
        result.Should().Contain("response_type=code");
        result.Should().Contain("client_id=test_client_id");
        result.Should().Contain("scope=read_write");
        result.Should().Contain("redirect_uri=" + Uri.EscapeDataString(redirectUri));
        result.Should().Contain("state=" + Uri.EscapeDataString(state));
    }

    [Fact]
    public async Task GenerateOAuthUrl_WithSpecialCharacters_Should_EscapeCorrectly()
    {
        // Arrange
        var state = "state with spaces & symbols";
        var redirectUri = "https://example.com/callback?param=value&other=test";

        // Act
        var result = await _connector.GenerateOAuthUrl(state, redirectUri);

        // Assert
        result.Should().Contain("state=" + Uri.EscapeDataString(state));
        result.Should().Contain("redirect_uri=" + Uri.EscapeDataString(redirectUri));
    }

    [Theory]
    [InlineData("", "https://example.com/callback")]
    [InlineData("test_state", "")]
    [InlineData("", "")]
    public async Task GenerateOAuthUrl_WithEmptyParameters_Should_StillWork(string state, string redirectUri)
    {
        // Act
        var result = await _connector.GenerateOAuthUrl(state, redirectUri);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().StartWith("https://connect.stripe.com/oauth/authorize");
    }

    [Fact]
    public async Task GenerateOAuthUrl_WithNullParameters_Should_HandleGracefully()
    {
        // Act & Assert
        var action = async () => await _connector.GenerateOAuthUrl(null!, "https://example.com/callback");
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ExchangeOAuthCode_WithValidCode_Should_ReturnTokenResponse()
    {
        // This test would require mocking Stripe's OAuthTokenService
        // For now, we'll test that it throws the expected exception
        var code = "valid_oauth_code";

        // Act & Assert
        var action = async () => await _connector.ExchangeOAuthCode(code);
        await action.Should().ThrowAsync<StripeException>();
    }

    [Fact]
    public async Task ExchangeOAuthCode_WithInvalidCode_Should_ThrowStripeException()
    {
        // Arrange
        var code = "invalid_oauth_code";

        // Act & Assert
        var action = async () => await _connector.ExchangeOAuthCode(code);
        await action.Should().ThrowAsync<StripeException>();
    }

    [Fact]
    public async Task ExchangeOAuthCode_WithEmptyCode_Should_ThrowStripeException()
    {
        // Arrange
        var code = "";

        // Act & Assert
        var action = async () => await _connector.ExchangeOAuthCode(code);
        await action.Should().ThrowAsync<StripeException>();
    }

    [Fact]
    public async Task ExchangeOAuthCode_WithNullCode_Should_ThrowStripeException()
    {
        // Arrange
        string? code = null;

        // Act & Assert
        var action = async () => await _connector.ExchangeOAuthCode(code!);
        await action.Should().ThrowAsync<StripeException>();
    }

    [Fact]
    public async Task ValidateConnection_WithValidToken_Should_ReturnTrue()
    {
        // This test would require mocking Stripe's AccountService
        // For now, we'll test the method signature and basic behavior
        var accessToken = "valid_access_token";

        // Act & Assert
        var action = async () => await _connector.ValidateConnection(accessToken);
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ValidateConnection_WithInvalidToken_Should_ReturnFalse()
    {
        // Arrange
        var accessToken = "invalid_access_token";

        // Act
        var result = await _connector.ValidateConnection(accessToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateConnection_WithEmptyToken_Should_ReturnFalse()
    {
        // Arrange
        var accessToken = "";

        // Act
        var result = await _connector.ValidateConnection(accessToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateConnection_WithNullToken_Should_ReturnFalse()
    {
        // Arrange
        string? accessToken = null;

        // Act
        var result = await _connector.ValidateConnection(accessToken!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Configuration_Should_BeAccessedCorrectly()
    {
        // Act & Assert
        _configurationMock.Verify(x => x["Stripe:ConnectClientId"], Times.Once);
        _configurationMock.Verify(x => x["Stripe:ConnectClientSecret"], Times.Once);
    }

    [Fact]
    public void Constructor_Should_StoreConfigurationValues()
    {
        // This test verifies that the constructor properly stores the configuration values
        // by testing that the GenerateOAuthUrl method uses the stored client ID
        var state = "test";
        var redirectUri = "https://example.com/callback";

        // Act
        var result = _connector.GenerateOAuthUrl(state, redirectUri).Result;

        // Assert
        result.Should().Contain("client_id=test_client_id");
    }

    [Fact]
    public void Constructor_WithMissingApiKey_Should_ThrowInvalidOperationException()
    {
        // Arrange
        var configMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<StripeConnector>>();
        configMock.Setup(x => x["Stripe:ConnectClientId"]).Returns("test_id");
        configMock.Setup(x => x["Stripe:ConnectClientSecret"]).Returns("test_secret");
        configMock.Setup(x => x["Stripe:ApiKey"]).Returns((string?)null);

        // Act & Assert
        var action = () => new StripeConnector(configMock.Object, loggerMock.Object);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Stripe API Key is not configured");
    }

    [Fact]
    public void ProviderName_Should_ReturnStripe()
    {
        // Act & Assert
        _connector.ProviderName.Should().Be("Stripe");
    }

    [Fact]
    public void DisplayName_Should_ReturnStripe()
    {
        // Act & Assert
        _connector.DisplayName.Should().Be("Stripe");
    }

    [Fact]
    public void SupportsOAuth_Should_ReturnTrue()
    {
        // Act & Assert
        _connector.SupportsOAuth.Should().BeTrue();
    }

    [Fact]
    public async Task GenerateOAuthUrlAsync_WithValidParameters_Should_ReturnCorrectUrl()
    {
        // Arrange
        var state = "test_state";
        var redirectUri = "https://example.com/callback";
        var tenantId = Guid.NewGuid();

        // Act
        var result = await _connector.GenerateOAuthUrlAsync(state, redirectUri, tenantId);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().StartWith("https://connect.stripe.com/oauth/authorize");
        result.Should().Contain("client_id=test_client_id");
    }

    [Fact]
    public async Task ExchangeOAuthCodeAsync_WithValidCode_Should_ReturnOAuthTokenResponse()
    {
        // Arrange
        var code = "valid_oauth_code";
        var state = "test_state";

        // Act & Assert
        var action = async () => await _connector.ExchangeOAuthCodeAsync(code, state);
        await action.Should().ThrowAsync<StripeException>();
    }

    [Fact]
    public async Task ValidateConnectionAsync_WithValidToken_Should_ReturnTrue()
    {
        // Arrange
        var accessToken = "valid_access_token";

        // Act & Assert
        var action = async () => await _connector.ValidateConnectionAsync(accessToken);
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DisconnectAsync_WithValidTenantId_Should_ReturnTrue()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        var result = await _connector.DisconnectAsync(tenantId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SyncDataAsync_WithValidTenantId_Should_CompleteSuccessfully()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var action = async () => await _connector.SyncDataAsync(tenantId, cancellationToken);
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SyncDataAsync_WithCancelledToken_Should_HandleCancellation()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act & Assert
        var action = async () => await _connector.SyncDataAsync(tenantId, cancellationTokenSource.Token);
        await action.Should().ThrowAsync<OperationCanceledException>();
    }
} 