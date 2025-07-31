import { Metadata } from 'next'
import Footer from '../components/Footer'
import Header from '../components/Header'

export const metadata: Metadata = {
  title: 'GDPR Compliance - SubscriptionAnalytics',
  description: 'Learn about SubscriptionAnalytics GDPR compliance and your data protection rights under the General Data Protection Regulation.',
  keywords: 'GDPR, data protection, privacy rights, EU data protection, data subject rights',
}

const GDPRPage = () => {
  return (
    <div className="min-h-screen bg-white">
      <Header />

      {/* Content */}
      <section className="pt-32 pb-20">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h1 className="text-4xl sm:text-5xl font-bold text-gray-900 mb-6">
              GDPR Compliance
            </h1>
            <p className="text-lg text-gray-600">
              Your Data Protection Rights Under the General Data Protection Regulation
            </p>
          </div>

          <div className="prose prose-lg max-w-none">
            <p className="text-gray-600 mb-8">
              At SubscriptionAnalytics, we are committed to protecting your privacy and ensuring compliance with the General Data Protection Regulation (GDPR). This page explains your rights and how we handle your personal data in accordance with GDPR requirements.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">What is GDPR?</h2>
            <p className="text-gray-600 mb-6">
              The General Data Protection Regulation (GDPR) is a comprehensive data protection law that applies to all organizations operating within the EU and those that offer goods or services to individuals in the EU. It gives you greater control over your personal data and ensures that organizations handle your data responsibly.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Your GDPR Rights</h2>
            <p className="text-gray-600 mb-4">
              Under GDPR, you have the following rights regarding your personal data:
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">1. Right to Access</h3>
            <p className="text-gray-600 mb-4">
              You have the right to request a copy of the personal data we hold about you and information about how we process it.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">2. Right to Rectification</h3>
            <p className="text-gray-600 mb-4">
              You have the right to request that we correct any inaccurate or incomplete personal data we hold about you.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">3. Right to Erasure (Right to be Forgotten)</h3>
            <p className="text-gray-600 mb-4">
              You have the right to request that we delete your personal data in certain circumstances, such as when the data is no longer necessary for the purpose for which it was collected.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">4. Right to Restrict Processing</h3>
            <p className="text-gray-600 mb-4">
              You have the right to request that we limit how we process your personal data in certain circumstances.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">5. Right to Data Portability</h3>
            <p className="text-gray-600 mb-4">
              You have the right to receive your personal data in a structured, commonly used, machine-readable format and to transmit that data to another controller.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">6. Right to Object</h3>
            <p className="text-gray-600 mb-4">
              You have the right to object to the processing of your personal data in certain circumstances, such as for direct marketing purposes.
            </p>

            <h3 className="text-xl font-semibold text-gray-900 mt-8 mb-4">7. Rights Related to Automated Decision Making</h3>
            <p className="text-gray-600 mb-6">
              You have the right not to be subject to a decision based solely on automated processing, including profiling, which produces legal effects concerning you or similarly significantly affects you.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">How to Exercise Your Rights</h2>
            <p className="text-gray-600 mb-4">
              To exercise any of your GDPR rights, you can:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li>Contact us at <strong>gdpr@subscriptionanalytics.com</strong></li>
              <li>Use our data subject rights request form</li>
              <li>Contact our Data Protection Officer</li>
            </ul>
            <p className="text-gray-600 mb-6">
              We will respond to your request within one month of receipt. If we need more time, we will inform you of the reason and the extended timeframe.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Legal Basis for Processing</h2>
            <p className="text-gray-600 mb-4">
              We process your personal data based on the following legal grounds:
            </p>
            <ul className="list-disc pl-6 mb-6 text-gray-600">
              <li><strong>Consent:</strong> When you explicitly agree to the processing of your personal data</li>
              <li><strong>Contract:</strong> When processing is necessary for the performance of our service agreement</li>
              <li><strong>Legitimate Interest:</strong> When processing is necessary for our legitimate business interests</li>
              <li><strong>Legal Obligation:</strong> When processing is required by law</li>
            </ul>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Data Transfers</h2>
            <p className="text-gray-600 mb-6">
              Your personal data may be transferred to and processed in countries outside the European Economic Area (EEA). We ensure that such transfers comply with GDPR requirements by implementing appropriate safeguards, such as Standard Contractual Clauses approved by the European Commission.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Data Retention</h2>
            <p className="text-gray-600 mb-6">
              We retain your personal data only for as long as necessary to fulfill the purposes for which it was collected, including for the purposes of satisfying any legal, accounting, or reporting requirements. Our retention periods are based on the nature of the data and the purpose for which it was collected.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Data Security</h2>
            <p className="text-gray-600 mb-6">
              We implement appropriate technical and organizational security measures to protect your personal data against unauthorized access, alteration, disclosure, or destruction. These measures include encryption, access controls, regular security assessments, and employee training.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Data Breach Notification</h2>
            <p className="text-gray-600 mb-6">
              In the event of a personal data breach that poses a risk to your rights and freedoms, we will notify the relevant supervisory authority within 72 hours of becoming aware of the breach. We will also notify you without undue delay if the breach is likely to result in a high risk to your rights and freedoms.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Your Right to Lodge a Complaint</h2>
            <p className="text-gray-600 mb-6">
              You have the right to lodge a complaint with a supervisory authority if you believe that our processing of your personal data infringes the GDPR. You can contact your local data protection authority or the supervisory authority in the EU member state where you reside, work, or where the alleged infringement occurred.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Data Protection Officer</h2>
            <p className="text-gray-600 mb-6">
              We have appointed a Data Protection Officer (DPO) to oversee our GDPR compliance. You can contact our DPO at:
            </p>
            <div className="bg-gray-50 rounded-lg p-6 mb-8">
              <p className="text-gray-600 mb-2">
                <strong>Email:</strong> dpo@subscriptionanalytics.com
              </p>
              <p className="text-gray-600 mb-2">
                <strong>Address:</strong> 123 Analytics Street, Tech City, TC 12345
              </p>
              <p className="text-gray-600">
                <strong>Phone:</strong> +1 (555) 123-4567
              </p>
            </div>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Updates to This Policy</h2>
            <p className="text-gray-600 mb-6">
              We may update this GDPR compliance information from time to time to reflect changes in our practices or applicable law. We will notify you of any material changes by posting the updated information on this page.
            </p>

            <h2 className="text-2xl font-bold text-gray-900 mt-12 mb-6">Contact Us</h2>
            <p className="text-gray-600 mb-6">
              If you have any questions about our GDPR compliance or your data protection rights, please contact us:
            </p>
            <div className="bg-gray-50 rounded-lg p-6 mb-8">
              <p className="text-gray-600 mb-2">
                <strong>General Inquiries:</strong> privacy@subscriptionanalytics.com
              </p>
              <p className="text-gray-600 mb-2">
                <strong>GDPR Requests:</strong> gdpr@subscriptionanalytics.com
              </p>
              <p className="text-gray-600 mb-2">
                <strong>Data Protection Officer:</strong> dpo@subscriptionanalytics.com
              </p>
              <p className="text-gray-600">
                <strong>Address:</strong> 123 Analytics Street, Tech City, TC 12345
              </p>
            </div>

            <div className="border-t border-gray-200 pt-8 mt-12">
              <p className="text-sm text-gray-500">
                This GDPR compliance information is effective as of January 15, 2024. For more information about our data practices, please see our <a href="/privacy" className="text-blue-600 hover:text-blue-700">Privacy Policy</a>.
              </p>
            </div>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  )
}

export default GDPRPage
