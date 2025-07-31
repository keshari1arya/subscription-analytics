import { ArrowRight, Calendar, Clock, Tag, User } from 'lucide-react'
import { Metadata } from 'next'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'Blog - Subscription Analytics Insights & Tips',
  description: 'Read the latest insights on subscription analytics, SaaS metrics, and business growth strategies from the SubscriptionAnalytics team.',
  keywords: 'subscription analytics blog, SaaS metrics, business growth, MRR tracking, churn analysis',
}

const BlogPage = () => {
  const featuredPost = {
    title: 'The Complete Guide to Subscription Analytics in 2024',
    excerpt: 'Learn the essential metrics every subscription business should track, from MRR and churn to customer lifetime value and expansion revenue.',
    author: 'Alex Chen',
    date: 'January 15, 2024',
    readTime: '8 min read',
    category: 'Analytics',
    image: '📊',
    slug: 'complete-guide-subscription-analytics-2024'
  }

  const posts = [
    {
      title: 'How to Reduce Churn with Data-Driven Insights',
      excerpt: 'Discover proven strategies to identify and prevent customer churn using advanced analytics and predictive modeling.',
      author: 'Sarah Rodriguez',
      date: 'January 10, 2024',
      readTime: '6 min read',
      category: 'Growth',
      image: '📈',
      slug: 'reduce-churn-data-driven-insights'
    },
    {
      title: 'MRR vs ARR: Understanding the Key Differences',
      excerpt: 'A deep dive into Monthly Recurring Revenue vs Annual Recurring Revenue and when to use each metric.',
      author: 'Michael Kim',
      date: 'January 8, 2024',
      readTime: '5 min read',
      category: 'Metrics',
      image: '💰',
      slug: 'mrr-vs-arr-key-differences'
    },
    {
      title: 'Building a Multi-Provider Subscription Analytics Dashboard',
      excerpt: 'Step-by-step guide to creating a unified analytics dashboard that works with Stripe, PayPal, and other payment providers.',
      author: 'Emily Watson',
      date: 'January 5, 2024',
      readTime: '10 min read',
      category: 'Technical',
      image: '🔧',
      slug: 'multi-provider-analytics-dashboard'
    },
    {
      title: 'The Hidden Costs of Poor Subscription Analytics',
      excerpt: 'Why inadequate analytics tools can cost your business more than you think, and how to avoid common pitfalls.',
      author: 'Alex Chen',
      date: 'January 3, 2024',
      readTime: '7 min read',
      category: 'Business',
      image: '⚠️',
      slug: 'hidden-costs-poor-analytics'
    },
    {
      title: 'Predictive Analytics for Subscription Businesses',
      excerpt: 'How machine learning can help you predict customer behavior, forecast revenue, and make better business decisions.',
      author: 'Sarah Rodriguez',
      date: 'December 28, 2023',
      readTime: '9 min read',
      category: 'AI/ML',
      image: '🤖',
      slug: 'predictive-analytics-subscription-businesses'
    },
    {
      title: 'Customer Lifetime Value: The Ultimate Guide',
      excerpt: 'Everything you need to know about calculating, tracking, and optimizing CLV for your subscription business.',
      author: 'Michael Kim',
      date: 'December 25, 2023',
      readTime: '12 min read',
      category: 'Metrics',
      image: '🎯',
      slug: 'customer-lifetime-value-ultimate-guide'
    }
  ]

  const categories = [
    { name: 'Analytics', count: 15 },
    { name: 'Growth', count: 12 },
    { name: 'Metrics', count: 8 },
    { name: 'Technical', count: 6 },
    { name: 'Business', count: 10 },
    { name: 'AI/ML', count: 4 }
  ]

  return (
    <div className="min-h-screen bg-white">
      <Header />

      {/* Hero Section */}
      <section className="pt-32 pb-20 bg-gradient-to-br from-blue-50 via-white to-indigo-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center">
            <h1 className="text-4xl sm:text-6xl font-bold text-gray-900 mb-6">
              Subscription Analytics <span className="text-blue-600">Blog</span>
            </h1>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto mb-12 leading-relaxed">
              Insights, tips, and strategies to help you build a better subscription business
            </p>
          </div>
        </div>
      </section>

      {/* Featured Post */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-12">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Featured Article
            </h2>
          </div>

          <div className="bg-gradient-to-br from-blue-50 to-indigo-50 rounded-2xl p-8 md:p-12">
            <div className="grid lg:grid-cols-2 gap-8 items-center">
              <div>
                <div className="flex items-center mb-4">
                  <Tag className="w-4 h-4 text-blue-600 mr-2" />
                  <span className="text-blue-600 font-medium">{featuredPost.category}</span>
                </div>
                <h3 className="text-2xl sm:text-3xl font-bold text-gray-900 mb-4">
                  {featuredPost.title}
                </h3>
                <p className="text-lg text-gray-600 mb-6 leading-relaxed">
                  {featuredPost.excerpt}
                </p>
                <div className="flex items-center text-sm text-gray-500 mb-6">
                  <User className="w-4 h-4 mr-2" />
                  <span className="mr-4">{featuredPost.author}</span>
                  <Calendar className="w-4 h-4 mr-2" />
                  <span className="mr-4">{featuredPost.date}</span>
                  <Clock className="w-4 h-4 mr-2" />
                  <span>{featuredPost.readTime}</span>
                </div>
                <a
                  href={`/blog/${featuredPost.slug}`}
                  className="inline-flex items-center bg-blue-600 text-white px-6 py-3 rounded-lg font-semibold hover:bg-blue-700 transition-colors"
                >
                  Read Article
                  <ArrowRight className="ml-2 w-4 h-4" />
                </a>
              </div>
              <div className="text-center">
                <div className="text-8xl mb-4">{featuredPost.image}</div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Categories */}
      <section className="py-16 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-12">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">
              Browse by Category
            </h2>
          </div>

          <div className="grid md:grid-cols-3 lg:grid-cols-6 gap-4">
            {categories.map((category, index) => (
              <a
                key={index}
                href={`/blog/category/${category.name.toLowerCase()}`}
                className="bg-white rounded-lg p-4 text-center hover:shadow-lg transition-shadow"
              >
                <div className="font-semibold text-gray-900 mb-1">{category.name}</div>
                <div className="text-sm text-gray-500">{category.count} articles</div>
              </a>
            ))}
          </div>
        </div>
      </section>

      {/* Latest Posts */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Latest Articles
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              Stay up to date with the latest insights and strategies
            </p>
          </div>

          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
            {posts.map((post, index) => (
              <article key={index} className="bg-white rounded-xl shadow-lg hover:shadow-xl transition-shadow border border-gray-100">
                <div className="p-6">
                  <div className="flex items-center mb-4">
                    <span className="text-3xl mr-3">{post.image}</span>
                    <div>
                      <div className="text-blue-600 font-medium text-sm">{post.category}</div>
                    </div>
                  </div>
                  <h3 className="text-xl font-bold text-gray-900 mb-3">
                    {post.title}
                  </h3>
                  <p className="text-gray-600 mb-4 leading-relaxed">
                    {post.excerpt}
                  </p>
                  <div className="flex items-center text-sm text-gray-500 mb-4">
                    <User className="w-4 h-4 mr-2" />
                    <span className="mr-4">{post.author}</span>
                    <Calendar className="w-4 h-4 mr-2" />
                    <span className="mr-4">{post.date}</span>
                    <Clock className="w-4 h-4 mr-2" />
                    <span>{post.readTime}</span>
                  </div>
                  <a
                    href={`/blog/${post.slug}`}
                    className="inline-flex items-center text-blue-600 font-semibold hover:text-blue-700 transition-colors"
                  >
                    Read More
                    <ArrowRight className="ml-2 w-4 h-4" />
                  </a>
                </div>
              </article>
            ))}
          </div>

          <div className="text-center mt-12">
            <a
              href="/blog/archive"
              className="bg-blue-600 text-white px-8 py-3 rounded-lg font-semibold hover:bg-blue-700 transition-colors"
            >
              View All Articles
            </a>
          </div>
        </div>
      </section>

      {/* Newsletter */}
      <section className="py-20 bg-gradient-to-r from-blue-600 to-indigo-700 text-white">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-3xl sm:text-4xl font-bold mb-6">
            Stay Updated
          </h2>
          <p className="text-xl opacity-90 mb-8 max-w-2xl mx-auto">
            Get the latest subscription analytics insights delivered to your inbox
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center max-w-md mx-auto">
            <input
              type="email"
              placeholder="Enter your email"
              className="flex-1 px-4 py-3 rounded-lg text-gray-900 focus:outline-none focus:ring-2 focus:ring-white"
            />
            <button className="bg-white text-blue-600 px-6 py-3 rounded-lg font-semibold hover:bg-gray-100 transition-colors">
              Subscribe
            </button>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  )
}

export default BlogPage
