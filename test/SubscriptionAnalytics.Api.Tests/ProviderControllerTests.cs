using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Api.Controllers;
using SubscriptionAnalytics.Api.Configuration;
using System.Text.Json;
using Xunit;

namespace SubscriptionAnalytics.Api.Tests;

public class ProviderControllerTests
{
    private readonly Mock<IConnectorFactory> _connectorFactoryMock;
    private readonly Mock<IProviderConnectionService> _connectionServiceMock;
    private readonly Mock<ITenantContext> _tenantContextMock;
    private readonly Mock<ILogger<ProviderController>> _loggerMock;
    private readonly Mock<IOptions<OAuthConfiguration>> _oauthConfigMock;
    private readonly Mock<IConnector> _stripeConnectorMock;
    private readonly Mock<IConnector> _payPalConnectorMock;
    private readonly ProviderController _controller;

    public ProviderControllerTests()
    {
        _connectorFactoryMock = new Mock<IConnectorFactory>();
        _connectionServiceMock = new Mock<IProviderConnectionService>();
        _tenantContextMock = new Mock<ITenantContext>();
        _loggerMock = new Mock<ILogger<ProviderController>>();
        _oauthConfigMock = new Mock<IOptions<OAuthConfiguration>>();
        _stripeConnectorMock = new Mock<IConnector>();
        _payPalConnectorMock = new Mock<IConnector>();

        // Setup OAuth configuration
        _oauthConfigMock.Setup(x => x.Value).Returns(new OAuthConfiguration
        {
            UiCallbackUrl = "http://localhost:4200/providers/oauth-callback"
        });

        _controller = new ProviderController(
            _connectorFactoryMock.Object,
            _connectionServiceMock.Object,
            _tenantContextMock.Object,
            _loggerMock.Object,
            _oauthConfigMock.Object);

        // Setup default connector mocks
        _stripeConnectorMock.Setup(x => x.ProviderName).Returns("stripe");
        _stripeConnectorMock.Setup(x => x.DisplayName).Returns("Stripe");
        _stripeConnectorMock.Setup(x => x.SupportsOAuth).Returns(true);

        _payPalConnectorMock.Setup(x => x.ProviderName).Returns("paypal");
        _payPalConnectorMock.Setup(x => x.DisplayName).Returns("PayPal");
        _payPalConnectorMock.Setup(x => x.SupportsOAuth).Returns(true);
    }

    [Fact]
    public void Constructor_WithValidDependencies_Should_CreateController()
    {
        // Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<ProviderController>();
    }

    [Fact]
    public void GetAvailableProviders_WithValidConnectors_Should_ReturnProviderList()
    {
        // Arrange
        var connectors = new List<IConnector> { _stripeConnectorMock.Object, _payPalConnectorMock.Object };
        _connectorFactoryMock.Setup(x => x.GetAllConnectors()).Returns(connectors);

        // Act
        var result = _controller.GetAvailableProviders();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<ConnectorInfo>>>();
        var actionResult = result as ActionResult<IEnumerable<ConnectorInfo>>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var providers = okResult!.Value as IEnumerable<ConnectorInfo>;
        providers.Should().NotBeNull();
        providers!.Count().Should().Be(2);
        providers.Should().Contain(p => p.ProviderName == "stripe" && p.DisplayName == "Stripe" && p.SupportsOAuth);
        providers.Should().Contain(p => p.ProviderName == "paypal" && p.DisplayName == "PayPal" && p.SupportsOAuth);
    }

    [Fact]
    public async Task InitiateConnection_WithValidStripeProvider_Should_ReturnAuthorizationUrl()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var expectedUrl = "https://connect.stripe.com/oauth/authorize?response_type=code&client_id=test&scope=read_write&redirect_uri=http%3A%2F%2Flocalhost%3A4200%2Fproviders%2Foauth-callback%3Fprovider%3Dstripe&state=";

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.InitiateConnection(provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var response = okResult!.Value as InitiateConnectionResponse;
        response.Should().NotBeNull();
        response!.AuthorizationUrl.Should().Be(expectedUrl);
        response.State.Should().NotBeNullOrEmpty();
        response.Provider.Should().Be(provider);
    }

    [Fact]
    public async Task InitiateConnection_WithValidPayPalProvider_Should_ReturnAuthorizationUrl()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "paypal";
        var expectedUrl = "https://www.paypal.com/connect?flowentry=static&client_id=test&scope=openid%20email%20profile%20https%3A%2F%2Furi.paypal.com%2Fservices%2Fpaypalattributes&redirect_uri=http%3A%2F%2Flocalhost%3A4200%2Fproviders%2Foauth-callback%3Fprovider%3Dpaypal&state=test_state";

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.PayPal)).Returns(_payPalConnectorMock.Object);
        _payPalConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.InitiateConnection(provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var response = okResult!.Value as InitiateConnectionResponse;
        response.Should().NotBeNull();
        response!.AuthorizationUrl.Should().Be(expectedUrl);
        response.State.Should().NotBeNullOrEmpty();
        response.Provider.Should().Be(provider);
    }

    [Fact]
    public async Task InitiateConnection_WithInvalidProvider_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "invalid_provider";

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);

        // Act
        var result = await _controller.InitiateConnection(provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = actionResult.Result as BadRequestObjectResult;

        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("Unsupported provider");
    }

    [Theory]
    [InlineData("stripe", ConnectorType.Stripe)]
    [InlineData("paypal", ConnectorType.PayPal)]
    [InlineData("STRIPE", ConnectorType.Stripe)]
    [InlineData("PAYPAL", ConnectorType.PayPal)]
    public async Task InitiateConnection_WithCaseInsensitiveProvider_Should_Work(string provider, ConnectorType expectedType)
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var expectedUrl = "https://test.com/oauth";

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectorFactoryMock.Setup(x => x.GetConnector(expectedType)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.InitiateConnection(provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var response = okResult!.Value as InitiateConnectionResponse;
        response.Should().NotBeNull();
        response!.AuthorizationUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public async Task HandleOAuthCallback_WithValidStripeCallback_Should_ReturnSuccess()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var code = "test_code";
        var state = "test_state";

        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123"
        };

        var connectionDto = new ProviderConnectionDto
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = provider,
            ProviderAccountId = "acct_test123",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow,
            Scope = "read_write"
        };

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCodeAsync(code, state))
            .ReturnsAsync(tokenResponse);
        _connectionServiceMock.Setup(x => x.SaveConnectionAsync(tenantId, provider, tokenResponse))
            .ReturnsAsync(connectionDto);

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, code, state);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var response = okResult!.Value as SuccessResponseDto;
        response!.Message.Should().Contain("stripe account connected successfully");
        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task HandleOAuthCallback_WithValidPayPalCallback_Should_ReturnSuccess()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "paypal";
        var code = "test_code";
        var state = "test_state";

        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "paypal_account_123"
        };

        var connectionDto = new ProviderConnectionDto
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = provider,
            ProviderAccountId = "paypal_account_123",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow,
            Scope = "openid email profile"
        };

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.PayPal)).Returns(_payPalConnectorMock.Object);
        _payPalConnectorMock.Setup(x => x.ExchangeOAuthCodeAsync(code, state))
            .ReturnsAsync(tokenResponse);
        _connectionServiceMock.Setup(x => x.SaveConnectionAsync(tenantId, provider, tokenResponse))
            .ReturnsAsync(connectionDto);

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, code, state);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var response = okResult!.Value as SuccessResponseDto;
        response!.Message.Should().Contain("paypal account connected successfully");
        response.Success.Should().BeTrue();
    }

    [Fact]
    public async Task HandleOAuthCallback_WithOAuthError_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var error = "access_denied";
        var errorDescription = "User denied access";

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, "code", "state", error, errorDescription);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Be(error);
    }

    [Fact]
    public async Task HandleOAuthCallback_WithOAuthErrorAndNoDescription_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var error = "access_denied";

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, "code", "state", error, null);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Be(error);
    }

    [Fact]
    public async Task HandleOAuthCallback_WithMissingCode_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var state = "test_state";

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, "", state);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Be("Missing required OAuth parameters");
    }

    [Fact]
    public async Task HandleOAuthCallback_WithMissingState_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var code = "test_code";

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, code, "");

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Be("Missing required OAuth parameters");
    }

    [Fact]
    public async Task HandleOAuthCallback_WithInvalidProvider_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "invalid_provider";
        var code = "test_code";
        var state = "test_state";

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, code, state);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("Unsupported provider");
    }

    [Fact]
    public async Task GetConnection_WithValidConnection_Should_ReturnConnection()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var connectionDto = new ProviderConnectionDto
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = provider,
            ProviderAccountId = "acct_test123",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow,
            Scope = "read_write"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectionServiceMock.Setup(x => x.GetConnectionAsync(tenantId, provider))
            .ReturnsAsync(connectionDto);

        // Act
        var result = await _controller.GetConnection(provider);

        // Assert
        result.Should().BeOfType<ActionResult<ProviderConnectionDto>>();
        var actionResult = result as ActionResult<ProviderConnectionDto>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var returnedConnection = okResult!.Value as ProviderConnectionDto;
        returnedConnection.Should().NotBeNull();
        returnedConnection!.Id.Should().Be(connectionDto.Id);
        returnedConnection.ProviderName.Should().Be(provider);
    }

    [Fact]
    public async Task GetConnection_WithNoConnection_Should_ReturnNotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectionServiceMock.Setup(x => x.GetConnectionAsync(tenantId, provider))
            .ReturnsAsync((ProviderConnectionDto?)null);

        // Act
        var result = await _controller.GetConnection(provider);

        // Assert
        result.Should().BeOfType<ActionResult<ProviderConnectionDto>>();
        var actionResult = result as ActionResult<ProviderConnectionDto>;
        actionResult!.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = actionResult.Result as NotFoundObjectResult;

        var errorResponse = notFoundResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("No stripe connection found");
    }

    [Fact]
    public async Task GetConnections_WithValidConnections_Should_ReturnConnections()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connections = new List<ProviderConnectionDto>
        {
            new ProviderConnectionDto
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                ProviderName = "stripe",
                ProviderAccountId = "acct_test123",
                Status = "Connected",
                ConnectedAt = DateTime.UtcNow,
                Scope = "read_write"
            },
            new ProviderConnectionDto
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                ProviderName = "paypal",
                ProviderAccountId = "paypal_account_123",
                Status = "Connected",
                ConnectedAt = DateTime.UtcNow,
                Scope = "openid email profile"
            }
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectionServiceMock.Setup(x => x.GetConnectionsAsync(tenantId))
            .ReturnsAsync(connections);

        // Act
        var result = await _controller.GetConnections();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<ProviderConnectionDto>>>();
        var actionResult = result as ActionResult<IEnumerable<ProviderConnectionDto>>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var returnedConnections = okResult!.Value as IEnumerable<ProviderConnectionDto>;
        returnedConnections.Should().NotBeNull();
        returnedConnections!.Count().Should().Be(2);
        returnedConnections.Should().Contain(c => c.ProviderName == "stripe");
        returnedConnections.Should().Contain(c => c.ProviderName == "paypal");
    }

    [Fact]
    public async Task GetConnections_WithEmptyConnections_Should_ReturnEmptyList()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var emptyConnections = new List<ProviderConnectionDto>();

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectionServiceMock.Setup(x => x.GetConnectionsAsync(tenantId))
            .ReturnsAsync(emptyConnections);

        // Act
        var result = await _controller.GetConnections();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<ProviderConnectionDto>>>();
        var actionResult = result as ActionResult<IEnumerable<ProviderConnectionDto>>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var returnedConnections = okResult!.Value as IEnumerable<ProviderConnectionDto>;
        returnedConnections.Should().NotBeNull();
        returnedConnections!.Count().Should().Be(0);
    }

    [Fact]
    public async Task DisconnectProvider_WithValidProvider_Should_ReturnNoContent()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectionServiceMock.Setup(x => x.DisconnectAsync(tenantId, provider))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DisconnectProvider(provider);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DisconnectProvider_WithNonExistentConnection_Should_ReturnNotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectionServiceMock.Setup(x => x.DisconnectAsync(tenantId, provider))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DisconnectProvider(provider);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;

        var errorResponse = notFoundResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("No stripe connection found");
    }

    [Theory]
    [InlineData("stripe")]
    [InlineData("paypal")]
    [InlineData("STRIPE")]
    [InlineData("PAYPAL")]
    public async Task DisconnectProvider_WithDifferentProviders_Should_Work(string provider)
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectionServiceMock.Setup(x => x.DisconnectAsync(tenantId, provider))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DisconnectProvider(provider);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    // ===== NEW OAUTH FROM UI TESTS =====

    [Fact]
    public async Task HandleOAuthCallbackFromUI_WithValidStripeCallback_Should_ReturnSuccess()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new OAuthCallbackRequest
        {
            Provider = "stripe",
            Code = "test_auth_code",
            State = "test_state"
        };

        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCodeAsync(request.Code, request.State))
            .ReturnsAsync(tokenResponse);
        _connectionServiceMock.Setup(x => x.SaveConnectionAsync(tenantId, request.Provider, tokenResponse))
            .ReturnsAsync(new ProviderConnectionDto());

        // Act
        var result = await _controller.HandleOAuthCallbackFromUI(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var jsonString = JsonSerializer.Serialize(okResult!.Value);
        var responseDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
        responseDict!["success"].GetBoolean().Should().BeTrue();
        responseDict["message"].GetString().Should().Contain("stripe connected successfully");
        responseDict["providerAccountId"].GetString().Should().Be("acct_test123");
    }

    [Fact]
    public async Task HandleOAuthCallbackFromUI_WithValidPayPalCallback_Should_ReturnSuccess()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new OAuthCallbackRequest
        {
            Provider = "paypal",
            Code = "test_auth_code",
            State = "test_state"
        };

        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "paypal_account_123"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.PayPal)).Returns(_payPalConnectorMock.Object);
        _payPalConnectorMock.Setup(x => x.ExchangeOAuthCodeAsync(request.Code, request.State))
            .ReturnsAsync(tokenResponse);
        _connectionServiceMock.Setup(x => x.SaveConnectionAsync(tenantId, request.Provider, tokenResponse))
            .ReturnsAsync(new ProviderConnectionDto());

        // Act
        var result = await _controller.HandleOAuthCallbackFromUI(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var jsonString = JsonSerializer.Serialize(okResult!.Value);
        var responseDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
        responseDict!["success"].GetBoolean().Should().BeTrue();
        responseDict["message"].GetString().Should().Contain("paypal connected successfully");
        responseDict["providerAccountId"].GetString().Should().Be("paypal_account_123");
    }

    [Fact]
    public async Task HandleOAuthCallbackFromUI_WithInvalidProvider_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new OAuthCallbackRequest
        {
            Provider = "invalid-provider",
            Code = "test_auth_code",
            State = "test_state"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);

        // Act
        var result = await _controller.HandleOAuthCallbackFromUI(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("Unsupported provider");
    }

    [Fact]
    public async Task HandleOAuthCallbackFromUI_WithEmptyTenantId_Should_ReturnBadRequest()
    {
        // Arrange
        var request = new OAuthCallbackRequest
        {
            Provider = "stripe",
            Code = "test_auth_code",
            State = "test_state"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(Guid.Empty);

        // Act
        var result = await _controller.HandleOAuthCallbackFromUI(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("Tenant context not found");
    }

    [Fact]
    public async Task HandleOAuthCallbackFromUI_WithFailedTokenExchange_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new OAuthCallbackRequest
        {
            Provider = "stripe",
            Code = "test_auth_code",
            State = "test_state"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCodeAsync(request.Code, request.State))
            .ReturnsAsync((OAuthTokenResponse)null);

        // Act
        var result = await _controller.HandleOAuthCallbackFromUI(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("Failed to exchange authorization code");
    }

    [Fact]
    public async Task HandleOAuthCallbackFromUI_WithEmptyAccessToken_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new OAuthCallbackRequest
        {
            Provider = "stripe",
            Code = "test_auth_code",
            State = "test_state"
        };

        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(tenantId);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCodeAsync(request.Code, request.State))
            .ReturnsAsync(tokenResponse);

        // Act
        var result = await _controller.HandleOAuthCallbackFromUI(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("Failed to exchange authorization code");
    }

    [Fact]
    public async Task InitiateConnection_WithEmptyTenantId_Should_ReturnBadRequest()
    {
        // Arrange
        var provider = "stripe";

        _tenantContextMock.Setup(x => x.TenantId).Returns(Guid.Empty);

        // Act
        var result = await _controller.InitiateConnection(provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        var errorResponse = badRequestResult!.Value as ErrorResponseDto;
        errorResponse!.Error.Should().Contain("Tenant context not found");
    }
}
