namespace SubscriptionAnalytics.Shared.DTOs;

public class ErrorResponse
{
    public string TraceId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Type { get; set; }
    public Dictionary<string, object>? Extensions { get; set; }
}

public class ValidationErrorResponse : ErrorResponse
{
    public Dictionary<string, string[]> ValidationErrors { get; set; } = new();
} 