import { motion } from 'framer-motion';
import React from 'react';

interface FormInputProps {
  type: 'text' | 'email' | 'tel' | 'textarea';
  name: string;
  label: string;
  placeholder?: string;
  value: string;
  error?: string;
  touched?: boolean;
  required?: boolean;
  onChange: (value: string) => void;
  onBlur: () => void;
  className?: string;
  rows?: number;
}

const FormInput: React.FC<FormInputProps> = ({
  type,
  name,
  label,
  placeholder,
  value,
  error,
  touched,
  required = false,
  onChange,
  onBlur,
  className = '',
  rows = 4,
}) => {
  const showError = error && touched;
  const inputId = `form-${name}`;

  const baseClasses = `
    w-full px-4 py-3 border rounded-lg transition-all duration-200
    focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent
    ${showError
      ? 'border-red-500 bg-red-50 focus:ring-red-500'
      : 'border-gray-300 bg-white hover:border-gray-400'
    }
    ${className}
  `;

  const labelClasses = `
    block text-sm font-medium mb-2 transition-colors duration-200
    ${showError ? 'text-red-600' : 'text-gray-700'}
  `;

  return (
    <div className="mb-6">
      <label htmlFor={inputId} className={labelClasses}>
        {label}
        {required && <span className="text-red-500 ml-1">*</span>}
      </label>

      {type === 'textarea' ? (
        <textarea
          id={inputId}
          name={name}
          value={value}
          placeholder={placeholder}
          onChange={(e) => onChange(e.target.value)}
          onBlur={onBlur}
          rows={rows}
          className={`${baseClasses} resize-none`}
        />
      ) : (
        <input
          id={inputId}
          type={type}
          name={name}
          value={value}
          placeholder={placeholder}
          onChange={(e) => onChange(e.target.value)}
          onBlur={onBlur}
          className={baseClasses}
        />
      )}

      {showError && (
        <motion.div
          initial={{ opacity: 0, y: -10 }}
          animate={{ opacity: 1, y: 0 }}
          className="mt-2 text-sm text-red-600 flex items-center"
        >
          <svg
            className="w-4 h-4 mr-1"
            fill="currentColor"
            viewBox="0 0 20 20"
          >
            <path
              fillRule="evenodd"
              d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z"
              clipRule="evenodd"
            />
          </svg>
          {error}
        </motion.div>
      )}
    </div>
  );
};

export default FormInput;
