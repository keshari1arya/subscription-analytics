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
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ConnectController> _logger;

    public ConnectController(
        IConnectorFactory connectorFactory,
        IProviderConnectionService connectionService,
        ITenantContext tenantContext,
        ILogger<ConnectController> logger)
    {
        _connectorFactory = connectorFactory;
        _connectionService = connectionService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    /// <summary>
    /// Gets all available payment providers
    /// </summary>
    [HttpGet("providers")]
    public ActionResult<IEnumerable<ConnectorInfo>> GetAvailableProviders()
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAvailableProviders");
            return StatusCode(500, new ErrorResponseDto("Internal server error"));
        }
    }

    /// <summary>
    /// Initiates OAuth flow for a payment provider
    /// </summary>
    [HttpPost("provider/{provider}")]
    public async Task<ActionResult<InitiateConnectionResponse>> InitiateConnection(
        [FromRoute] string provider)
    {
        // Add null check and debugging
        if (_tenantContext == null)
        {
            _logger.LogError("TenantContext is null in ConnectController");
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        _logger.LogDebug("Tenant ID from context: {TenantId}", tenantId);

        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found. Please provide X-Tenant-Id header."));
        }

        if (!Enum.TryParse<ConnectorType>(provider, true, out var connectorType))
        {
            return BadRequest(new ErrorResponseDto($"Unsupported provider: {provider}"));
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

    // TODO: Remove AllowAnonymous and add Authorize once we have a way to secure redirect urls 
    // TODO: Add a way to validate the tenantId in the callback url
    // TODO: or move the callback url to a different controller
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

            return BadRequest(new ErrorResponseDto(error));
        }

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
        {
            return BadRequest(new ErrorResponseDto("Missing required OAuth parameters"));
        }

        if (!Enum.TryParse<ConnectorType>(provider, true, out var connectorType))
        {
            return BadRequest(new ErrorResponseDto($"Unsupported provider: {provider}"));
        }

        var connector = _connectorFactory.GetConnector(connectorType);
        var tokenResponse = await connector.ExchangeOAuthCodeAsync(code, state);

        // Store the connection in the database
        var connection = await _connectionService.SaveConnectionAsync(tenantId, provider, tokenResponse);

        return Ok(new SuccessResponseDto(true, $"{provider} account connected successfully"));
    }

    /// <summary>
    /// Gets the current connection status for a provider
    /// </summary>
    [HttpGet("provider/{provider}")]
    public async Task<ActionResult<ProviderConnectionDto>> GetConnection([FromRoute] string provider)
    {
        if (_tenantContext == null)
        {
            _logger.LogError("TenantContext is null in ConnectController");
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;

        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found. Please provide X-Tenant-Id header."));
        }

        var connection = await _connectionService.GetConnectionAsync(tenantId, provider);

        if (connection == null)
        {
            return NotFound(new ErrorResponseDto($"No {provider} connection found for this tenant"));
        }

        return Ok(connection);
    }

    /// <summary>
    /// Gets all connections for a tenant
    /// </summary>
    [HttpGet("connections")]
    public async Task<ActionResult<IEnumerable<ProviderConnectionDto>>> GetConnections()
    {
        if (_tenantContext == null)
        {
            _logger.LogError("TenantContext is null in ConnectController");
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;

        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found. Please provide X-Tenant-Id header."));
        }

        var connections = await _connectionService.GetConnectionsAsync(tenantId);
        return Ok(connections);
    }

    /// <summary>
    /// Disconnects a payment provider for a tenant
    /// </summary>
    [HttpDelete("provider/{provider}")]
    public async Task<IActionResult> DisconnectProvider([FromRoute] string provider)
    {
        if (_tenantContext == null)
        {
            _logger.LogError("TenantContext is null in ConnectController");
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;

        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found. Please provide X-Tenant-Id header."));
        }

        var success = await _connectionService.DisconnectAsync(tenantId, provider);

        if (!success)
        {
            return NotFound(new ErrorResponseDto($"No {provider} connection found for this tenant"));
        }

        return NoContent();
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