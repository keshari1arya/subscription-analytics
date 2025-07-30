import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of, timer } from 'rxjs';
import { catchError, map, mergeMap, switchMap } from 'rxjs/operators';
import { ProviderService } from 'src/app/api-client/api/provider.service';
import { ConnectorInfo } from 'src/app/api-client/model/connectorInfo';
import { OAuthCallbackRequest } from 'src/app/api-client/model/oAuthCallbackRequest';
import * as ProvidersActions from './providers.actions';

@Injectable()
export class ProvidersEffects {

  loadProviders$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.loadProviders),
    mergeMap(() => this.providerService.apiProviderGet()
      .pipe(
        map((providers: any) => {
          // Convert the response to ConnectorInfo array
          const connectorInfos: ConnectorInfo[] = Array.isArray(providers) ? providers : [];
          return ProvidersActions.loadProvidersSuccess({ providers: connectorInfos });
        }),
        catchError(error => of(ProvidersActions.loadProvidersFailure({ error: error.message || 'Failed to load providers' })))
      ))
  ));

  installProvider$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.installProvider),
    mergeMap(({ providerName }) => this.providerService.apiProviderProviderPost(providerName)
      .pipe(
        map(response => {
          // If response has an authorization URL, it's an OAuth redirect
          if (response && (response as any).authorizationUrl) {
            window.location.href = (response as any).authorizationUrl;
            return ProvidersActions.installProviderSuccess({ provider: { providerName, displayName: providerName } as any });
          }
          // Otherwise, return the response as the provider
          return ProvidersActions.installProviderSuccess({ provider: response as any });
        }),
        catchError(error => of(ProvidersActions.installProviderFailure({ error: error.message || 'Failed to install provider' })))
      ))
  ));

  disconnectProvider$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.disconnectProvider),
    mergeMap(({ providerName }) => this.providerService.apiProviderProviderDelete(providerName)
      .pipe(
        map(() => ProvidersActions.disconnectProviderSuccess({ providerName })),
        catchError(error => of(ProvidersActions.disconnectProviderFailure({
          providerName,
          error: error.error?.message || error.message || 'Failed to disconnect provider'
        })))
      ))
  ));

  loadConnections$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.loadConnections),
    mergeMap(() => this.providerService.apiProviderConnectionsGet()
      .pipe(
        map(connections => ProvidersActions.loadConnectionsSuccess({ connections })),
        catchError(error => of(ProvidersActions.loadConnectionsFailure({ error: error.message || 'Failed to load connections' })))
      ))
  ));

  handleOAuthCallback$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.handleOAuthCallback),
    mergeMap(({ provider, code, state }) => {
      const oauthData: OAuthCallbackRequest = {
        provider: provider,
        code: code,
        state: state
      };

      return this.providerService.apiProviderOauthCallbackFromUiPost(oauthData)
        .pipe(
          map(result => ProvidersActions.handleOAuthCallbackSuccess({ provider, result })),
          catchError(error => of(ProvidersActions.handleOAuthCallbackFailure({ error: error.error?.message || 'Failed to complete OAuth flow' })))
        );
    })
  ));

  startSync$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.startSync),
    mergeMap(({ providerName }) => this.providerService.apiProviderProviderSyncPost(providerName)
      .pipe(
        map(response => ProvidersActions.startSyncSuccess({ providerName, jobId: response.jobId || '' })),
        catchError(error => of(ProvidersActions.startSyncFailure({
          providerName,
          error: error.error?.message || error.message || 'Failed to start sync'
        })))
      ))
  ));

  startSyncSuccess$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.startSyncSuccess),
    map(({ providerName, jobId }) => ProvidersActions.monitorSyncProgress({ providerName, jobId }))
  ));

  monitorSyncProgress$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.monitorSyncProgress),
    mergeMap(({ providerName, jobId }) =>
      timer(0, 2000).pipe( // Check every 2 seconds
        switchMap(() => this.providerService.apiProviderSyncStatusJobIdGet(jobId)),
        map(status => {
          if (status.status === 'Running') {
            return ProvidersActions.updateSyncProgress({
              providerName,
              progress: status.progress || 0,
              status: status.status
            });
          } else {
            return ProvidersActions.syncCompleted({
              providerName,
              status: status.status,
              errorMessage: status.errorMessage
            });
          }
        }),
        catchError(error => of(ProvidersActions.syncCompleted({
          providerName,
          status: 'Failed',
          errorMessage: error.error?.message || error.message || 'Failed to get sync status'
        })))
      )
    )
  ));

  cancelSync$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.cancelSync),
    mergeMap(({ providerName, jobId }) => this.providerService.apiProviderSyncJobIdDelete(jobId)
      .pipe(
        map(() => ProvidersActions.cancelSyncSuccess({ providerName })),
        catchError(error => of(ProvidersActions.cancelSyncFailure({
          providerName,
          error: error.error?.message || error.message || 'Failed to cancel sync'
        })))
      ))
  ));

  constructor(
    private actions$: Actions,
    private providerService: ProviderService
  ) {}
}
