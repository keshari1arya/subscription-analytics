using System;
using System.Threading.Tasks;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Application.Interfaces;

public interface IStripeInstallationService
{
    Task<InitiateStripeConnectionResponse> InitiateConnection(Guid tenantId);
    Task<StripeConnectionDto> HandleOAuthCallback(Guid tenantId, StripeOAuthCallbackRequest request);
    Task<StripeConnectionDto?> GetConnection(Guid tenantId);
    Task<bool> DisconnectStripe(Guid tenantId);
} 