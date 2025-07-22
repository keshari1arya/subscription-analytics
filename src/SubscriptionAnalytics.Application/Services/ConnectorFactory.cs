using Microsoft.Extensions.Logging;
using SubscriptionAnalytics.Shared.Enums;
using SubscriptionAnalytics.Shared.Interfaces;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Connectors.PayPal.Abstractions;

namespace SubscriptionAnalytics.Application.Services;

public class ConnectorFactory : IConnectorFactory
{
    private readonly IStripeConnector _stripeConnector;
    private readonly IPayPalConnector _payPalConnector;
    private readonly ILogger<ConnectorFactory> _logger;

    public ConnectorFactory(
        IStripeConnector stripeConnector,
        IPayPalConnector payPalConnector,
        ILogger<ConnectorFactory> logger)
    {
        _stripeConnector = stripeConnector;
        _payPalConnector = payPalConnector;
        _logger = logger;
    }

    public IConnector GetConnector(ConnectorType connectorType)
    {
        return connectorType switch
        {
            ConnectorType.Stripe => _stripeConnector as IConnector ?? 
                throw new InvalidOperationException($"Stripe connector is not properly configured"),
            ConnectorType.PayPal => _payPalConnector as IConnector ?? 
                throw new InvalidOperationException($"PayPal connector is not properly configured"),
            _ => throw new ArgumentException($"Unsupported connector type: {connectorType}")
        };
    }

    public IEnumerable<IConnector> GetAllConnectors()
    {
        var connectors = new List<IConnector>();
        
        if (_stripeConnector is IConnector stripeConnector)
            connectors.Add(stripeConnector);
            
        if (_payPalConnector is IConnector payPalConnector)
            connectors.Add(payPalConnector);
            
        return connectors;
    }

    public bool SupportsConnector(ConnectorType connectorType)
    {
        return connectorType switch
        {
            ConnectorType.Stripe => _stripeConnector is IConnector,
            ConnectorType.PayPal => _payPalConnector is IConnector,
            _ => false
        };
    }
} 