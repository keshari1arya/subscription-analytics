import { ArrowRight, Check, DollarSign, Shield, Star, X, Zap } from 'lucide-react'
import { Metadata } from 'next'
import Link from 'next/link'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'SubscriptionAnalytics vs Competitors - Why Choose Us',
  description: 'Compare SubscriptionAnalytics with other subscription analytics platforms. See why we offer the best unified analytics, multi-provider support, and actionable insights.',
  keywords: 'subscription analytics comparison, vs competitors, unified dashboard, multi-provider support',
}

const ComparisonPage = () => {
  const competitors = [
    {
      name: 'Stripe Dashboard',
      logo: '💳',
      pros: ['Native Stripe integration', 'Real-time data', 'Good for Stripe-only businesses'],
      cons: ['Stripe only', 'Limited analytics', 'No multi-provider support', 'Basic reporting'],
      rating: 3.5
    },
    {
      name: 'ChartMogul',
      logo: '📊',
      pros: ['Comprehensive analytics', 'Good reporting', 'Multiple integrations'],
      cons: ['Expensive pricing', 'Complex setup', 'Limited real-time data', 'Steep learning curve'],
      rating: 4.0
    },
    {
      name: 'Baremetrics',
      logo: '📈',
      pros: ['Good MRR tracking', 'Churn analysis', 'Simple interface'],
      cons: ['Limited integrations', 'Expensive for small businesses', 'No real-time sync', 'Basic features'],
      rating: 3.8
    },
    {
      name: 'ProfitWell',
      logo: '💰',
      pros: ['Revenue recognition', 'Good for SaaS', 'Multiple integrations'],
      cons: ['Complex pricing', 'Limited analytics', 'Poor customer support', 'Slow updates'],
      rating: 3.2
    }
  ]

  const ourFeatures = [
    { icon: Zap, text: 'Planned unified multi-provider dashboard' },
    { icon: Zap, text: 'Future real-time data synchronization' },
    { icon: Zap, text: 'Advanced predictive analytics roadmap' },
    { icon: Zap, text: 'Custom reporting & insights planned' },
    { icon: DollarSign, text: 'Affordable pricing for all sizes' },
    { icon: Zap, text: 'Easy setup & integration design' },
    { icon: Zap, text: 'Comprehensive API access planned' },
    { icon: Shield, text: 'Community support & feedback' },
    { icon: Zap, text: 'White-label solutions roadmap' },
    { icon: Shield, text: 'Advanced security & compliance planned' }
  ]

  return (
    <div className="min-h-screen bg-white">
      <Header />

      {/* Hero Section */}
      <section className="pt-32 pb-20 bg-gradient-to-br from-blue-50 via-white to-indigo-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center">
            <h1 className="text-4xl sm:text-6xl font-bold text-gray-900 mb-6">
              Why Choose <span className="text-blue-600">SubscriptionAnalytics</span>?
            </h1>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto mb-12 leading-relaxed">
              See how we stack up against the competition and why businesses choose us for their subscription analytics needs.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <Link
                href="/#waitlist"
                className="bg-blue-600 text-white px-8 py-4 rounded-lg font-semibold hover:bg-blue-700 transition-colors inline-flex items-center"
              >
                Join Waitlist
                <ArrowRight className="ml-2 w-5 h-5" />
              </Link>
              <Link
                href="/#screenshots"
                className="border-2 border-blue-600 text-blue-600 px-8 py-4 rounded-lg font-semibold hover:bg-blue-50 transition-colors"
              >
                View Demo
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* Our Advantages */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              What Makes Us Different
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              We combine the best features from all platforms while adding unique capabilities that others can&apos;t match.
            </p>
          </div>

          <div className="grid md:grid-cols-2 lg:grid-cols-5 gap-6">
            {ourFeatures.map((feature, index) => (
              <div key={index} className="bg-gradient-to-br from-blue-50 to-indigo-50 p-6 rounded-xl border border-blue-100 hover:shadow-lg transition-all duration-300 hover:scale-105">
                <div className="flex items-center mb-3">
                  <feature.icon className="w-5 h-5 text-blue-600 mr-3" />
                  <span className="font-semibold text-gray-900 text-sm leading-tight">{feature.text}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Comparison Table */}
      <section className="py-20 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Platform Comparison
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              See how we compare to leading subscription analytics platforms
            </p>
          </div>

          <div className="grid gap-8">
            {competitors.map((competitor, index) => (
              <div key={index} className="bg-white rounded-2xl shadow-xl border border-gray-100 p-8 hover:shadow-2xl transition-all duration-300">
                <div className="flex items-center justify-between mb-8">
                  <div className="flex items-center">
                    <span className="text-4xl mr-6">{competitor.logo}</span>
                    <div>
                      <h3 className="text-2xl font-bold text-gray-900">{competitor.name}</h3>
                      <div className="flex items-center mt-2">
                        {[...Array(5)].map((_, i) => (
                          <Star
                            key={i}
                            className={`w-5 h-5 ${i < Math.floor(competitor.rating) ? 'text-yellow-400 fill-current' : 'text-gray-300'}`}
                          />
                        ))}
                        <span className="ml-3 text-sm text-gray-600 font-medium">({competitor.rating}/5)</span>
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center justify-center flex-1">
                    <span className="text-sm text-gray-500 font-medium bg-gray-100 px-3 py-1 rounded-full">vs</span>
                  </div>
                  <div className="text-right">
                    <div className="text-2xl font-bold text-blue-600">SubscriptionAnalytics</div>
                  </div>
                </div>

                <div className="grid md:grid-cols-2 gap-8">
                  <div className="bg-green-50 rounded-xl p-6">
                    <h4 className="font-semibold text-gray-900 mb-4 flex items-center text-lg">
                      <Check className="w-5 h-5 text-green-500 mr-3" />
                      What they do well
                    </h4>
                    <ul className="space-y-3">
                      {competitor.pros.map((pro, i) => (
                        <li key={i} className="flex items-start">
                          <Check className="w-4 h-4 text-green-500 mr-3 mt-1 flex-shrink-0" />
                          <span className="text-gray-700">{pro}</span>
                        </li>
                      ))}
                    </ul>
                  </div>

                  <div className="bg-red-50 rounded-xl p-6">
                    <h4 className="font-semibold text-gray-900 mb-4 flex items-center text-lg">
                      <X className="w-5 h-5 text-red-500 mr-3" />
                      Their limitations
                    </h4>
                    <ul className="space-y-3">
                      {competitor.cons.map((con, i) => (
                        <li key={i} className="flex items-start">
                          <X className="w-4 h-4 text-red-500 mr-3 mt-1 flex-shrink-0" />
                          <span className="text-gray-700">{con}</span>
                        </li>
                      ))}
                    </ul>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Why Choose Us */}
      <section className="py-20 bg-gradient-to-r from-blue-600 via-blue-700 to-indigo-700 text-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold mb-6">
              Why Businesses Choose SubscriptionAnalytics
            </h2>
            <p className="text-xl opacity-90 max-w-3xl mx-auto">
              We solve the problems that other platforms can&apos;t address effectively
            </p>
          </div>

          <div className="grid md:grid-cols-3 gap-8">
            <div className="text-center bg-white/10 rounded-2xl p-8 backdrop-blur-sm">
              <div className="bg-white/20 rounded-full w-20 h-20 flex items-center justify-center mx-auto mb-6">
                <span className="text-3xl">🔗</span>
              </div>
              <h3 className="text-xl font-semibold mb-4">Multi-Provider Support</h3>
              <p className="opacity-90 leading-relaxed">
                Connect Stripe, PayPal, Square, and more in one unified dashboard. No more switching between platforms or dealing with fragmented data.
              </p>
            </div>

            <div className="text-center bg-white/10 rounded-2xl p-8 backdrop-blur-sm">
              <div className="bg-white/20 rounded-full w-20 h-20 flex items-center justify-center mx-auto mb-6">
                <span className="text-3xl">⚡</span>
              </div>
              <h3 className="text-xl font-semibold mb-4">Real-Time Analytics</h3>
              <p className="opacity-90 leading-relaxed">
                Get instant insights with real-time data synchronization. Make decisions based on current data, not yesterday&apos;s reports.
              </p>
            </div>

            <div className="text-center bg-white/10 rounded-2xl p-8 backdrop-blur-sm">
              <div className="bg-white/20 rounded-full w-20 h-20 flex items-center justify-center mx-auto mb-6">
                <span className="text-3xl">💰</span>
              </div>
              <h3 className="text-xl font-semibold mb-4">Affordable Pricing</h3>
              <p className="opacity-90 leading-relaxed">
                Start free and scale as you grow. No hidden fees or complex pricing tiers. Pay only for what you use.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-20 bg-white">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-6">
            Ready to See the Difference?
          </h2>
          <p className="text-lg text-gray-600 mb-10 max-w-2xl mx-auto leading-relaxed">
            Join thousands of businesses that have switched to SubscriptionAnalytics for better insights, unified data, and actionable results.
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Link
              href="/#waitlist"
              className="bg-blue-600 text-white px-10 py-4 rounded-lg font-semibold hover:bg-blue-700 transition-colors inline-flex items-center justify-center"
            >
              Join Waitlist
              <ArrowRight className="ml-2 w-5 h-5" />
            </Link>
            <Link
              href="/#screenshots"
              className="border-2 border-blue-600 text-blue-600 px-10 py-4 rounded-lg font-semibold hover:bg-blue-50 transition-colors"
            >
              View Demo
            </Link>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  )
}

export default ComparisonPage
