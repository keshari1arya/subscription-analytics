import type { Metadata } from 'next'
import { Inter } from 'next/font/google'
import Script from 'next/script'
import './globals.css'

const inter = Inter({ subsets: ['latin'] })

export const metadata: Metadata = {
  title: 'SubscriptionAnalytics - Unified Subscription Intelligence Platform',
  description: 'Connect multiple payment providers (Stripe, PayPal, Square) in one dashboard. Get unified analytics, real-time insights, and predictive analytics for your subscription business.',
  keywords: [
    'subscription analytics',
    'revenue intelligence',
    'MRR tracking',
    'ARR tracking',
    'churn analysis',
    'payment providers',
    'Stripe analytics',
    'PayPal analytics',
    'unified dashboard',
    'subscription metrics',
    'business intelligence',
    'SaaS analytics'
  ],
  authors: [{ name: 'SubscriptionAnalytics Team' }],
  creator: 'SubscriptionAnalytics',
  publisher: 'SubscriptionAnalytics',
  formatDetection: {
    email: false,
    address: false,
    telephone: false,
  },
  metadataBase: new URL('https://subscriptionanalytics.com'),
  alternates: {
    canonical: '/',
  },
  openGraph: {
    title: 'SubscriptionAnalytics - Unified Subscription Intelligence Platform',
    description: 'Connect multiple payment providers in one dashboard. Get unified analytics, real-time insights, and predictive analytics for your subscription business.',
    url: 'https://subscriptionanalytics.com',
    siteName: 'SubscriptionAnalytics',
    images: [
      {
        url: '/og-image.svg',
        width: 1200,
        height: 630,
        alt: 'SubscriptionAnalytics - Unified Subscription Intelligence Platform',
      },
    ],
    locale: 'en_US',
    type: 'website',
  },
  twitter: {
    card: 'summary_large_image',
    title: 'SubscriptionAnalytics - Unified Subscription Intelligence Platform',
    description: 'Connect multiple payment providers in one dashboard. Get unified analytics, real-time insights, and predictive analytics for your subscription business.',
    images: ['/og-image.svg'],
  },
  robots: {
    index: true,
    follow: true,
    googleBot: {
      index: true,
      follow: true,
      'max-video-preview': -1,
      'max-image-preview': 'large',
      'max-snippet': -1,
    },
  },
  verification: {
    google: 'your-google-verification-code',
  },
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  const jsonLd = {
    "@context": "https://schema.org",
    "@type": "SoftwareApplication",
    "name": "SubscriptionAnalytics",
    "description": "Unified subscription analytics platform that connects multiple payment providers in one dashboard",
    "applicationCategory": "BusinessApplication",
    "operatingSystem": "Web",
    "url": "https://subscriptionanalytics.com",
    "author": {
      "@type": "Organization",
      "name": "SubscriptionAnalytics"
    },
    "offers": {
      "@type": "Offer",
      "price": "0",
      "priceCurrency": "USD",
      "description": "Free 14-day trial"
    },
    "aggregateRating": {
      "@type": "AggregateRating",
      "ratingValue": "4.8",
      "ratingCount": "2847"
    }
  }

  return (
    <html lang="en">
      <head>
        <script
          type="application/ld+json"
          dangerouslySetInnerHTML={{ __html: JSON.stringify(jsonLd) }}
        />
      </head>
      <body className={inter.className}>
        {/* Google Analytics */}
        <Script
          src="https://www.googletagmanager.com/gtag/js?id=GA_MEASUREMENT_ID"
          strategy="afterInteractive"
        />
        <Script id="google-analytics" strategy="afterInteractive">
          {`
            window.dataLayer = window.dataLayer || [];
            function gtag(){dataLayer.push(arguments);}
            gtag('js', new Date());
            gtag('config', 'GA_MEASUREMENT_ID', {
              page_title: 'SubscriptionAnalytics Landing Page',
              page_location: window.location.href,
            });
          `}
        </Script>
        {children}
      </body>
    </html>
  )
}
