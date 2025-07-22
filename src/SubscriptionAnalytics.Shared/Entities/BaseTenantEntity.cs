using System;

namespace SubscriptionAnalytics.Shared.Entities;

/// <summary>
/// Base entity for all tenant-related entities.
/// Provides common properties and behavior for entities that belong to a specific tenant.
/// </summary>
public abstract class BaseTenantEntity : BaseEntity
{
    public Guid TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }
} 

public abstract class BaseEntity 
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public string DeletedBy { get; set; } = string.Empty;
}