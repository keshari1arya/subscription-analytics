using System.Text.Json.Nodes;

namespace SubscriptionAnalytics.Shared.Entities;

public class SyncedCustomer
{
    public string CustomerId { get; set; } = string.Empty; // PK
    public Guid UserId { get; set; } // FK to users
    public Guid TenantId { get; set; } // FK to tenants
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedStripeAt { get; set; }
    public bool Livemode { get; set; }
    public JsonObject Metadata { get; set; } = new();
    public DateTime SyncedAt { get; set; } = DateTime.UtcNow;
} 