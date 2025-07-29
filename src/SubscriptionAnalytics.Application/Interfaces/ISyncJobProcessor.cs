using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Application.Interfaces;

public interface ISyncJobProcessor
{
    Task<SyncJob> ProcessFullSyncAsync(SyncJob syncJob);
}
