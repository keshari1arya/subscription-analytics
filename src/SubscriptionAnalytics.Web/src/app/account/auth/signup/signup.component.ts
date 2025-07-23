import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

// import { AuthenticationService } from '../../../core/services/auth.service';
import { environment } from '../../../../environments/environment';
import { first } from 'rxjs/operators';
import { UserProfileService } from '../../../core/services/user.service';
import { Store } from '@ngrx/store';
import { Register, RegisterFailure } from 'src/app/store/Authentication/authentication.actions';
import { CommonModule } from '@angular/common';
import { PasswordValidator } from '../../../core/validators/password.validator';
import { AuthenticationState } from 'src/app/store/Authentication/authentication.reducer';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
  standalone:true,
  imports:[CommonModule,FormsModule,ReactiveFormsModule]
})
export class SignupComponent implements OnInit, OnDestroy {

  signupForm: UntypedFormGroup;
  submitted: any = false;
  error: any = '';
  successmsg: any = false;
  passwordStrength: number = 0;
  passwordStrengthLabel: string = '';
  passwordStrengthColor: string = 'danger';
  
  // Store observables
  authState$: Observable<AuthenticationState>;
  loading$: Observable<boolean>;
  error$: Observable<string | null>;
  
  private subscriptions = new Subscription();

  // set the currenr year
  year: number = new Date().getFullYear();

  // tslint:disable-next-line: max-line-length
  constructor(private formBuilder: UntypedFormBuilder, private route: ActivatedRoute, private router: Router,
    private userService: UserProfileService, public store: Store) { 
    // Initialize store observables
    this.authState$ = this.store.select((state: any) => state.auth);
    this.loading$ = this.store.select((state: any) => state.auth.registerLoading);
    this.error$ = this.store.select((state: any) => state.auth.error);
  }

  ngOnInit() {
    this.signupForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, PasswordValidator.strongPassword()]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });

    // Subscribe to password changes for strength indicator
    this.signupForm.get('password')?.valueChanges.subscribe(password => {
      this.updatePasswordStrength(password);
    });
  }

  updatePasswordStrength(password: string) {
    this.passwordStrength = PasswordValidator.getPasswordStrength(password);
    this.passwordStrengthLabel = PasswordValidator.getPasswordStrengthLabel(password);
    this.passwordStrengthColor = PasswordValidator.getPasswordStrengthColor(password);
  }

  passwordMatchValidator(form: UntypedFormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
    
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    
    return null;
  }

  // convenience getter for easy access to form fields
  get signupFormControls() { return this.signupForm.controls; }

  /**
   * On submit form
   */
  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.signupForm.invalid) {
      return;
    }

    const email = this.signupFormControls['email'].value;
    const password = this.signupFormControls['password'].value;
    const confirmPassword = this.signupFormControls['confirmPassword'].value;

    if (password !== confirmPassword) {
      // Dispatch error action for password mismatch
      this.store.dispatch(RegisterFailure({ error: 'Passwords do not match' }));
      return;
    }

    //Dispatch Action
    this.store.dispatch(Register({ email: email, password: password }));
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}
