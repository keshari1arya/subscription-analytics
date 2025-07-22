using System;

namespace SubscriptionAnalytics.Shared.DTOs;

public class InitiateStripeConnectionResponse
{
    public string AuthorizationUrl { get; set; }
    public string State { get; set; }
}

public class StripeOAuthCallbackRequest
{
    public string Code { get; set; }
    public string State { get; set; }
    public string Scope { get; set; }
}

public class StripeOAuthTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string StripeAccountId { get; set; }
    public string TokenType { get; set; }
    public string Scope { get; set; }
}

public class StripeConnectionDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string StripeAccountId { get; set; }
    public string Status { get; set; }
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }
} 