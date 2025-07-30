'use client'

import { motion } from 'framer-motion';
import { CheckCircle, Mail, MapPin, Phone, Send } from 'lucide-react';
import React, { useState } from 'react';
import { useFormValidation, VALIDATION_RULES } from '../lib/formValidation';
import FormInput from './FormInput';

const ContactSection: React.FC = () => {
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
    {
      firstName: '',
      lastName: '',
      email: '',
      company: '',
      message: '',
    },
    {
      firstName: VALIDATION_RULES.firstName,
      lastName: VALIDATION_RULES.lastName,
      email: VALIDATION_RULES.email,
      company: VALIDATION_RULES.company,
      message: VALIDATION_RULES.message,
    }
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
      const response = await fetch('/api/contact', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      });

      const result = await response.json();

      if (response.ok) {
        setIsSubmitted(true);
        reset();

        // Track analytics
        if (typeof window !== 'undefined' && (window as any).gtag) {
          (window as any).gtag('event', 'contact_form_submit', {
            event_category: 'engagement',
            event_label: 'contact_form',
          });
        }
      } else {
        setSubmitError(result.message || 'Failed to send message. Please try again.');
      }
    } catch (error) {
      console.error('Contact submission error:', error);
      setSubmitError('Network error. Please check your connection and try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (field: string, value: string) => {
    handleChange(field, value);
    setSubmitError(''); // Clear previous errors when user starts typing
  };

  const contactInfo = [
    {
      icon: Mail,
      title: 'Email',
      value: 'hello@subscriptionanalytics.com',
      href: 'mailto:hello@subscriptionanalytics.com',
    },
    {
      icon: Phone,
      title: 'Phone',
      value: '+1 (555) 123-4567',
      href: 'tel:+15551234567',
    },
    {
      icon: MapPin,
      title: 'Location',
      value: 'San Francisco, CA',
      href: '#',
    },
  ];

  return (
    <section id="contact" className="py-20 bg-gray-50">
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          whileInView={{ opacity: 1, y: 0 }}
          viewport={{ once: true }}
          transition={{ duration: 0.6 }}
          className="text-center mb-16"
        >
          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Get in Touch
          </h2>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            Have questions about SubscriptionAnalytics? We&apos;d love to hear from you.
            Send us a message and we&apos;ll respond as soon as possible.
          </p>
        </motion.div>

        <div className="grid lg:grid-cols-2 gap-12">
          {/* Contact Information */}
          <motion.div
            initial={{ opacity: 0, x: -20 }}
            whileInView={{ opacity: 1, x: 0 }}
            viewport={{ once: true }}
            transition={{ duration: 0.6, delay: 0.2 }}
          >
            <h3 className="text-2xl font-semibold text-gray-900 mb-8">
              Contact Information
            </h3>

            <div className="space-y-6">
              {contactInfo.map((info, index) => (
                <motion.a
                  key={info.title}
                  href={info.href}
                  initial={{ opacity: 0, x: -20 }}
                  whileInView={{ opacity: 1, x: 0 }}
                  viewport={{ once: true }}
                  transition={{ duration: 0.6, delay: 0.3 + index * 0.1 }}
                  className="flex items-center p-4 bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow duration-200"
                >
                  <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center mr-4">
                    <info.icon className="w-6 h-6 text-blue-600" />
                  </div>
                  <div>
                    <h4 className="font-medium text-gray-900">{info.title}</h4>
                    <p className="text-gray-600">{info.value}</p>
                  </div>
                </motion.a>
              ))}
            </div>

            <motion.div
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.6, delay: 0.6 }}
              className="mt-8 p-6 bg-blue-50 rounded-lg"
            >
              <h4 className="font-semibold text-blue-900 mb-2">
                Business Hours
              </h4>
              <p className="text-blue-800 text-sm">
                Monday - Friday: 9:00 AM - 6:00 PM PST<br />
                Saturday: 10:00 AM - 2:00 PM PST<br />
                Sunday: Closed
              </p>
            </motion.div>
          </motion.div>

          {/* Contact Form */}
          <motion.div
            initial={{ opacity: 0, x: 20 }}
            whileInView={{ opacity: 1, x: 0 }}
            viewport={{ once: true }}
            transition={{ duration: 0.6, delay: 0.4 }}
          >
            {isSubmitted ? (
              <motion.div
                initial={{ opacity: 0, scale: 0.9 }}
                animate={{ opacity: 1, scale: 1 }}
                className="text-center p-8 bg-green-50 rounded-2xl border border-green-200"
              >
                <CheckCircle className="w-16 h-16 text-green-500 mx-auto mb-4" />
                <h3 className="text-2xl font-bold text-green-800 mb-2">
                  Message Sent!
                </h3>
                <p className="text-green-700 mb-4">
                  Thank you for reaching out. We&apos;ll get back to you within 24 hours.
                </p>
                <button
                  onClick={() => setIsSubmitted(false)}
                  className="text-green-600 hover:text-green-700 font-medium underline"
                >
                  Send another message
                </button>
              </motion.div>
            ) : (
              <form onSubmit={handleSubmit} className="space-y-6">
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                  <FormInput
                    type="text"
                    name="firstName"
                    label="First Name"
                    placeholder="Enter your first name"
                    value={data.firstName}
                    error={errors.firstName}
                    touched={touched.firstName}
                    required={true}
                    onChange={(value) => handleInputChange('firstName', value)}
                    onBlur={() => handleBlur('firstName')}
                  />

                  <FormInput
                    type="text"
                    name="lastName"
                    label="Last Name"
                    placeholder="Enter your last name"
                    value={data.lastName}
                    error={errors.lastName}
                    touched={touched.lastName}
                    required={true}
                    onChange={(value) => handleInputChange('lastName', value)}
                    onBlur={() => handleBlur('lastName')}
                  />
                </div>

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

                <FormInput
                  type="text"
                  name="company"
                  label="Company (Optional)"
                  placeholder="Enter your company name"
                  value={data.company}
                  error={errors.company}
                  touched={touched.company}
                  required={false}
                  onChange={(value) => handleInputChange('company', value)}
                  onBlur={() => handleBlur('company')}
                />

                <FormInput
                  type="textarea"
                  name="message"
                  label="Message"
                  placeholder="Tell us about your subscription analytics needs..."
                  value={data.message}
                  error={errors.message}
                  touched={touched.message}
                  required={true}
                  onChange={(value) => handleInputChange('message', value)}
                  onBlur={() => handleBlur('message')}
                  rows={6}
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
                    flex items-center justify-center space-x-2
                    ${isLoading || !isValid
                      ? 'bg-gray-400 cursor-not-allowed'
                      : 'bg-blue-600 hover:bg-blue-700 shadow-lg hover:shadow-xl'
                    }
                  `}
                >
                  {isLoading ? (
                    <>
                      <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
                      <span>Sending message...</span>
                    </>
                  ) : (
                    <>
                      <Send className="w-5 h-5" />
                      <span>Send Message</span>
                    </>
                  )}
                </motion.button>

                <p className="text-xs text-gray-500 text-center">
                  By submitting this form, you agree to our privacy policy.
                  We&apos;ll never share your information with third parties.
                </p>
              </form>
            )}
          </motion.div>
        </div>
      </div>
    </section>
  );
};

export default ContactSection;
