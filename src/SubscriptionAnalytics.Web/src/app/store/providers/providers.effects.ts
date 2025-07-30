import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
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

  constructor(
    private actions$: Actions,
    private providerService: ProviderService
  ) {}
}
