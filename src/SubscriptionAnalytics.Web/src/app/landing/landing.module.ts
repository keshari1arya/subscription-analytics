import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { LandingComponent } from './landing.component';
import { LandingNavComponent } from './landing-nav/landing-nav.component';
import { WaitlistAdminComponent } from './waitlist-admin/waitlist-admin.component';

const routes: Routes = [
  {
    path: '',
    component: LandingComponent
  },
  {
    path: 'admin',
    component: WaitlistAdminComponent
  }
];

@NgModule({
  declarations: [
    LandingComponent,
    LandingNavComponent,
    WaitlistAdminComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class LandingModule { } 