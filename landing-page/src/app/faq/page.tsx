import type { Metadata } from 'next'
import FAQSection from '../components/FAQSection'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'FAQ - Frequently Asked Questions | SubscriptionAnalytics',
  description: 'Find answers to common questions about SubscriptionAnalytics. Learn about features, pricing, security, integrations, and more.',
  keywords: 'FAQ, frequently asked questions, subscription analytics, support, help, features, pricing, security',
  openGraph: {
    title: 'FAQ - Frequently Asked Questions | SubscriptionAnalytics',
    description: 'Find answers to common questions about SubscriptionAnalytics. Learn about features, pricing, security, integrations, and more.',
    type: 'website',
    url: 'https://subscriptionanalytics.com/faq',
  },
  twitter: {
    card: 'summary_large_image',
    title: 'FAQ - Frequently Asked Questions | SubscriptionAnalytics',
    description: 'Find answers to common questions about SubscriptionAnalytics. Learn about features, pricing, security, integrations, and more.',
  },
}

export default function FAQPage() {
  return (
    <main className="min-h-screen">
      <Header />
      <div className="pt-32">
        <FAQSection />
      </div>
      <Footer />
    </main>
  )
}
