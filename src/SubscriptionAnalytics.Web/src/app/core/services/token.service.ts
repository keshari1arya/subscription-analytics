import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface TokenData {
  accessToken: string;
  refreshToken?: string;
  expiresAt?: number;
}

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly ACCESS_TOKEN_KEY = 'accessToken';
  private readonly REFRESH_TOKEN_KEY = 'refreshToken';
  private readonly TOKEN_EXPIRY_KEY = 'tokenExpiry';
  private readonly CURRENT_USER_KEY = 'currentUser';

  private tokenSubject = new BehaviorSubject<TokenData | null>(this.getTokensFromStorage());
  public tokens$ = this.tokenSubject.asObservable();

  /**
   * Store tokens in localStorage and update subject
   */
  setTokens(accessToken: string, refreshToken?: string, expiresAt?: number): void {
    const tokenData: TokenData = {
      accessToken,
      refreshToken,
      expiresAt
    };

    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    if (refreshToken) {
      localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
    }
    if (expiresAt) {
      localStorage.setItem(this.TOKEN_EXPIRY_KEY, expiresAt.toString());
    }

    this.tokenSubject.next(tokenData);
  }

  /**
   * Get access token from localStorage
   */
  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  /**
   * Get refresh token from localStorage
   */
  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  /**
   * Get token expiry time
   */
  getTokenExpiry(): number | null {
    const expiry = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
    return expiry ? parseInt(expiry, 10) : null;
  }

  /**
   * Check if token is valid (not expired)
   */
  isTokenValid(): boolean {
    const token = this.getAccessToken();
    if (!token) return false;

    const expiry = this.getTokenExpiry();
    if (!expiry) return true; // If no expiry set, assume valid

    return Date.now() < expiry;
  }

  /**
   * Clear all tokens from localStorage
   */
  clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.TOKEN_EXPIRY_KEY);
    localStorage.removeItem(this.CURRENT_USER_KEY);
    
    this.tokenSubject.next(null);
  }

  /**
   * Get current user from localStorage
   */
  getCurrentUser(): any | null {
    const userStr = localStorage.getItem(this.CURRENT_USER_KEY);
    if (userStr) {
      try {
        return JSON.parse(userStr);
      } catch (e) {
        console.error('Error parsing user from storage:', e);
        return null;
      }
    }
    return null;
  }

  /**
   * Set current user in localStorage
   */
  setCurrentUser(user: any): void {
    localStorage.setItem(this.CURRENT_USER_KEY, JSON.stringify(user));
  }

  /**
   * Get tokens from localStorage for initial state
   */
  private getTokensFromStorage(): TokenData | null {
    const accessToken = this.getAccessToken();
    if (!accessToken) return null;

    return {
      accessToken,
      refreshToken: this.getRefreshToken(),
      expiresAt: this.getTokenExpiry()
    };
  }

  /**
   * Check if user is logged in
   */
  isLoggedIn(): boolean {
    return this.getAccessToken() !== null && this.isTokenValid();
  }

  /**
   * Logout the user
   */
  logout(): void {
    this.clearTokens();
  }
} 