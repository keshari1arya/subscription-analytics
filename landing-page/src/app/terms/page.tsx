import { Metadata } from 'next'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'Terms of Service - SubscriptionAnalytics',
  description: 'Read the terms and conditions governing your use of SubscriptionAnalytics services and platform.',
  keywords: 'terms of service, terms and conditions, user agreement, service terms',
}

const TermsPage = () => {
  return (
    <div className="min-h-screen bg-white">
      <Header />

      {/* Content */}
      <section className="pt-32 pb-20">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h1 className="text-4xl sm:text-5xl font-bold text-gray-900 mb-6">
              Terms of Service
            </h1>
            <p className="text-lg text-gray-600">
              Last updated: January 15, 2024
            </p>
          </div>

          <div className="prose prose-lg max-w-none">
            <p className="text-gray-600 mb-8">
              These Terms of Service ("Terms") govern your use of the SubscriptionAnalytics platform and services. By accessing or using our services, you agree to be bound by these Terms.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">1. Acceptance of Terms</h2>
            <p className="text-gray-600 mb-6">
              By accessing or using SubscriptionAnalytics, you agree to be bound by these Terms. If you disagree with any part of these terms, you may not access our services.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">2. Description of Service</h2>
            <p className="text-gray-600 mb-4">
              SubscriptionAnalytics provides a unified subscription analytics platform that allows businesses to:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Connect multiple payment providers and data sources</li>
              <li>Track and analyze subscription metrics and KPIs</li>
              <li>Generate reports and insights</li>
              <li>Monitor customer behavior and revenue trends</li>
              <li>Access predictive analytics and forecasting tools</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">3. User Accounts</h2>
            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">3.1 Account Creation</h3>
            <p className="text-gray-600 mb-4">
              To use our services, you must create an account. You agree to:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Provide accurate, current, and complete information</li>
              <li>Maintain and update your account information</li>
              <li>Keep your account credentials secure</li>
              <li>Accept responsibility for all activities under your account</li>
            </ul>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">3.2 Account Security</h3>
            <p className="text-gray-600 mb-6">
              You are responsible for maintaining the confidentiality of your account credentials and for all activities that occur under your account. Notify us immediately of any unauthorized use.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">4. Acceptable Use</h2>
            <p className="text-gray-600 mb-4">
              You agree to use our services only for lawful purposes and in accordance with these Terms. You agree not to:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Use the service for any illegal or unauthorized purpose</li>
              <li>Attempt to gain unauthorized access to our systems</li>
              <li>Interfere with or disrupt the service or servers</li>
              <li>Transmit viruses, malware, or other harmful code</li>
              <li>Violate any applicable laws or regulations</li>
              <li>Use the service to process data you don't have rights to</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">5. Data and Privacy</h2>
            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">5.1 Data Ownership</h3>
            <p className="text-gray-600 mb-6">
              You retain ownership of your data. We process your data in accordance with our <a href="/privacy" className="text-blue-600 hover:text-blue-700">Privacy Policy</a>.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">5.2 Data Processing</h3>
            <p className="text-gray-600 mb-6">
              By using our services, you authorize us to process your data to provide the subscription analytics services. We implement appropriate security measures to protect your data.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">6. Payment Terms</h2>
            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">6.1 Pricing</h3>
            <p className="text-gray-600 mb-4">
              Our pricing is available on our website and may change with notice. You agree to pay all fees associated with your chosen plan.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">6.2 Billing</h3>
            <p className="text-gray-600 mb-4">
              Billing occurs on a recurring basis (monthly or annually) depending on your plan. You authorize us to charge your payment method for all fees.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">6.3 Refunds</h3>
            <p className="text-gray-600 mb-6">
              We offer a 30-day money-back guarantee for new customers. Refunds are processed within 5-10 business days.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">7. Service Availability</h2>
            <p className="text-gray-600 mb-6">
              We strive to maintain high service availability but do not guarantee uninterrupted access. We may perform maintenance with reasonable notice. Our current uptime target is 99.9%.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">8. Intellectual Property</h2>
            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">8.1 Our Rights</h3>
            <p className="text-gray-600 mb-6">
              SubscriptionAnalytics and its content are protected by intellectual property laws. You may not copy, modify, or distribute our software or content without permission.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">8.2 Your Rights</h3>
            <p className="text-gray-600 mb-6">
              You retain ownership of your data and any customizations you create. You grant us a license to process your data to provide our services.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">9. Limitation of Liability</h2>
            <p className="text-gray-600 mb-6">
              To the maximum extent permitted by law, SubscriptionAnalytics shall not be liable for any indirect, incidental, special, consequential, or punitive damages, including lost profits, data, or business opportunities.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">10. Disclaimers</h2>
            <p className="text-gray-600 mb-6">
              Our services are provided "as is" without warranties of any kind. We disclaim all warranties, express or implied, including but not limited to warranties of merchantability, fitness for a particular purpose, and non-infringement.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">11. Termination</h2>
            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">11.1 Termination by You</h3>
            <p className="text-gray-600 mb-4">
              You may cancel your account at any time through your account settings or by contacting our support team.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">11.2 Termination by Us</h3>
            <p className="text-gray-600 mb-4">
              We may terminate or suspend your account immediately, without prior notice, for conduct that we believe violates these Terms or is harmful to other users or our service.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">11.3 Effect of Termination</h3>
            <p className="text-gray-600 mb-6">
              Upon termination, your right to use the service ceases immediately. We may delete your account and data after 30 days, unless required by law to retain it longer.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">12. Governing Law</h2>
            <p className="text-gray-600 mb-6">
              These Terms shall be governed by and construed in accordance with the laws of the State of California, without regard to its conflict of law provisions.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">13. Dispute Resolution</h2>
            <p className="text-gray-600 mb-6">
              Any disputes arising from these Terms or your use of our services shall be resolved through binding arbitration in accordance with the rules of the American Arbitration Association.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">14. Changes to Terms</h2>
            <p className="text-gray-600 mb-6">
              We may modify these Terms at any time. We will notify you of material changes by email or through our service. Your continued use of our services after such changes constitutes acceptance of the new Terms.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">15. Contact Information</h2>
            <p className="text-gray-600 mb-6">
              If you have any questions about these Terms, please contact us:
            </p>
            <div className="bg-gray-50 rounded-lg p-6 mb-8">
              <p className="text-gray-600 mb-2">
                <strong>Email:</strong> legal@subscriptionanalytics.com
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
                These Terms of Service are effective as of January 15, 2024, and will remain in effect except with respect to any changes in their provisions in the future, which will be in effect immediately after being posted on this page.
              </p>
            </div>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  )
}

export default TermsPage
