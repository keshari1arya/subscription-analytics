using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionAnalytics.Shared.Entities;

public class SyncedCustomer : BaseTenantEntity
{
    public string CustomerId { get; set; } = string.Empty; // PK
    public Guid UserId { get; set; } // FK to users
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedStripeAt { get; set; }
    public bool Livemode { get; set; }
    public DateTime SyncedAt { get; set; } = DateTime.UtcNow;
} 