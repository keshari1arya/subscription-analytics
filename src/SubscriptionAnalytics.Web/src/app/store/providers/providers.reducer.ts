import { createReducer, on } from '@ngrx/store';
import { ProvidersState } from './providers.state';
import * as ProvidersActions from './providers.actions';

export const initialState: ProvidersState = {
  providers: [],
  loading: false,
  error: null,
  installingProvider: null,
  connections: [],
  connectionsLoading: false,
  connectionsError: null,
  oauthCallbackLoading: false,
  oauthCallbackError: null
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
  
  // Clear Error
  on(ProvidersActions.clearProviderError, (state) => ({
    ...state,
    error: null
  }))
);

export { ProvidersState }; 