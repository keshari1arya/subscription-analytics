import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing-nav',
  templateUrl: './landing-nav.component.html'
})
export class LandingNavComponent {
  constructor(private router: Router) {}

  scrollToSection(sectionId: string): void {
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }

  goToSignup(): void {
    this.router.navigate(['/auth/register']);
  }

  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
} 