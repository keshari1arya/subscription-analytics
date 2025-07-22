using System;

namespace SubscriptionAnalytics.Shared.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual StripeConnection? StripeConnection { get; set; }
} 