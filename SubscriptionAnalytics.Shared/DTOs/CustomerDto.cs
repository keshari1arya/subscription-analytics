namespace SubscriptionAnalytics.Shared.DTOs;

public class CustomerDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Livemode { get; set; }
    public object? Metadata { get; set; }
    public DateTime SyncedAt { get; set; }
} 