using System;

namespace SubscriptionAnalytics.Shared.Entities;

public class StripeConnection : BaseTenantEntity
{
    public string StripeAccountId { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;  // Encrypted
    public string RefreshToken { get; set; } = string.Empty; // Encrypted
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    public string Status { get; set; } = string.Empty; // Connected, Disconnected
} 