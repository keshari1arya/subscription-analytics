using Microsoft.AspNetCore.Http;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface IWebhookHandler
{
    Task HandleWebhookAsync(HttpRequest request);
} 