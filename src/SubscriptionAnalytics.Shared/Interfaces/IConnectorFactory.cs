using SubscriptionAnalytics.Shared.Enums;

namespace SubscriptionAnalytics.Shared.Interfaces;

public interface IConnectorFactory
{
    IConnector GetConnector(ConnectorType connectorType);
    IEnumerable<IConnector> GetAllConnectors();
    bool SupportsConnector(ConnectorType connectorType);
} 