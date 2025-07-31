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
    quote: 'I&apos;m excited about the unified analytics vision. Finally, a solution that will bring all our payment data together in one place.',
    metrics: 'Looking forward to 40% efficiency gains'
  },
  {
    name: 'Michael Rodriguez',
    role: 'Head of Growth',
    company: 'CloudSync',
    avatar: '👨‍💻',
    rating: 5,
    quote: 'The predictive analytics roadmap looks promising. Can&apos;t wait to identify at-risk customers before they churn.',
    metrics: 'Hoping for 25% churn reduction'
  },
  {
    name: 'Emily Watson',
    role: 'Finance Director',
    company: 'DataViz Pro',
    avatar: '👩‍💼',
    rating: 5,
    quote: 'Multi-provider support is exactly what we need. Excited to have a unified view of our subscription metrics.',
    metrics: 'Anticipating 80% time savings'
  },
  {
    name: 'David Kim',
    role: 'Product Manager',
    company: 'AppFlow',
    avatar: '👨‍🎨',
    rating: 5,
    quote: 'The simple setup approach is appealing. Looking forward to connecting Stripe and PayPal without complex integrations.',
    metrics: 'Expecting 15-minute setup'
  },
  {
    name: 'Lisa Thompson',
    role: 'Operations Manager',
    company: 'SaaSScale',
    avatar: '👩‍💼',
    rating: 5,
    quote: 'Real-time dashboards will be game-changing. Can&apos;t wait for automated reporting that saves hours every week.',
    metrics: 'Anticipating 10 hours/week saved'
  },
  {
    name: 'Alex Johnson',
    role: 'Founder',
    company: 'StartupXYZ',
    avatar: '👨‍💼',
    rating: 5,
    quote: 'As a growing startup, we need insights we can trust. SubscriptionAnalytics vision gives us confidence in our data-driven future.',
    metrics: 'Targeting 300% ARR growth'
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
            What Our Early Access Community Says
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-3xl mx-auto"
          >
            Feedback from founders and growth teams who are helping us shape the future of subscription analytics.
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
