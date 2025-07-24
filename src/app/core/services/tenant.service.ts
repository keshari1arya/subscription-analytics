import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiTenantService, UserTenantDto, UserTenantsResponse } from '../../api-client';

@Injectable({ providedIn: 'root' })
export class TenantService {
  private currentTenantId: string | null = null;
  private currentTenantInfo: UserTenantDto | null = null;

  constructor(private apiTenantService: ApiTenantService) {}

  /**
   * Initialize tenant context after user login
   */
  initializeTenantContext(): Observable<boolean> {
    return this.getUserTenants().pipe(
      map(tenants => {
        if (tenants.length > 0) {
          const firstTenant = tenants[0];
          this.setCurrentTenantId(firstTenant.tenantId || '');
          this.setCurrentTenantInfo(firstTenant);
          return true;
        }
        return false;
      }),
      catchError(error => {
        console.error('Error initializing tenant context:', error);
        return of(false);
      })
    );
  }

  /**
   * Get all tenants for the current user
   */
  getUserTenants(): Observable<UserTenantDto[]> {
    return this.apiTenantService.apiTenantMyTenantsGet().pipe(
      map((response: UserTenantsResponse) => {
        return response.tenants || [];
      }),
      catchError(error => {
        console.error('Error fetching user tenants:', error);
        return of([]);
      })
    );
  }

  /**
   * Switch to a different tenant
   */
  switchTenant(tenantId: string): Observable<boolean> {
    // TODO: Implement API call to validate user access to this tenant
    return of(true).pipe(
      map(() => {
        this.setCurrentTenantId(tenantId);
        return true;
      }),
      catchError(error => {
        console.error('Error switching tenant:', error);
        return of(false);
      })
    );
  }

  /**
   * Get current tenant ID
   */
  getCurrentTenantId(): string | null {
    return this.currentTenantId;
  }

  /**
   * Set current tenant ID
   */
  setCurrentTenantId(tenantId: string): void {
    this.currentTenantId = tenantId;
  }

  /**
   * Get current tenant info
   */
  getCurrentTenantInfo(): UserTenantDto | null {
    return this.currentTenantInfo;
  }

  /**
   * Set current tenant info
   */
  setCurrentTenantInfo(tenantInfo: UserTenantDto): void {
    this.currentTenantInfo = tenantInfo;
  }

  /**
   * Clear tenant context (used on logout)
   */
  clearTenantContext(): void {
    this.currentTenantId = null;
    this.currentTenantInfo = null;
  }

  /**
   * Check if user has access to a specific tenant
   */
  hasAccessToTenant(tenantId: string): Observable<boolean> {
    // TODO: Implement API call to check user access to tenant
    return of(true);
  }
} 

function Injectable(arg0: { providedIn: string; }): (target: typeof TenantService, context: ClassDecoratorContext<typeof TenantService>) => void | typeof TenantService {
    throw new Error("Function not implemented.");
}


function of(arg0: boolean) {
    throw new Error("Function not implemented.");
}


function map(arg0: (response: UserTenantsResponse) => any): any {
    throw new Error("Function not implemented.");
}


function catchError(arg0: (error: any) => any): any {
    throw new Error("Function not implemented.");
}
