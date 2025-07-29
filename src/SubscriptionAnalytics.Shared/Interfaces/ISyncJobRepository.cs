using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface ISyncJobRepository
{
    Task<SyncJob?> GetByIdAsync(Guid id);
    Task<IEnumerable<SyncJob>> GetByTenantIdAsync(Guid tenantId);
    Task<SyncJob> AddAsync(SyncJob syncJob);
    Task<SyncJob> UpdateAsync(SyncJob syncJob);
}
