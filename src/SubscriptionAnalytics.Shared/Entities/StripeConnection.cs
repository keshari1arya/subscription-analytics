using System;

namespace SubscriptionAnalytics.Shared.Entities;

public class StripeConnection
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string StripeAccountId { get; set; }
    public string AccessToken { get; set; }  // Encrypted
    public string RefreshToken { get; set; } // Encrypted
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    public string Status { get; set; } // Connected, Disconnected
    
    // Navigation properties
    public virtual Tenant Tenant { get; set; }
} 