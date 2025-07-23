import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { login } from '../../../store/Authentication/authentication.actions';

import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthenticationState } from '../../../store/Authentication/authentication.reducer';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone:true,
  imports:[CommonModule,FormsModule,ReactiveFormsModule]
})

/**
 * Login component
 */
export class LoginComponent implements OnInit, OnDestroy {

  loginForm: UntypedFormGroup;
  submitted: any = false;
  returnUrl: string;
  fieldTextType!: boolean;
  
  // Store observables
  authState$: Observable<AuthenticationState>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  
  private subscriptions = new Subscription();

  // set the currenr year
  year: number = new Date().getFullYear();

  // tslint:disable-next-line: max-line-length
  constructor(private formBuilder: UntypedFormBuilder, private route: ActivatedRoute, private router: Router, private store: Store) { 
    // Initialize store observables
    this.authState$ = this.store.select((state: any) => state.auth);
    this.loading$ = this.store.select((state: any) => state.auth.loading);
    this.error$ = this.store.select((state: any) => state.auth.error);
  }

  ngOnInit() {
    if (localStorage.getItem('currentUser')) {
      this.router.navigate(['/']);
    }
    // form validation
    this.loginForm = this.formBuilder.group({
      email: ['admin@themesbrand.com', [Validators.required, Validators.email]],
      password: ['123456', [Validators.required]],
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }

  /**
   * Form submit
   */
  onSubmit() {
    this.submitted = true;

    const email = this.f['email'].value; // Get the email from the form
    const password = this.f['password'].value; // Get the password from the form

    // Dispatch login action
    this.store.dispatch(login({ email, password }));
  }

  /**
 * Password Hide/Show
 */
  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}
