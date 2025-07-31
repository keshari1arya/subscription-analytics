import { Metadata } from 'next'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'Cookie Policy - SubscriptionAnalytics',
  description: 'Learn about how SubscriptionAnalytics uses cookies and similar technologies to enhance your experience.',
  keywords: 'cookie policy, cookies, tracking technologies, web analytics, user experience',
}

const CookiesPage = () => {
  return (
    <div className="min-h-screen bg-white">
      <Header />

      {/* Content */}
      <section className="pt-32 pb-20">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h1 className="text-4xl sm:text-5xl font-bold text-gray-900 mb-6">
              Cookie Policy
            </h1>
            <p className="text-lg text-gray-600">
              Last updated: January 15, 2024
            </p>
          </div>

          <div className="prose prose-lg max-w-none">
            <p className="text-gray-600 mb-8">
              This Cookie Policy explains how SubscriptionAnalytics uses cookies and similar technologies to enhance your experience on our website and platform. By using our services, you consent to the use of cookies in accordance with this policy.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">What Are Cookies?</h2>
            <p className="text-gray-600 mb-6">
              Cookies are small text files that are stored on your device (computer, tablet, or mobile phone) when you visit a website. They help websites remember information about your visit, such as your preferred language and other settings, which can make your next visit easier and the site more useful to you.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">How We Use Cookies</h2>
            <p className="text-gray-600 mb-4">
              We use cookies for several purposes:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>To provide you with a better user experience</li>
              <li>To remember your preferences and settings</li>
              <li>To analyze how our website is used</li>
              <li>To improve our services and functionality</li>
              <li>To provide personalized content and advertisements</li>
              <li>To ensure security and prevent fraud</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Types of Cookies We Use</h2>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">1. Essential Cookies</h3>
            <p className="text-gray-600 mb-4">
              These cookies are necessary for the website to function properly. They enable basic functions like page navigation, access to secure areas, and form submissions. The website cannot function properly without these cookies.
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Authentication cookies to keep you logged in</li>
              <li>Security cookies to protect against fraud</li>
              <li>Session cookies to maintain your session</li>
            </ul>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">2. Performance Cookies</h3>
            <p className="text-gray-600 mb-4">
              These cookies help us understand how visitors interact with our website by collecting and reporting information anonymously.
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Google Analytics cookies to track website usage</li>
              <li>Error tracking cookies to identify and fix issues</li>
              <li>Performance monitoring cookies</li>
            </ul>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">3. Functional Cookies</h3>
            <p className="text-gray-600 mb-4">
              These cookies enable the website to provide enhanced functionality and personalization. They may be set by us or by third-party providers whose services we have added to our pages.
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Language preference cookies</li>
              <li>Theme and display preference cookies</li>
              <li>Form auto-fill cookies</li>
            </ul>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">4. Marketing Cookies</h3>
            <p className="text-gray-600 mb-4">
              These cookies are used to track visitors across websites. The intention is to display ads that are relevant and engaging for the individual user.
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Advertising cookies for targeted marketing</li>
              <li>Social media cookies for sharing functionality</li>
              <li>Retargeting cookies for personalized ads</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Third-Party Cookies</h2>
            <p className="text-gray-600 mb-4">
              We may use third-party services that also use cookies. These services include:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li><strong>Google Analytics:</strong> To analyze website traffic and user behavior</li>
              <li><strong>Google Ads:</strong> For advertising and conversion tracking</li>
              <li><strong>Facebook Pixel:</strong> For social media advertising</li>
              <li><strong>Stripe:</strong> For payment processing</li>
              <li><strong>Intercom:</strong> For customer support and chat</li>
            </ul>
            <p className="text-gray-600 mb-6">
              These third-party services have their own privacy policies and cookie policies. We encourage you to review their policies for more information.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Cookie Duration</h2>
            <p className="text-gray-600 mb-4">
              Cookies can be classified by their duration:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li><strong>Session Cookies:</strong> These cookies are temporary and are deleted when you close your browser</li>
              <li><strong>Persistent Cookies:</strong> These cookies remain on your device for a set period or until you delete them</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Managing Your Cookie Preferences</h2>
            <p className="text-gray-600 mb-4">
              You have several options for managing cookies:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li><strong>Browser Settings:</strong> Most browsers allow you to control cookies through their settings</li>
              <li><strong>Cookie Consent:</strong> We provide cookie consent banners that allow you to choose which cookies to accept</li>
              <li><strong>Opt-Out Tools:</strong> You can use browser extensions or opt-out tools to manage cookies</li>
            </ul>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">How to Disable Cookies</h3>
            <p className="text-gray-600 mb-4">
              You can disable cookies through your browser settings. However, please note that disabling certain cookies may affect the functionality of our website:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li><strong>Chrome:</strong> Settings → Privacy and security → Cookies and other site data</li>
              <li><strong>Firefox:</strong> Options → Privacy & Security → Cookies and Site Data</li>
              <li><strong>Safari:</strong> Preferences → Privacy → Manage Website Data</li>
              <li><strong>Edge:</strong> Settings → Cookies and site permissions → Cookies and site data</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Your Choices</h2>
            <p className="text-gray-600 mb-6">
              When you first visit our website, you will see a cookie consent banner that allows you to:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Accept all cookies</li>
              <li>Reject non-essential cookies</li>
              <li>Customize your cookie preferences</li>
              <li>Learn more about our cookie usage</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Updates to This Policy</h2>
            <p className="text-gray-600 mb-6">
              We may update this Cookie Policy from time to time to reflect changes in our practices or for other operational, legal, or regulatory reasons. We will notify you of any material changes by posting the updated policy on this page.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Contact Us</h2>
            <p className="text-gray-600 mb-6">
              If you have any questions about our use of cookies or this Cookie Policy, please contact us:
            </p>
            <div className="bg-gray-50 rounded-lg p-6 mb-8">
              <p className="text-gray-600 mb-2">
                <strong>Email:</strong> privacy@subscriptionanalytics.com
              </p>
              <p className="text-gray-600 mb-2">
                <strong>Address:</strong> 123 Analytics Street, Tech City, TC 12345
              </p>
              <p className="text-gray-600">
                <strong>Phone:</strong> +1 (555) 123-4567
              </p>
            </div>

            <div className="border-t border-gray-200 pt-8 mt-12">
              <p className="text-sm text-gray-500">
                This Cookie Policy is effective as of January 15, 2024. For more information about our data practices, please see our <a href="/privacy" className="text-blue-600 hover:text-blue-700">Privacy Policy</a>.
              </p>
            </div>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  )
}

export default CookiesPage
