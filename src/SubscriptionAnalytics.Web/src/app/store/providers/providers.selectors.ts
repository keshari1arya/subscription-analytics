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