using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Api.Controllers;

[ApiController]
[Route("api/connect")]
[Authorize]
public class ConnectController : ControllerBase
{
    private readonly IConnectorFactory _connectorFactory;
    private readonly IProviderConnectionService _connectionService;
    private readonly ILogger<ConnectController> _logger;

    public ConnectController(
        IConnectorFactory connectorFactory,
        IProviderConnectionService connectionService,
        ILogger<ConnectController> logger)
    {
        _connectorFactory = connectorFactory;
        _connectionService = connectionService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all available payment providers
    /// </summary>
    [HttpGet("providers")]
    public ActionResult<IEnumerable<ConnectorInfo>> GetAvailableProviders()
    {
        var connectors = _connectorFactory.GetAllConnectors();
        var providers = connectors.Select(c => new ConnectorInfo
        {
            ProviderName = c.ProviderName,
            DisplayName = c.DisplayName,
            SupportsOAuth = c.SupportsOAuth
        });

        return Ok(providers);
    }

    /// <summary>
    /// Initiates OAuth flow for a payment provider
    /// </summary>
    [HttpPost("tenant/{tenantId:guid}/provider/{provider}")]
    public async Task<ActionResult<InitiateConnectionResponse>> InitiateConnection(
        Guid tenantId, 
        [FromRoute] string provider)
    {
        try
        {
            if (!Enum.TryParse<ConnectorType>(provider, true, out var connectorType))
            {
                return BadRequest(new { error = $"Unsupported provider: {provider}" });
            }

            var connector = _connectorFactory.GetConnector(connectorType);
            var state = Guid.NewGuid().ToString();
            var redirectUri = $"https://localhost:7001/api/connect/tenant/{tenantId}/provider/{provider}/oauth-callback";
            
            var authUrl = await connector.GenerateOAuthUrlAsync(state, redirectUri, tenantId);

            return Ok(new InitiateConnectionResponse
            {
                AuthorizationUrl = authUrl,
                State = state,
                Provider = provider
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating {Provider} connection for tenant {TenantId}", provider, tenantId);
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    /// <summary>
    /// Handles OAuth callback from payment providers
    /// </summary>
    [HttpGet("tenant/{tenantId:guid}/provider/{provider}/oauth-callback")]
    [AllowAnonymous] // Payment providers call this endpoint directly
    public async Task<IActionResult> HandleOAuthCallback(
        Guid tenantId,
        [FromRoute] string provider,
        [FromQuery] string code,
        [FromQuery] string state,
        [FromQuery] string? error = null,
        [FromQuery] string? error_description = null)
    {
        // Handle OAuth errors from providers
        if (!string.IsNullOrEmpty(error))
        {
            _logger.LogWarning("OAuth error for tenant {TenantId} with provider {Provider}: {Error} - {Description}", 
                tenantId, provider, error, error_description);
            
            return BadRequest(new { 
                error = error, 
                description = error_description ?? "OAuth authorization failed" 
            });
        }

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
        {
            return BadRequest(new { error = "Missing required OAuth parameters" });
        }

        try
        {
            if (!Enum.TryParse<ConnectorType>(provider, true, out var connectorType))
            {
                return BadRequest(new { error = $"Unsupported provider: {provider}" });
            }

            var connector = _connectorFactory.GetConnector(connectorType);
            var tokenResponse = await connector.ExchangeOAuthCodeAsync(code, state);
            
            // Store the connection in the database
            var connection = await _connectionService.SaveConnectionAsync(tenantId, provider, tokenResponse);
            
            return Ok(new { 
                message = $"{provider} account connected successfully",
                provider = provider,
                connectionId = connection.Id,
                providerAccountId = connection.ProviderAccountId,
                status = connection.Status
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid OAuth callback for tenant {TenantId} with provider {Provider}: {Message}", 
                tenantId, provider, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling OAuth callback for tenant {TenantId} with provider {Provider}", 
                tenantId, provider);
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    /// <summary>
    /// Gets the current connection status for a provider
    /// </summary>
    [HttpGet("tenant/{tenantId:guid}/provider/{provider}")]
    public async Task<ActionResult<ProviderConnectionDto>> GetConnection(Guid tenantId, [FromRoute] string provider)
    {
        try
        {
            var connection = await _connectionService.GetConnectionAsync(tenantId, provider);
            
            if (connection == null)
            {
                return NotFound(new { error = $"No {provider} connection found for this tenant" });
            }

            return Ok(connection);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting {Provider} connection for tenant {TenantId}", provider, tenantId);
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    /// <summary>
    /// Gets all connections for a tenant
    /// </summary>
    [HttpGet("tenant/{tenantId:guid}/connections")]
    public async Task<ActionResult<IEnumerable<ProviderConnectionDto>>> GetConnections(Guid tenantId)
    {
        try
        {
            var connections = await _connectionService.GetConnectionsAsync(tenantId);
            return Ok(connections);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting connections for tenant {TenantId}", tenantId);
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    /// <summary>
    /// Disconnects a payment provider for a tenant
    /// </summary>
    [HttpDelete("tenant/{tenantId:guid}/provider/{provider}")]
    public async Task<IActionResult> DisconnectProvider(Guid tenantId, [FromRoute] string provider)
    {
        try
        {
            var success = await _connectionService.DisconnectAsync(tenantId, provider);
            
            if (!success)
            {
                return NotFound(new { error = $"No {provider} connection found for this tenant" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disconnecting {Provider} for tenant {TenantId}", provider, tenantId);
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }
}

public class ConnectorInfo
{
    public string ProviderName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool SupportsOAuth { get; set; }
}

public class InitiateConnectionResponse
{
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
} 