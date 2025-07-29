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
