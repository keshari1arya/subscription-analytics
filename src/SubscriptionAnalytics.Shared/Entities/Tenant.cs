using System;

namespace SubscriptionAnalytics.Shared.Entities;

/// <summary>
/// Represents a tenant/organization in the multi-tenant system.
/// This is the root entity that owns all tenant-related data.
/// </summary>
public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual StripeConnection? StripeConnection { get; set; }
    public virtual ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
    public virtual ICollection<SyncedCustomer> SyncedCustomers { get; set; } = new List<SyncedCustomer>();
} 