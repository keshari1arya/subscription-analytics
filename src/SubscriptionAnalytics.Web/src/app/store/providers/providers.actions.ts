import { createAction, props } from '@ngrx/store';
import { SubscriptionAnalyticsApiControllersConnectorInfo } from 'src/app/api-client/model/subscriptionAnalyticsApiControllersConnectorInfo';

// Load Providers
export const loadProviders = createAction('[Providers] Load Providers');
export const loadProvidersSuccess = createAction('[Providers] Load Providers Success', props<{ providers: SubscriptionAnalyticsApiControllersConnectorInfo[] }>());
export const loadProvidersFailure = createAction('[Providers] Load Providers Failure', props<{ error: string }>());

// Install Provider
export const installProvider = createAction('[Providers] Install Provider', props<{ providerName: string }>());
export const installProviderSuccess = createAction('[Providers] Install Provider Success', props<{ provider: SubscriptionAnalyticsApiControllersConnectorInfo }>());
export const installProviderFailure = createAction('[Providers] Install Provider Failure', props<{ error: string }>());

// Clear Error
export const clearProviderError = createAction('[Providers] Clear Error'); 