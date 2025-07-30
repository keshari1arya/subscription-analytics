import { TestBed } from '@angular/core/testing';
import { TokenService } from './token.service';

describe('TokenService', () => {
  let service: TokenService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TokenService);

    // Clear localStorage before each test
    localStorage.clear();
  });

  afterEach(() => {
    // Clear localStorage after each test
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should clear all localStorage values on logout', () => {
    // Set some test data in localStorage
    localStorage.setItem('accessToken', 'test-access-token');
    localStorage.setItem('refreshToken', 'test-refresh-token');
    localStorage.setItem('currentUser', JSON.stringify({ id: 1, email: 'test@example.com' }));
    localStorage.setItem('currentTenantId', 'test-tenant-id');
    localStorage.setItem('currentTenantInfo', JSON.stringify({ tenantId: 'test-tenant-id', name: 'Test Tenant' }));
    localStorage.setItem('early_access_email', 'test@example.com');
    localStorage.setItem('test-key', 'test-value');

    // Verify data was set
    expect(localStorage.getItem('accessToken')).toBe('test-access-token');
    expect(localStorage.getItem('refreshToken')).toBe('test-refresh-token');
    expect(localStorage.getItem('currentUser')).toBeTruthy();
    expect(localStorage.getItem('currentTenantId')).toBe('test-tenant-id');
    expect(localStorage.getItem('currentTenantInfo')).toBeTruthy();
    expect(localStorage.getItem('early_access_email')).toBe('test@example.com');
    expect(localStorage.getItem('test-key')).toBe('test-value');

    // Call logout
    service.logout();

    // Verify all localStorage values are cleared
    expect(localStorage.getItem('accessToken')).toBeNull();
    expect(localStorage.getItem('refreshToken')).toBeNull();
    expect(localStorage.getItem('currentUser')).toBeNull();
    expect(localStorage.getItem('currentTenantId')).toBeNull();
    expect(localStorage.getItem('currentTenantInfo')).toBeNull();
    expect(localStorage.getItem('early_access_email')).toBeNull();
    expect(localStorage.getItem('test-key')).toBeNull();
  });

  it('should clear tokens and user data on clearTokens', () => {
    // Set test data
    service.setTokens('test-access-token', 'test-refresh-token', Date.now() + 3600000);
    service.setCurrentUser({ id: 1, email: 'test@example.com' });

    // Verify data was set
    expect(service.getAccessToken()).toBe('test-access-token');
    expect(service.getRefreshToken()).toBe('test-refresh-token');
    expect(service.getCurrentUser()).toBeTruthy();

    // Call clearTokens
    service.clearTokens();

    // Verify tokens and user data are cleared
    expect(service.getAccessToken()).toBeNull();
    expect(service.getRefreshToken()).toBeNull();
    expect(service.getCurrentUser()).toBeNull();
  });
});
