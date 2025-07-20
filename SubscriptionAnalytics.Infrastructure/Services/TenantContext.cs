using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    public Guid TenantId { get; set; }
} 