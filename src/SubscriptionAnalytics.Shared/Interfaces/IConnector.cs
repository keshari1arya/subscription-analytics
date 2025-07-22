namespace SubscriptionAnalytics.Shared.Interfaces;

public interface IConnector
{
    string ProviderName { get; }
    Task SyncDataAsync(Guid tenantId, CancellationToken cancellationToken);
    // Optionally: Task HandleWebhookAsync(...);
} 