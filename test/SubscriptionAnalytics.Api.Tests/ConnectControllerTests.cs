using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Api.Controllers;
using System.Text.Json;
using Xunit;

namespace SubscriptionAnalytics.Api.Tests;

public class ConnectControllerTests
{
    private readonly Mock<IConnectorFactory> _connectorFactoryMock;
    private readonly Mock<IProviderConnectionService> _connectionServiceMock;
    private readonly Mock<ILogger<ConnectController>> _loggerMock;
    private readonly Mock<IConnector> _stripeConnectorMock;
    private readonly Mock<IConnector> _payPalConnectorMock;
    private readonly ConnectController _controller;

    public ConnectControllerTests()
    {
        _connectorFactoryMock = new Mock<IConnectorFactory>();
        _connectionServiceMock = new Mock<IProviderConnectionService>();
        _loggerMock = new Mock<ILogger<ConnectController>>();
        _stripeConnectorMock = new Mock<IConnector>();
        _payPalConnectorMock = new Mock<IConnector>();

        _controller = new ConnectController(
            _connectorFactoryMock.Object,
            _connectionServiceMock.Object,
            _loggerMock.Object);

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
        _controller.Should().BeOfType<ConnectController>();
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
        var expectedUrl = "https://connect.stripe.com/oauth/authorize?response_type=code&client_id=test&scope=read_write&redirect_uri=https%3A%2F%2Flocalhost%3A7001%2Fapi%2Fconnect%2Ftenant%2F" + tenantId + "%2Fprovider%2Fstripe%2Foauth-callback&state=";

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

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
        var expectedUrl = "https://www.paypal.com/connect?flowentry=static&client_id=test&scope=openid%20email%20profile%20https%3A%2F%2Furi.paypal.com%2Fservices%2Fpaypalattributes&redirect_uri=https%3A%2F%2Flocalhost%3A7001%2Fapi%2Fconnect%2Ftenant%2F" + tenantId + "%2Fprovider%2Fpaypal%2Foauth-callback&state=";

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.PayPal)).Returns(_payPalConnectorMock.Object);
        _payPalConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

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

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        
        var jsonString = JsonSerializer.Serialize(badRequestResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Contain("Unsupported provider");
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

        _connectorFactoryMock.Setup(x => x.GetConnector(expectedType)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

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
        
        var jsonString = JsonSerializer.Serialize(okResult!.Value);
        var responseDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
        responseDict!["message"].GetString().Should().Contain("stripe account connected successfully");
        responseDict["provider"].GetString().Should().Be(provider);
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
        
        var jsonString = JsonSerializer.Serialize(okResult!.Value);
        var responseDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
        responseDict!["message"].GetString().Should().Contain("paypal account connected successfully");
        responseDict["provider"].GetString().Should().Be(provider);
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
        
        var jsonString = JsonSerializer.Serialize(badRequestResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Be(error);
        errorResponse["description"].Should().Be(errorDescription);
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
        
        var jsonString = JsonSerializer.Serialize(badRequestResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Be(error);
        errorResponse["description"].Should().Be("OAuth authorization failed");
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
        
        var jsonString = JsonSerializer.Serialize(badRequestResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Be("Missing required OAuth parameters");
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
        
        var jsonString = JsonSerializer.Serialize(badRequestResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Be("Missing required OAuth parameters");
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
        
        var jsonString = JsonSerializer.Serialize(badRequestResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Contain("Unsupported provider");
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

        _connectionServiceMock.Setup(x => x.GetConnectionAsync(tenantId, provider))
            .ReturnsAsync(connectionDto);

        // Act
        var result = await _controller.GetConnection(tenantId, provider);

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

        _connectionServiceMock.Setup(x => x.GetConnectionAsync(tenantId, provider))
            .ReturnsAsync((ProviderConnectionDto?)null);

        // Act
        var result = await _controller.GetConnection(tenantId, provider);

        // Assert
        result.Should().BeOfType<ActionResult<ProviderConnectionDto>>();
        var actionResult = result as ActionResult<ProviderConnectionDto>;
        actionResult!.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        
        var jsonString = JsonSerializer.Serialize(notFoundResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Contain("No stripe connection found");
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

        _connectionServiceMock.Setup(x => x.GetConnectionsAsync(tenantId))
            .ReturnsAsync(connections);

        // Act
        var result = await _controller.GetConnections(tenantId);

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

        _connectionServiceMock.Setup(x => x.GetConnectionsAsync(tenantId))
            .ReturnsAsync(emptyConnections);

        // Act
        var result = await _controller.GetConnections(tenantId);

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

        _connectionServiceMock.Setup(x => x.DisconnectAsync(tenantId, provider))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DisconnectProvider(tenantId, provider);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DisconnectProvider_WithNonExistentConnection_Should_ReturnNotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _connectionServiceMock.Setup(x => x.DisconnectAsync(tenantId, provider))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DisconnectProvider(tenantId, provider);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        
        var jsonString = JsonSerializer.Serialize(notFoundResult!.Value);
        var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        errorResponse!["error"].Should().Contain("No stripe connection found");
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

        _connectionServiceMock.Setup(x => x.DisconnectAsync(tenantId, provider))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DisconnectProvider(tenantId, provider);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
} 