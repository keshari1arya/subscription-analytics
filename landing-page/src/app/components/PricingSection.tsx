'use client'

import { motion } from 'framer-motion'
import { Check } from 'lucide-react'
import Button from './Button'

const plans = [
  {
    name: 'Early Access',
    price: 'Free',
    period: '',
    description: 'Perfect for early adopters and beta testers',
    features: [
      'Core analytics features',
      'Community feedback access',
      'Priority feature requests',
      'Direct product input',
      'Exclusive early access'
    ],
    popular: true,
    color: 'border-blue-500',
    bgColor: 'bg-blue-50'
  },
  {
    name: 'Founder',
    price: '$49',
    period: '/month',
    description: 'For founders building subscription businesses',
    features: [
      'All core features',
      'Multi-provider support',
      'Basic analytics',
      'Community support',
      'Feature priority access'
    ],
    popular: false,
    color: 'border-gray-200',
    bgColor: 'bg-white'
  },
  {
    name: 'Growth',
    price: '$99',
    period: '/month',
    description: 'For growing businesses with advanced needs',
    features: [
      'Advanced analytics',
      'Predictive insights',
      'Custom integrations',
      'Priority support',
      'API access',
      'White-label options'
    ],
    popular: false,
    color: 'border-gray-200',
    bgColor: 'bg-white'
  }
]

export default function PricingSection() {
  return (
    <section id="pricing" className="py-20 bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-6"
          >
            Simple, Transparent Pricing
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-3xl mx-auto"
          >
            Choose the plan that fits your business. All plans include our core features
            with no hidden fees or setup costs.
          </motion.p>
        </div>

        <div className="grid md:grid-cols-3 gap-8">
          {plans.map((plan, index) => (
            <motion.div
              key={plan.name}
              initial={{ opacity: 0, y: 30 }}
              whileInView={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.8, delay: index * 0.2 }}
              viewport={{ once: true }}
              className="relative"
            >
              {plan.popular && (
                <div className="absolute -top-4 left-1/2 transform -translate-x-1/2">
                  <span className="bg-blue-600 text-white px-4 py-2 rounded-full text-sm font-semibold">
                    Most Popular
                  </span>
                </div>
              )}

              <div className={`${plan.bgColor} p-8 rounded-2xl border-2 ${plan.color} h-full hover:shadow-lg transition-shadow duration-300`}>
                <div className="text-center mb-8">
                  <h3 className="text-2xl font-bold text-gray-900 mb-2">
                    {plan.name}
                  </h3>
                  <div className="mb-4">
                    <span className="text-4xl font-bold text-gray-900">
                      {plan.price}
                    </span>
                    <span className="text-gray-600">
                      {plan.period}
                    </span>
                  </div>
                  <p className="text-gray-600">
                    {plan.description}
                  </p>
                </div>

                <ul className="space-y-4 mb-8">
                  {plan.features.map((feature) => (
                    <li key={feature} className="flex items-start">
                      <Check className="w-5 h-5 text-green-500 mt-0.5 mr-3 flex-shrink-0" />
                      <span className="text-gray-700">{feature}</span>
                    </li>
                  ))}
                </ul>

                <div className="text-center">
                  {plan.name === 'Enterprise' ? (
                    <Button variant="outline" size="lg" href="#contact" className="w-full">
                      Contact Sales
                    </Button>
                  ) : (
                    <Button size="lg" href="#waitlist" className="w-full">
                      Start Free Trial
                    </Button>
                  )}
                </div>
              </div>
            </motion.div>
          ))}
        </div>

        {/* Additional Info */}
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, delay: 0.6 }}
          viewport={{ once: true }}
          className="mt-16 text-center"
        >
          <div className="bg-white rounded-2xl p-8 shadow-lg border border-gray-200">
            <h3 className="text-xl font-semibold text-gray-900 mb-4">
              All Plans Include
            </h3>
            <div className="grid md:grid-cols-3 gap-6 text-sm text-gray-600">
              <div className="flex items-center justify-center">
                <Check className="w-4 h-4 text-green-500 mr-2" />
                <span>No setup fees</span>
              </div>
              <div className="flex items-center justify-center">
                <Check className="w-4 h-4 text-green-500 mr-2" />
                <span>Cancel anytime</span>
              </div>
              <div className="flex items-center justify-center">
                <Check className="w-4 h-4 text-green-500 mr-2" />
                <span>Data encryption</span>
              </div>
            </div>
          </div>
        </motion.div>
      </div>
    </section>
  )
}
