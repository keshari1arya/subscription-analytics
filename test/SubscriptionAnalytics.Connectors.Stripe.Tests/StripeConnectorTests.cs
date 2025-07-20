using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SubscriptionAnalytics.Connectors.Stripe.Services;
using SubscriptionAnalytics.Shared.Interfaces;
using Xunit;

namespace SubscriptionAnalytics.Connectors.Stripe.Tests;

public class StripeConnectorTests
{
    private readonly IConnector _connector = new StripeConnector();

    [Fact]
    public void ProviderName_Should_Be_Stripe()
    {
        _connector.ProviderName.Should().Be("Stripe");
    }

    [Fact]
    public async Task SyncDataAsync_Should_CompleteSuccessfully()
    {
        var tenantId = Guid.NewGuid();
        var token = CancellationToken.None;
        var task = _connector.SyncDataAsync(tenantId, token);
        await task;
        task.IsCompletedSuccessfully.Should().BeTrue();
    }
} 