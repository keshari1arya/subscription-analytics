import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LandingService, WaitlistSignup } from './landing.service';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit, AfterViewInit {
  signupForm: FormGroup;
  isSubmitting = false;
  submitted = false;
  
  // Typing animation texts
  private texts = [
    'Connect all payment providers',
    'Get unified subscription insights',
    'Launching soon',
    'Join the waitlist'
  ];
  private currentTextIndex = 0;
  private currentCharIndex = 0;
  private isDeleting = false;
  private typingSpeed = 80;
  private deletingSpeed = 40;
  private pauseTime = 1500;

  // Mock data for advanced dashboard
  churnPredictions = [
    { name: 'John Smith', plan: 'Pro Plan', risk: 85 },
    { name: 'Sarah Johnson', plan: 'Basic Plan', risk: 45 },
    { name: 'Mike Wilson', plan: 'Enterprise', risk: 92 },
    { name: 'Lisa Brown', plan: 'Pro Plan', risk: 23 }
  ];

  providerStats = [
    { name: 'Stripe', icon: 'fab fa-stripe', color: 'text-primary', revenue: '$28,450', transactions: '1,247' },
    { name: 'PayPal', icon: 'fab fa-paypal', color: 'text-info', revenue: '$12,230', transactions: '456' },
    { name: 'Square', icon: 'fas fa-square', color: 'text-success', revenue: '$8,920', transactions: '234' }
  ];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private landingService: LandingService
  ) {
    this.signupForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      company: ['', Validators.required],
      role: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // Component initialization
  }



  ngAfterViewInit(): void {
    this.startTypingAnimation();
    this.setupTypingContainer();
  }

  private setupTypingContainer(): void {
    const typingElement = document.getElementById('typing-text');
    const container = typingElement?.parentElement;
    
    if (typingElement && container) {
      // Calculate the maximum width needed for all texts
      const maxTextWidth = Math.max(...this.texts.map(text => {
        const tempSpan = document.createElement('span');
        tempSpan.style.font = window.getComputedStyle(typingElement).font;
        tempSpan.style.visibility = 'hidden';
        tempSpan.style.position = 'absolute';
        tempSpan.style.whiteSpace = 'nowrap';
        tempSpan.textContent = text;
        document.body.appendChild(tempSpan);
        const width = tempSpan.offsetWidth;
        document.body.removeChild(tempSpan);
        return width;
      }));
      
      // Get viewport width and ensure container doesn't exceed it
      const viewportWidth = window.innerWidth;
      const maxContainerWidth = Math.min(maxTextWidth + 60, viewportWidth - 50); // Leave 50px margin
      
      // Set the container width and center it
      container.style.width = `${maxContainerWidth}px`;
      container.style.margin = '0 auto';
      container.style.maxWidth = '100%';
    }
  }



  private startTypingAnimation(): void {
    const typingElement = document.getElementById('typing-text');
    if (!typingElement) return;

    const type = () => {
      const currentText = this.texts[this.currentTextIndex];
      
      if (this.isDeleting) {
        // Deleting text
        typingElement.textContent = currentText.substring(0, this.currentCharIndex - 1);
        this.currentCharIndex--;
      } else {
        // Typing text
        typingElement.textContent = currentText.substring(0, this.currentCharIndex + 1);
        this.currentCharIndex++;
      }

      // Add color classes for different texts and ensure cursor color matches
      if (this.currentTextIndex === 1) {
        typingElement.className = 'typing-text text-warning';
      } else if (this.currentTextIndex === 2) {
        typingElement.className = 'typing-text text-info';
      } else if (this.currentTextIndex === 3) {
        typingElement.className = 'typing-text text-success';
      } else if (this.currentTextIndex === 4) {
        typingElement.className = 'typing-text text-primary';
      } else if (this.currentTextIndex === 5) {
        typingElement.className = 'typing-text text-light';
      } else {
        typingElement.className = 'typing-text';
      }

      let speed = this.typingSpeed;

      if (this.isDeleting) {
        speed = this.deletingSpeed;
      }

      if (!this.isDeleting && this.currentCharIndex === currentText.length) {
        // Pause at end of typing
        speed = this.pauseTime;
        this.isDeleting = true;
      } else if (this.isDeleting && this.currentCharIndex === 0) {
        // Move to next text
        this.isDeleting = false;
        this.currentTextIndex = (this.currentTextIndex + 1) % this.texts.length;
        speed = 500; // Pause before starting next text
      }

      setTimeout(type, speed);
    };

    type();
  }

  // Helper methods for chart styling
  getRetentionColor(retention: number): string {
    if (retention >= 80) return '#28a745';
    if (retention >= 60) return '#ffc107';
    return '#dc3545';
  }

  getRetentionOpacity(index: number): number {
    return 1 - (index * 0.15);
  }

  getRiskClass(risk: number): string {
    if (risk >= 80) return 'high-risk';
    if (risk >= 50) return 'medium-risk';
    return 'low-risk';
  }

  onSubmit(): void {
    if (this.signupForm.valid) {
      this.isSubmitting = true;
      
      const signup: WaitlistSignup = {
        email: this.signupForm.value.email,
        company: this.signupForm.value.company,
        role: this.signupForm.value.role,
        timestamp: new Date()
      };
      
      this.landingService.submitWaitlistSignup(signup).subscribe({
        next: (response) => {
          this.submitted = true;
          this.isSubmitting = false;
          
          // Store email for later use
          localStorage.setItem('early_access_email', signup.email);
          
          // Redirect to signup after 2 seconds
          setTimeout(() => {
            this.router.navigate(['/auth/register']);
          }, 2000);
        },
        error: (error) => {
          console.error('Error submitting signup:', error);
          this.isSubmitting = false;
          // Handle error - could show a toast notification
        }
      });
    }
  }

  scrollToSection(sectionId: string): void {
    console.log('scrollToSection called with:', sectionId);
    const element = document.getElementById(sectionId);
    if (element) {
      console.log('Element found, scrolling to:', element);
      element.scrollIntoView({ behavior: 'smooth' });
    } else {
      console.log('Element not found for id:', sectionId);
    }
  }
} 