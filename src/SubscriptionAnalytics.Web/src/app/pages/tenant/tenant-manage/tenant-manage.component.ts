import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TenantService } from '../../../core/services/tenant.service';
import { UserTenantDto } from '../../../api-client';

@Component({
  selector: 'app-tenant-manage',
  standalone: false,
  templateUrl: './tenant-manage.component.html'
})
export class TenantManageComponent implements OnInit {
  currentTenant: UserTenantDto | null = null;
  loading = false;
  error: string | null = null;

  constructor(
    private tenantService: TenantService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCurrentTenant();
  }

  loadCurrentTenant(): void {
    this.loading = true;
    this.error = null;
    
    // Get current tenant info from service
    this.currentTenant = this.tenantService.getCurrentTenantInfo();
    
    if (this.currentTenant) {
      this.loading = false;
    } else {
      // If no tenant info in memory, try to load from API
      this.tenantService.getUserTenants().subscribe({
        next: (tenants) => {
          if (tenants.length > 0) {
            this.currentTenant = tenants[0];
            this.tenantService.setCurrentTenantInfo(tenants[0]);
          }
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load tenant information';
          this.loading = false;
          console.error('Error loading tenant:', err);
        }
      });
    }
  }

  createTenant(): void {
    this.router.navigate(['/tenant/create']);
  }
} 