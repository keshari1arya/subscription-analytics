import { createReducer, on } from '@ngrx/store';
import * as ProvidersActions from './providers.actions';
import { ProvidersState } from './providers.state';

export const initialState: ProvidersState = {
  providers: [],
  loading: false,
  error: null,
  installingProvider: null,
  connections: [],
  connectionsLoading: false,
  connectionsError: null,
  oauthCallbackLoading: false,
  oauthCallbackError: null,
  // Sync state
  syncingProviders: new Set<string>(),
  syncProgress: {},
  syncErrors: {},
  syncJobs: {}
};

export const providersReducer = createReducer(
  initialState,

  // Load Providers
  on(ProvidersActions.loadProviders, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(ProvidersActions.loadProvidersSuccess, (state, { providers }) => ({
    ...state,
    providers,
    loading: false,
    error: null
  })),

  on(ProvidersActions.loadProvidersFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Install Provider
  on(ProvidersActions.installProvider, (state, { providerName }) => ({
    ...state,
    installingProvider: providerName,
    error: null
  })),

  on(ProvidersActions.installProviderSuccess, (state, { provider }) => ({
    ...state,
    installingProvider: null,
    providers: state.providers.map(p =>
      p.providerName === provider.providerName ? provider : p
    )
  })),

  on(ProvidersActions.installProviderFailure, (state, { error }) => ({
    ...state,
    installingProvider: null,
    error
  })),

  // Disconnect Provider
  on(ProvidersActions.disconnectProvider, (state, { providerName }) => ({
    ...state,
    error: null
  })),

  on(ProvidersActions.disconnectProviderSuccess, (state, { providerName }) => ({
    ...state,
    // Remove the connection from the connections array
    connections: state.connections.filter(conn => conn.providerName !== providerName)
  })),

  on(ProvidersActions.disconnectProviderFailure, (state, { providerName, error }) => ({
    ...state,
    error
  })),

  // Load Connections
  on(ProvidersActions.loadConnections, (state) => ({
    ...state,
    connectionsLoading: true,
    connectionsError: null
  })),

  on(ProvidersActions.loadConnectionsSuccess, (state, { connections }) => ({
    ...state,
    connections,
    connectionsLoading: false,
    connectionsError: null
  })),

  on(ProvidersActions.loadConnectionsFailure, (state, { error }) => ({
    ...state,
    connectionsLoading: false,
    connectionsError: error
  })),

  // Handle OAuth Callback
  on(ProvidersActions.handleOAuthCallback, (state) => ({
    ...state,
    oauthCallbackLoading: true,
    oauthCallbackError: null
  })),

  on(ProvidersActions.handleOAuthCallbackSuccess, (state, { provider, result }) => ({
    ...state,
    oauthCallbackLoading: false,
    oauthCallbackError: null
  })),

  on(ProvidersActions.handleOAuthCallbackFailure, (state, { error }) => ({
    ...state,
    oauthCallbackLoading: false,
    oauthCallbackError: error
  })),

  // Start Sync
  on(ProvidersActions.startSync, (state, { providerName }) => ({
    ...state,
    syncingProviders: new Set([...state.syncingProviders, providerName]),
    syncProgress: { ...state.syncProgress, [providerName]: 0 },
    syncErrors: { ...state.syncErrors, [providerName]: '' }
  })),

  on(ProvidersActions.startSyncSuccess, (state, { providerName, jobId }) => ({
    ...state,
    syncJobs: { ...state.syncJobs, [providerName]: jobId }
  })),

  on(ProvidersActions.startSyncFailure, (state, { providerName, error }) => ({
    ...state,
    syncingProviders: new Set([...state.syncingProviders].filter(p => p !== providerName)),
    syncErrors: { ...state.syncErrors, [providerName]: error },
    syncProgress: { ...state.syncProgress, [providerName]: 0 }
  })),

  // Update Sync Progress
  on(ProvidersActions.updateSyncProgress, (state, { providerName, progress, status }) => ({
    ...state,
    syncProgress: { ...state.syncProgress, [providerName]: progress }
  })),

  // Sync Completed
  on(ProvidersActions.syncCompleted, (state, { providerName, status, errorMessage }) => ({
    ...state,
    syncingProviders: new Set([...state.syncingProviders].filter(p => p !== providerName)),
    syncProgress: { ...state.syncProgress, [providerName]: status === 'Completed' ? 100 : 0 },
    syncErrors: {
      ...state.syncErrors,
      [providerName]: status === 'Failed' ? (errorMessage || 'Sync failed') : ''
    },
    syncJobs: { ...state.syncJobs, [providerName]: undefined }
  })),

  // Cancel Sync
  on(ProvidersActions.cancelSync, (state, { providerName }) => ({
    ...state,
    syncingProviders: new Set([...state.syncingProviders].filter(p => p !== providerName)),
    syncProgress: { ...state.syncProgress, [providerName]: 0 },
    syncErrors: { ...state.syncErrors, [providerName]: '' },
    syncJobs: { ...state.syncJobs, [providerName]: undefined }
  })),

  on(ProvidersActions.cancelSyncSuccess, (state, { providerName }) => ({
    ...state,
    syncProgress: { ...state.syncProgress, [providerName]: 0 },
    syncErrors: { ...state.syncErrors, [providerName]: '' }
  })),

  on(ProvidersActions.cancelSyncFailure, (state, { providerName, error }) => ({
    ...state,
    syncErrors: { ...state.syncErrors, [providerName]: error }
  })),

  // Clear Error
  on(ProvidersActions.clearProviderError, (state) => ({
    ...state,
    error: null
  }))
);

export { ProvidersState };
