import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { PagetitleComponent } from 'src/app/shared/ui/pagetitle/pagetitle.component';
import { LoaderComponent } from 'src/app/shared/ui/loader/loader.component';
import { AlertModule } from 'ngx-bootstrap/alert';
import * as ProvidersActions from 'src/app/store/providers/providers.actions';
import * as ProvidersSelectors from 'src/app/store/providers/providers.selectors';

@Component({
  selector: 'app-oauth-callback',
  templateUrl: './oauth-callback.component.html',
  standalone: true,
  imports: [
    CommonModule,
    PagetitleComponent,
    LoaderComponent,
    AlertModule
  ]
})
export class OAuthCallbackComponent implements OnInit {
  loading = true;
  error: string | null = null;
  success = false;
  provider: string = '';
  processingMessage = '';

  breadCrumbItems: Array<{}> = [
    { label: 'Dashboard', path: '/dashboard' },
    { label: 'Providers', path: '/providers' },
    { label: 'Connecting...', active: true }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private store: Store
  ) {}

  ngOnInit() {
    this.handleOAuthCallback();
  }

  private handleOAuthCallback() {
    // Get query parameters from the callback URL
    this.route.queryParams.subscribe(params => {
      const provider = params['provider'];
      const code = params['code'];
      const state = params['state'];
      const error = params['error'];
      const errorDescription = params['error_description'];

      this.provider = provider || 'unknown';

      if (error) {
        this.handleError(error, errorDescription);
        return;
      }

      if (!code) {
        this.handleError('missing_code', 'Authorization code is missing');
        return;
      }

      this.processOAuthCallback(provider, code, state);
    });
  }

           private processOAuthCallback(provider: string, code: string, state: string) {
           this.processingMessage = `Processing your ${provider} connection...`;
       
           // Dispatch NGRX action to handle OAuth callback
           this.store.dispatch(ProvidersActions.handleOAuthCallback({ provider, code, state }));
           
           // Subscribe to the result
           this.store.select(ProvidersSelectors.selectOAuthCallbackLoading).subscribe(loading => {
             if (!loading) {
               this.store.select(ProvidersSelectors.selectOAuthCallbackError).subscribe(error => {
                 if (error) {
                   this.handleError('api_error', error);
                 } else {
                   this.success = true;
                   this.loading = false;
                   this.processingMessage = `${provider} connected successfully!`;
                   
                   // Auto-redirect after 3 seconds
                   setTimeout(() => {
                     this.router.navigate(['/providers']);
                   }, 3000);
                 }
               });
             }
           });
         }

  private handleError(errorCode: string, errorMessage: string) {
    this.loading = false;
    this.error = this.getErrorMessage(errorCode, errorMessage);
  }

  private getErrorMessage(errorCode: string, errorMessage: string): string {
    switch (errorCode) {
      case 'access_denied':
        return 'Connection was cancelled. You can try again anytime.';
      case 'missing_code':
        return 'Authorization code is missing. Please try connecting again.';
      case 'api_error':
        return `Connection failed: ${errorMessage}`;
      default:
        return `Connection failed: ${errorMessage}`;
    }
  }

  retryConnection() {
    this.loading = true;
    this.error = null;
    this.router.navigate(['/providers']);
  }

  goToProviders() {
    this.router.navigate(['/providers']);
  }
} 