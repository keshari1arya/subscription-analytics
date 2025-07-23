using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Api.Controllers;

// TODO: Refactor StripeController to a generic ConnectController or IntegrationsController 
//       to support multiple payment providers (Stripe, PayPal, Braintree) with loosely coupled architecture
// TODO: Create generic IPaymentConnector interface and ConnectorFactory pattern 
//       to dynamically handle different payment provider integrations

[ApiController]
[Route("api/stripe")]
[Authorize]
public class StripeController : ControllerBase
{
    private readonly IStripeInstallationService _stripeInstallationService;
    private readonly ILogger<StripeController> _logger;

    public StripeController(
        IStripeInstallationService stripeInstallationService,
        ILogger<StripeController> logger)
    {
        _stripeInstallationService = stripeInstallationService;
        _logger = logger;
    }

    /// <summary>
    /// Initiates Stripe Connect OAuth flow for a tenant
    /// </summary>
    [HttpPost("tenant/{tenantId:guid}/connect")]
    public async Task<ActionResult<InitiateStripeConnectionResponse>> InitiateConnection(Guid tenantId)
    {
        try
        {
            var result = await _stripeInstallationService.InitiateConnection(tenantId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Tenant not found for Stripe connection {TenantId}: {Message}", 
                tenantId, ex.Message);
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Failed to initiate Stripe connection for tenant {TenantId}: {Message}", 
                tenantId, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error initiating Stripe connection for tenant {TenantId}", tenantId);
            return StatusCode(500, new { error = "An unexpected error occurred" });
        }
    }

    /// <summary>
    /// Handles OAuth callback from Stripe Connect
    /// </summary>
    [HttpGet("tenant/{tenantId:guid}/oauth-callback")]
    [AllowAnonymous] // Stripe calls this endpoint directly
    public async Task<IActionResult> HandleOAuthCallback(
        Guid tenantId,
        [FromQuery] string code,
        [FromQuery] string state,
        [FromQuery] string? error = null,
        [FromQuery] string? error_description = null)
    {
        // Handle OAuth errors from Stripe
        if (!string.IsNullOrEmpty(error))
        {
            _logger.LogWarning("Stripe OAuth error for tenant {TenantId}: {Error} - {Description}", 
                tenantId, error, error_description);
            
            // In a real app, you'd redirect to a frontend error page
            return BadRequest(new ErrorResponseDto(error));
        }

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
        {
            return BadRequest(new ErrorResponseDto("Missing required OAuth parameters"));
        }

        try
        {
            var request = new StripeOAuthCallbackRequest
            {
                Code = code,
                State = state
            };

            var connection = await _stripeInstallationService.HandleOAuthCallback(tenantId, request);
            
            // In a real app, you'd redirect to a frontend success page
            return Ok(new SuccessResponseDto(true, "Stripe account connected successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Tenant not found for OAuth callback {TenantId}: {Message}", 
                tenantId, ex.Message);
            return NotFound(new ErrorResponseDto(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Invalid OAuth callback for tenant {TenantId}: {Message}", 
                tenantId, ex.Message);
            return BadRequest(new ErrorResponseDto(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling OAuth callback for tenant {TenantId}", tenantId);
            return StatusCode(500, new ErrorResponseDto("An unexpected error occurred"));
        }
    }

    /// <summary>
    /// Gets the current Stripe connection status for a tenant
    /// </summary>
    [HttpGet("tenant/{tenantId:guid}/connection")]
    public async Task<ActionResult<StripeConnectionDto?>> GetConnection(Guid tenantId)
    {
        try
        {
            var connection = await _stripeInstallationService.GetConnection(tenantId);
            return Ok(connection);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Stripe connection for tenant {TenantId}", tenantId);
            return StatusCode(500, new ErrorResponseDto("An unexpected error occurred"));
        }
    }

    /// <summary>
    /// Disconnects a Stripe account for a tenant
    /// </summary>
    [HttpDelete("tenant/{tenantId:guid}/connection")]
    public async Task<IActionResult> DisconnectAccount(Guid tenantId)
    {
        try
        {
            var success = await _stripeInstallationService.DisconnectStripe(tenantId);
            
            if (!success)
            {
                return NotFound(new ErrorResponseDto("No Stripe connection found for this tenant"));
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disconnecting Stripe account for tenant {TenantId}", tenantId);
            return StatusCode(500, new ErrorResponseDto("An unexpected error occurred"));
        }
    }
} 