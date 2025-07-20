using System.Threading;
using System.Threading.Tasks;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface ISyncProcessor
{
    Task SyncCustomersAsync(Guid tenantId, CancellationToken cancellationToken);
} 