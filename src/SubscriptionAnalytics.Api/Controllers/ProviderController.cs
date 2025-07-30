using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Infrastructure.Services;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SubscriptionAnalytics.Api.Configuration;

namespace SubscriptionAnalytics.Api.Controllers;

[ApiController]
[Route("api/provider")]
// [Authorize] // Temporarily disabled for testing
public class ProviderController : ControllerBase
{
    private readonly IConnectorFactory _connectorFactory;
    private readonly IProviderConnectionService _connectionService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ProviderController> _logger;
    private readonly IOptions<OAuthConfiguration> _oauthConfig;
    private readonly ISyncJobService _syncJobService;

    public ProviderController(
        IConnectorFactory connectorFactory,
        IProviderConnectionService connectionService,
        ITenantContext tenantContext,
        ILogger<ProviderController> logger,
        IOptions<OAuthConfiguration> oauthConfig,
        ISyncJobService syncJobService)
    {
        _connectorFactory = connectorFactory;
        _connectionService = connectionService;
        _tenantContext = tenantContext;
        _logger = logger;
        _oauthConfig = oauthConfig;
        _syncJobService = syncJobService;
    }

    /// <summary>
    /// Gets all available payment providers
    /// </summary>
    [HttpGet()]
    [AllowAnonymous] // Allow anonymous access for testing
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
    [HttpPost("{provider}")]
    public async Task<ActionResult<InitiateConnectionResponse>> InitiateConnection(
        [FromRoute] string provider)
    {
        // Add null check and debugging
        if (_tenantContext == null)
        {
            _logger.LogError("TenantContext is null in ProviderController");
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
        if (connector == null)
        {
            return BadRequest(new ErrorResponseDto($"Provider {provider} not found"));
        }

        if (!connector.SupportsOAuth)
        {
            return BadRequest(new ErrorResponseDto($"Provider {provider} does not support OAuth"));
        }

        var state = Guid.NewGuid().ToString();
        var redirectUri = $"{_oauthConfig.Value.UiCallbackUrl}?provider={provider}";

        var oauthUrl = await connector.GenerateOAuthUrlAsync(state, redirectUri, tenantId);

        return Ok(new InitiateConnectionResponse
        {
            AuthorizationUrl = oauthUrl,
            State = state,
            Provider = provider
        });
    }

    /// <summary>
    /// Handles OAuth callback from UI
    /// </summary>
    [HttpPost("oauth-callback-from-ui")]
    public async Task<IActionResult> HandleOAuthCallbackFromUI(
        [FromBody] OAuthCallbackRequest request)
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        if (!Enum.TryParse<ConnectorType>(request.Provider, true, out var connectorType))
        {
            return BadRequest(new ErrorResponseDto($"Unsupported provider: {request.Provider}"));
        }

        var connector = _connectorFactory.GetConnector(connectorType);
        if (connector == null)
        {
            return BadRequest(new ErrorResponseDto($"Provider {request.Provider} not found"));
        }

        var tokenResponse = await connector.ExchangeOAuthCodeAsync(request.Code, request.State);
        var connection = await _connectionService.SaveConnectionAsync(tenantId, request.Provider, tokenResponse);

        return Ok(new ProviderConnectionDto
        {
            Id = connection.Id,
            ProviderName = connection.ProviderName,
            Status = connection.Status,
            ConnectedAt = connection.ConnectedAt,
            LastSyncedAt = connection.LastSyncedAt
        });
    }

    /// <summary>
    /// Handles OAuth callback from payment provider
    /// </summary>
    [HttpGet("tenant/{tenantId:guid}/{provider}/oauth-callback")]
    [AllowAnonymous] // Payment providers call this endpoint directly
    public async Task<IActionResult> HandleOAuthCallback(
        Guid tenantId,
        [FromRoute] string provider,
        [FromQuery] string code,
        [FromQuery] string state,
        [FromQuery] string? error = null,
        [FromQuery] string? error_description = null)
    {
        if (!string.IsNullOrEmpty(error))
        {
            _logger.LogWarning("OAuth error: {Error} - {Description}", error, error_description);
            return Redirect($"{_oauthConfig.Value.UiCallbackUrl}?error={error}&error_description={error_description}");
        }

        if (!Enum.TryParse<ConnectorType>(provider, true, out var connectorType))
        {
            return BadRequest(new ErrorResponseDto($"Unsupported provider: {provider}"));
        }

        var connector = _connectorFactory.GetConnector(connectorType);
        if (connector == null)
        {
            return BadRequest(new ErrorResponseDto($"Provider {provider} not found"));
        }

        var tokenResponse = await connector.ExchangeOAuthCodeAsync(code, state);
        var connection = await _connectionService.SaveConnectionAsync(tenantId, provider, tokenResponse);

        return Redirect($"{_oauthConfig.Value.UiCallbackUrl}?success=true&provider={provider}");
    }

    /// <summary>
    /// Gets connection status for a specific provider
    /// </summary>
    [HttpGet("{provider}/status")]
    public async Task<ActionResult<ProviderConnectionDto>> GetConnection([FromRoute] string provider)
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        var connection = await _connectionService.GetConnectionAsync(tenantId, provider);
        if (connection == null)
        {
            return NotFound(new ErrorResponseDto($"No connection found for provider {provider}"));
        }

        return Ok(connection);
    }

    /// <summary>
    /// Gets all connections for the current tenant
    /// </summary>
    [HttpGet("connections")]
    public async Task<ActionResult<IEnumerable<ProviderConnectionDto>>> GetConnections()
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        var connections = await _connectionService.GetConnectionsAsync(tenantId);
        return Ok(connections);
    }

    /// <summary>
    /// Disconnects a provider
    /// </summary>
    [HttpDelete("{provider}")]
    public async Task<IActionResult> DisconnectProvider([FromRoute] string provider)
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        if (!Enum.TryParse<ConnectorType>(provider, true, out var connectorType))
        {
            return BadRequest(new ErrorResponseDto($"Unsupported provider: {provider}"));
        }

        var connector = _connectorFactory.GetConnector(connectorType);
        if (connector == null)
        {
            return BadRequest(new ErrorResponseDto($"Provider {provider} not found"));
        }

        await connector.DisconnectAsync(tenantId);
        await _connectionService.DisconnectAsync(tenantId, provider);

        return Ok(new { message = $"Successfully disconnected {provider}" });
    }

    /// <summary>
    /// Initiates a sync job for a provider (just adds job to database)
    /// </summary>
    [HttpPost("{provider}/sync")]
    public async Task<ActionResult<SyncJobResponseDto>> SyncProvider([FromRoute] string provider)
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        // Check if provider is connected
        var connection = await _connectionService.GetConnectionAsync(tenantId, provider);
        if (connection == null)
        {
            return BadRequest(new ErrorResponseDto($"Provider {provider} is not connected"));
        }

        // For now, we'll pass null for access token since it's encrypted in the database
        // The worker will need to decrypt it when processing the job
        var syncJob = await _syncJobService.CreateJobAsync(
            tenantId: tenantId,
            providerName: provider,
            jobType: SyncJobType.FullSync,
            accessToken: null, // Will be retrieved from database by worker
            refreshToken: null, // Will be retrieved from database by worker
            additionalData: null // Will be retrieved from database by worker
        );

        _logger.LogInformation("Created sync job {JobId} for tenant {TenantId}, provider {Provider}",
            syncJob.Id, tenantId, provider);

        return Ok(new SyncJobResponseDto
        {
            JobId = syncJob.Id,
            Status = syncJob.Status.ToString(),
            Message = "Sync job created successfully. Worker will process it automatically."
        });
    }

    /// <summary>
    /// Gets sync job status
    /// </summary>
    [HttpGet("sync/status/{jobId:guid}")]
    public async Task<ActionResult<SyncJobStatusResponseDto>> GetSyncJobStatus([FromRoute] Guid jobId)
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        var syncJob = await _syncJobService.GetJobAsync(jobId);
        if (syncJob == null)
        {
            return NotFound(new ErrorResponseDto($"Sync job {jobId} not found"));
        }

        // Ensure the job belongs to the current tenant
        if (syncJob.TenantId != tenantId)
        {
            return Forbid();
        }

        return Ok(new SyncJobStatusResponseDto
        {
            JobId = syncJob.Id,
            Status = syncJob.Status.ToString(),
            Progress = syncJob.Progress,
            ErrorMessage = syncJob.ErrorMessage,
            StartedAt = syncJob.StartedAt,
            CompletedAt = syncJob.CompletedAt,
            RetryCount = syncJob.RetryCount
        });
    }

    /// <summary>
    /// Gets sync history for the current tenant
    /// </summary>
    [HttpGet("sync/history")]
    public async Task<ActionResult<IEnumerable<SyncJobHistoryResponseDto>>> GetSyncHistory()
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        var syncJobs = await _syncJobService.GetJobsByTenantAsync(tenantId);
        var history = syncJobs.Select(job => new SyncJobHistoryResponseDto
        {
            JobId = job.Id,
            ProviderName = job.ProviderName,
            JobType = job.JobType.ToString(),
            Status = job.Status.ToString(),
            Progress = job.Progress,
            ErrorMessage = job.ErrorMessage,
            StartedAt = job.StartedAt,
            CompletedAt = job.CompletedAt,
            RetryCount = job.RetryCount,
            CreatedAt = job.CreatedAt
        });

        return Ok(history);
    }

    /// <summary>
    /// Cancels a sync job
    /// </summary>
    [HttpDelete("sync/{jobId:guid}")]
    public async Task<ActionResult<SyncJobCancelResponseDto>> CancelSyncJob([FromRoute] Guid jobId)
    {
        if (_tenantContext == null)
        {
            return BadRequest(new ErrorResponseDto("Tenant context service is not available."));
        }

        var tenantId = _tenantContext.TenantId;
        if (tenantId == Guid.Empty)
        {
            return BadRequest(new ErrorResponseDto("Tenant context not found"));
        }

        var syncJob = await _syncJobService.GetJobAsync(jobId);
        if (syncJob == null)
        {
            return NotFound(new ErrorResponseDto($"Sync job {jobId} not found"));
        }

        // Ensure the job belongs to the current tenant
        if (syncJob.TenantId != tenantId)
        {
            return Forbid();
        }

        // Only allow cancellation of pending or running jobs
        if (syncJob.Status != SyncJobStatus.Pending && syncJob.Status != SyncJobStatus.Running)
        {
            return BadRequest(new ErrorResponseDto($"Cannot cancel job with status {syncJob.Status}"));
        }

        await _syncJobService.UpdateJobStatusAsync(jobId, SyncJobStatus.Cancelled, errorMessage: "Cancelled by user");

        return Ok(new SyncJobCancelResponseDto
        {
            JobId = jobId,
            Status = SyncJobStatus.Cancelled.ToString(),
            Message = "Sync job cancelled successfully"
        });
    }
}
