import { Metadata } from 'next'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'Privacy Policy - SubscriptionAnalytics',
  description: 'Learn how SubscriptionAnalytics collects, uses, and protects your personal information and data.',
  keywords: 'privacy policy, data protection, GDPR, personal information, data collection',
}

const PrivacyPage = () => {
  return (
    <div className="min-h-screen bg-white">
      <Header />

      {/* Content */}
      <section className="pt-32 pb-20">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h1 className="text-4xl sm:text-5xl font-bold text-gray-900 mb-6">
              Privacy Policy
            </h1>
            <p className="text-lg text-gray-600">
              Last updated: January 15, 2024
            </p>
          </div>

          <div className="prose prose-lg max-w-none">
            <p className="text-gray-600 mb-8">
              At SubscriptionAnalytics, we take your privacy seriously. This Privacy Policy explains how we collect, use, disclose, and safeguard your information when you use our subscription analytics platform.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Information We Collect</h2>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">Personal Information</h3>
            <p className="text-gray-600 mb-4">
              We may collect personal information that you voluntarily provide to us when you:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Create an account or register for our services</li>
              <li>Contact us for support or inquiries</li>
              <li>Subscribe to our newsletter or marketing communications</li>
              <li>Participate in surveys, promotions, or events</li>
            </ul>
            <p className="text-gray-600 mb-6">
              This information may include your name, email address, company name, job title, and other contact details.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">Usage Information</h3>
            <p className="text-gray-600 mb-4">
              We automatically collect certain information about your use of our platform, including:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Log data (IP address, browser type, pages visited)</li>
              <li>Device information (device type, operating system)</li>
              <li>Usage patterns and analytics data</li>
              <li>Performance and error data</li>
            </ul>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">Payment Information</h3>
            <p className="text-gray-600 mb-6">
              When you subscribe to our paid services, we collect payment information through our secure payment processors. We do not store your complete payment card information on our servers.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">How We Use Your Information</h2>
            <p className="text-gray-600 mb-4">
              We use the information we collect to:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Provide and maintain our subscription analytics services</li>
              <li>Process payments and manage your account</li>
              <li>Send you important service updates and notifications</li>
              <li>Respond to your inquiries and provide customer support</li>
              <li>Improve our platform and develop new features</li>
              <li>Send marketing communications (with your consent)</li>
              <li>Comply with legal obligations</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Information Sharing and Disclosure</h2>
            <p className="text-gray-600 mb-4">
              We do not sell, trade, or otherwise transfer your personal information to third parties, except in the following circumstances:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li><strong>Service Providers:</strong> We may share information with trusted third-party service providers who assist us in operating our platform</li>
              <li><strong>Legal Requirements:</strong> We may disclose information if required by law or to protect our rights and safety</li>
              <li><strong>Business Transfers:</strong> In the event of a merger, acquisition, or sale of assets, your information may be transferred</li>
              <li><strong>Consent:</strong> We may share information with your explicit consent</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Data Security</h2>
            <p className="text-gray-600 mb-6">
              We implement appropriate technical and organizational security measures to protect your personal information against unauthorized access, alteration, disclosure, or destruction. These measures include:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Encryption of data in transit and at rest</li>
              <li>Regular security assessments and updates</li>
              <li>Access controls and authentication measures</li>
              <li>Employee training on data protection</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Your Rights and Choices</h2>
            <p className="text-gray-600 mb-4">
              Depending on your location, you may have certain rights regarding your personal information:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li><strong>Access:</strong> Request access to your personal information</li>
              <li><strong>Correction:</strong> Request correction of inaccurate information</li>
              <li><strong>Deletion:</strong> Request deletion of your personal information</li>
              <li><strong>Portability:</strong> Request a copy of your data in a portable format</li>
              <li><strong>Objection:</strong> Object to certain processing activities</li>
              <li><strong>Withdrawal:</strong> Withdraw consent for marketing communications</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Cookies and Tracking Technologies</h2>
            <p className="text-gray-600 mb-6">
              We use cookies and similar tracking technologies to enhance your experience on our platform. You can control cookie settings through your browser preferences. For more information, please see our <a href="/cookies" className="text-blue-600 hover:text-blue-700">Cookie Policy</a>.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">International Data Transfers</h2>
            <p className="text-gray-600 mb-6">
              Your information may be transferred to and processed in countries other than your own. We ensure appropriate safeguards are in place to protect your information in accordance with this Privacy Policy and applicable data protection laws.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Children's Privacy</h2>
            <p className="text-gray-600 mb-6">
              Our services are not intended for children under the age of 16. We do not knowingly collect personal information from children under 16. If you believe we have collected information from a child under 16, please contact us immediately.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Changes to This Privacy Policy</h2>
            <p className="text-gray-600 mb-6">
              We may update this Privacy Policy from time to time. We will notify you of any material changes by posting the new Privacy Policy on this page and updating the "Last updated" date. We encourage you to review this Privacy Policy periodically.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Contact Us</h2>
            <p className="text-gray-600 mb-6">
              If you have any questions about this Privacy Policy or our data practices, please contact us:
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
                This Privacy Policy is effective as of January 15, 2024, and will remain in effect except with respect to any changes in its provisions in the future, which will be in effect immediately after being posted on this page.
              </p>
            </div>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  )
}

export default PrivacyPage
