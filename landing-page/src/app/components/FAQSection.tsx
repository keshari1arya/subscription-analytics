'use client'

import { motion } from 'framer-motion'
import { ChevronDown, ChevronUp } from 'lucide-react'
import Link from 'next/link'
import { useState } from 'react'

const faqs = [
  {
    question: 'What payment providers does SubscriptionAnalytics support?',
    answer: 'We support all major payment providers including Stripe, PayPal, Square, Adyen, and many others. Our platform is designed to work with any payment processor you use.'
  },
  {
    question: 'How long does it take to set up SubscriptionAnalytics?',
    answer: 'Setup takes just minutes, not weeks. Simply connect your payment providers through our secure OAuth flow and your data will start syncing immediately.'
  },
  {
    question: 'Is my data secure with SubscriptionAnalytics?',
    answer: 'Absolutely. We use bank-level encryption, SOC 2 compliance, and never store sensitive payment information. Your data is encrypted in transit and at rest.'
  },
  {
    question: 'Can I export my data from SubscriptionAnalytics?',
    answer: 'Yes, you can export all your analytics data in multiple formats including CSV, JSON, and through our comprehensive API for custom integrations.'
  },
  {
    question: 'Do you offer a free trial?',
    answer: 'Yes! We offer a 14-day free trial with full access to all features. No credit card required to get started.'
  },
  {
    question: 'What metrics does SubscriptionAnalytics track?',
    answer: 'We track all key subscription metrics including MRR, ARR, churn rate, customer lifetime value, expansion revenue, and more across all your payment providers.'
  },
  {
    question: 'Can I integrate SubscriptionAnalytics with my existing tools?',
    answer: 'Yes, we offer comprehensive API access and integrations with popular tools like Slack, Zapier, and custom webhooks for real-time notifications.'
  },
  {
    question: 'What kind of support do you provide?',
    answer: 'We offer 24/7 customer support via email, live chat, and phone. Plus, we provide comprehensive documentation and video tutorials.'
  }
]

export default function FAQSection() {
  const [openIndex, setOpenIndex] = useState<number | null>(null)

  const toggleFAQ = (index: number) => {
    setOpenIndex(openIndex === index ? null : index)
  }

  return (
    <section id="faq" className="py-20 bg-gray-50">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-6"
          >
            Frequently Asked Questions
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-2xl mx-auto"
          >
            Everything you need to know about SubscriptionAnalytics
          </motion.p>
        </div>

        <div className="space-y-4">
          {faqs.map((faq, index) => (
            <motion.div
              key={index}
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.6, delay: index * 0.1 }}
              viewport={{ once: true }}
              className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden"
            >
              <button
                onClick={() => toggleFAQ(index)}
                className="w-full px-6 py-4 text-left flex items-center justify-between hover:bg-gray-50 transition-colors"
                aria-expanded={openIndex === index}
                aria-controls={`faq-answer-${index}`}
              >
                <h3 className="text-lg font-semibold text-gray-900 pr-4">
                  {faq.question}
                </h3>
                {openIndex === index ? (
                  <ChevronUp className="w-5 h-5 text-gray-500 flex-shrink-0" />
                ) : (
                  <ChevronDown className="w-5 h-5 text-gray-500 flex-shrink-0" />
                )}
              </button>

              <div
                id={`faq-answer-${index}`}
                className={`px-6 transition-all duration-300 ease-in-out ${
                  openIndex === index ? 'pb-6 max-h-96 opacity-100' : 'max-h-0 opacity-0 overflow-hidden'
                }`}
              >
                <p className="text-gray-600 leading-relaxed">
                  {faq.answer}
                </p>
              </div>
            </motion.div>
          ))}
        </div>

        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, delay: 0.6 }}
          viewport={{ once: true }}
          className="text-center mt-12"
        >
          <p className="text-gray-600 mb-6">
            Still have questions? We&apos;re here to help!
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Link
              href="/#contact"
              className="bg-blue-600 text-white px-8 py-3 rounded-lg font-semibold hover:bg-blue-700 transition-colors"
            >
              Contact Support
            </Link>
            <Link
              href="/#waitlist"
              className="border-2 border-blue-600 text-blue-600 px-8 py-3 rounded-lg font-semibold hover:bg-blue-50 transition-colors"
            >
              Start Free Trial
            </Link>
          </div>
        </motion.div>
      </div>
    </section>
  )
}
