namespace SubscriptionAnalytics.Shared.Entities;

public class Customer : BaseTenantEntity
{
    public string CustomerId { get; set; } = string.Empty; // Provider-specific customer ID
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CustomerCreatedAt { get; set; }
    public bool Livemode { get; set; }
    public DateTime SyncedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<StripeCustomer> StripeCustomers { get; set; } = new List<StripeCustomer>();
}
