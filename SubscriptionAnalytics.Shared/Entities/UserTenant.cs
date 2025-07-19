namespace SubscriptionAnalytics.Shared.Entities;

public class UserTenant
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string Role { get; set; } = string.Empty;
} 