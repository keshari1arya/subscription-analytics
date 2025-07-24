import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

export interface WaitlistSignup {
  email: string;
  company: string;
  role: string;
  timestamp: Date;
}

@Injectable({
  providedIn: 'root'
})
export class LandingService {
  private readonly STORAGE_KEY = 'waitlist_signups';

  constructor(private http: HttpClient) {}

  submitWaitlistSignup(signup: WaitlistSignup): Observable<any> {
    // For now, we'll store locally for market validation
    // In production, this would call an API endpoint
    const signups = this.getStoredSignups();
    signups.push(signup);
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(signups));
    
    // Simulate API call
    return of({ success: true, message: 'Successfully joined waitlist!' }).pipe(
      delay(1000)
    );
  }

  getWaitlistStats(): Observable<any> {
    const signups = this.getStoredSignups();
    const stats = {
      totalSignups: signups.length,
      byRole: this.groupByRole(signups),
      byCompany: this.groupByCompany(signups),
      recentSignups: signups.slice(-5)
    };
    
    return of(stats);
  }

  private getStoredSignups(): WaitlistSignup[] {
    const stored = localStorage.getItem(this.STORAGE_KEY);
    return stored ? JSON.parse(stored) : [];
  }

  private groupByRole(signups: WaitlistSignup[]): any {
    return signups.reduce((acc, signup) => {
      acc[signup.role] = (acc[signup.role] || 0) + 1;
      return acc;
    }, {});
  }

  private groupByCompany(signups: WaitlistSignup[]): any {
    return signups.reduce((acc, signup) => {
      acc[signup.company] = (acc[signup.company] || 0) + 1;
      return acc;
    }, {});
  }
} 