import { SubscriptionAnalyticsApiControllersConnectorInfo } from 'src/app/api-client/model/subscriptionAnalyticsApiControllersConnectorInfo';

export interface ProvidersState {
  providers: SubscriptionAnalyticsApiControllersConnectorInfo[];
  loading: boolean;
  error: string | null;
  installingProvider: string | null;
} 