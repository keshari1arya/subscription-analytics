'use client'

import { motion } from 'framer-motion'
import { Maximize2, Pause, Play, Settings, Volume2 } from 'lucide-react'
import { useState } from 'react'

export default function DemoVideo() {
  const [isPlaying, setIsPlaying] = useState(false)
  const [isMuted, setIsMuted] = useState(false)

  return (
    <section className="py-20 bg-gradient-to-br from-gray-50 to-gray-100">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-6"
          >
            See SubscriptionAnalytics in Action
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-3xl mx-auto"
          >
            Watch how we seamlessly integrate data from Stripe, PayPal, and Square
            to give you a unified view of your subscription business.
          </motion.p>
        </div>

        <div className="grid lg:grid-cols-2 gap-12 items-center">
          {/* Video Player */}
          <motion.div
            initial={{ opacity: 0, x: -30 }}
            whileInView={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="relative"
          >
            <div className="bg-white rounded-2xl shadow-2xl overflow-hidden">
              {/* Video Container */}
              <div className="relative aspect-video bg-gradient-to-br from-blue-600 to-indigo-700">
                {/* Video Placeholder */}
                <div className="absolute inset-0 flex items-center justify-center">
                  <div className="text-center text-white">
                    <div className="w-24 h-24 bg-white/20 rounded-full flex items-center justify-center mb-6 mx-auto">
                      <Play className="w-12 h-12 ml-1" />
                    </div>
                    <h3 className="text-2xl font-bold mb-2">Product Demo</h3>
                    <p className="text-blue-100">
                      Coming Soon
                      <span className="ml-1 inline-flex space-x-1">
                        <span className="w-1 h-1 bg-blue-100 rounded-full animate-dot-grow" style={{ animationDelay: '0ms' }}></span>
                        <span className="w-1 h-1 bg-blue-100 rounded-full animate-dot-grow" style={{ animationDelay: '500ms' }}></span>
                        <span className="w-1 h-1 bg-blue-100 rounded-full animate-dot-grow" style={{ animationDelay: '1000ms' }}></span>
                      </span>
                    </p>
                  </div>
                </div>

                {/* Video Controls */}
                <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black/80 to-transparent p-6">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-4">
                      <button
                        onClick={() => setIsPlaying(!isPlaying)}
                        className="text-white hover:text-blue-300 transition-colors"
                      >
                        {isPlaying ? <Pause className="w-6 h-6" /> : <Play className="w-6 h-6" />}
                      </button>
                      <button
                        onClick={() => setIsMuted(!isMuted)}
                        className="text-white hover:text-blue-300 transition-colors"
                      >
                        <Volume2 className="w-5 h-5" />
                      </button>
                    </div>
                    <div className="flex items-center space-x-2">
                      <button className="text-white hover:text-blue-300 transition-colors">
                        <Settings className="w-5 h-5" />
                      </button>
                      <button className="text-white hover:text-blue-300 transition-colors">
                        <Maximize2 className="w-5 h-5" />
                      </button>
                    </div>
                  </div>
                </div>

                {/* Progress Bar */}
                <div className="absolute bottom-16 left-6 right-6">
                  <div className="w-full bg-white/20 rounded-full h-1">
                    <div className="bg-blue-400 h-1 rounded-full" style={{ width: '35%' }}></div>
                  </div>
                </div>
              </div>

              {/* Video Info */}
              <div className="p-6">
                <h3 className="text-xl font-bold text-gray-900 mb-2">
                  Complete Platform Walkthrough
                </h3>
                <p className="text-gray-600 mb-4">
                  See how SubscriptionAnalytics brings together data from multiple payment providers
                  to give you actionable insights about your subscription business.
                </p>
                <div className="flex items-center space-x-4 text-sm text-gray-500">
                  {/* <span>Coming Soon</span> */}
                  {/* <span>•</span> */}
                  <span>HD Quality</span>
                  <span>•</span>
                  <span>No sound</span>
                </div>
              </div>
            </div>
          </motion.div>

          {/* Video Content */}
          <motion.div
            initial={{ opacity: 0, x: 30 }}
            whileInView={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="space-y-8"
          >
            <div>
              <h3 className="text-2xl font-bold text-gray-900 mb-4">
                What You&apos;ll Learn
              </h3>
              <div className="space-y-4">
                <div className="flex items-start space-x-3">
                  <div className="w-6 h-6 bg-blue-600 text-white rounded-full flex items-center justify-center text-sm font-bold mt-0.5">
                    1
                  </div>
                  <div>
                    <h4 className="font-semibold text-gray-900">Multi-Provider Integration</h4>
                    <p className="text-gray-600 text-sm">
                      See how we connect Stripe, PayPal, and Square in one dashboard
                    </p>
                  </div>
                </div>
                <div className="flex items-start space-x-3">
                  <div className="w-6 h-6 bg-blue-600 text-white rounded-full flex items-center justify-center text-sm font-bold mt-0.5">
                    2
                  </div>
                  <div>
                    <h4 className="font-semibold text-gray-900">Real-Time Analytics</h4>
                    <p className="text-gray-600 text-sm">
                      Watch live data synchronization and instant insights
                    </p>
                  </div>
                </div>
                <div className="flex items-start space-x-3">
                  <div className="w-6 h-6 bg-blue-600 text-white rounded-full flex items-center justify-center text-sm font-bold mt-0.5">
                    3
                  </div>
                  <div>
                    <h4 className="font-semibold text-gray-900">Advanced Reporting</h4>
                    <p className="text-gray-600 text-sm">
                      Explore MRR tracking, churn analysis, and customer insights
                    </p>
                  </div>
                </div>
                <div className="flex items-start space-x-3">
                  <div className="w-6 h-6 bg-blue-600 text-white rounded-full flex items-center justify-center text-sm font-bold mt-0.5">
                    4
                  </div>
                  <div>
                    <h4 className="font-semibold text-gray-900">Custom Dashboards</h4>
                    <p className="text-gray-600 text-sm">
                      See how to create personalized views for your team
                    </p>
                  </div>
                </div>
              </div>
            </div>

            <div className="bg-blue-50 rounded-xl p-6 border border-blue-200">
              <h4 className="font-semibold text-blue-900 mb-2">Key Highlights</h4>
              <ul className="space-y-2 text-sm text-blue-800">
                <li>• Unified data from multiple payment providers</li>
                <li>• Real-time MRR and ARR calculations</li>
                <li>• Advanced churn prediction algorithms</li>
                <li>• Custom alerting and notifications</li>
                <li>• Export capabilities for reporting</li>
              </ul>
            </div>

            <div className="flex flex-col sm:flex-row space-y-4 sm:space-y-0 sm:space-x-4">
              <button className="bg-blue-600 text-white px-8 py-3 rounded-lg font-semibold hover:bg-blue-700 transition-colors flex items-center justify-center">
                <Play className="w-5 h-5 mr-2" />
                Watch Full Demo
              </button>
              <button className="border border-blue-600 text-blue-600 px-8 py-3 rounded-lg font-semibold hover:bg-blue-50 transition-colors">
                Schedule a Call
              </button>
            </div>

            <div className="text-center text-sm text-gray-500">
              <p>Free 14-day trial • No credit card required</p>
            </div>
          </motion.div>
        </div>
      </div>
    </section>
  )
}
