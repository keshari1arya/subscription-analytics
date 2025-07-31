import type { Metadata } from 'next'

export const metadata: Metadata = {
  title: 'Careers - Join SubscriptionAnalytics Team',
  description: 'Join our mission to democratize subscription analytics. Explore open positions and learn about our culture at SubscriptionAnalytics.',
  keywords: 'careers, jobs, work at SubscriptionAnalytics, remote jobs, tech careers',
}

export default function CareersLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return children
}
