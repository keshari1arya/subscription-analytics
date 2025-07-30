namespace SubscriptionAnalytics.Shared.Interfaces;

public interface IStripeSyncService
{
    Task<int> SyncCustomersAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default);
    Task<int> SyncSubscriptionsAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default);
    Task<int> SyncPaymentsAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default);
    Task<int> SyncAllDataAsync(Guid tenantId, string accessToken, CancellationToken cancellationToken = default);
}
