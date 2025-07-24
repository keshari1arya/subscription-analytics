import { Injectable } from '@angular/core';
import { TenantService } from './tenant.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AppInitService {

  constructor(
    private tenantService: TenantService,
    private tokenService: TokenService
  ) {}

  /**
   * Initialize app data from localStorage
   */
  initializeApp(): void {
    // Load tenant context from localStorage if user is logged in
    if (this.tokenService.isLoggedIn()) {
      this.tenantService.loadTenantContextFromStorage();
    }
  }
} 