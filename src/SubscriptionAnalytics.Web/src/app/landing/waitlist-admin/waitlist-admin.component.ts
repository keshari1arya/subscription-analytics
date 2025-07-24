import { Component, OnInit } from '@angular/core';
import { LandingService } from '../landing.service';

@Component({
  selector: 'app-waitlist-admin',
  templateUrl: './waitlist-admin.component.html'
})
export class WaitlistAdminComponent implements OnInit {
  stats: any = {};
  loading = true;

  constructor(private landingService: LandingService) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.loading = true;
    this.landingService.getWaitlistStats().subscribe({
      next: (stats) => {
        this.stats = stats;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading stats:', error);
        this.loading = false;
      }
    });
  }

  exportData(): void {
    const data = localStorage.getItem('waitlist_signups');
    if (data) {
      const blob = new Blob([data], { type: 'application/json' });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'waitlist-data.json';
      a.click();
      window.URL.revokeObjectURL(url);
    }
  }

  clearData(): void {
    if (confirm('Are you sure you want to clear all waitlist data?')) {
      localStorage.removeItem('waitlist_signups');
      this.loadStats();
    }
  }

  getObjectKeys(obj: any): string[] {
    return Object.keys(obj || {});
  }
} 