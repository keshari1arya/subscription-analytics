'use client'

import { motion } from 'framer-motion'
import { useState } from 'react'
import Button from './Button'

export default function WaitlistSection() {
  const [email, setEmail] = useState('')
  const [isSubmitted, setIsSubmitted] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsLoading(true)

    try {
      const response = await fetch('/api/waitlist', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email }),
      })

      if (response.ok) {
        setIsSubmitted(true)
        setEmail('')
      }
    } catch (error) {
      console.error('Error submitting email:', error)
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <section id="waitlist" className="py-20 bg-gradient-to-r from-blue-600 to-indigo-700">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          whileInView={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          viewport={{ once: true }}
        >
          <h2 className="text-3xl md:text-4xl font-bold text-white mb-6">
            Join the Early Access Program
          </h2>
          <p className="text-xl text-blue-100 mb-8 max-w-2xl mx-auto">
            We&apos;re building the future of subscription analytics. Join our exclusive early access program
            and be among the first to experience unified subscription intelligence.
          </p>

          {/* MVP Status Banner */}
          <div className="bg-white/10 backdrop-blur-sm border border-white/20 rounded-lg p-4 mb-8 max-w-2xl mx-auto shadow-lg">
            <p className="text-white text-sm font-medium">
              🚀 <strong>MVP in Development:</strong> We&apos;re actively building core features.
              Early access members will get priority access and direct input on product development.
            </p>
          </div>

          {!isSubmitted ? (
            <motion.form
              onSubmit={handleSubmit}
              className="max-w-md mx-auto"
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.6, delay: 0.2 }}
              viewport={{ once: true }}
            >
              <div className="flex flex-col sm:flex-row gap-3">
                <input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="Enter your email address"
                  className="flex-1 px-5 py-3 rounded-lg border-0 text-gray-900 placeholder-gray-500 bg-white/90 backdrop-blur-sm shadow-lg focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-blue-600 focus:bg-white transition-all duration-200"
                  required
                />
                <Button
                  type="submit"
                  disabled={isLoading}
                  className="bg-gradient-to-r from-blue-500 to-indigo-600 hover:from-blue-600 hover:to-indigo-700 text-white font-semibold px-6 py-3 rounded-lg shadow-lg hover:shadow-xl transition-all duration-200 transform hover:scale-105"
                >
                  {isLoading ? (
                    <div className="flex items-center">
                      <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                      Joining...
                    </div>
                  ) : (
                    'Join Early Access'
                  )}
                </Button>
              </div>
            </motion.form>
          ) : (
            <motion.div
              initial={{ opacity: 0, scale: 0.95 }}
              animate={{ opacity: 1, scale: 1 }}
              className="bg-white/10 backdrop-blur-sm border border-white/20 rounded-xl p-6 max-w-md mx-auto shadow-lg"
            >
              <h3 className="text-xl font-semibold text-white mb-2">
                🎉 Welcome to Early Access!
              </h3>
              <p className="text-blue-100">
                You&apos;re now on our exclusive waitlist. We&apos;ll notify you as soon as we launch new features.
              </p>
            </motion.div>
          )}

          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, delay: 0.4 }}
            viewport={{ once: true }}
            className="mt-8 text-blue-100 text-sm"
          >
            <p>Already 2,847+ founders and growth teams on the waitlist</p>
            <p className="mt-2 text-xs opacity-75">
              No spam, ever. Unsubscribe anytime.
            </p>
          </motion.div>
        </motion.div>
      </div>
    </section>
  )
}
