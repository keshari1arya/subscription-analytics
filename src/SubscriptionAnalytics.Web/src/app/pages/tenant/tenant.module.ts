import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { TenantManageComponent } from './tenant-manage/tenant-manage.component';
import { TenantSettingsComponent } from './tenant-settings/tenant-settings.component';
import { CreateTenantComponent } from './create-tenant/create-tenant.component';

const routes: Routes = [
  {
    path: 'manage',
    component: TenantManageComponent
  },
  {
    path: 'settings',
    component: TenantSettingsComponent
  },
  {
    path: 'create',
    component: CreateTenantComponent
  },
  {
    path: '',
    component: TenantManageComponent
  }
];

@NgModule({
  declarations: [
    TenantManageComponent,
    TenantSettingsComponent,
    CreateTenantComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class TenantModule { } 