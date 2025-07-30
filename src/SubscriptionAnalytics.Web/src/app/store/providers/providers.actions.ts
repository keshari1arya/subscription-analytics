import { createAction, props } from '@ngrx/store';
import { ConnectorInfo } from 'src/app/api-client/model/connectorInfo';

// Load Providers
export const loadProviders = createAction('[Providers] Load Providers');
export const loadProvidersSuccess = createAction('[Providers] Load Providers Success', props<{ providers: ConnectorInfo[] }>());
export const loadProvidersFailure = createAction('[Providers] Load Providers Failure', props<{ error: string }>());

// Install Provider
export const installProvider = createAction('[Providers] Install Provider', props<{ providerName: string }>());
export const installProviderSuccess = createAction('[Providers] Install Provider Success', props<{ provider: ConnectorInfo }>());
export const installProviderFailure = createAction('[Providers] Install Provider Failure', props<{ error: string }>());

// Disconnect Provider
export const disconnectProvider = createAction('[Providers] Disconnect Provider', props<{ providerName: string }>());
export const disconnectProviderSuccess = createAction('[Providers] Disconnect Provider Success', props<{ providerName: string }>());
export const disconnectProviderFailure = createAction('[Providers] Disconnect Provider Failure', props<{ providerName: string; error: string }>());

// Load Connections
export const loadConnections = createAction('[Providers] Load Connections');
export const loadConnectionsSuccess = createAction('[Providers] Load Connections Success', props<{ connections: any[] }>());
export const loadConnectionsFailure = createAction('[Providers] Load Connections Failure', props<{ error: string }>());

// Handle OAuth Callback
export const handleOAuthCallback = createAction('[Providers] Handle OAuth Callback', props<{ provider: string; code: string; state: string }>());
export const handleOAuthCallbackSuccess = createAction('[Providers] Handle OAuth Callback Success', props<{ provider: string; result: any }>());
export const handleOAuthCallbackFailure = createAction('[Providers] Handle OAuth Callback Failure', props<{ error: string }>());

// Sync Provider
export const startSync = createAction('[Providers] Start Sync', props<{ providerName: string }>());
export const startSyncSuccess = createAction('[Providers] Start Sync Success', props<{ providerName: string; jobId: string }>());
export const startSyncFailure = createAction('[Providers] Start Sync Failure', props<{ providerName: string; error: string }>());

// Monitor Sync Progress
export const monitorSyncProgress = createAction('[Providers] Monitor Sync Progress', props<{ providerName: string; jobId: string }>());
export const updateSyncProgress = createAction('[Providers] Update Sync Progress', props<{ providerName: string; progress: number; status: string; errorMessage?: string }>());
export const syncCompleted = createAction('[Providers] Sync Completed', props<{ providerName: string; status: string; errorMessage?: string }>());

// Cancel Sync
export const cancelSync = createAction('[Providers] Cancel Sync', props<{ providerName: string; jobId: string }>());
export const cancelSyncSuccess = createAction('[Providers] Cancel Sync Success', props<{ providerName: string }>());
export const cancelSyncFailure = createAction('[Providers] Cancel Sync Failure', props<{ providerName: string; error: string }>());

// Clear Error
export const clearProviderError = createAction('[Providers] Clear Error');
