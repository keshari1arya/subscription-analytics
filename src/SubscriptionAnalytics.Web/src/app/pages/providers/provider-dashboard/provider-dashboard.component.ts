import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsModalRef, BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { ConnectorInfo } from 'src/app/api-client';
import { LoaderComponent } from 'src/app/shared/ui/loader/loader.component';
import { PagetitleComponent } from 'src/app/shared/ui/pagetitle/pagetitle.component';
import * as ProvidersActions from 'src/app/store/providers/providers.actions';
import * as ProvidersSelectors from 'src/app/store/providers/providers.selectors';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-provider-dashboard',
  templateUrl: './provider-dashboard.component.html',
  styleUrls: ['./provider-dashboard.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    PagetitleComponent,
    LoaderComponent,
    ModalModule,
    BsDropdownModule,
    AlertModule
  ],
  providers: [BsModalService]
})
export class ProviderDashboardComponent implements OnInit {
  providers$: Observable<ConnectorInfo[]>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  installingProvider$: Observable<string | null>;
  connections$: Observable<any[]>;
  providersWithStatus$: Observable<any[]>;
  modalRef?: BsModalRef;

  // Sync state from store
  syncingProviders$: Observable<Set<string>>;
  syncProgress$: Observable<{ [key: string]: number }>;
  syncErrors$: Observable<{ [key: string]: string }>;
  syncJobs$: Observable<{ [key: string]: string }>;

  breadCrumbItems: Array<{}> = [
    { label: 'Dashboard', path: '/dashboard' },
    { label: 'Integrations', path: '/providers' },
    { label: 'Connect Apps', active: true }
  ];

  constructor(
    private store: Store,
    private modalService: BsModalService
  ) {
    this.providers$ = this.store.select(ProvidersSelectors.selectProviders);
    this.loading$ = this.store.select(ProvidersSelectors.selectProvidersLoading);
    this.error$ = this.store.select(ProvidersSelectors.selectProvidersError);
    this.installingProvider$ = this.store.select(ProvidersSelectors.selectInstallingProvider);
    this.connections$ = this.store.select(ProvidersSelectors.selectConnections);

    // Sync state selectors
    this.syncingProviders$ = this.store.select(ProvidersSelectors.selectSyncingProviders);
    this.syncProgress$ = this.store.select(ProvidersSelectors.selectSyncProgress);
    this.syncErrors$ = this.store.select(ProvidersSelectors.selectSyncErrors);
    this.syncJobs$ = this.store.select(ProvidersSelectors.selectSyncJobs);

    // Combine providers with connection status
    this.providersWithStatus$ = combineLatest([
      this.providers$,
      this.connections$
    ]).pipe(
      map(([providers, connections]) => {
        return providers.map(provider => ({
          ...provider,
          isConnected: connections.some(conn =>
            conn.providerName?.toLowerCase() === provider.providerName?.toLowerCase() &&
            conn.status === 'Connected'
          ),
          connection: connections.find(conn =>
            conn.providerName?.toLowerCase() === provider.providerName?.toLowerCase()
          )
        }));
      })
    );
  }

  ngOnInit() {
    this.loadProviders();
    this.loadConnections();
  }

  loadProviders() {
    this.store.dispatch(ProvidersActions.loadProviders());
  }

  loadConnections() {
    this.store.dispatch(ProvidersActions.loadConnections());
  }

  installProvider(provider: ConnectorInfo) {
    if (!provider.providerName) {
      return;
    }
    this.store.dispatch(ProvidersActions.installProvider({ providerName: provider.providerName }));
  }

  clearError() {
    this.store.dispatch(ProvidersActions.clearProviderError());
  }

  // Sync functionality using store
  startSync(providerName: string) {
    if (!providerName) {
      return;
    }
    this.store.dispatch(ProvidersActions.startSync({ providerName }));
  }

  cancelSync(providerName: string) {
    this.syncJobs$.subscribe(syncJobs => {
      const jobId = syncJobs[providerName];
      if (jobId) {
        this.store.dispatch(ProvidersActions.cancelSync({ providerName, jobId }));
      }
    }).unsubscribe();
  }

  retrySync(providerName: string) {
    // Clear error and start sync again
    this.store.dispatch(ProvidersActions.startSync({ providerName }));
  }

  disconnectProvider(provider: any) {
    if (!provider.providerName) {
      return;
    }

    Swal.fire({
      title: 'Disconnect Provider?',
      text: `Are you sure you want to disconnect ${provider.displayName || provider.providerName}? This will remove all connection data.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, disconnect!',
      cancelButtonText: 'Cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        // Dispatch disconnect action
        this.store.dispatch(ProvidersActions.disconnectProvider({ providerName: provider.providerName }));

        Swal.fire(
          'Disconnected!',
          `${provider.displayName || provider.providerName} has been disconnected.`,
          'success'
        );
      }
    });
  }

  // Helper methods for template
  isSyncing(providerName: string): Observable<boolean> {
    return this.syncingProviders$.pipe(
      map(syncingProviders => syncingProviders.has(providerName))
    );
  }

  getSyncProgress(providerName: string): Observable<number> {
    return this.syncProgress$.pipe(
      map(syncProgress => syncProgress[providerName] || 0)
    );
  }

  getSyncError(providerName: string): Observable<string> {
    return this.syncErrors$.pipe(
      map(syncErrors => syncErrors[providerName] || '')
    );
  }

  getProviderIcon(providerName: string): string {
    // Return appropriate icon based on provider name
    switch (providerName?.toLowerCase()) {
      case 'stripe':
        return 'assets/images/brands/stripe.svg';
      case 'paypal':
        return 'assets/images/brands/paypal.svg';
      default:
        return 'assets/images/brands/default-provider.png';
    }
  }

  getProviderDescription(provider: any): string {
    const providerName = provider.providerName?.toLowerCase();

    switch (providerName) {
      case 'stripe':
        if (provider.isConnected) {
          return 'Your Stripe account is connected and ready to sync payment data and subscriptions.';
        }
        return 'Connect your Stripe account to sync payment data and subscriptions.';
      case 'paypal':
        if (provider.isConnected) {
          return 'Your PayPal account is connected and ready to sync payment data and subscriptions.';
        }
        return 'Connect your PayPal account to sync payment data and subscriptions.';
      default:
        if (provider.isConnected) {
          return 'Your account is connected and ready to sync data and subscriptions.';
        }
        return 'Connect your account to sync data and subscriptions.';
    }
  }

  getConnectionStatus(provider: any): string {
    if (provider.isConnected) {
      return 'Connected';
    }
    return 'Not Connected';
  }

  getConnectionStatusClass(provider: any): string {
    if (provider.isConnected) {
      return 'success';
    }
    return 'secondary';
  }

  getConnectionDate(provider: any): string {
    if (provider.connection?.connectedAt) {
      return new Date(provider.connection.connectedAt).toLocaleDateString();
    }
    return '';
  }
}
