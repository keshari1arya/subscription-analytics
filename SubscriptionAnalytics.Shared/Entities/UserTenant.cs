namespace SubscriptionAnalytics.Shared.Entities;

public class UserTenant
{
    public string UserId { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public virtual Tenant? Tenant { get; set; }
} 