using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Api.Controllers;
using Xunit;

namespace SubscriptionAnalytics.Api.Tests;

public class ConnectControllerTests
{
    private readonly Mock<IConnectorFactory> _connectorFactoryMock;
    private readonly Mock<ILogger<ConnectController>> _loggerMock;
    private readonly Mock<IConnector> _stripeConnectorMock;
    private readonly Mock<IConnector> _payPalConnectorMock;
    private readonly ConnectController _controller;

    public ConnectControllerTests()
    {
        _connectorFactoryMock = new Mock<IConnectorFactory>();
        _loggerMock = new Mock<ILogger<ConnectController>>();
        _stripeConnectorMock = new Mock<IConnector>();
        _payPalConnectorMock = new Mock<IConnector>();

        // Setup default connector mocks
        _stripeConnectorMock.Setup(x => x.ProviderName).Returns("Stripe");
        _stripeConnectorMock.Setup(x => x.DisplayName).Returns("Stripe");
        _stripeConnectorMock.Setup(x => x.SupportsOAuth).Returns(true);

        _payPalConnectorMock.Setup(x => x.ProviderName).Returns("PayPal");
        _payPalConnectorMock.Setup(x => x.DisplayName).Returns("PayPal");
        _payPalConnectorMock.Setup(x => x.SupportsOAuth).Returns(true);

        _controller = new ConnectController(_connectorFactoryMock.Object, _loggerMock.Object);
        
        // Setup HTTP context
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost", 5069);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public void Constructor_WithValidDependencies_Should_CreateController()
    {
        // Act & Assert
        _controller.Should().NotBeNull();
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
        providers.Should().HaveCount(2);
        providers.Should().Contain(p => p.ProviderName == "Stripe" && p.DisplayName == "Stripe" && p.SupportsOAuth);
        providers.Should().Contain(p => p.ProviderName == "PayPal" && p.DisplayName == "PayPal" && p.SupportsOAuth);
    }

    [Fact]
    public void GetAvailableProviders_WhenFactoryThrowsException_Should_Return500()
    {
        // Arrange
        _connectorFactoryMock.Setup(x => x.GetAllConnectors()).Throws(new Exception("Test exception"));

        // Act
        var result = _controller.GetAvailableProviders();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<ConnectorInfo>>>();
        var actionResult = result as ActionResult<IEnumerable<ConnectorInfo>>;
        actionResult!.Result.Should().BeOfType<ObjectResult>();
        var objectResult = actionResult.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task InitiateConnection_WithValidStripeProvider_Should_ReturnAuthorizationUrl()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";
        var expectedUrl = "https://connect.stripe.com/oauth/authorize?response_type=code&client_id=test&scope=read_write&redirect_uri=https%3A%2F%2Flocalhost%3A5069%2Fapi%2Fconnect%2Ftenant%2F" + tenantId + "%2Fprovider%2Fstripe%2Foauth-callback&state=";

        _connectorFactoryMock.Setup(x => x.SupportsConnector(ConnectorType.Stripe)).Returns(true);
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
        var expectedUrl = "https://www.paypal.com/connect?flowentry=static&client_id=test&scope=openid%20email%20profile%20https%3A%2F%2Furi.paypal.com%2Fservices%2Fpaypalattributes&redirect_uri=https%3A%2F%2Flocalhost%3A5069%2Fapi%2Fconnect%2Ftenant%2F" + tenantId + "%2Fprovider%2Fpaypal%2Foauth-callback&state=";

        _connectorFactoryMock.Setup(x => x.SupportsConnector(ConnectorType.PayPal)).Returns(true);
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
        var error = badRequestResult!.Value as dynamic;
        ((string)error!.error).Should().Contain("Unsupported provider");
    }

    [Fact]
    public async Task InitiateConnection_WithUnsupportedConnector_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _connectorFactoryMock.Setup(x => x.SupportsConnector(ConnectorType.Stripe)).Returns(false);

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        var error = badRequestResult!.Value as dynamic;
        ((string)error!.error).Should().Contain("is not supported");
    }

    [Fact]
    public async Task InitiateConnection_WithConnectorNotSupportingOAuth_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _connectorFactoryMock.Setup(x => x.SupportsConnector(ConnectorType.Stripe)).Returns(true);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.SupportsOAuth).Returns(false);

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        var error = badRequestResult!.Value as dynamic;
        ((string)error!.error).Should().Contain("does not support OAuth");
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
            ExpiresIn = 3600
        };

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCodeAsync(code, state)).ReturnsAsync(tokenResponse);

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, code, state);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value as dynamic;
        ((string)response!.message).Should().Contain("connected successfully");
        ((string)response.provider).Should().Be(provider);
        ((string)response.accessToken).Should().Be("test_access_token");
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
        var response = badRequestResult!.Value as dynamic;
        ((string)response!.error).Should().Be(error);
        ((string)response.description).Should().Be(errorDescription);
    }

    [Fact]
    public async Task HandleOAuthCallback_WithMissingParameters_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, provider, "", "");

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var error = badRequestResult!.Value as dynamic;
        ((string)error!.error).Should().Be("Missing required OAuth parameters");
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
        var error = badRequestResult!.Value as dynamic;
        ((string)error!.error).Should().Contain("Unsupported provider");
    }

    [Fact]
    public async Task DisconnectProvider_WithValidStripeProvider_Should_ReturnNoContent()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.DisconnectAsync(tenantId)).ReturnsAsync(true);

        // Act
        var result = await _controller.DisconnectProvider(tenantId, provider);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DisconnectProvider_WithInvalidProvider_Should_ReturnBadRequest()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "invalid_provider";

        // Act
        var result = await _controller.DisconnectProvider(tenantId, provider);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var error = badRequestResult!.Value as dynamic;
        ((string)error!.error).Should().Contain("Unsupported provider");
    }

    [Fact]
    public async Task DisconnectProvider_WhenDisconnectFails_Should_ReturnNotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.DisconnectAsync(tenantId)).ReturnsAsync(false);

        // Act
        var result = await _controller.DisconnectProvider(tenantId, provider);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        var error = notFoundResult!.Value as dynamic;
        ((string)error!.error).Should().Contain("No Stripe connection found");
    }

    [Fact]
    public async Task DisconnectProvider_WhenExceptionOccurs_Should_Return500()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Throws(new Exception("Test exception"));

        // Act
        var result = await _controller.DisconnectProvider(tenantId, provider);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
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

        _connectorFactoryMock.Setup(x => x.SupportsConnector(expectedType)).Returns(true);
        _connectorFactoryMock.Setup(x => x.GetConnector(expectedType)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ReturnsAsync(expectedUrl);

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task InitiateConnection_WhenConnectorThrowsException_Should_Return500()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var provider = "stripe";

        _connectorFactoryMock.Setup(x => x.SupportsConnector(ConnectorType.Stripe)).Returns(true);
        _connectorFactoryMock.Setup(x => x.GetConnector(ConnectorType.Stripe)).Returns(_stripeConnectorMock.Object);
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrlAsync(It.IsAny<string>(), It.IsAny<string>(), tenantId))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.InitiateConnection(tenantId, provider);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateConnectionResponse>;
        actionResult!.Result.Should().BeOfType<ObjectResult>();
        var objectResult = actionResult.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
    }
} 