import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { IdentityService } from '../../api-client/api/identity.service';
import { RefreshRequest } from '../../api-client/model/refreshRequest';
import { logout } from '../../store/Authentication/authentication.actions';
import { TokenService } from '../services/token.service';

export function tokenRefreshInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  const tokenService = inject(TokenService);
  const identityService = inject(IdentityService);
  const router = inject(Router);
  const store = inject(Store);

  // Skip token refresh for auth endpoints to avoid infinite loops
  if (request.url.includes('/api/identity/login') ||
      request.url.includes('/api/identity/refresh') ||
      request.url.includes('/api/identity/register')) {
    return next(request);
  }

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !request.url.includes('/api/identity/refresh')) {
        return handleTokenRefresh(request, next, tokenService, identityService, router, store);
      }
      return throwError(() => error);
    })
  );
}

function handleTokenRefresh(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn,
  tokenService: TokenService,
  identityService: IdentityService,
  router: Router,
  store: Store
): Observable<HttpEvent<unknown>> {
  const refreshToken = tokenService.getRefreshToken();

  if (!refreshToken) {
    // No refresh token available, logout and redirect to login
    store.dispatch(logout());
    router.navigate(['/auth/login']);
    return throwError(() => new Error('No refresh token available'));
  }

  // Create refresh request
  const refreshRequest: RefreshRequest = {
    refreshToken: refreshToken
  };

  return identityService.apiIdentityRefreshPost(refreshRequest).pipe(
    switchMap((response: any) => {
      // Update tokens with new access token
      tokenService.setTokens(
        response.accessToken,
        response.refreshToken,
        response.expiresIn ? Date.now() + (response.expiresIn * 1000) : undefined
      );

      // Retry the original request with new token
      const newRequest = request.clone({
        setHeaders: {
          Authorization: `Bearer ${response.accessToken}`
        }
      });

      return next(newRequest);
    }),
    catchError((refreshError) => {
      // Refresh token failed, logout and redirect to login
      console.error('Token refresh failed:', refreshError);
      store.dispatch(logout());
      router.navigate(['/auth/login']);
      return throwError(() => refreshError);
    })
  );
}
