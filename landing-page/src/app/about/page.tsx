import { Globe, Heart, Target, Users, Zap } from 'lucide-react'
import { Metadata } from 'next'
import Link from 'next/link'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'About SubscriptionAnalytics - Our Story & Mission',
  description: 'Learn about SubscriptionAnalytics, our mission to unify subscription analytics, and the team behind the platform that helps businesses make data-driven decisions.',
  keywords: 'about us, subscription analytics team, company mission, unified analytics platform',
}

const AboutPage = () => {
  const values = [
    {
      icon: Target,
      title: 'Customer Success',
      description: 'We believe in putting our customers first. Every feature we build is designed to solve real problems that subscription businesses face.'
    },
    {
      icon: Zap,
      title: 'Innovation',
      description: 'We constantly push the boundaries of what\'s possible in subscription analytics, bringing cutting-edge technology to businesses of all sizes.'
    },
    {
      icon: Users,
      title: 'Transparency',
      description: 'We believe in open, honest communication with our customers. No hidden fees, no surprises - just clear, straightforward pricing and features.'
    },
    {
      icon: Heart,
      title: 'Community',
      description: 'We\'re building more than a platform - we\'re building a community of subscription businesses that support and learn from each other.'
    }
  ]

  const team = [
    {
      name: 'Alex Chen',
      role: 'CEO & Founder',
      bio: 'Former VP of Product at Stripe, with 15+ years in fintech and subscription analytics.',
      avatar: '👨‍💼'
    },
    {
      name: 'Sarah Rodriguez',
      role: 'CTO',
      bio: 'Ex-Google engineer specializing in real-time data systems and scalable architecture.',
      avatar: '👩‍💻'
    },
    {
      name: 'Michael Kim',
      role: 'Head of Product',
      bio: 'Product leader with experience at ChartMogul and Baremetrics, passionate about user experience.',
      avatar: '👨‍🎨'
    },
    {
      name: 'Emily Watson',
      role: 'Head of Customer Success',
      bio: 'Dedicated to ensuring every customer gets the most value from our platform.',
      avatar: '👩‍💼'
    }
  ]

  return (
    <div className="min-h-screen bg-white">
      <Header />

      {/* Hero Section */}
      <section className="pt-32 pb-20 bg-gradient-to-br from-blue-50 via-white to-indigo-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center">
            <h1 className="text-4xl sm:text-6xl font-bold text-gray-900 mb-6">
              About <span className="text-blue-600">SubscriptionAnalytics</span>
            </h1>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto mb-12 leading-relaxed">
              We&apos;re on a mission to democratize subscription analytics, making powerful insights accessible to businesses of all sizes.
            </p>
          </div>
        </div>
      </section>

      {/* Our Story */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="grid lg:grid-cols-2 gap-12 items-center">
            <div>
              <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-6">
                Our Story
              </h2>
              <div className="space-y-6 text-lg text-gray-600 leading-relaxed">
                <p>
                  SubscriptionAnalytics was born from frustration. Our founder, Alex Chen, was running a SaaS company and found himself constantly switching between different payment provider dashboards to understand his business metrics.
                </p>
                <p>
                  &ldquo;I was spending more time compiling data from Stripe, PayPal, and other platforms than actually analyzing it,&rdquo; Alex recalls. &ldquo;There had to be a better way.&rdquo;
                </p>
                <p>
                  In 2023, we set out to solve this problem. We built a unified platform that connects all your payment providers and gives you a single source of truth for your subscription analytics.
                </p>
                <p>
                  Today, we&apos;re helping thousands of businesses make data-driven decisions about their subscription businesses, from startups to enterprise companies.
                </p>
              </div>
            </div>
            <div className="bg-gradient-to-br from-blue-100 to-indigo-100 rounded-2xl p-8">
              <div className="text-center">
                <Globe className="w-16 h-16 text-blue-600 mx-auto mb-4" />
                <h3 className="text-2xl font-bold text-gray-900 mb-4">Our Mission</h3>
                <p className="text-lg text-gray-600 leading-relaxed">
                  To empower every subscription business with the insights they need to grow, scale, and succeed in the digital economy.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Our Values */}
      <section className="py-20 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Our Values
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              These core principles guide everything we do
            </p>
          </div>

          <div className="grid md:grid-cols-2 gap-8">
            {values.map((value, index) => (
              <div key={index} className="bg-white rounded-xl p-8 shadow-lg hover:shadow-xl transition-shadow">
                <div className="flex items-start">
                  <div className="bg-blue-100 rounded-lg p-3 mr-4">
                    <value.icon className="w-6 h-6 text-blue-600" />
                  </div>
                  <div>
                    <h3 className="text-xl font-semibold text-gray-900 mb-3">{value.title}</h3>
                    <p className="text-gray-600 leading-relaxed">{value.description}</p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Team */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Meet Our Team
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              The passionate people behind SubscriptionAnalytics
            </p>
          </div>

          <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-8">
            {team.map((member, index) => (
              <div key={index} className="text-center">
                <div className="bg-gradient-to-br from-blue-100 to-indigo-100 rounded-full w-24 h-24 flex items-center justify-center mx-auto mb-4">
                  <span className="text-3xl">{member.avatar}</span>
                </div>
                <h3 className="text-xl font-semibold text-gray-900 mb-2">{member.name}</h3>
                <p className="text-blue-600 font-medium mb-3">{member.role}</p>
                <p className="text-gray-600 text-sm leading-relaxed">{member.bio}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Stats */}
      <section className="py-20 bg-gradient-to-r from-blue-600 to-indigo-700 text-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold mb-4">
              Our Impact
            </h2>
            <p className="text-xl opacity-90">
              Numbers that tell our story
            </p>
          </div>

          <div className="grid md:grid-cols-4 gap-8">
            <div className="text-center">
              <div className="text-4xl font-bold mb-2">10,000+</div>
              <div className="text-blue-100">Businesses Served</div>
            </div>
            <div className="text-center">
              <div className="text-4xl font-bold mb-2">$2B+</div>
              <div className="text-blue-100">Revenue Tracked</div>
            </div>
            <div className="text-center">
              <div className="text-4xl font-bold mb-2">50+</div>
              <div className="text-blue-100">Payment Providers</div>
            </div>
            <div className="text-center">
              <div className="text-4xl font-bold mb-2">99.9%</div>
              <div className="text-blue-100">Uptime</div>
            </div>
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="py-20 bg-white">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-6">
            Join Our Mission
          </h2>
          <p className="text-lg text-gray-600 mb-10 max-w-2xl mx-auto leading-relaxed">
            Ready to transform how you understand your subscription business? Join thousands of companies already using SubscriptionAnalytics.
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Link
              href="/#waitlist"
              className="bg-blue-600 text-white px-10 py-4 rounded-lg font-semibold hover:bg-blue-700 transition-colors"
            >
              Get Started Today
            </Link>
            <Link
              href="/#contact"
              className="border-2 border-blue-600 text-blue-600 px-10 py-4 rounded-lg font-semibold hover:bg-blue-50 transition-colors"
            >
              Contact Us
            </Link>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  )
}

export default AboutPage
