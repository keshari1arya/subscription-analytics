using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Connectors.PayPal.Services;

public class PayPalConnector : IConnector
{
    public string ProviderName => "PayPal";

    public Task SyncDataAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        // TODO: Implement PayPal sync logic
        return Task.CompletedTask;
    }
} 