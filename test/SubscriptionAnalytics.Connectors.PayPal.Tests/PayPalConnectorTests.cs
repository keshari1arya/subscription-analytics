using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SubscriptionAnalytics.Connectors.PayPal.Services;
using SubscriptionAnalytics.Shared.Interfaces;
using Xunit;

namespace SubscriptionAnalytics.Connectors.PayPal.Tests;

public class PayPalConnectorTests
{
    private readonly IConnector _connector = new PayPalConnector();

    [Fact]
    public void ProviderName_Should_Be_PayPal()
    {
        _connector.ProviderName.Should().Be("PayPal");
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