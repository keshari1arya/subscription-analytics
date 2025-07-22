using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Application.Services;

public class StripeInstallationService : IStripeInstallationService
{
    private readonly IStripeConnector _stripeConnector;
    private readonly AppDbContext _dbContext;
    private readonly IEncryptionService _encryptionService;

    public StripeInstallationService(
        IStripeConnector stripeConnector,
        AppDbContext dbContext,
        IEncryptionService encryptionService)
    {
        _stripeConnector = stripeConnector;
        _dbContext = dbContext;
        _encryptionService = encryptionService;
    }

    public async Task<InitiateStripeConnectionResponse> InitiateConnection(Guid tenantId)
    {
        // Verify tenant exists
        var tenant = await _dbContext.Tenants.FindAsync(tenantId);
        if (tenant == null)
            throw new ArgumentException("Tenant not found", nameof(tenantId));

        // Check if already connected
        var existingConnection = await _dbContext.StripeConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.Status == "Connected");
        
        if (existingConnection != null)
            throw new InvalidOperationException("Stripe is already connected to this tenant");

        // Generate CSRF state token
        var state = GenerateSecureToken();
        
        // Build redirect URI
        var redirectUri = $"http://localhost:5069/api/stripe/tenant/{tenantId}/oauth-callback";
        
        // Generate OAuth URL
        var authUrl = await _stripeConnector.GenerateOAuthUrl(state, redirectUri);

        return new InitiateStripeConnectionResponse
        {
            AuthorizationUrl = authUrl,
            State = state
        };
    }

    public async Task<StripeConnectionDto> HandleOAuthCallback(Guid tenantId, StripeOAuthCallbackRequest request)
    {
        // Verify tenant exists
        var tenant = await _dbContext.Tenants.FindAsync(tenantId);
        if (tenant == null)
            throw new ArgumentException("Tenant not found", nameof(tenantId));

        // TODO: Validate CSRF state token (store it temporarily during initiation)
        
        // Exchange authorization code for tokens
        var tokenResponse = await _stripeConnector.ExchangeOAuthCode(request.Code);
        
        // Validate the connection
        var isValid = await _stripeConnector.ValidateConnection(tokenResponse.AccessToken);
        if (!isValid)
            throw new InvalidOperationException("Failed to validate Stripe connection");

        // Remove any existing connection for this tenant
        var existingConnection = await _dbContext.StripeConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId);
        
        if (existingConnection != null)
        {
            _dbContext.StripeConnections.Remove(existingConnection);
        }

        // Create new connection
        var connection = new StripeConnection
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            StripeAccountId = tokenResponse.StripeAccountId,
            AccessToken = _encryptionService.Encrypt(tokenResponse.AccessToken),
            RefreshToken = _encryptionService.Encrypt(tokenResponse.RefreshToken ?? ""),
            ConnectedAt = DateTime.UtcNow,
            Status = "Connected"
        };

        _dbContext.StripeConnections.Add(connection);
        await _dbContext.SaveChangesAsync();

        return MapToDto(connection);
    }

    public async Task<StripeConnectionDto?> GetConnection(Guid tenantId)
    {
        var connection = await _dbContext.StripeConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.Status == "Connected");

        return connection != null ? MapToDto(connection) : null;
    }

    public async Task<bool> DisconnectStripe(Guid tenantId)
    {
        var connection = await _dbContext.StripeConnections
            .FirstOrDefaultAsync(c => c.TenantId == tenantId);

        if (connection == null)
            return false;

        connection.Status = "Disconnected";
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private string GenerateSecureToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

    private static StripeConnectionDto MapToDto(StripeConnection connection)
    {
        return new StripeConnectionDto
        {
            Id = connection.Id,
            TenantId = connection.TenantId,
            StripeAccountId = connection.StripeAccountId,
            Status = connection.Status,
            ConnectedAt = connection.ConnectedAt,
            LastSyncedAt = connection.LastSyncedAt
        };
    }
} 