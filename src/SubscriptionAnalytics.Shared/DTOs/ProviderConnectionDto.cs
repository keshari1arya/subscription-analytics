namespace SubscriptionAnalytics.Shared.DTOs;

public class ProviderConnectionDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string ProviderAccountId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    public DateTime? TokenExpiresAt { get; set; }
    public string Scope { get; set; } = string.Empty;
} 