import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ProvidersState } from './providers.state';

export const selectProvidersState = createFeatureSelector<ProvidersState>('providers');

export const selectProviders = createSelector(
  selectProvidersState,
  (state) => state.providers
);

export const selectProvidersLoading = createSelector(
  selectProvidersState,
  (state) => state.loading
);

export const selectProvidersError = createSelector(
  selectProvidersState,
  (state) => state.error
);

export const selectInstallingProvider = createSelector(
  selectProvidersState,
  (state) => state.installingProvider
);

export const selectConnections = createSelector(
  selectProvidersState,
  (state) => state.connections
);

export const selectConnectionsLoading = createSelector(
  selectProvidersState,
  (state) => state.connectionsLoading
);

export const selectConnectionsError = createSelector(
  selectProvidersState,
  (state) => state.connectionsError
);

export const selectOAuthCallbackLoading = createSelector(
  selectProvidersState,
  (state) => state.oauthCallbackLoading
);

export const selectOAuthCallbackError = createSelector(
  selectProvidersState,
  (state) => state.oauthCallbackError
);

// Sync selectors
export const selectSyncingProviders = createSelector(
  selectProvidersState,
  (state) => state.syncingProviders
);

export const selectSyncProgress = createSelector(
  selectProvidersState,
  (state) => state.syncProgress
);

export const selectSyncErrors = createSelector(
  selectProvidersState,
  (state) => state.syncErrors
);

export const selectSyncJobs = createSelector(
  selectProvidersState,
  (state) => state.syncJobs
);

export const selectIsSyncing = createSelector(
  selectProvidersState,
  (state) => (providerName: string) => state.syncingProviders.has(providerName)
);

export const selectSyncProgressForProvider = createSelector(
  selectProvidersState,
  (state) => (providerName: string) => state.syncProgress[providerName] || 0
);

export const selectSyncErrorForProvider = createSelector(
  selectProvidersState,
  (state) => (providerName: string) => state.syncErrors[providerName] || ''
);

export const selectSyncJobForProvider = createSelector(
  selectProvidersState,
  (state) => (providerName: string) => state.syncJobs[providerName] || ''
);
