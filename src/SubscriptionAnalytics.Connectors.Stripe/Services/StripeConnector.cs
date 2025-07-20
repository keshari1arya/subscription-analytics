using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Connectors.Stripe.Services;

public class StripeConnector : IConnector
{
    public string ProviderName => "Stripe";

    public Task SyncDataAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        // TODO: Implement Stripe sync logic
        return Task.CompletedTask;
    }
} 