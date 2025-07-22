using SubscriptionAnalytics.Shared.Constants;

namespace SubscriptionAnalytics.Shared.Entities;

public class UserTenant : BaseTenantEntity
{
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// Role of the user within the tenant. Allowed: TenantAdmin, TenantUser, SupportUser, ReadOnlyUser
    /// </summary>
    public string Role { get; set; } = string.Empty;

    public static bool IsValidTenantRole(string role)
    {
        return role == Roles.TenantAdmin ||
               role == Roles.TenantUser ||
               role == Roles.SupportUser ||
               role == Roles.ReadOnlyUser;
    }
} 