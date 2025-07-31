'use client'

import { AlertTriangle, CheckCircle, Download, Edit, Eye, Info, Lock, Shield, Trash2 } from 'lucide-react'
import Footer from '../components/Footer'
import Header from '../components/Header'

export default function GDPRPage() {
  const dataRights = [
    {
      right: 'Right to Access',
      description: 'You can request a copy of all personal data we hold about you',
      icon: Eye,
      color: 'blue'
    },
    {
      right: 'Right to Rectification',
      description: 'You can request correction of inaccurate or incomplete data',
      icon: Edit,
      color: 'green'
    },
    {
      right: 'Right to Erasure',
      description: 'You can request deletion of your personal data ("right to be forgotten")',
      icon: Trash2,
      color: 'red'
    },
    {
      right: 'Right to Portability',
      description: 'You can request your data in a structured, machine-readable format',
      icon: Download,
      color: 'purple'
    },
    {
      right: 'Right to Restrict Processing',
      description: 'You can request limitation of how we process your data',
      icon: Lock,
      color: 'orange'
    },
    {
      right: 'Right to Object',
      description: 'You can object to processing of your data for specific purposes',
      icon: AlertTriangle,
      color: 'yellow'
    }
  ]

  const dataProcessing = [
    {
      purpose: 'Account Management',
      legalBasis: 'Contract Performance',
      dataRetention: 'Duration of account + 7 years',
      dataTypes: 'Name, email, company, usage data',
      status: 'active'
    },
    {
      purpose: 'Service Provision',
      legalBasis: 'Contract Performance',
      dataRetention: 'Duration of service + 3 years',
      dataTypes: 'Subscription data, payment information, usage analytics',
      status: 'active'
    },
    {
      purpose: 'Customer Support',
      legalBasis: 'Legitimate Interest',
      dataRetention: '3 years after last interaction',
      dataTypes: 'Contact information, support tickets, communication history',
      status: 'active'
    },
    {
      purpose: 'Marketing Communications',
      legalBasis: 'Consent',
      dataRetention: 'Until consent withdrawal',
      dataTypes: 'Email address, preferences, engagement metrics',
      status: 'active'
    },
    {
      purpose: 'Analytics & Improvement',
      legalBasis: 'Legitimate Interest',
      dataRetention: '2 years',
      dataTypes: 'Usage patterns, performance data, aggregated statistics',
      status: 'active'
    },
    {
      purpose: 'Legal Compliance',
      legalBasis: 'Legal Obligation',
      dataRetention: '7 years',
      dataTypes: 'Financial records, audit trails, compliance documentation',
      status: 'active'
    }
  ]

  const dataTransfers = [
    {
      recipient: 'Cloud Infrastructure (AWS)',
      location: 'United States',
      purpose: 'Data hosting and processing',
      safeguards: 'Standard Contractual Clauses, EU-US Data Privacy Framework',
      status: 'adequate'
    },
    {
      recipient: 'Payment Processors (Stripe)',
      location: 'United States',
      purpose: 'Payment processing',
      safeguards: 'Standard Contractual Clauses, PCI DSS compliance',
      status: 'adequate'
    },
    {
      recipient: 'Analytics Services (Google Analytics)',
      location: 'United States',
      purpose: 'Website analytics',
      safeguards: 'Standard Contractual Clauses, data anonymization',
      status: 'adequate'
    },
    {
      recipient: 'Customer Support (Zendesk)',
      location: 'United States',
      purpose: 'Customer service management',
      safeguards: 'Standard Contractual Clauses, data minimization',
      status: 'adequate'
    }
  ]

  const securityMeasures = [
    {
      measure: 'Data Encryption',
      description: 'All data encrypted in transit and at rest using AES-256',
      status: 'implemented',
      icon: Lock
    },
    {
      measure: 'Access Controls',
      description: 'Role-based access control with multi-factor authentication',
      status: 'implemented',
      icon: Shield
    },
    {
      measure: 'Regular Audits',
      description: 'Annual security audits and penetration testing',
      status: 'implemented',
      icon: CheckCircle
    },
    {
      measure: 'Data Minimization',
      description: 'Only collect data necessary for specified purposes',
      status: 'implemented',
      icon: Info
    },
    {
      measure: 'Breach Notification',
      description: '72-hour notification process for data breaches',
      status: 'implemented',
      icon: AlertTriangle
    },
    {
      measure: 'Staff Training',
      description: 'Regular GDPR and data protection training for all staff',
      status: 'implemented',
      icon: CheckCircle
    }
  ]

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'active':
      case 'implemented':
      case 'adequate':
        return 'bg-green-100 text-green-800'
      case 'pending':
        return 'bg-yellow-100 text-yellow-800'
      case 'inactive':
        return 'bg-red-100 text-red-800'
      default:
        return 'bg-gray-100 text-gray-800'
    }
  }

  const getStatusText = (status: string) => {
    switch (status) {
      case 'active':
      case 'implemented':
      case 'adequate':
        return 'Active'
      case 'pending':
        return 'Pending'
      case 'inactive':
        return 'Inactive'
      default:
        return status
    }
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />

      {/* Hero Section */}
      <section className="pt-32 pb-20 bg-gradient-to-br from-blue-600 via-indigo-700 to-purple-800 text-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center">
            <div className="flex justify-center mb-6">
              <div className="bg-white/20 rounded-full p-4">
                <Shield className="w-12 h-12 text-white" />
              </div>
            </div>
            <h1 className="text-4xl sm:text-6xl font-bold mb-6">
              GDPR Compliance
            </h1>
            <p className="text-xl opacity-90 max-w-3xl mx-auto leading-relaxed">
              We are committed to protecting your privacy and ensuring full compliance with the General Data Protection Regulation (GDPR).
            </p>
          </div>
        </div>
      </section>

      {/* Data Rights Section */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Your Data Rights
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              Under GDPR, you have specific rights regarding your personal data. Here's how we support them:
            </p>
          </div>

          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
            {dataRights.map((right, index) => (
              <div key={index} className="bg-white rounded-xl shadow-lg border border-gray-100 p-6 hover:shadow-xl transition-shadow">
                <div className="flex items-center mb-4">
                  <div className={`bg-${right.color}-100 rounded-lg p-3 mr-4`}>
                    <right.icon className={`w-6 h-6 text-${right.color}-600`} />
                  </div>
                  <h3 className="text-lg font-semibold text-gray-900">{right.right}</h3>
                </div>
                <p className="text-gray-600 leading-relaxed">{right.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Data Processing Activities */}
      <section className="py-20 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Data Processing Activities
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              Transparent overview of how we process your personal data
            </p>
          </div>

          <div className="bg-white rounded-xl shadow-lg overflow-hidden">
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Processing Purpose
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider hidden md:table-cell">
                      Legal Basis
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider hidden lg:table-cell">
                      Retention Period
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider hidden xl:table-cell">
                      Data Types
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Status
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {dataProcessing.map((activity, index) => (
                    <tr key={index} className="hover:bg-gray-50">
                      <td className="px-3 sm:px-6 py-3 sm:py-4">
                        <div className="text-sm font-medium text-gray-900">{activity.purpose}</div>
                        <div className="md:hidden text-xs text-gray-500 mt-1">
                          <strong>Legal Basis:</strong> {activity.legalBasis}
                        </div>
                        <div className="lg:hidden text-xs text-gray-500 mt-1">
                          <strong>Retention:</strong> {activity.dataRetention}
                        </div>
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4 text-sm text-gray-900 hidden md:table-cell">
                        {activity.legalBasis}
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4 text-sm text-gray-900 hidden lg:table-cell">
                        {activity.dataRetention}
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4 text-sm text-gray-900 hidden xl:table-cell">
                        <div className="max-w-xs">{activity.dataTypes}</div>
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4">
                        <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(activity.status)}`}>
                          {getStatusText(activity.status)}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </section>

      {/* Data Transfers */}
      <section className="py-20 bg-white">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              International Data Transfers
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              We ensure adequate protection for data transferred outside the EEA
            </p>
          </div>

          <div className="bg-white rounded-xl shadow-lg overflow-hidden border border-gray-200">
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Recipient
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider hidden md:table-cell">
                      Location
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider hidden lg:table-cell">
                      Purpose
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider hidden xl:table-cell">
                      Safeguards
                    </th>
                    <th className="px-3 sm:px-6 py-3 sm:py-4 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Status
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {dataTransfers.map((transfer, index) => (
                    <tr key={index} className="hover:bg-gray-50">
                      <td className="px-3 sm:px-6 py-3 sm:py-4">
                        <div className="text-sm font-medium text-gray-900">{transfer.recipient}</div>
                        <div className="md:hidden text-xs text-gray-500 mt-1">
                          <strong>Location:</strong> {transfer.location}
                        </div>
                        <div className="lg:hidden text-xs text-gray-500 mt-1">
                          <strong>Purpose:</strong> {transfer.purpose}
                        </div>
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4 text-sm text-gray-900 hidden md:table-cell">
                        {transfer.location}
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4 text-sm text-gray-900 hidden lg:table-cell">
                        {transfer.purpose}
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4 text-sm text-gray-900 hidden xl:table-cell">
                        <div className="max-w-xs">{transfer.safeguards}</div>
                      </td>
                      <td className="px-3 sm:px-6 py-3 sm:py-4">
                        <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(transfer.status)}`}>
                          {getStatusText(transfer.status)}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </section>

      {/* Security Measures */}
      <section className="py-20 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-4">
              Security & Technical Measures
            </h2>
            <p className="text-lg text-gray-600 max-w-2xl mx-auto">
              Comprehensive security measures to protect your personal data
            </p>
          </div>

          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-8">
            {securityMeasures.map((measure, index) => (
              <div key={index} className="bg-white rounded-xl shadow-lg border border-gray-100 p-6 hover:shadow-xl transition-shadow">
                <div className="flex items-start">
                  <div className="bg-green-100 rounded-lg p-3 mr-4">
                    <measure.icon className="w-6 h-6 text-green-600" />
                  </div>
                  <div className="flex-1">
                    <h3 className="text-lg font-semibold text-gray-900 mb-3">{measure.measure}</h3>
                    <p className="text-gray-600 leading-relaxed mb-4">{measure.description}</p>
                    <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(measure.status)}`}>
                      {getStatusText(measure.status)}
                    </span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Contact Information */}
      <section className="py-20 bg-white">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-3xl sm:text-4xl font-bold text-gray-900 mb-6">
            Contact Our Data Protection Officer
          </h2>
          <p className="text-lg text-gray-600 mb-8 max-w-2xl mx-auto">
            For any questions about your data rights or our GDPR compliance, please contact our Data Protection Officer.
          </p>

          <div className="bg-gradient-to-r from-blue-50 to-indigo-50 rounded-xl p-8 border border-blue-200">
            <div className="grid md:grid-cols-2 gap-8">
              <div className="text-left">
                <h3 className="text-xl font-semibold text-gray-900 mb-4">Data Protection Officer</h3>
                <div className="space-y-3 text-gray-600">
                  <p><strong>Email:</strong> dpo@subscriptionanalytics.com</p>
                  <p><strong>Phone:</strong> +1 (555) 123-4567</p>
                  <p><strong>Address:</strong> 123 Privacy Street, Data City, DC 12345</p>
                </div>
              </div>
              <div className="text-left">
                <h3 className="text-xl font-semibold text-gray-900 mb-4">Response Time</h3>
                <div className="space-y-3 text-gray-600">
                  <p><strong>Data Requests:</strong> Within 30 days</p>
                  <p><strong>General Inquiries:</strong> Within 48 hours</p>
                  <p><strong>Breach Notifications:</strong> Within 72 hours</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Last Updated */}
      <section className="py-12 bg-gray-50">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <p className="text-gray-500">
            <strong>Last Updated:</strong> December 2024 |
            <a href="/privacy" className="text-blue-600 hover:text-blue-700 ml-2">View Privacy Policy</a>
          </p>
        </div>
      </section>

      <Footer />
    </div>
  )
}
