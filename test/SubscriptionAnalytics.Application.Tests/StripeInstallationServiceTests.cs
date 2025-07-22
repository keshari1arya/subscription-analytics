using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Application.Tests;

public class StripeInstallationServiceTests
{
    private readonly StripeInstallationService _service;
    private readonly Mock<IStripeConnector> _stripeConnectorMock;
    private readonly Mock<IEncryptionService> _encryptionServiceMock;
    private readonly AppDbContext _dbContext;

    public StripeInstallationServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new AppDbContext(options);

        _stripeConnectorMock = new Mock<IStripeConnector>();
        _encryptionServiceMock = new Mock<IEncryptionService>();

        _service = new StripeInstallationService(
            _stripeConnectorMock.Object,
            _dbContext,
            _encryptionServiceMock.Object);
    }

    [Fact]
    public void Constructor_Should_CreateService()
    {
        // Act & Assert
        _service.Should().NotBeNull();
        _service.Should().BeOfType<StripeInstallationService>();
    }

    #region InitiateConnection Tests

    [Fact]
    public async Task InitiateConnection_Should_ReturnValidResponse_WhenSuccessful()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        var expectedAuthUrl = "https://connect.stripe.com/oauth/authorize?client_id=test&state=test";
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(expectedAuthUrl);

        // Act
        var result = await _service.InitiateConnection(tenantId);

        // Assert
        result.Should().NotBeNull();
        result.AuthorizationUrl.Should().Be(expectedAuthUrl);
        result.State.Should().NotBeNullOrEmpty();
        result.State.Length.Should().BeGreaterThanOrEqualTo(20); // Base64 encoded 32 bytes
    }

    [Fact]
    public async Task InitiateConnection_Should_ThrowArgumentException_WhenTenantNotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act & Assert
        var act = async () => await _service.InitiateConnection(tenantId);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Tenant not found*")
            .WithParameterName("tenantId");
    }

    [Fact]
    public async Task InitiateConnection_Should_ThrowInvalidOperationException_WhenAlreadyConnected()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        var existingConnection = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Status = "Connected",
            StripeAccountId = "acct_test123"
        };

        _dbContext.Tenants.Add(tenant);
        _dbContext.StripeConnections.Add(existingConnection);
        await _dbContext.SaveChangesAsync();

        // Act & Assert
        var act = async () => await _service.InitiateConnection(tenantId);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Stripe is already connected*");
    }

    [Fact]
    public async Task InitiateConnection_Should_GenerateSecureStateToken()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("https://connect.stripe.com/oauth/authorize");

        // Act
        var result1 = await _service.InitiateConnection(tenantId);
        var result2 = await _service.InitiateConnection(tenantId);

        // Assert
        result1.State.Should().NotBe(result2.State); // Each call should generate unique state
        result1.State.Should().NotBeNullOrEmpty();
        result2.State.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task InitiateConnection_Should_BuildCorrectRedirectUri()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        string capturedRedirectUri = null!;
        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrl(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((state, redirectUri) => capturedRedirectUri = redirectUri)
            .ReturnsAsync("https://connect.stripe.com/oauth/authorize");

        // Act
        await _service.InitiateConnection(tenantId);

        // Assert
        capturedRedirectUri.Should().Be($"http://localhost:5069/api/stripe/tenant/{tenantId}/oauth-callback");
    }

    #endregion

    #region HandleOAuthCallback Tests

    [Fact]
    public async Task HandleOAuthCallback_Should_ReturnValidConnection_WhenSuccessful()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        var request = new StripeOAuthCallbackRequest
        {
            Code = "test_code",
            State = "test_state"
        };

        var tokenResponse = new StripeOAuthTokenResponse
        {
            AccessToken = "access_token_123",
            RefreshToken = "refresh_token_123",
            StripeAccountId = "acct_test123",
            TokenType = "bearer",
            Scope = "read_write"
        };

        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCode(request.Code))
            .ReturnsAsync(tokenResponse);
        _stripeConnectorMock.Setup(x => x.ValidateConnection(tokenResponse.AccessToken))
            .ReturnsAsync(true);

        _encryptionServiceMock.Setup(x => x.Encrypt(tokenResponse.AccessToken))
            .Returns("encrypted_access_token");
        _encryptionServiceMock.Setup(x => x.Encrypt(tokenResponse.RefreshToken))
            .Returns("encrypted_refresh_token");

        // Act
        var result = await _service.HandleOAuthCallback(tenantId, request);

        // Assert
        result.Should().NotBeNull();
        result.TenantId.Should().Be(tenantId);
        result.StripeAccountId.Should().Be("acct_test123");
        result.Status.Should().Be("Connected");
        result.ConnectedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_ThrowArgumentException_WhenTenantNotFound()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var request = new StripeOAuthCallbackRequest
        {
            Code = "test_code",
            State = "test_state"
        };

        // Act & Assert
        var act = async () => await _service.HandleOAuthCallback(tenantId, request);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Tenant not found*")
            .WithParameterName("tenantId");
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_ThrowInvalidOperationException_WhenValidationFails()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        var request = new StripeOAuthCallbackRequest
        {
            Code = "test_code",
            State = "test_state"
        };

        var tokenResponse = new StripeOAuthTokenResponse
        {
            AccessToken = "access_token_123",
            StripeAccountId = "acct_test123"
        };

        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCode(request.Code))
            .ReturnsAsync(tokenResponse);
        _stripeConnectorMock.Setup(x => x.ValidateConnection(tokenResponse.AccessToken))
            .ReturnsAsync(false);

        // Act & Assert
        var act = async () => await _service.HandleOAuthCallback(tenantId, request);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Failed to validate Stripe connection*");
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_RemoveExistingConnection_WhenPresent()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        var existingConnection = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Status = "Connected",
            StripeAccountId = "acct_old123"
        };

        _dbContext.Tenants.Add(tenant);
        _dbContext.StripeConnections.Add(existingConnection);
        await _dbContext.SaveChangesAsync();

        var request = new StripeOAuthCallbackRequest
        {
            Code = "test_code",
            State = "test_state"
        };

        var tokenResponse = new StripeOAuthTokenResponse
        {
            AccessToken = "access_token_123",
            StripeAccountId = "acct_new123"
        };

        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCode(request.Code))
            .ReturnsAsync(tokenResponse);
        _stripeConnectorMock.Setup(x => x.ValidateConnection(tokenResponse.AccessToken))
            .ReturnsAsync(true);

        _encryptionServiceMock.Setup(x => x.Encrypt(It.IsAny<string>()))
            .Returns("encrypted_token");

        // Act
        var result = await _service.HandleOAuthCallback(tenantId, request);

        // Assert
        result.StripeAccountId.Should().Be("acct_new123");
        _dbContext.StripeConnections.Count(c => c.TenantId == tenantId).Should().Be(1);
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_EncryptTokens()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        var request = new StripeOAuthCallbackRequest
        {
            Code = "test_code",
            State = "test_state"
        };

        var tokenResponse = new StripeOAuthTokenResponse
        {
            AccessToken = "access_token_123",
            RefreshToken = "refresh_token_123",
            StripeAccountId = "acct_test123"
        };

        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCode(request.Code))
            .ReturnsAsync(tokenResponse);
        _stripeConnectorMock.Setup(x => x.ValidateConnection(tokenResponse.AccessToken))
            .ReturnsAsync(true);

        _encryptionServiceMock.Setup(x => x.Encrypt(tokenResponse.AccessToken))
            .Returns("encrypted_access_token");
        _encryptionServiceMock.Setup(x => x.Encrypt(tokenResponse.RefreshToken))
            .Returns("encrypted_refresh_token");

        // Act
        await _service.HandleOAuthCallback(tenantId, request);

        // Assert
        _encryptionServiceMock.Verify(x => x.Encrypt(tokenResponse.AccessToken), Times.Once);
        _encryptionServiceMock.Verify(x => x.Encrypt(tokenResponse.RefreshToken), Times.Once);
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_HandleNullRefreshToken()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        var request = new StripeOAuthCallbackRequest
        {
            Code = "test_code",
            State = "test_state"
        };

        var tokenResponse = new StripeOAuthTokenResponse
        {
            AccessToken = "access_token_123",
            RefreshToken = null,
            StripeAccountId = "acct_test123"
        };

        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCode(request.Code))
            .ReturnsAsync(tokenResponse);
        _stripeConnectorMock.Setup(x => x.ValidateConnection(tokenResponse.AccessToken))
            .ReturnsAsync(true);

        _encryptionServiceMock.Setup(x => x.Encrypt(It.IsAny<string>()))
            .Returns("encrypted_token");

        // Act
        var result = await _service.HandleOAuthCallback(tenantId, request);

        // Assert
        result.Should().NotBeNull();
        _encryptionServiceMock.Verify(x => x.Encrypt(""), Times.Once); // Empty string for null refresh token
    }

    #endregion

    #region GetConnection Tests

    [Fact]
    public async Task GetConnection_Should_ReturnConnection_WhenExists()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connection = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            StripeAccountId = "acct_test123",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow.AddDays(-1),
            LastSyncedAt = DateTime.UtcNow.AddHours(-2)
        };

        _dbContext.StripeConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetConnection(tenantId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(connection.Id);
        result.TenantId.Should().Be(tenantId);
        result.StripeAccountId.Should().Be("acct_test123");
        result.Status.Should().Be("Connected");
        result.ConnectedAt.Should().Be(connection.ConnectedAt);
        result.LastSyncedAt.Should().Be(connection.LastSyncedAt);
    }

    [Fact]
    public async Task GetConnection_Should_ReturnNull_WhenNoConnectionExists()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        var result = await _service.GetConnection(tenantId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetConnection_Should_ReturnNull_WhenConnectionNotConnected()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connection = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            StripeAccountId = "acct_test123",
            Status = "Disconnected",
            ConnectedAt = DateTime.UtcNow.AddDays(-1)
        };

        _dbContext.StripeConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetConnection(tenantId);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region DisconnectStripe Tests

    [Fact]
    public async Task DisconnectStripe_Should_ReturnTrue_WhenConnectionExists()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connection = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            StripeAccountId = "acct_test123",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow.AddDays(-1)
        };

        _dbContext.StripeConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.DisconnectStripe(tenantId);

        // Assert
        result.Should().BeTrue();
        var updatedConnection = await _dbContext.StripeConnections.FindAsync(connection.Id);
        updatedConnection!.Status.Should().Be("Disconnected");
    }

    [Fact]
    public async Task DisconnectStripe_Should_ReturnFalse_WhenNoConnectionExists()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        var result = await _service.DisconnectStripe(tenantId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DisconnectStripe_Should_DisconnectFirstConnection_WhenMultipleExist()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connection1 = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Status = "Connected"
        };
        var connection2 = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Status = "Connected"
        };

        _dbContext.StripeConnections.AddRange(connection1, connection2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.DisconnectStripe(tenantId);

        // Assert
        result.Should().BeTrue();
        var connections = await _dbContext.StripeConnections
            .Where(c => c.TenantId == tenantId)
            .ToListAsync();
        // Only the first connection should be disconnected
        connections.Should().Contain(c => c.Status == "Disconnected");
        connections.Should().Contain(c => c.Status == "Connected");
    }

    #endregion

    #region Edge Cases and Additional Tests

    [Fact]
    public async Task InitiateConnection_Should_HandleInactiveTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = false };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("https://connect.stripe.com/oauth/authorize");

        // Act
        var result = await _service.InitiateConnection(tenantId);

        // Assert
        result.Should().NotBeNull();
        result.AuthorizationUrl.Should().Be("https://connect.stripe.com/oauth/authorize");
        result.State.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_HandleEmptyRefreshToken()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        var request = new StripeOAuthCallbackRequest
        {
            Code = "test_code",
            State = "test_state"
        };

        var tokenResponse = new StripeOAuthTokenResponse
        {
            AccessToken = "access_token_123",
            RefreshToken = "",
            StripeAccountId = "acct_test123"
        };

        _stripeConnectorMock.Setup(x => x.ExchangeOAuthCode(request.Code))
            .ReturnsAsync(tokenResponse);
        _stripeConnectorMock.Setup(x => x.ValidateConnection(tokenResponse.AccessToken))
            .ReturnsAsync(true);

        _encryptionServiceMock.Setup(x => x.Encrypt(It.IsAny<string>()))
            .Returns("encrypted_token");

        // Act
        var result = await _service.HandleOAuthCallback(tenantId, request);

        // Assert
        result.Should().NotBeNull();
        _encryptionServiceMock.Verify(x => x.Encrypt(""), Times.Once);
    }

    [Fact]
    public async Task GetConnection_Should_HandleMultipleConnections_ReturnFirstConnected()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connection1 = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Status = "Disconnected",
            ConnectedAt = DateTime.UtcNow.AddDays(-2)
        };
        var connection2 = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow.AddDays(-1)
        };

        _dbContext.StripeConnections.AddRange(connection1, connection2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetConnection(tenantId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(connection2.Id);
        result.Status.Should().Be("Connected");
    }

    [Fact]
    public async Task DisconnectStripe_Should_HandleAlreadyDisconnectedConnection()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connection = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Status = "Disconnected"
        };

        _dbContext.StripeConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.DisconnectStripe(tenantId);

        // Assert
        result.Should().BeTrue();
        var updatedConnection = await _dbContext.StripeConnections.FindAsync(connection.Id);
        updatedConnection!.Status.Should().Be("Disconnected");
    }

    [Fact]
    public async Task InitiateConnection_Should_GenerateUniqueStateTokens()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Test Tenant", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        _stripeConnectorMock.Setup(x => x.GenerateOAuthUrl(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("https://connect.stripe.com/oauth/authorize");

        // Act
        var results = new List<InitiateStripeConnectionResponse>();
        for (int i = 0; i < 10; i++)
        {
            var result = await _service.InitiateConnection(tenantId);
            results.Add(result);
        }

        // Assert
        var uniqueStates = results.Select(r => r.State).Distinct();
        uniqueStates.Count().Should().Be(10); // All states should be unique
    }

    #endregion

    public void Dispose()
    {
        _dbContext.Dispose();
    }
} 