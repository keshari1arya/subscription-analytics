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

// Load Connections
export const loadConnections = createAction('[Providers] Load Connections');
export const loadConnectionsSuccess = createAction('[Providers] Load Connections Success', props<{ connections: any[] }>());
export const loadConnectionsFailure = createAction('[Providers] Load Connections Failure', props<{ error: string }>());

// Handle OAuth Callback
export const handleOAuthCallback = createAction('[Providers] Handle OAuth Callback', props<{ provider: string; code: string; state: string }>());
export const handleOAuthCallbackSuccess = createAction('[Providers] Handle OAuth Callback Success', props<{ provider: string; result: any }>());
export const handleOAuthCallbackFailure = createAction('[Providers] Handle OAuth Callback Failure', props<{ error: string }>());

// Clear Error
export const clearProviderError = createAction('[Providers] Clear Error');
