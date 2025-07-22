using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Exceptions;
using SubscriptionAnalytics.Shared.Constants;

namespace SubscriptionAnalytics.Application.Services;

public class TenantService : ITenantService
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<TenantService> _logger;

    public TenantService(
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        ILogger<TenantService> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<TenantDto> CreateTenantAsync(CreateTenantRequest request, string userId)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Tenants.Add(tenant);

        // Create UserTenant relationship with TenantAdmin role
        var userTenant = new UserTenant
        {
            UserId = userId,
            TenantId = tenant.Id,
            Role = Roles.TenantAdmin,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserTenants.Add(userTenant);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created tenant: {TenantName} with ID: {TenantId} for user: {UserId}", tenant.Name, tenant.Id, userId);

        return new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            CreatedAt = tenant.CreatedAt,
            IsActive = tenant.IsActive
        };
    }

    public async Task<TenantDto?> GetTenantByIdAsync(Guid tenantId)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId);

        if (tenant == null)
            return null;

        return new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            CreatedAt = tenant.CreatedAt,
            IsActive = tenant.IsActive
        };
    }

    public async Task<List<TenantDto>> GetAllTenantsAsync()
    {
        var tenants = await _context.Tenants
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync();

        return tenants.Select(t => new TenantDto
        {
            Id = t.Id,
            Name = t.Name,
            CreatedAt = t.CreatedAt,
            IsActive = t.IsActive
        }).ToList();
    }

    public async Task<UserTenantDto> AssignUserToTenantAsync(AssignUserToTenantRequest request)
    {
        if (!UserTenant.IsValidTenantRole(request.Role))
            throw new BusinessException($"Invalid tenant role: {request.Role}");
        // Find user by email
        var user = await _userManager.FindByEmailAsync(request.UserEmail);
        if (user == null)
        {
            throw new NotFoundException($"User with email {request.UserEmail} not found");
        }

        // Check if tenant exists
        var tenant = await _context.Tenants.FindAsync(request.TenantId);
        if (tenant == null)
        {
            throw new NotFoundException($"Tenant with ID {request.TenantId} not found");
        }

        // Check if user is already assigned to this tenant
        var existingUserTenant = await _context.UserTenants
            .FirstOrDefaultAsync(ut => ut.UserId == user.Id && ut.TenantId == request.TenantId);

        if (existingUserTenant != null)
        {
            // Update existing role
            existingUserTenant.Role = request.Role;
            await _context.SaveChangesAsync();

            return new UserTenantDto
            {
                UserId = user.Id,
                TenantId = request.TenantId,
                Role = request.Role,
                CreatedAt = existingUserTenant.CreatedAt,
                TenantName = tenant.Name,
                UserEmail = user.Email
            };
        }

        // Create new user-tenant relationship
        var userTenant = new UserTenant
        {
            UserId = user.Id,
            TenantId = request.TenantId,
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserTenants.Add(userTenant);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Assigned user {UserEmail} to tenant {TenantName} with role {Role}", 
            user.Email, tenant.Name, request.Role);

        return new UserTenantDto
        {
            UserId = user.Id,
            TenantId = request.TenantId,
            Role = request.Role,
            CreatedAt = userTenant.CreatedAt,
            TenantName = tenant.Name,
            UserEmail = user.Email
        };
    }

    public async Task<UserTenantsResponse> GetUserTenantsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {userId} not found");
        }

        var userTenants = await _context.UserTenants
            .Include(ut => ut.Tenant)
            .Where(ut => ut.UserId == userId)
            .ToListAsync();

        var userTenantDtos = userTenants.Select(ut => new UserTenantDto
        {
            UserId = ut.UserId,
            TenantId = ut.TenantId,
            Role = ut.Role,
            CreatedAt = ut.CreatedAt,
            TenantName = ut.Tenant?.Name,
            UserEmail = user.Email
        }).ToList();

        return new UserTenantsResponse
        {
            UserId = userId,
            UserEmail = user.Email ?? string.Empty,
            Tenants = userTenantDtos
        };
    }

    public async Task<bool> RemoveUserFromTenantAsync(string userId, Guid tenantId)
    {
        var userTenant = await _context.UserTenants
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);

        if (userTenant == null)
            return false;

        _context.UserTenants.Remove(userTenant);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Removed user {UserId} from tenant {TenantId}", userId, tenantId);
        return true;
    }

    public async Task<bool> UpdateUserTenantRoleAsync(string userId, Guid tenantId, string newRole)
    {
        if (!UserTenant.IsValidTenantRole(newRole))
            return false;
        var userTenant = await _context.UserTenants
            .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);

        if (userTenant == null)
            return false;

        userTenant.Role = newRole;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated user {UserId} role to {Role} in tenant {TenantId}", 
            userId, newRole, tenantId);
        return true;
    }

    public async Task<bool> AssignAppRoleAsync(string userId, string appRole)
    {
        if (appRole != Roles.AppAdmin && appRole != Roles.SupportUser)
            throw new BusinessException($"Invalid app-level role: {appRole}");
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        var result = await _userManager.AddToRoleAsync(user, appRole);
        return result.Succeeded;
    }

    public async Task<bool> RemoveAppRoleAsync(string userId, string appRole)
    {
        if (appRole != Roles.AppAdmin && appRole != Roles.SupportUser)
            throw new BusinessException($"Invalid app-level role: {appRole}");
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        var result = await _userManager.RemoveFromRoleAsync(user, appRole);
        return result.Succeeded;
    }
} 