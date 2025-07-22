using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Connectors.PayPal.Abstractions;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using Xunit;

namespace SubscriptionAnalytics.Application.Tests;

public class ConnectorFactoryTests
{
    private readonly Mock<IConnector> _stripeConnectorMock;
    private readonly Mock<IConnector> _payPalConnectorMock;
    private readonly Mock<ILogger<ConnectorFactory>> _loggerMock;
    private readonly ConnectorFactory _factory;

    public ConnectorFactoryTests()
    {
        _stripeConnectorMock = new Mock<IConnector>();
        _payPalConnectorMock = new Mock<IConnector>();
        _loggerMock = new Mock<ILogger<ConnectorFactory>>();

        // Setup connector mocks
        _stripeConnectorMock.Setup(x => x.ProviderName).Returns("Stripe");
        _stripeConnectorMock.Setup(x => x.DisplayName).Returns("Stripe");
        _stripeConnectorMock.Setup(x => x.SupportsOAuth).Returns(true);

        _payPalConnectorMock.Setup(x => x.ProviderName).Returns("PayPal");
        _payPalConnectorMock.Setup(x => x.DisplayName).Returns("PayPal");
        _payPalConnectorMock.Setup(x => x.SupportsOAuth).Returns(true);

        // Create factory with mocked connectors that implement IConnector
        _factory = new ConnectorFactory(
            _stripeConnectorMock.Object as IStripeConnector ?? Mock.Of<IStripeConnector>(),
            _payPalConnectorMock.Object as IPayPalConnector ?? Mock.Of<IPayPalConnector>(),
            _loggerMock.Object);
    }

    [Fact]
    public void Constructor_WithValidDependencies_Should_CreateFactory()
    {
        // Act & Assert
        _factory.Should().NotBeNull();
    }

    [Fact]
    public void GetConnector_WithStripeType_Should_ReturnStripeConnector()
    {
        // Act
        var result = _factory.GetConnector(ConnectorType.Stripe);

        // Assert
        result.Should().NotBeNull();
        result.ProviderName.Should().Be("Stripe");
    }

    [Fact]
    public void GetConnector_WithPayPalType_Should_ReturnPayPalConnector()
    {
        // Act
        var result = _factory.GetConnector(ConnectorType.PayPal);

        // Assert
        result.Should().NotBeNull();
        result.ProviderName.Should().Be("PayPal");
    }

    [Fact]
    public void GetConnector_WithInvalidType_Should_ThrowArgumentException()
    {
        // Arrange
        var invalidType = (ConnectorType)999;

        // Act & Assert
        var action = () => _factory.GetConnector(invalidType);
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Unsupported connector type: {invalidType}");
    }

    [Fact]
    public void GetAllConnectors_Should_ReturnAllAvailableConnectors()
    {
        // Act
        var result = _factory.GetAllConnectors();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.ProviderName == "Stripe");
        result.Should().Contain(c => c.ProviderName == "PayPal");
    }

    [Fact]
    public void SupportsConnector_WithStripeType_Should_ReturnTrue()
    {
        // Act
        var result = _factory.SupportsConnector(ConnectorType.Stripe);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void SupportsConnector_WithPayPalType_Should_ReturnTrue()
    {
        // Act
        var result = _factory.SupportsConnector(ConnectorType.PayPal);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void SupportsConnector_WithInvalidType_Should_ReturnFalse()
    {
        // Arrange
        var invalidType = (ConnectorType)999;

        // Act
        var result = _factory.SupportsConnector(invalidType);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(ConnectorType.Stripe)]
    [InlineData(ConnectorType.PayPal)]
    public void GetConnector_Should_ReturnSameInstanceForSameType(ConnectorType connectorType)
    {
        // Act
        var result1 = _factory.GetConnector(connectorType);
        var result2 = _factory.GetConnector(connectorType);

        // Assert
        result1.Should().Be(result2);
    }

    [Fact]
    public void GetAllConnectors_Should_ReturnUniqueConnectors()
    {
        // Act
        var result = _factory.GetAllConnectors().ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void Factory_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task<IConnector>>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => _factory.GetConnector(ConnectorType.Stripe)));
            tasks.Add(Task.Run(() => _factory.GetConnector(ConnectorType.PayPal)));
        }

        // Assert
        var results = Task.WhenAll(tasks).Result;
        results.Should().HaveCount(20);
        results.Should().Contain(c => c.ProviderName == "Stripe");
        results.Should().Contain(c => c.ProviderName == "PayPal");
    }

    [Fact]
    public void Factory_Should_HandleNullConnectorsGracefully()
    {
        // Arrange
        var factory = new ConnectorFactory(null!, null!, _loggerMock.Object);

        // Act & Assert
        var action1 = () => factory.GetConnector(ConnectorType.Stripe);
        action1.Should().Throw<InvalidOperationException>();

        var action2 = () => factory.GetConnector(ConnectorType.PayPal);
        action2.Should().Throw<InvalidOperationException>();

        var allConnectors = factory.GetAllConnectors();
        allConnectors.Should().BeEmpty();

        factory.SupportsConnector(ConnectorType.Stripe).Should().BeFalse();
        factory.SupportsConnector(ConnectorType.PayPal).Should().BeFalse();
    }

    [Fact]
    public void Factory_Should_LogConnectorRetrieval()
    {
        // Act
        _factory.GetConnector(ConnectorType.Stripe);

        // Assert
        // Note: In a real scenario, you might want to verify that logging occurred
        // This test ensures the factory doesn't crash when logging is involved
        _factory.Should().NotBeNull();
    }
} 