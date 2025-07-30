import React from 'react';

// Form validation utilities for UI components

export interface ValidationRule {
  required?: boolean;
  minLength?: number;
  maxLength?: number;
  pattern?: RegExp;
  custom?: (value: string) => string | null;
}

export interface ValidationRules {
  [key: string]: ValidationRule;
}

export interface ValidationErrors {
  [key: string]: string;
}

// Common validation patterns
export const PATTERNS = {
  EMAIL: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
  PHONE: /^[\+]?[1-9][\d]{0,15}$/,
  NAME: /^[a-zA-Z\s'-]+$/,
  COMPANY: /^[a-zA-Z0-9\s&.,'-]+$/,
  MESSAGE: /^[\s\S]*$/, // Allow any character including newlines
};

// Common validation messages
export const MESSAGES = {
  REQUIRED: 'This field is required',
  EMAIL: 'Please enter a valid email address',
  MIN_LENGTH: (min: number) => `Must be at least ${min} characters`,
  MAX_LENGTH: (max: number) => `Must be no more than ${max} characters`,
  INVALID_FORMAT: 'Invalid format',
  NAME_FORMAT: 'Please enter a valid name (letters, spaces, hyphens, apostrophes only)',
  COMPANY_FORMAT: 'Please enter a valid company name',
  PHONE_FORMAT: 'Please enter a valid phone number',
};

// Validation function
export const validateFieldValue = (
  value: string,
  rules: ValidationRule
): string | null => {
  // Required check
  if (rules.required && (!value || value.trim() === '')) {
    return MESSAGES.REQUIRED;
  }

  // Skip other validations if field is empty and not required
  if (!value || value.trim() === '') {
    return null;
  }

  const trimmedValue = value.trim();

  // Min length check
  if (rules.minLength && trimmedValue.length < rules.minLength) {
    return MESSAGES.MIN_LENGTH(rules.minLength);
  }

  // Max length check
  if (rules.maxLength && trimmedValue.length > rules.maxLength) {
    return MESSAGES.MAX_LENGTH(rules.maxLength);
  }

  // Pattern check
  if (rules.pattern && !rules.pattern.test(trimmedValue)) {
    return MESSAGES.INVALID_FORMAT;
  }

  // Custom validation
  if (rules.custom) {
    return rules.custom(trimmedValue);
  }

  return null;
};

// Validate entire form
export const validateForm = (
  data: { [key: string]: string },
  rules: ValidationRules
): ValidationErrors => {
  const errors: ValidationErrors = {};

  Object.keys(rules).forEach(field => {
    const error = validateFieldValue(data[field] || '', rules[field]);
    if (error) {
      errors[field] = error;
    }
  });

  return errors;
};

// Predefined validation rules
export const VALIDATION_RULES = {
  email: {
    required: true,
    pattern: PATTERNS.EMAIL,
    maxLength: 254,
  },
  firstName: {
    required: true,
    pattern: PATTERNS.NAME,
    minLength: 2,
    maxLength: 50,
  },
  lastName: {
    required: true,
    pattern: PATTERNS.NAME,
    minLength: 2,
    maxLength: 50,
  },
  company: {
    required: false,
    pattern: PATTERNS.COMPANY,
    maxLength: 100,
  },
  message: {
    required: true,
    minLength: 10,
    maxLength: 1000,
  },
  phone: {
    required: false,
    pattern: PATTERNS.PHONE,
    maxLength: 20,
  },
};

// Real-time validation hook
export const useFormValidation = (
  initialData: { [key: string]: string },
  rules: ValidationRules
) => {
  const [data, setData] = React.useState(initialData);
  const [errors, setErrors] = React.useState<ValidationErrors>({});
  const [touched, setTouched] = React.useState<{ [key: string]: boolean }>({});

  const validateField = (field: string, value: string) => {
    const error = validateFieldValue(value, rules[field]);
    setErrors(prev => ({
      ...prev,
      [field]: error || '',
    }));
  };

  const handleChange = (field: string, value: string) => {
    setData(prev => ({ ...prev, [field]: value }));

    // Only validate if field has been touched
    if (touched[field]) {
      validateField(field, value);
    }
  };

  const handleBlur = (field: string) => {
    setTouched(prev => ({ ...prev, [field]: true }));
    validateField(field, data[field] || '');
  };

  const validateAll = () => {
    const newErrors = validateForm(data, rules);
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const reset = () => {
    setData(initialData);
    setErrors({});
    setTouched({});
  };

  return {
    data,
    errors,
    touched,
    handleChange,
    handleBlur,
    validateAll,
    reset,
    isValid: Object.keys(errors).length === 0,
  };
};
