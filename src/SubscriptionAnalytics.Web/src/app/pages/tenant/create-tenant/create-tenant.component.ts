import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { TenantService } from '../../../core/services/tenant.service';
import { CreateTenantRequest } from '../../../api-client';

@Component({
  selector: 'app-create-tenant',
  templateUrl: './create-tenant.component.html'
})
export class CreateTenantComponent implements OnInit {
  createTenantForm!: FormGroup;
  loading = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private tenantService: TenantService,
    private router: Router,
    private store: Store
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.createTenantForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]]
    });
  }

  onSubmit(): void {
    if (this.createTenantForm.valid) {
      this.loading = true;
      this.error = null;

      const request: CreateTenantRequest = {
        name: this.createTenantForm.get('name')?.value
      };

      this.tenantService.createTenant(request).subscribe({
        next: (tenant) => {
          console.log('Tenant created successfully:', tenant);
          this.loading = false;
          // Redirect to main application
          this.router.navigate(['/app']);
        },
        error: (error) => {
          console.error('Error creating tenant:', error);
          this.loading = false;
          this.error = error.error?.detail || 'Failed to create tenant. Please try again.';
        }
      });
    }
  }

  get name() {
    return this.createTenantForm.get('name');
  }
} 