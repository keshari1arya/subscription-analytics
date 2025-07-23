import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class PasswordValidator {
  /**
   * Custom password validator that checks:
   * - At least 6 characters long
   * - At least one uppercase character
   * - At least one lowercase character
   * - At least one digit
   * - At least one non-alphanumeric character
   */
  static strongPassword(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null; // Let required validator handle empty values
      }

      const password = control.value;
      const errors: ValidationErrors = {};

      // Check minimum length
      if (password.length < 6) {
        errors['minLength'] = { requiredLength: 6, actualLength: password.length };
      }

      // Check for uppercase character
      if (!/[A-Z]/.test(password)) {
        errors['uppercase'] = { message: 'Password must contain at least one uppercase letter' };
      }

      // Check for lowercase character
      if (!/[a-z]/.test(password)) {
        errors['lowercase'] = { message: 'Password must contain at least one lowercase letter' };
      }

      // Check for digit
      if (!/\d/.test(password)) {
        errors['digit'] = { message: 'Password must contain at least one digit' };
      }

      // Check for non-alphanumeric character
      if (!/[^A-Za-z0-9]/.test(password)) {
        errors['specialChar'] = { message: 'Password must contain at least one special character' };
      }

      return Object.keys(errors).length > 0 ? errors : null;
    };
  }

  /**
   * Get password strength score (0-4)
   */
  static getPasswordStrength(password: string): number {
    if (!password) return 0;

    let score = 0;
    
    if (password.length >= 6) score++;
    if (/[A-Z]/.test(password)) score++;
    if (/[a-z]/.test(password)) score++;
    if (/\d/.test(password)) score++;
    if (/[^A-Za-z0-9]/.test(password)) score++;

    return score;
  }

  /**
   * Get password strength label
   */
  static getPasswordStrengthLabel(password: string): string {
    const strength = this.getPasswordStrength(password);
    
    switch (strength) {
      case 0:
      case 1:
        return 'Very Weak';
      case 2:
        return 'Weak';
      case 3:
        return 'Medium';
      case 4:
        return 'Strong';
      case 5:
        return 'Very Strong';
      default:
        return 'Very Weak';
    }
  }

  /**
   * Get password strength color class
   */
  static getPasswordStrengthColor(password: string): string {
    const strength = this.getPasswordStrength(password);
    
    switch (strength) {
      case 0:
      case 1:
        return 'danger';
      case 2:
        return 'warning';
      case 3:
        return 'info';
      case 4:
        return 'success';
      case 5:
        return 'success';
      default:
        return 'danger';
    }
  }
} 