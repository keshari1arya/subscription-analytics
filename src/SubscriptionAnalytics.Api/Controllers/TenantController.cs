using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TenantController : ControllerBase
{
    private readonly ITenantService _tenantService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<TenantController> _logger;

    public TenantController(
        ITenantService tenantService,
        UserManager<IdentityUser> userManager,
        ILogger<TenantController> logger)
    {
        _tenantService = tenantService;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TenantDto>> CreateTenant([FromBody] CreateTenantRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var tenant = await _tenantService.CreateTenantAsync(request);
        return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenant);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TenantDto>> GetTenant(Guid id)
    {
        var tenant = await _tenantService.GetTenantByIdAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }

        return Ok(tenant);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<TenantDto>>> GetAllTenants()
    {
        var tenants = await _tenantService.GetAllTenantsAsync();
        return Ok(tenants);
    }

    [HttpPost("assign-user")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserTenantDto>> AssignUserToTenant([FromBody] AssignUserToTenantRequest request)
    {
        var userTenant = await _tenantService.AssignUserToTenantAsync(request);
        return Ok(userTenant);
    }

    [HttpGet("my-tenants")]
    public async Task<ActionResult<UserTenantsResponse>> GetMyTenants()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var userTenants = await _tenantService.GetUserTenantsAsync(user.Id);
        return Ok(userTenants);
    }

    [HttpDelete("{tenantId:guid}/users/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RemoveUserFromTenant(Guid tenantId, string userId)
    {
        var result = await _tenantService.RemoveUserFromTenantAsync(userId, tenantId);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut("{tenantId:guid}/users/{userId}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateUserTenantRole(Guid tenantId, string userId, [FromBody] string newRole)
    {
        var result = await _tenantService.UpdateUserTenantRoleAsync(userId, tenantId, newRole);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
} 