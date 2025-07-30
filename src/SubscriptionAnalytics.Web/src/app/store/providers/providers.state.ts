
import { ConnectorInfo } from 'src/app/api-client/model/connectorInfo';

export interface SyncState {
  syncingProviders: Set<string>;
  syncProgress: { [key: string]: number };
  syncErrors: { [key: string]: string };
  syncJobs: { [key: string]: string };
}

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
  // Sync state
  syncingProviders: Set<string>;
  syncProgress: { [key: string]: number };
  syncErrors: { [key: string]: string };
  syncJobs: { [key: string]: string };
}
