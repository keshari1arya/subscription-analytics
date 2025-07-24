import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { TenantService } from '../services/tenant.service';

@Injectable({
  providedIn: 'root'
})
export class TenantGuard implements CanActivate {
  constructor(
    private tenantService: TenantService,
    private router: Router
  ) {}

  canActivate(): Observable<boolean> {
    return this.tenantService.getUserTenants().pipe(
      map(tenants => {
        if (tenants.length === 0) {
          // No tenants found, redirect to create tenant page
          this.router.navigate(['/tenant']);
          return false;
        }
        
        // User has tenants, allow access
        return true;
      }),
      catchError(error => {
        console.error('Error checking user tenants:', error);
        // On error, redirect to create tenant page
        this.router.navigate(['/tenant']);
        return of(false);
      })
    );
  }
} 