namespace SubscriptionAnalytics.Shared.Interfaces;

public interface ISyncProcessor
{
    Task SyncCustomersAsync(Guid tenantId, CancellationToken cancellationToken);
} 