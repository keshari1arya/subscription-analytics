
import { ConnectorInfo } from 'src/app/api-client/model/connectorInfo';

export interface ProvidersState {
  providers: ConnectorInfo[];
  loading: boolean;
  error: string | null;
  installingProvider: string | null;
  connections: any[];
  connectionsLoading: boolean;
  connectionsError: string | null;
  oauthCallbackLoading: boolean;
  oauthCallbackError: string | null;
}
