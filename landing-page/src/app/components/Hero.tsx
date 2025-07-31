'use client'

import { motion } from 'framer-motion'
import { ArrowRight, CheckCircle, Play } from 'lucide-react'
import Button from './Button'

export default function Hero() {
  return (
    <section className="relative min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 via-white to-indigo-50 overflow-hidden">
      {/* Background Elements */}
      <div className="absolute inset-0 overflow-hidden">
                {/* Bar Chart */}
        <div className="absolute top-20 left-10 w-80 h-60 bg-gradient-to-br from-blue-200 to-blue-300 rounded-lg mix-blend-multiply filter blur-md opacity-40 animate-blob">
          <div className="absolute bottom-4 left-4 w-8 h-16 bg-blue-400 rounded-sm"></div>
          <div className="absolute bottom-4 left-16 w-8 h-24 bg-blue-400 rounded-sm"></div>
          <div className="absolute bottom-4 left-28 w-8 h-20 bg-blue-400 rounded-sm"></div>
          <div className="absolute bottom-4 left-40 w-8 h-32 bg-blue-400 rounded-sm"></div>
          <div className="absolute bottom-4 left-52 w-8 h-28 bg-blue-400 rounded-sm"></div>
        </div>

        {/* Line Chart */}
        <div className="absolute top-40 right-10 w-80 h-60 bg-gradient-to-br from-purple-200 to-purple-300 rounded-lg mix-blend-multiply filter blur-md opacity-40 animate-blob animation-delay-2000">
          <svg className="w-full h-full" viewBox="0 0 100 60">
            <path d="M10,50 L25,40 L40,35 L55,25 L70,20 L85,15" stroke="rgba(147, 51, 234, 0.4)" strokeWidth="3" fill="none" strokeLinecap="round"/>
            <circle cx="25" cy="40" r="2" fill="rgba(147, 51, 234, 0.5)"/>
            <circle cx="40" cy="35" r="2" fill="rgba(147, 51, 234, 0.5)"/>
            <circle cx="55" cy="25" r="2" fill="rgba(147, 51, 234, 0.5)"/>
            <circle cx="70" cy="20" r="2" fill="rgba(147, 51, 234, 0.5)"/>
            <circle cx="85" cy="15" r="2" fill="rgba(147, 51, 234, 0.5)"/>
          </svg>
        </div>

        {/* Pie Chart */}
        <div className="absolute -bottom-8 left-20 w-72 h-72 bg-gradient-to-br from-indigo-200 to-indigo-300 rounded-full mix-blend-multiply filter blur-md opacity-40 animate-blob animation-delay-4000">
          <svg className="w-full h-full" viewBox="0 0 100 100">
            <circle cx="50" cy="50" r="30" fill="rgba(99, 102, 241, 0.4)" stroke="rgba(99, 102, 241, 0.5)" strokeWidth="2"/>
            <path d="M50,50 L50,20 A30,30 0 0,1 70,50 Z" fill="rgba(139, 92, 246, 0.4)"/>
            <path d="M50,50 L70,50 A30,30 0 0,1 60,70 Z" fill="rgba(168, 85, 247, 0.4)"/>
          </svg>
        </div>

        {/* Analytics Dashboard Grid */}
        <div className="absolute top-1/2 right-1/4 w-64 h-48 bg-gradient-to-br from-green-200 to-green-300 rounded-lg mix-blend-multiply filter blur-md opacity-35 animate-blob animation-delay-1000">
          <div className="absolute top-4 left-4 w-12 h-8 bg-green-400 rounded-sm"></div>
          <div className="absolute top-4 left-20 w-12 h-12 bg-green-400 rounded-sm"></div>
          <div className="absolute top-4 left-36 w-12 h-6 bg-green-400 rounded-sm"></div>
          <div className="absolute top-20 left-4 w-12 h-10 bg-green-400 rounded-sm"></div>
          <div className="absolute top-20 left-20 w-12 h-8 bg-green-400 rounded-sm"></div>
          <div className="absolute top-20 left-36 w-12 h-14 bg-green-400 rounded-sm"></div>
        </div>
      </div>

      <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pt-20 pb-16">
        <div className="text-center">
          {/* Main Headline */}
          <motion.h1
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            className="text-4xl md:text-6xl lg:text-7xl font-bold text-gray-900 mb-6"
          >
            Unified{' '}
            <span className="text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-indigo-600">
              Subscription Analytics
            </span>
            {' '}Platform
          </motion.h1>

          {/* Subheadline */}
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            className="text-xl md:text-2xl text-gray-600 mb-8 max-w-4xl mx-auto"
          >
            Connect multiple payment providers (Stripe, PayPal, Square) in one dashboard.
            Get unified analytics, real-time insights, and predictive analytics for your subscription business.
          </motion.p>

                    {/* MVP Status */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.4 }}
            className="bg-blue-50 border border-blue-200 rounded-lg p-4 mb-8 max-w-2xl mx-auto shadow-sm"
          >
            <p className="text-blue-800 text-sm font-medium">
              🚀 <strong>Early Access:</strong> We&apos;re actively building core features.
              Join our waitlist to get priority access and shape the product roadmap.
            </p>
          </motion.div>

          {/* Trust Indicators */}
          <motion.div
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.4 }}
            className="flex flex-wrap justify-center items-center gap-6 mb-8 text-sm text-gray-600"
          >
            <div className="flex items-center gap-2">
              <CheckCircle className="w-5 h-5 text-green-500" />
              <span>Free early access</span>
            </div>
            <div className="flex items-center gap-2">
              <CheckCircle className="w-5 h-5 text-green-500" />
              <span>No credit card required</span>
            </div>
            <div className="flex items-center gap-2">
              <CheckCircle className="w-5 h-5 text-green-500" />
              <span>Shape the product</span>
            </div>
          </motion.div>

          {/* CTA Buttons */}
          <motion.div
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.6 }}
            className="flex flex-col sm:flex-row gap-4 justify-center items-center mb-12"
          >
            <Button size="lg" href="#waitlist" className="group">
              Join Waitlist
              <ArrowRight className="ml-2 w-5 h-5 group-hover:translate-x-1 transition-transform" />
            </Button>
            <Button variant="outline" size="lg" href="#demo" className="group">
              <Play className="mr-2 w-5 h-5" />
              Watch Demo
            </Button>
          </motion.div>

          {/* Social Proof */}
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.8, delay: 0.8 }}
            className="text-center"
          >
            <p className="text-gray-500 mb-2">Already 2,847 people on the waitlist</p>
            <div className="flex justify-center items-center space-x-1">
              {[...Array(5)].map((_, i) => (
                <div
                  key={i}
                  className="w-8 h-8 bg-gray-300 rounded-full flex items-center justify-center text-xs font-semibold text-gray-600"
                  role="img"
                  aria-label={`User avatar ${i + 1}`}
                >
                  {String.fromCharCode(65 + i)}
                </div>
              ))}
              <span className="text-sm text-gray-500 ml-2">+2,842 more</span>
            </div>
          </motion.div>
        </div>
      </div>

      {/* Scroll Indicator */}
      <motion.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 1, delay: 1 }}
        className="absolute bottom-8 left-1/2 transform -translate-x-1/2"
      >
        <div className="w-6 h-10 border-2 border-gray-400 rounded-full flex justify-center">
          <motion.div
            animate={{ y: [0, 12, 0] }}
            transition={{ duration: 1.5, repeat: Infinity }}
            className="w-1 h-3 bg-gray-400 rounded-full mt-2"
          />
        </div>
      </motion.div>
    </section>
  )
}
