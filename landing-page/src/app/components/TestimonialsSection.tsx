'use client'

import { motion } from 'framer-motion'
import { Quote, Star } from 'lucide-react'

const testimonials = [
  {
    name: 'Sarah Chen',
    role: 'CEO',
    company: 'TechFlow SaaS',
    avatar: '👩‍💼',
    rating: 5,
    quote: 'SubscriptionAnalytics transformed how we understand our business. We went from spending hours compiling data to having real-time insights across all our payment providers.',
    metrics: 'MRR increased by 40%'
  },
  {
    name: 'Michael Rodriguez',
    role: 'Head of Growth',
    company: 'CloudSync',
    avatar: '👨‍💻',
    rating: 5,
    quote: 'The predictive analytics feature helped us identify at-risk customers before they churned. Our retention rate improved by 25% in just 3 months.',
    metrics: 'Churn reduced by 25%'
  },
  {
    name: 'Emily Watson',
    role: 'Finance Director',
    company: 'DataViz Pro',
    avatar: '👩‍💼',
    rating: 5,
    quote: 'Finally, a platform that gives us a unified view of our subscription metrics. The multi-provider support is exactly what we needed.',
    metrics: 'Reporting time reduced by 80%'
  },
  {
    name: 'David Kim',
    role: 'Product Manager',
    company: 'AppFlow',
    avatar: '👨‍🎨',
    rating: 5,
    quote: 'Setup was incredibly easy. We connected Stripe and PayPal in minutes, and the insights started flowing immediately.',
    metrics: 'Setup completed in 15 minutes'
  },
  {
    name: 'Lisa Thompson',
    role: 'Operations Manager',
    company: 'SaaSScale',
    avatar: '👩‍💼',
    rating: 5,
    quote: 'The real-time alerts and automated reporting save us hours every week. It&apos;s like having a dedicated analytics team.',
    metrics: 'Time saved: 10 hours/week'
  },
  {
    name: 'Alex Johnson',
    role: 'Founder',
    company: 'StartupXYZ',
    avatar: '👨‍💼',
    rating: 5,
    quote: 'As a growing startup, we needed insights we could trust. SubscriptionAnalytics gives us the data-driven confidence to make bold decisions.',
    metrics: 'ARR growth: 300%'
  }
]

export default function TestimonialsSection() {
  return (
    <section className="py-20 bg-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-6"
          >
            Trusted by Growing Businesses
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-3xl mx-auto"
          >
            See how SubscriptionAnalytics is helping businesses make data-driven decisions and scale their subscription revenue.
          </motion.p>
        </div>

        <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
          {testimonials.map((testimonial, index) => (
            <motion.div
              key={index}
              initial={{ opacity: 0, y: 30 }}
              whileInView={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.6, delay: index * 0.1 }}
              viewport={{ once: true }}
              className="bg-gray-50 rounded-2xl p-6 hover:shadow-lg transition-shadow duration-300"
            >
              {/* Rating */}
              <div className="flex items-center mb-4">
                {[...Array(testimonial.rating)].map((_, i) => (
                  <Star key={i} className="w-5 h-5 text-yellow-400 fill-current" />
                ))}
              </div>

              {/* Quote */}
              <div className="mb-6">
                <Quote className="w-8 h-8 text-blue-600 mb-3" />
                <p className="text-gray-700 leading-relaxed">
                  &ldquo;{testimonial.quote}&rdquo;
                </p>
              </div>

              {/* Author */}
              <div className="flex items-center mb-4">
                <div className="w-12 h-12 bg-gradient-to-br from-blue-100 to-indigo-100 rounded-full flex items-center justify-center text-2xl mr-4">
                  {testimonial.avatar}
                </div>
                <div>
                  <h4 className="font-semibold text-gray-900">{testimonial.name}</h4>
                  <p className="text-sm text-gray-600">{testimonial.role} at {testimonial.company}</p>
                </div>
              </div>

              {/* Metrics */}
              <div className="bg-blue-50 rounded-lg p-3">
                <p className="text-sm font-semibold text-blue-700">
                  📈 {testimonial.metrics}
                </p>
              </div>
            </motion.div>
          ))}
        </div>

        {/* Stats */}
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, delay: 0.6 }}
          viewport={{ once: true }}
          className="mt-16 grid md:grid-cols-4 gap-8 text-center"
        >
          <div>
            <div className="text-3xl font-bold text-blue-600 mb-2">2,847+</div>
            <div className="text-gray-600">Active Users</div>
          </div>
          <div>
            <div className="text-3xl font-bold text-blue-600 mb-2">$2B+</div>
            <div className="text-gray-600">Revenue Tracked</div>
          </div>
          <div>
            <div className="text-3xl font-bold text-blue-600 mb-2">99.9%</div>
            <div className="text-gray-600">Uptime</div>
          </div>
          <div>
            <div className="text-3xl font-bold text-blue-600 mb-2">4.8/5</div>
            <div className="text-gray-600">Customer Rating</div>
          </div>
        </motion.div>
      </div>
    </section>
  )
}
