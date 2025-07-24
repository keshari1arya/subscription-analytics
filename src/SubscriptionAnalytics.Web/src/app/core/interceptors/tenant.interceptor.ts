import { HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { inject } from '@angular/core';
import { TenantService } from '../services/tenant.service';

export function tenantInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  const tenantService = inject(TenantService);
  
  // Only add tenant header for API requests
  console.log('TenantInterceptor - Request URL:', request.url);
  console.log('TenantInterceptor - API URL:', environment.apiUrl);
  console.log('TenantInterceptor - API Base URL:', environment.apiBaseUrl);
  
  if (request.url.startsWith(environment.apiUrl) || request.url.startsWith(environment.apiBaseUrl)) {
    // Get tenant ID from tenant service
    const tenantId = tenantService.getCurrentTenantId();
    console.log('TenantInterceptor - Tenant ID:', tenantId);
    
    if (tenantId) {
      // Clone the request and add the tenant header
      const modifiedRequest = request.clone({
        setHeaders: {
          'X-Tenant-Id': tenantId
        }
      });
      console.log('TenantInterceptor - Added header X-Tenant-Id:', tenantId);
      return next(modifiedRequest);
    } else {
      console.log('TenantInterceptor - No tenant ID found');
    }
  } else {
    console.log('TenantInterceptor - Not an API request, skipping');
  }
  
  return next(request);
} 