import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, mergeMap, tap } from 'rxjs/operators';
import { TenantService } from 'src/app/core/services/tenant.service';
import { UserProfileService } from 'src/app/core/services/user.service';
import { environment } from 'src/environments/environment';
import { IdentityService } from '../../api-client/api/identity.service';
import { AccessTokenResponse, LoginRequest, RegisterRequest } from '../../api-client/model/models';
import { TokenService } from '../../core/services/token.service';
import { login, loginFailure, loginSuccess, logout, logoutSuccess, Register, RegisterFailure, RegisterSuccess } from './authentication.actions';

@Injectable()
export class AuthenticationEffects {

  Register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(Register),
      exhaustMap(({ email, password }) => {
        if (environment.defaultauth === 'fakebackend') {
          return this.userService.register({ email, password }).pipe(
            map((user) => {
              this.router.navigate(['/auth/login']);
              return RegisterSuccess({ user })
            }),
            catchError((error) => {
              console.error('Register error:', error);
              let errorMessage = 'Registration failed. Please try again.';

              if (error.error && error.error.errors) {
                // Handle validation errors from API
                const errors = error.error.errors;
                if (errors.DuplicateUserName) {
                  errorMessage = errors.DuplicateUserName[0];
                } else if (errors.Password) {
                  errorMessage = errors.Password[0];
                } else {
                  // Get first error message
                  const firstError = Object.values(errors)[0];
                  errorMessage = Array.isArray(firstError) ? firstError[0] : firstError;
                }
              } else if (error.error && error.error.title) {
                errorMessage = error.error.title;
              } else if (error.message) {
                errorMessage = error.message;
              }

              return of(RegisterFailure({ error: errorMessage }));
            })
          );
        } else {
          const registerRequest: RegisterRequest = {
            email: email,
            password: password
          };

          return this.identityService.apiIdentityRegisterPost(registerRequest).pipe(
            map((response) => {
              this.router.navigate(['/auth/login']);
              return RegisterSuccess({ user: { email: email, id: 1 } })
            }),
            catchError((error) => {
              console.error('Register error:', error);
              let errorMessage = 'Registration failed. Please try again.';

              if (error.error && error.error.errors) {
                // Handle validation errors from API
                const errors = error.error.errors;
                if (errors.DuplicateUserName) {
                  errorMessage = errors.DuplicateUserName[0];
                } else if (errors.Password) {
                  errorMessage = errors.Password[0];
                } else {
                  // Get first error message
                  const firstError = Object.values(errors)[0];
                  errorMessage = Array.isArray(firstError) ? firstError[0] : firstError;
                }
              } else if (error.error && error.error.title) {
                errorMessage = error.error.title;
              } else if (error.message) {
                errorMessage = error.message;
              }

              return of(RegisterFailure({ error: errorMessage }));
            })
          )
        }
      })
    )
  );



  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(login),
      exhaustMap(({ email, password }) => {
        const loginRequest: LoginRequest = {
          email: email,
          password: password
        };

        return this.identityService.apiIdentityLoginPost(undefined, undefined, loginRequest, 'body', false).pipe(
          mergeMap((response: AccessTokenResponse) => {
            const user = {
              id: 1,
              email: email,
              token: response.accessToken
            };

            // Store tokens using TokenService
            this.tokenService.setTokens(response.accessToken, response.refreshToken);
            this.tokenService.setCurrentUser(user);

            // Initialize tenant context after successful login
            return this.tenantService.initializeTenantContext().pipe(
              map((hasTenants) => {
                if (hasTenants) {
                  // User has tenants, navigate to main app
                  this.router.navigate(['/app']);
                } else {
                  // User has no tenants, redirect to create tenant page
                  this.router.navigate(['/tenant']);
                }
                return loginSuccess({ user });
              })
            );
          }),
          catchError((error) => {
            console.error('Login error:', error);
            let errorMessage = 'Login failed. Please try again.';

            if (error.error && error.error.errors) {
              // Handle validation errors from API
              const errors = error.error.errors;
              if (errors.InvalidCredentials) {
                errorMessage = errors.InvalidCredentials[0];
              } else if (errors.DuplicateUserName) {
                errorMessage = errors.DuplicateUserName[0];
              } else {
                // Get first error message
                const firstError = Object.values(errors)[0];
                errorMessage = Array.isArray(firstError) ? firstError[0] : firstError;
              }
            } else if (error.error && error.error.title) {
              errorMessage = error.error.title;
            } else if (error.error && error.error.detail) {
              errorMessage = error.error.detail;
            } else if (error.message) {
              errorMessage = error.message;
            }

            return of(loginFailure({ error: errorMessage }));
          })
        )

      })
    )
  );


  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(logout),
      tap(() => {
        // Clear tokens using TokenService
        this.tokenService.clearTokens();
        // Clear tenant context
        this.tenantService.clearTenantContext();

        // Clear ALL localStorage values
        this.clearAllLocalStorage();
      }),
      exhaustMap(() => of(logoutSuccess()))
    )
  );

  /**
   * Clear all localStorage values
   */
  private clearAllLocalStorage(): void {
    try {
      // Clear all localStorage items
      localStorage.clear();
      console.log('All localStorage values cleared on logout');
    } catch (error) {
      console.error('Error clearing localStorage:', error);
    }
  }

  constructor(
    @Inject(Actions) private actions$: Actions,
    private identityService: IdentityService,
    private tokenService: TokenService,
    private userService: UserProfileService,
    private tenantService: TenantService,
    private router: Router) { }

}
