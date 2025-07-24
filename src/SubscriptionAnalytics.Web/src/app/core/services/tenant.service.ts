import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, catchError, mergeMap } from 'rxjs/operators';
import { TenantService as ApiTenantService, UserTenantDto, UserTenantsResponse, CreateTenantRequest, TenantDto } from '../../api-client';

@Injectable({ providedIn: 'root' })
export class TenantService {
  private currentTenantId: string | null = null;
  private currentTenantInfo: UserTenantDto | null = null;
  
  // LocalStorage keys
  private readonly CURRENT_TENANT_ID_KEY = 'currentTenantId';
  private readonly CURRENT_TENANT_INFO_KEY = 'currentTenantInfo';

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
        // No tenants found - this will be handled by the authentication effect
        return false;
      }),
      catchError(error => {
        console.error('Error initializing tenant context:', error);
        return of(false);
      })
    );
  }

  /**
   * Load tenant context from localStorage on app startup
   */
  loadTenantContextFromStorage(): void {
    try {
      const tenantId = localStorage.getItem(this.CURRENT_TENANT_ID_KEY);
      const tenantInfo = localStorage.getItem(this.CURRENT_TENANT_INFO_KEY);
      
      console.log('TenantService - Loading from localStorage - tenantId:', tenantId);
      console.log('TenantService - Loading from localStorage - tenantInfo:', tenantInfo);
      
      if (tenantId) {
        this.currentTenantId = tenantId;
        console.log('TenantService - Set currentTenantId to:', tenantId);
      }
      
      if (tenantInfo) {
        this.currentTenantInfo = JSON.parse(tenantInfo);
        console.log('TenantService - Set currentTenantInfo to:', this.currentTenantInfo);
      }
    } catch (error) {
      console.error('Error loading tenant context from storage:', error);
      this.clearTenantContext();
    }
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
    // If not loaded in memory, try to load from localStorage
    if (!this.currentTenantId) {
      console.log('TenantService - Loading tenant context from storage');
      this.loadTenantContextFromStorage();
    }
    console.log('TenantService - Current tenant ID:', this.currentTenantId);
    return this.currentTenantId;
  }

  /**
   * Set current tenant ID
   */
  setCurrentTenantId(tenantId: string): void {
    this.currentTenantId = tenantId;
    localStorage.setItem(this.CURRENT_TENANT_ID_KEY, tenantId);
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
    localStorage.setItem(this.CURRENT_TENANT_INFO_KEY, JSON.stringify(tenantInfo));
  }

  /**
   * Clear tenant context (used on logout)
   */
  clearTenantContext(): void {
    this.currentTenantId = null;
    this.currentTenantInfo = null;
    localStorage.removeItem(this.CURRENT_TENANT_ID_KEY);
    localStorage.removeItem(this.CURRENT_TENANT_INFO_KEY);
  }

  /**
   * Check if user has access to a specific tenant
   */
  hasAccessToTenant(tenantId: string): Observable<boolean> {
    // TODO: Implement API call to check user access to tenant
    return of(true);
  }

  /**
   * Create a new tenant for the current user
   */
  createTenant(request: CreateTenantRequest): Observable<TenantDto> {
    return this.apiTenantService.apiTenantPost(request).pipe(
      mergeMap((response: any) => {
        // After creating tenant, initialize tenant context
        return this.initializeTenantContext().pipe(
          map(() => response)
        );
      }),
      catchError(error => {
        console.error('Error creating tenant:', error);
        throw error;
      })
    );
  }
} 