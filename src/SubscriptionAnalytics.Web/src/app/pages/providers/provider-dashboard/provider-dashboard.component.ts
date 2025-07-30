import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsModalRef, BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { Observable, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { ConnectorInfo, ProviderService, SyncJobResponseDto, SyncJobStatusResponseDto } from 'src/app/api-client';
import { LoaderComponent } from 'src/app/shared/ui/loader/loader.component';
import { PagetitleComponent } from 'src/app/shared/ui/pagetitle/pagetitle.component';
import * as ProvidersActions from 'src/app/store/providers/providers.actions';
import * as ProvidersSelectors from 'src/app/store/providers/providers.selectors';


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

  // Sync state
  syncingProviders: Set<string> = new Set();
  syncProgress: { [key: string]: number } = {};
  syncErrors: { [key: string]: string } = {};
  syncJobs: { [key: string]: string } = {};

  breadCrumbItems: Array<{}> = [
    { label: 'Dashboard', path: '/dashboard' },
    { label: 'Integrations', path: '/providers' },
    { label: 'Connect Apps', active: true }
  ];

  constructor(
    private store: Store,
    private modalService: BsModalService,
    private providerService: ProviderService
  ) {
    this.providers$ = this.store.select(ProvidersSelectors.selectProviders);
    this.loading$ = this.store.select(ProvidersSelectors.selectProvidersLoading);
    this.error$ = this.store.select(ProvidersSelectors.selectProvidersError);
    this.installingProvider$ = this.store.select(ProvidersSelectors.selectInstallingProvider);
    this.connections$ = this.store.select(ProvidersSelectors.selectConnections);

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

  // Sync functionality
  startSync(providerName: string) {
    if (!providerName || this.syncingProviders.has(providerName)) {
      return;
    }

    this.syncingProviders.add(providerName);
    this.syncProgress[providerName] = 0;
    this.syncErrors[providerName] = '';

    this.providerService.apiProviderProviderSyncPost(providerName).subscribe({
      next: (response: SyncJobResponseDto) => {
        if (response.jobId) {
          this.syncJobs[providerName] = response.jobId;
          this.monitorSyncProgress(providerName, response.jobId);
        }
      },
      error: (error) => {
        this.handleSyncError(providerName, 'Failed to start sync: ' + (error.error?.message || error.message));
      }
    });
  }

  private monitorSyncProgress(providerName: string, jobId: string) {
    const interval = setInterval(() => {
      this.providerService.apiProviderSyncStatusJobIdGet(jobId).subscribe({
        next: (status: SyncJobStatusResponseDto) => {
          this.updateSyncProgress(providerName, status);

          if (status.status === 'Completed' || status.status === 'Failed' || status.status === 'Cancelled') {
            clearInterval(interval);
            this.syncingProviders.delete(providerName);
            delete this.syncJobs[providerName];
          }
        },
        error: (error) => {
          clearInterval(interval);
          this.handleSyncError(providerName, 'Failed to get sync status: ' + (error.error?.message || error.message));
        }
      });
    }, 2000); // Check every 2 seconds
  }

  private updateSyncProgress(providerName: string, status: SyncJobStatusResponseDto) {
    if (status.status === 'Running') {
      // Use the progress property from the API response
      if (status.progress !== undefined) {
        this.syncProgress[providerName] = status.progress;
      } else {
        // Fallback to simple progress
        this.syncProgress[providerName] = Math.min(this.syncProgress[providerName] + 10, 90);
      }
    } else if (status.status === 'Completed') {
      this.syncProgress[providerName] = 100;
      setTimeout(() => {
        delete this.syncProgress[providerName];
      }, 2000);
    } else if (status.status === 'Failed') {
      this.handleSyncError(providerName, status.errorMessage || 'Sync failed');
    }
  }

  private handleSyncError(providerName: string, error: string) {
    this.syncingProviders.delete(providerName);
    this.syncErrors[providerName] = error;
    delete this.syncProgress[providerName];
    delete this.syncJobs[providerName];
  }

  cancelSync(providerName: string) {
    const jobId = this.syncJobs[providerName];
    if (jobId) {
      this.providerService.apiProviderSyncJobIdDelete(jobId).subscribe({
        next: () => {
          this.syncingProviders.delete(providerName);
          delete this.syncProgress[providerName];
          delete this.syncErrors[providerName];
          delete this.syncJobs[providerName];
        },
        error: (error) => {
          this.handleSyncError(providerName, 'Failed to cancel sync: ' + (error.error?.message || error.message));
        }
      });
    }
  }

  retrySync(providerName: string) {
    delete this.syncErrors[providerName];
    this.startSync(providerName);
  }

  isSyncing(providerName: string): boolean {
    return this.syncingProviders.has(providerName);
  }

  getSyncProgress(providerName: string): number {
    return this.syncProgress[providerName] || 0;
  }

  getSyncError(providerName: string): string {
    return this.syncErrors[providerName] || '';
  }

  getProviderIcon(providerName: string): string {
    // Return appropriate icon based on provider name
    switch (providerName?.toLowerCase()) {
      case 'stripe':
        return 'assets/images/brands/stripe.png';
      case 'paypal':
        return 'assets/images/brands/paypal.png';
      default:
        return 'assets/images/brands/default-provider.png';
    }
  }

  getProviderDescription(providerName: string): string {
    switch (providerName?.toLowerCase()) {
      case 'stripe':
        return 'Connect your Stripe account to sync payment data and subscriptions.';
      case 'paypal':
        return 'Connect your PayPal account to sync payment data and subscriptions.';
      default:
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
