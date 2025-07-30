'use client'

import { motion } from 'framer-motion'
import { AlertTriangle, BarChart3, Clock, X } from 'lucide-react'

const problems = [
  {
    icon: X,
    title: 'Limited Provider Support',
    description: 'Most tools only support Stripe, leaving you with fragmented data from other providers like PayPal and Square.',
    color: 'text-red-500',
    bgColor: 'bg-red-50'
  },
  {
    icon: BarChart3,
    title: 'Fragmented Data',
    description: 'Your subscription data is scattered across multiple platforms with no unified view or insights.',
    color: 'text-orange-500',
    bgColor: 'bg-orange-50'
  },
  {
    icon: Clock,
    title: 'Complex Setup',
    description: 'Weeks of integration work required to connect multiple providers and build custom dashboards.',
    color: 'text-yellow-500',
    bgColor: 'bg-yellow-50'
  }
]

export default function ProblemSection() {
  return (
    <section id="problem" className="py-20 bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-6"
          >
            The Problem with Current Solutions
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-3xl mx-auto"
          >
            Traditional analytics tools fall short when you&apos;re using multiple payment providers.
                          Here&apos;s what you&apos;re dealing with:
          </motion.p>
        </div>

        <div className="grid md:grid-cols-3 gap-8">
          {problems.map((problem, index) => (
            <motion.div
              key={problem.title}
              initial={{ opacity: 0, y: 30 }}
              whileInView={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.8, delay: index * 0.2 }}
              viewport={{ once: true }}
              className="relative"
            >
              <div className={`${problem.bgColor} p-8 rounded-2xl h-full border border-gray-200 hover:shadow-lg transition-shadow duration-300`}>
                <div className={`${problem.color} mb-4`}>
                  <problem.icon className="w-12 h-12" />
                </div>
                <h3 className="text-xl font-semibold text-gray-900 mb-4">
                  {problem.title}
                </h3>
                <p className="text-gray-600 leading-relaxed">
                  {problem.description}
                </p>
              </div>
            </motion.div>
          ))}
        </div>

        {/* Impact Statement */}
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, delay: 0.6 }}
          viewport={{ once: true }}
          className="mt-16 text-center"
        >
          <div className="bg-white rounded-2xl p-8 shadow-lg border border-gray-200">
            <AlertTriangle className="w-16 h-16 text-red-500 mx-auto mb-4" />
            <h3 className="text-2xl font-bold text-gray-900 mb-4">
              The Result? Missed Opportunities
            </h3>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              Without unified analytics, you&apos;re making business decisions with incomplete data.
              You&apos;re missing insights that could drive growth, reduce churn, and optimize your subscription business.
            </p>
          </div>
        </motion.div>
      </div>
    </section>
  )
}
