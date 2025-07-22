using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Entities;
using Xunit;

namespace SubscriptionAnalytics.Application.Tests;

public class ProviderConnectionServiceTests : IDisposable
{
    private readonly Mock<IEncryptionService> _encryptionServiceMock;
    private readonly Mock<ILogger<ProviderConnectionService>> _loggerMock;
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly AppDbContext _dbContext;
    private readonly ProviderConnectionService _service;

    public ProviderConnectionServiceTests()
    {
        _encryptionServiceMock = new Mock<IEncryptionService>();
        _loggerMock = new Mock<ILogger<ProviderConnectionService>>();
        
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new AppDbContext(_dbContextOptions);
        _service = new ProviderConnectionService(_dbContext, _encryptionServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetConnectionAsync_WithExistingConnection_Should_ReturnConnection()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var connection = new ProviderConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = providerName,
            ProviderAccountId = "acct_test123",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow
        };

        _dbContext.ProviderConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetConnectionAsync(tenantId, providerName);

        // Assert
        result.Should().NotBeNull();
        result!.TenantId.Should().Be(tenantId);
        result.ProviderName.Should().Be(providerName);
        result.ProviderAccountId.Should().Be("acct_test123");
        result.Status.Should().Be("Connected");
    }

    [Fact]
    public async Task GetConnectionAsync_WithNonExistingConnection_Should_ReturnNull()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";

        // Act
        var result = await _service.GetConnectionAsync(tenantId, providerName);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetConnectionAsync_WithDisconnectedConnection_Should_ReturnNull()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var connection = new ProviderConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = providerName,
            ProviderAccountId = "acct_test123",
            Status = "Disconnected",
            ConnectedAt = DateTime.UtcNow
        };

        _dbContext.ProviderConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetConnectionAsync(tenantId, providerName);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task SaveConnectionAsync_WithNewConnection_Should_CreateAndReturnConnection()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123",
            Scope = "read_write"
        };

        _encryptionServiceMock.Setup(x => x.Encrypt("test_access_token"))
            .Returns("encrypted_access_token");

        // Act
        var result = await _service.SaveConnectionAsync(tenantId, providerName, tokenResponse);

        // Assert
        result.Should().NotBeNull();
        result.TenantId.Should().Be(tenantId);
        result.ProviderName.Should().Be(providerName);
        result.ProviderAccountId.Should().Be("acct_test123");
        result.Status.Should().Be("Connected");
        result.Scope.Should().Be("read_write");

        var savedConnection = await _dbContext.ProviderConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.ProviderName == providerName);
        savedConnection.Should().NotBeNull();
        savedConnection!.Status.Should().Be("Connected");
    }

    [Fact]
    public async Task SaveConnectionAsync_WithExistingConnection_Should_ReplaceAndReturnNewConnection()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var existingConnection = new ProviderConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = providerName,
            ProviderAccountId = "old_account",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow.AddDays(-1)
        };

        _dbContext.ProviderConnections.Add(existingConnection);
        await _dbContext.SaveChangesAsync();

        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "new_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "new_account",
            Scope = "read_write"
        };

        _encryptionServiceMock.Setup(x => x.Encrypt("new_access_token"))
            .Returns("encrypted_new_access_token");

        // Act
        var result = await _service.SaveConnectionAsync(tenantId, providerName, tokenResponse);

        // Assert
        result.Should().NotBeNull();
        result.ProviderAccountId.Should().Be("new_account");

        var connections = await _dbContext.ProviderConnections
            .Where(c => c.TenantId == tenantId && c.ProviderName == providerName)
            .ToListAsync();
        connections.Should().HaveCount(1);
        connections[0].ProviderAccountId.Should().Be("new_account");
    }

    [Fact]
    public async Task SaveConnectionAsync_WithRefreshToken_Should_EncryptAndSave()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            RefreshToken = "test_refresh_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123"
        };

        _encryptionServiceMock.Setup(x => x.Encrypt("test_access_token"))
            .Returns("encrypted_access_token");
        _encryptionServiceMock.Setup(x => x.Encrypt("test_refresh_token"))
            .Returns("encrypted_refresh_token");

        // Act
        var result = await _service.SaveConnectionAsync(tenantId, providerName, tokenResponse);

        // Assert
        result.Should().NotBeNull();
        _encryptionServiceMock.Verify(x => x.Encrypt("test_refresh_token"), Times.Once);
    }

    [Fact]
    public async Task SaveConnectionAsync_WithNullRefreshToken_Should_NotEncrypt()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            RefreshToken = null,
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123"
        };

        _encryptionServiceMock.Setup(x => x.Encrypt("test_access_token"))
            .Returns("encrypted_access_token");

        // Act
        var result = await _service.SaveConnectionAsync(tenantId, providerName, tokenResponse);

        // Assert
        result.Should().NotBeNull();
        _encryptionServiceMock.Verify(x => x.Encrypt(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SaveConnectionAsync_WithAdditionalData_Should_SaveCorrectly()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123",
            AdditionalData = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 123 }
            }
        };

        _encryptionServiceMock.Setup(x => x.Encrypt("test_access_token"))
            .Returns("encrypted_access_token");

        // Act
        var result = await _service.SaveConnectionAsync(tenantId, providerName, tokenResponse);

        // Assert
        result.Should().NotBeNull();
        var savedConnection = await _dbContext.ProviderConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.ProviderName == providerName);
        savedConnection.Should().NotBeNull();
        savedConnection!.AdditionalData.Should().ContainKey("key1");
        savedConnection.AdditionalData["key1"].Should().Be("value1");
        savedConnection.AdditionalData["key2"].Should().Be("123");
    }

    [Fact]
    public async Task SaveConnectionAsync_WithNullAdditionalData_Should_HandleGracefully()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var tokenResponse = new OAuthTokenResponse
        {
            AccessToken = "test_access_token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ProviderAccountId = "acct_test123",
            AdditionalData = null
        };

        _encryptionServiceMock.Setup(x => x.Encrypt("test_access_token"))
            .Returns("encrypted_access_token");

        // Act
        var result = await _service.SaveConnectionAsync(tenantId, providerName, tokenResponse);

        // Assert
        result.Should().NotBeNull();
        var savedConnection = await _dbContext.ProviderConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.ProviderName == providerName);
        savedConnection.Should().NotBeNull();
        savedConnection!.AdditionalData.Should().BeNull();
    }

    [Fact]
    public async Task DisconnectAsync_WithExistingConnection_Should_UpdateStatusAndReturnTrue()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";
        var connection = new ProviderConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = providerName,
            ProviderAccountId = "acct_test123",
            Status = "Connected",
            ConnectedAt = DateTime.UtcNow
        };

        _dbContext.ProviderConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.DisconnectAsync(tenantId, providerName);

        // Assert
        result.Should().BeTrue();
        var updatedConnection = await _dbContext.ProviderConnections
            .FirstOrDefaultAsync(c => c.Id == connection.Id);
        updatedConnection.Should().NotBeNull();
        updatedConnection!.Status.Should().Be("Disconnected");
    }

    [Fact]
    public async Task DisconnectAsync_WithNonExistingConnection_Should_ReturnFalse()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var providerName = "stripe";

        // Act
        var result = await _service.DisconnectAsync(tenantId, providerName);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetConnectionsAsync_WithConnectedConnections_Should_ReturnAllConnections()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connections = new List<ProviderConnection>
        {
            new ProviderConnection
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                ProviderName = "stripe",
                ProviderAccountId = "acct_stripe",
                Status = "Connected",
                ConnectedAt = DateTime.UtcNow
            },
            new ProviderConnection
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                ProviderName = "paypal",
                ProviderAccountId = "acct_paypal",
                Status = "Connected",
                ConnectedAt = DateTime.UtcNow
            },
            new ProviderConnection
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                ProviderName = "disconnected",
                ProviderAccountId = "acct_disconnected",
                Status = "Disconnected",
                ConnectedAt = DateTime.UtcNow
            }
        };

        _dbContext.ProviderConnections.AddRange(connections);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetConnectionsAsync(tenantId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.ProviderName == "stripe");
        result.Should().Contain(c => c.ProviderName == "paypal");
        result.Should().NotContain(c => c.ProviderName == "disconnected");
    }

    [Fact]
    public async Task GetConnectionsAsync_WithNoConnections_Should_ReturnEmptyList()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        var result = await _service.GetConnectionsAsync(tenantId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetConnectionsAsync_WithDifferentTenant_Should_NotReturnOtherTenantConnections()
    {
        // Arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        
        var connections = new List<ProviderConnection>
        {
            new ProviderConnection
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId1,
                ProviderName = "stripe",
                ProviderAccountId = "acct_stripe",
                Status = "Connected",
                ConnectedAt = DateTime.UtcNow
            },
            new ProviderConnection
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId2,
                ProviderName = "paypal",
                ProviderAccountId = "acct_paypal",
                Status = "Connected",
                ConnectedAt = DateTime.UtcNow
            }
        };

        _dbContext.ProviderConnections.AddRange(connections);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetConnectionsAsync(tenantId1);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(c => c.ProviderName == "stripe");
        result.Should().NotContain(c => c.ProviderName == "paypal");
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
} 