import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import { ConnectService } from 'src/app/api-client/api/connect.service';
import * as ProvidersActions from './providers.actions';

@Injectable()
export class ProvidersEffects {

  loadProviders$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.loadProviders),
    mergeMap(() => this.connectService.apiConnectProvidersGet()
      .pipe(
        map(providers => ProvidersActions.loadProvidersSuccess({ providers })),
        catchError(error => of(ProvidersActions.loadProvidersFailure({ error: error.message || 'Failed to load providers' })))
      ))
  ));

  installProvider$ = createEffect(() => this.actions$.pipe(
    ofType(ProvidersActions.installProvider),
    mergeMap(({ providerName }) => this.connectService.apiConnectProviderProviderPost(providerName)
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

  constructor(
    private actions$: Actions,
    private connectService: ConnectService
  ) {}
} 