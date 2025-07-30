'use client'

import { motion } from 'framer-motion';
import { CheckCircle, Mail, Users } from 'lucide-react';
import React, { useState } from 'react';
import { useFormValidation, VALIDATION_RULES } from '../lib/formValidation';
import FormInput from './FormInput';

const WaitlistSection: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [submitError, setSubmitError] = useState('');

  const {
    data,
    errors,
    touched,
    handleChange,
    handleBlur,
    validateAll,
    reset,
    isValid,
  } = useFormValidation(
    { email: '' },
    { email: VALIDATION_RULES.email }
  );

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Validate all fields
    if (!validateAll()) {
      return;
    }

    setIsLoading(true);
    setSubmitError('');

    try {
      const response = await fetch('/api/waitlist', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email: data.email }),
      });

      const result = await response.json();

      if (response.ok) {
        setIsSubmitted(true);
        reset();

        // Track analytics
        if (typeof window !== 'undefined' && (window as unknown as { gtag?: unknown }).gtag) {
          ((window as unknown as { gtag: unknown }).gtag as (event: string, params: Record<string, unknown>) => void)('waitlist_signup', {
            event_category: 'engagement',
            event_label: 'waitlist_form',
          });
        }
      } else {
        setSubmitError(result.message || 'Failed to join waitlist. Please try again.');
      }
    } catch (error) {
      console.error('Waitlist submission error:', error);
      setSubmitError('Network error. Please check your connection and try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (field: string, value: string) => {
    handleChange(field, value);
    setSubmitError(''); // Clear previous errors when user starts typing
  };

  return (
    <section id="waitlist" className="py-20 bg-gradient-to-br from-blue-50 to-indigo-100">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true }}
          transition={{ duration: 0.6 }}
          className="text-center mb-12"
        >
          <motion.div
            initial={{ scale: 0 }}
            whileInView={{ scale: 1 }}
            viewport={{ once: true }}
            transition={{ delay: 0.2, type: 'spring', stiffness: 200 }}
            className="inline-flex items-center justify-center w-16 h-16 bg-blue-100 rounded-full mb-6"
          >
            <Mail className="w-8 h-8 text-blue-600" />
          </motion.div>

          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Join the Waitlist
          </h2>
          <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto">
            Be among the first to experience unified subscription analytics.
            Get early access and exclusive pricing when we launch.
          </p>

          <div className="flex items-center justify-center space-x-8 text-sm text-gray-500 mb-8">
            <div className="flex items-center">
              <Users className="w-4 h-4 mr-2" />
              <span>2,847 people already joined</span>
            </div>
            <div className="flex items-center">
              <CheckCircle className="w-4 h-4 mr-2 text-green-500" />
              <span>Free early access</span>
            </div>
          </div>
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true }}
          transition={{ duration: 0.6, delay: 0.3 }}
          className="max-w-md mx-auto"
        >
          {isSubmitted ? (
            <motion.div
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              className="text-center p-8 bg-green-50 rounded-2xl border border-green-200"
            >
              <CheckCircle className="w-16 h-16 text-green-500 mx-auto mb-4" />
              <h3 className="text-2xl font-bold text-green-800 mb-2">
                You&apos;re on the list!
              </h3>
              <p className="text-green-700 mb-4">
                We&apos;ll notify you as soon as SubscriptionAnalytics is ready.
                Get ready to transform your subscription analytics!
              </p>
              <button
                onClick={() => setIsSubmitted(false)}
                className="text-green-600 hover:text-green-700 font-medium underline"
              >
                Join with another email
              </button>
            </motion.div>
          ) : (
            <form onSubmit={handleSubmit} className="space-y-6">
              <FormInput
                type="email"
                name="email"
                label="Email Address"
                placeholder="Enter your email address"
                value={data.email}
                error={errors.email}
                touched={touched.email}
                required={true}
                onChange={(value) => handleInputChange('email', value)}
                onBlur={() => handleBlur('email')}
              />

              {submitError && (
                <motion.div
                  initial={{ opacity: 0, y: -10 }}
                  animate={{ opacity: 1, y: 0 }}
                  className="p-4 bg-red-50 border border-red-200 rounded-lg"
                >
                  <div className="flex items-center">
                    <svg
                      className="w-5 h-5 text-red-500 mr-2"
                      fill="currentColor"
                      viewBox="0 0 20 20"
                    >
                      <path
                        fillRule="evenodd"
                        d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z"
                        clipRule="evenodd"
                      />
                    </svg>
                    <span className="text-red-700 text-sm">{submitError}</span>
                  </div>
                </motion.div>
              )}

              <motion.button
                type="submit"
                disabled={isLoading || !isValid}
                whileHover={{ scale: isLoading ? 1 : 1.02 }}
                whileTap={{ scale: isLoading ? 1 : 0.98 }}
                className={`
                  w-full py-4 px-8 rounded-lg font-semibold text-white transition-all duration-200
                  ${isLoading || !isValid
                    ? 'bg-gray-400 cursor-not-allowed'
                    : 'bg-blue-600 hover:bg-blue-700 shadow-lg hover:shadow-xl'
                  }
                `}
              >
                {isLoading ? (
                  <div className="flex items-center justify-center">
                    <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white mr-2"></div>
                    Joining waitlist...
                  </div>
                ) : (
                  'Join Waitlist'
                )}
              </motion.button>

              <p className="text-xs text-gray-500 text-center">
                By joining, you agree to receive updates about SubscriptionAnalytics.
                We respect your privacy and won&apos;t spam you.
              </p>
            </form>
          )}
        </motion.div>

        <motion.div
          initial={{ opacity: 0 }}
          whileInView={{ opacity: 1 }}
          viewport={{ once: true }}
          transition={{ duration: 0.6, delay: 0.6 }}
          className="mt-12 grid grid-cols-1 md:grid-cols-3 gap-8 text-center"
        >
          <div className="p-6 bg-white rounded-xl shadow-sm">
            <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center mx-auto mb-4">
              <CheckCircle className="w-6 h-6 text-blue-600" />
            </div>
            <h3 className="font-semibold text-gray-900 mb-2">Early Access</h3>
            <p className="text-sm text-gray-600">
              Be the first to try our unified analytics platform
            </p>
          </div>

          <div className="p-6 bg-white rounded-xl shadow-sm">
            <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center mx-auto mb-4">
              <Users className="w-6 h-6 text-green-600" />
            </div>
            <h3 className="font-semibold text-gray-900 mb-2">Exclusive Pricing</h3>
            <p className="text-sm text-gray-600">
              Get special launch pricing and discounts
            </p>
          </div>

          <div className="p-6 bg-white rounded-xl shadow-sm">
            <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center mx-auto mb-4">
              <Mail className="w-6 h-6 text-purple-600" />
            </div>
            <h3 className="font-semibold text-gray-900 mb-2">Product Updates</h3>
            <p className="text-sm text-gray-600">
              Stay informed about new features and improvements
            </p>
          </div>
        </motion.div>
      </div>
    </section>
  );
};

export default WaitlistSection;
