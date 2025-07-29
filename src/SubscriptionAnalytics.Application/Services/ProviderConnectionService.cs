using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Entities;
using System.Text.Json;

namespace SubscriptionAnalytics.Application.Services;

public interface IProviderConnectionService
{
    Task<ProviderConnectionDto?> GetConnectionAsync(Guid tenantId, string providerName);
    Task<ProviderConnectionDto> SaveConnectionAsync(Guid tenantId, string providerName, OAuthTokenResponse tokenResponse);
    Task<bool> DisconnectAsync(Guid tenantId, string providerName);
    Task<IEnumerable<ProviderConnectionDto>> GetConnectionsAsync(Guid tenantId);
}

public class ProviderConnectionService : IProviderConnectionService
{
    private readonly AppDbContext _dbContext;
    private readonly IEncryptionService _encryptionService;
    private readonly ILogger<ProviderConnectionService> _logger;

    public ProviderConnectionService(
        AppDbContext dbContext,
        IEncryptionService encryptionService,
        ILogger<ProviderConnectionService> logger)
    {
        _dbContext = dbContext;
        _encryptionService = encryptionService;
        _logger = logger;
    }

    public async Task<ProviderConnectionDto?> GetConnectionAsync(Guid tenantId, string providerName)
    {
        var connection = await _dbContext.ProviderConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId &&
                                     c.ProviderName == providerName &&
                                     c.Status == "Connected");

        return connection != null ? MapToDto(connection) : null;
    }

    public async Task<ProviderConnectionDto> SaveConnectionAsync(Guid tenantId, string providerName, OAuthTokenResponse tokenResponse)
    {
        // Remove any existing connection for this tenant and provider
        var existingConnection = await _dbContext.ProviderConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.ProviderName == providerName);

        if (existingConnection != null)
        {
            _dbContext.ProviderConnections.Remove(existingConnection);
        }

        // Calculate token expiration
        DateTime? tokenExpiresAt = null;
        if (tokenResponse.ExpiresIn.HasValue)
        {
            tokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn.Value);
        }

        // Create new connection
        var connection = new ProviderConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProviderName = providerName,
            ProviderAccountId = tokenResponse.ProviderAccountId ?? string.Empty,
            AccessToken = _encryptionService.Encrypt(tokenResponse.AccessToken),
            RefreshToken = !string.IsNullOrEmpty(tokenResponse.RefreshToken)
                ? _encryptionService.Encrypt(tokenResponse.RefreshToken)
                : null,
            TokenType = tokenResponse.TokenType,
            ExpiresIn = tokenResponse.ExpiresIn,
            TokenExpiresAt = tokenExpiresAt,
            Scope = tokenResponse.Scope ?? string.Empty,
            ConnectedAt = DateTime.UtcNow,
            Status = "Connected",
            AdditionalData = tokenResponse.AdditionalData?.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.ToString() ?? string.Empty)
        };

        _dbContext.ProviderConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Saved {Provider} connection for tenant {TenantId}", providerName, tenantId);
        return MapToDto(connection);
    }

    public async Task<bool> DisconnectAsync(Guid tenantId, string providerName)
    {
        var connection = await _dbContext.ProviderConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.ProviderName == providerName);

        if (connection == null)
            return false;

        connection.Status = "Disconnected";
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Disconnected {Provider} for tenant {TenantId}", providerName, tenantId);
        return true;
    }

    public async Task<IEnumerable<ProviderConnectionDto>> GetConnectionsAsync(Guid tenantId)
    {
        var connections = await _dbContext.ProviderConnections
            .Where(c => c.TenantId == tenantId && c.Status == "Connected")
            .ToListAsync();

        return connections.Select(MapToDto);
    }

    private static ProviderConnectionDto MapToDto(ProviderConnection connection)
    {
        return new ProviderConnectionDto
        {
            Id = connection.Id,
            TenantId = connection.TenantId,
            ProviderName = connection.ProviderName,
            ProviderAccountId = connection.ProviderAccountId,
            Status = connection.Status,
            ConnectedAt = connection.ConnectedAt,
            LastSyncedAt = connection.LastSyncedAt,
            TokenExpiresAt = connection.TokenExpiresAt,
            Scope = connection.Scope
        };
    }
}
