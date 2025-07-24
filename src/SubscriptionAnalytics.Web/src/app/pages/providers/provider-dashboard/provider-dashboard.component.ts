import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { SubscriptionAnalyticsApiControllersConnectorInfo } from 'src/app/api-client/model/subscriptionAnalyticsApiControllersConnectorInfo';
import { PagetitleComponent } from 'src/app/shared/ui/pagetitle/pagetitle.component';
import { LoaderComponent } from 'src/app/shared/ui/loader/loader.component';
import { BsModalService, BsModalRef, ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AlertModule } from 'ngx-bootstrap/alert';
import { Observable } from 'rxjs';
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
  ]
})
export class ProviderDashboardComponent implements OnInit {
  providers$: Observable<SubscriptionAnalyticsApiControllersConnectorInfo[]>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  installingProvider$: Observable<string | null>;
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
  }

  ngOnInit() {
    this.loadProviders();
  }

  loadProviders() {
    this.store.dispatch(ProvidersActions.loadProviders());
  }

  installProvider(provider: SubscriptionAnalyticsApiControllersConnectorInfo) {
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
} 