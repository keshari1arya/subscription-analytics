namespace SubscriptionAnalytics.Shared.Entities;

public class ProviderConnection : BaseTenantEntity
{
    public string ProviderName { get; set; } = string.Empty; // "Stripe", "PayPal", etc.
    public string ProviderAccountId { get; set; } = string.Empty; // Stripe Account ID, PayPal Merchant ID, etc.
    public string AccessToken { get; set; } = string.Empty; // Encrypted
    public string? RefreshToken { get; set; } // Encrypted, optional
    public string? TokenType { get; set; } // "Bearer", etc.
    public int? ExpiresIn { get; set; } // Token expiration in seconds
    public DateTime? TokenExpiresAt { get; set; } // Calculated expiration time
    public string Scope { get; set; } = string.Empty; // OAuth scope
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    public string Status { get; set; } = string.Empty; // "Connected", "Disconnected", "Expired"
    public Dictionary<string, string>? AdditionalData { get; set; } // Provider-specific data as JSON
} 