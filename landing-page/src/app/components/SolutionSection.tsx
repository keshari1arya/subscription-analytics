'use client'

import { motion } from 'framer-motion'
import { Brain, CheckCircle, Clock, Shield, TrendingUp, Zap } from 'lucide-react'

const solutions = [
  {
    icon: Zap,
    title: 'Multi-Provider Support',
    description: 'Connect Stripe, PayPal, Square, and more in one unified dashboard.',
    color: 'text-blue-500',
    bgColor: 'bg-blue-50'
  },
  {
    icon: TrendingUp,
    title: 'Unified Analytics',
    description: 'Complete view across all platforms with real-time MRR, ARR, and churn monitoring.',
    color: 'text-green-500',
    bgColor: 'bg-green-50'
  },
  {
    icon: Brain,
    title: 'Predictive Analytics',
    description: 'AI-powered insights to predict churn, optimize pricing, and identify growth opportunities.',
    color: 'text-purple-500',
    bgColor: 'bg-purple-50'
  },
  {
    icon: Shield,
    title: 'Enterprise Security',
    description: 'SOC 2 compliant with bank-level encryption and multi-tenant architecture.',
    color: 'text-indigo-500',
    bgColor: 'bg-indigo-50'
  },
  {
    icon: Clock,
    title: 'Easy Integration',
    description: 'Connect in minutes, not weeks. No complex setup or custom development required.',
    color: 'text-orange-500',
    bgColor: 'bg-orange-50'
  },
  {
    icon: CheckCircle,
    title: 'Real-time Insights',
    description: 'Live dashboards with automated reporting and instant alerts for critical metrics.',
    color: 'text-teal-500',
    bgColor: 'bg-teal-50'
  }
]

export default function SolutionSection() {
  return (
    <section id="features" className="py-20 bg-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-6"
          >
            Why Choose SubscriptionAnalytics?
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-3xl mx-auto"
          >
            We&apos;ve built the platform that solves the fragmentation problem.
                          Here&apos;s what makes us different:
          </motion.p>
        </div>

        <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
          {solutions.map((solution, index) => (
            <motion.div
              key={solution.title}
              initial={{ opacity: 0, y: 30 }}
              whileInView={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.8, delay: index * 0.1 }}
              viewport={{ once: true }}
              className="relative group"
            >
              <div className={`${solution.bgColor} p-8 rounded-2xl h-full border border-gray-200 hover:shadow-lg transition-all duration-300 group-hover:scale-105`}>
                <div className={`${solution.color} mb-4`}>
                  <solution.icon className="w-12 h-12" />
                </div>
                <h3 className="text-xl font-semibold text-gray-900 mb-4">
                  {solution.title}
                </h3>
                <p className="text-gray-600 leading-relaxed">
                  {solution.description}
                </p>
              </div>
            </motion.div>
          ))}
        </div>

        {/* Value Proposition */}
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, delay: 0.6 }}
          viewport={{ once: true }}
          className="mt-16 text-center"
        >
          <div className="bg-gradient-to-r from-blue-600 to-indigo-600 rounded-2xl p-8 text-white">
            <h3 className="text-2xl font-bold mb-4">
              The Result? Complete Business Intelligence
            </h3>
            <p className="text-lg text-blue-100 max-w-2xl mx-auto">
              Make data-driven decisions with confidence. Get the complete picture of your subscription business
              across all payment providers in one unified platform.
            </p>
          </div>
        </motion.div>
      </div>
    </section>
  )
}
