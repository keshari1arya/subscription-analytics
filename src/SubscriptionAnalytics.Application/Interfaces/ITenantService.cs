using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Application.Interfaces;

public interface ITenantService
{
    Task<TenantDto> CreateTenantAsync(CreateTenantRequest request);
    Task<TenantDto?> GetTenantByIdAsync(Guid tenantId);
    Task<List<TenantDto>> GetAllTenantsAsync();
    Task<UserTenantDto> AssignUserToTenantAsync(AssignUserToTenantRequest request);
    Task<UserTenantsResponse> GetUserTenantsAsync(string userId);
    Task<bool> RemoveUserFromTenantAsync(string userId, Guid tenantId);
    Task<bool> UpdateUserTenantRoleAsync(string userId, Guid tenantId, string newRole);
    /// <summary>
    /// Assign a global (app-level) role to a user. Only AppAdmin can call this.
    /// </summary>
    Task<bool> AssignAppRoleAsync(string userId, string appRole);

    /// <summary>
    /// Remove a global (app-level) role from a user. Only AppAdmin can call this.
    /// </summary>
    Task<bool> RemoveAppRoleAsync(string userId, string appRole);
} 