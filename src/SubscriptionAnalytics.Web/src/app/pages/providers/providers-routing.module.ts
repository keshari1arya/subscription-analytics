import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProviderDashboardComponent } from './provider-dashboard/provider-dashboard.component';
import { OAuthCallbackComponent } from './oauth-callback/oauth-callback.component';

const routes: Routes = [
  {
    path: '',
    component: ProviderDashboardComponent
  },
  {
    path: 'oauth-callback',
    component: OAuthCallbackComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProvidersRoutingModule { } 