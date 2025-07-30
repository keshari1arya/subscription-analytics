'use client'

import { motion } from 'framer-motion'
import { Activity, ArrowDownRight, ArrowUpRight, BarChart3, ChevronLeft, ChevronRight, Clock, CreditCard, TrendingUp, Users } from 'lucide-react'
import React, { useState } from 'react'

const screenshots = [
  {
    id: 1,
    title: 'Revenue Dashboard',
    description: 'Unified MRR/ARR tracking across all payment providers',
    icon: BarChart3,
    color: 'from-blue-600 to-indigo-700',
    type: 'dashboard'
  },
  {
    id: 2,
    title: 'Customer Analytics',
    description: 'Deep insights into customer behavior and churn patterns',
    icon: Users,
    color: 'from-green-600 to-emerald-700',
    type: 'customers'
  },
  {
    id: 3,
    title: 'Subscription Metrics',
    description: 'Comprehensive subscription and payment analytics',
    icon: CreditCard,
    color: 'from-purple-600 to-pink-700',
    type: 'subscriptions'
  }
]

export default function ScreenshotsSection() {
  const [currentIndex, setCurrentIndex] = useState(0)

  const nextScreenshot = () => {
    setCurrentIndex((prev) => (prev + 1) % screenshots.length)
  }

  const prevScreenshot = () => {
    setCurrentIndex((prev) => (prev - 1 + screenshots.length) % screenshots.length)
  }

  const renderDashboard = () => (
    <div className="bg-white rounded-lg shadow-lg overflow-hidden w-full mx-auto h-96">
      {/* Header */}
      <div className="bg-gray-900 px-4 py-3 flex items-center justify-between">
        <div className="flex items-center space-x-3">
          <div className="w-6 h-6 bg-blue-600 rounded"></div>
          <span className="text-white font-semibold text-sm">SubscriptionAnalytics</span>
        </div>
        <div className="hidden sm:flex items-center space-x-3 text-gray-300 text-xs">
          <span className="text-white">Dashboard</span>
          <span>Customers</span>
          <span>Subscriptions</span>
        </div>
        <div className="flex items-center space-x-2">
          <div className="w-6 h-6 bg-gray-700 rounded-full"></div>
          <span className="text-white text-xs hidden sm:block">John Doe</span>
        </div>
      </div>

      {/* Content */}
      <div className="p-4 h-80 overflow-y-auto">
        <div className="flex items-center justify-between mb-3">
          <h1 className="text-lg font-bold text-gray-900">Revenue Dashboard</h1>
          <div className="flex items-center space-x-2">
            <span className="text-xs text-gray-500">2 min ago</span>
            <div className="w-2 h-2 bg-green-500 rounded-full"></div>
          </div>
        </div>

        {/* Stats Grid */}
        <div className="grid grid-cols-2 gap-3 mb-3">
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">MRR</p>
                <p className="text-lg font-bold text-gray-900">$45,230</p>
              </div>
              <div className="flex items-center text-green-600">
                <ArrowUpRight className="w-3 h-3 mr-1" />
                <span className="text-xs">+12.5%</span>
              </div>
            </div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">ARR</p>
                <p className="text-lg font-bold text-gray-900">$542,760</p>
              </div>
              <div className="flex items-center text-green-600">
                <ArrowUpRight className="w-3 h-3 mr-1" />
                <span className="text-xs">+8.2%</span>
              </div>
            </div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">Customers</p>
                <p className="text-lg font-bold text-gray-900">1,847</p>
              </div>
              <div className="flex items-center text-green-600">
                <ArrowUpRight className="w-3 h-3 mr-1" />
                <span className="text-xs">+5.1%</span>
              </div>
            </div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">Churn Rate</p>
                <p className="text-lg font-bold text-gray-900">2.1%</p>
              </div>
              <div className="flex items-center text-red-600">
                <ArrowDownRight className="w-3 h-3 mr-1" />
                <span className="text-xs">-0.3%</span>
              </div>
            </div>
          </div>
        </div>

        {/* Chart */}
        <div className="bg-gray-50 border border-gray-200 rounded-lg p-3 mb-3">
          <h3 className="text-sm font-semibold text-gray-900 mb-2">Revenue Growth</h3>
          <div className="h-16 flex items-end space-x-1">
            {[40, 55, 45, 70, 60, 85, 75, 90].map((height, i) => (
              <div key={i} className="flex-1 bg-blue-600 rounded-t" style={{ height: `${height}%` }}></div>
            ))}
          </div>
          <div className="flex justify-between text-xs text-gray-500 mt-1">
            <span>Jan</span><span>Feb</span><span>Mar</span><span>Apr</span>
            <span>May</span><span>Jun</span><span>Jul</span><span>Aug</span>
          </div>
        </div>

        {/* Recent Activity */}
        <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
          <h3 className="text-sm font-semibold text-gray-900 mb-2">Recent Activity</h3>
          <div className="space-y-1">
            <div className="flex items-center space-x-2 p-1 bg-green-50 rounded">
              <div className="w-2 h-2 bg-green-500 rounded-full"></div>
              <span className="text-xs">New customer: Acme Corp ($2,500/month)</span>
              <span className="text-xs text-gray-500 ml-auto">2 min</span>
            </div>
            <div className="flex items-center space-x-2 p-1 bg-blue-50 rounded">
              <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
              <span className="text-xs">Upgrade: TechStart Pro (+$500/month)</span>
              <span className="text-xs text-gray-500 ml-auto">15 min</span>
            </div>
            <div className="flex items-center space-x-2 p-1 bg-yellow-50 rounded">
              <div className="w-2 h-2 bg-yellow-500 rounded-full"></div>
              <span className="text-xs">Payment failed: StartupXYZ</span>
              <span className="text-xs text-gray-500 ml-auto">1 hour</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  )

  const renderCustomers = () => (
    <div className="bg-white rounded-lg shadow-lg overflow-hidden w-full mx-auto h-96">
      {/* Header */}
      <div className="bg-gray-900 px-4 py-3 flex items-center justify-between">
        <div className="flex items-center space-x-3">
          <div className="w-6 h-6 bg-green-600 rounded"></div>
          <span className="text-white font-semibold text-sm">SubscriptionAnalytics</span>
        </div>
        <div className="hidden sm:flex items-center space-x-3 text-gray-300 text-xs">
          <span>Dashboard</span>
          <span className="text-white">Customers</span>
          <span>Subscriptions</span>
        </div>
        <div className="flex items-center space-x-2">
          <div className="w-6 h-6 bg-gray-700 rounded-full"></div>
          <span className="text-white text-xs hidden sm:block">John Doe</span>
        </div>
      </div>

      {/* Content */}
      <div className="p-4 h-80 overflow-y-auto">
        <div className="flex items-center justify-between mb-3">
          <h1 className="text-lg font-bold text-gray-900">Customer Analytics</h1>
          <div className="flex items-center space-x-2">
            <button className="px-3 py-1 bg-blue-600 text-white rounded text-xs">Export</button>
            <button className="px-3 py-1 border border-gray-300 text-gray-700 rounded text-xs">Filter</button>
          </div>
        </div>

        {/* Customer Table */}
        <div className="bg-gray-50 border border-gray-200 rounded-lg overflow-hidden">
          <div className="bg-gray-100 px-3 py-2 border-b border-gray-200">
            <div className="grid grid-cols-12 gap-2 text-xs font-semibold text-gray-700">
              <div className="col-span-5">Customer</div>
              <div className="col-span-3">Plan</div>
              <div className="col-span-2">MRR</div>
              <div className="col-span-2">Status</div>
            </div>
          </div>
          <div className="divide-y divide-gray-200">
            <div className="px-3 py-2 hover:bg-gray-50">
              <div className="grid grid-cols-12 gap-2 items-center">
                <div className="col-span-5 flex items-center space-x-2 min-w-0">
                  <div className="w-6 h-6 bg-blue-600 rounded-full flex items-center justify-center text-white text-xs font-semibold flex-shrink-0">AC</div>
                  <div className="min-w-0 flex-1">
                    <div className="font-semibold text-gray-900 text-xs truncate">Acme Corp</div>
                    <div className="text-xs text-gray-500 truncate">acme@example.com</div>
                  </div>
                </div>
                <div className="col-span-3">
                  <span className="px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded-full whitespace-nowrap">Enterprise</span>
                </div>
                <div className="col-span-2 font-semibold text-gray-900 text-sm">$2,500</div>
                <div className="col-span-2">
                  <span className="px-2 py-1 bg-green-100 text-green-800 text-xs rounded-full whitespace-nowrap">Active</span>
                </div>
              </div>
            </div>
            <div className="px-3 py-2 hover:bg-gray-50">
              <div className="grid grid-cols-12 gap-2 items-center">
                <div className="col-span-5 flex items-center space-x-2 min-w-0">
                  <div className="w-6 h-6 bg-green-600 rounded-full flex items-center justify-center text-white text-xs font-semibold flex-shrink-0">TS</div>
                  <div className="min-w-0 flex-1">
                    <div className="font-semibold text-gray-900 text-xs truncate">TechStart</div>
                    <div className="text-xs text-gray-500 truncate">tech@startup.com</div>
                  </div>
                </div>
                <div className="col-span-3">
                  <span className="px-2 py-1 bg-purple-100 text-purple-800 text-xs rounded-full whitespace-nowrap">Professional</span>
                </div>
                <div className="col-span-2 font-semibold text-gray-900 text-sm">$1,200</div>
                <div className="col-span-2">
                  <span className="px-2 py-1 bg-green-100 text-green-800 text-xs rounded-full whitespace-nowrap">Active</span>
                </div>
              </div>
            </div>
            <div className="px-3 py-2 hover:bg-gray-50">
              <div className="grid grid-cols-12 gap-2 items-center">
                <div className="col-span-5 flex items-center space-x-2 min-w-0">
                  <div className="w-6 h-6 bg-purple-600 rounded-full flex items-center justify-center text-white text-xs font-semibold flex-shrink-0">IL</div>
                  <div className="min-w-0 flex-1">
                    <div className="font-semibold text-gray-900 text-xs truncate">InnovateLabs</div>
                    <div className="text-xs text-gray-500 truncate">hello@innovate.com</div>
                  </div>
                </div>
                <div className="col-span-3">
                  <span className="px-2 py-1 bg-gray-100 text-gray-800 text-xs rounded-full whitespace-nowrap">Starter</span>
                </div>
                <div className="col-span-2 font-semibold text-gray-900 text-sm">$500</div>
                <div className="col-span-2">
                  <span className="px-2 py-1 bg-green-100 text-green-800 text-xs rounded-full whitespace-nowrap">Active</span>
                </div>
              </div>
            </div>
            <div className="px-3 py-2 hover:bg-gray-50">
              <div className="grid grid-cols-12 gap-2 items-center">
                <div className="col-span-5 flex items-center space-x-2 min-w-0">
                  <div className="w-6 h-6 bg-red-600 rounded-full flex items-center justify-center text-white text-xs font-semibold flex-shrink-0">OC</div>
                  <div className="min-w-0 flex-1">
                    <div className="font-semibold text-gray-900 text-xs truncate">OldCompany</div>
                    <div className="text-xs text-gray-500 truncate">contact@old.com</div>
                  </div>
                </div>
                <div className="col-span-3">
                  <span className="px-2 py-1 bg-purple-100 text-purple-800 text-xs rounded-full whitespace-nowrap">Professional</span>
                </div>
                <div className="col-span-2 font-semibold text-gray-900 text-sm">$0</div>
                <div className="col-span-2">
                  <span className="px-2 py-1 bg-red-100 text-red-800 text-xs rounded-full whitespace-nowrap">Churned</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Pagination */}
        <div className="flex items-center justify-between mt-3">
          <div className="text-xs text-gray-600">Showing 1-4 of 1,847</div>
          <div className="flex items-center space-x-1">
            <button className="px-2 py-1 border border-gray-300 rounded text-xs">Prev</button>
            <button className="px-2 py-1 bg-blue-600 text-white rounded text-xs">1</button>
            <button className="px-2 py-1 border border-gray-300 rounded text-xs">2</button>
            <button className="px-2 py-1 border border-gray-300 rounded text-xs">Next</button>
          </div>
        </div>
      </div>
    </div>
  )

  const renderSubscriptions = () => (
    <div className="bg-white rounded-lg shadow-lg overflow-hidden w-full mx-auto h-96">
      {/* Header */}
      <div className="bg-gray-900 px-4 py-3 flex items-center justify-between">
        <div className="flex items-center space-x-3">
          <div className="w-6 h-6 bg-purple-600 rounded"></div>
          <span className="text-white font-semibold text-sm">SubscriptionAnalytics</span>
        </div>
        <div className="hidden sm:flex items-center space-x-3 text-gray-300 text-xs">
          <span>Dashboard</span>
          <span>Customers</span>
          <span className="text-white">Subscriptions</span>
        </div>
        <div className="flex items-center space-x-2">
          <div className="w-6 h-6 bg-gray-700 rounded-full"></div>
          <span className="text-white text-xs hidden sm:block">John Doe</span>
        </div>
      </div>

      {/* Content */}
      <div className="p-4 h-80 overflow-y-auto">
        <div className="flex items-center justify-between mb-3">
          <h1 className="text-lg font-bold text-gray-900">Subscription Metrics</h1>
          <div className="flex items-center space-x-2">
            <button className="px-3 py-1 bg-purple-600 text-white rounded text-xs">Report</button>
            <button className="px-3 py-1 border border-gray-300 text-gray-700 rounded text-xs">Settings</button>
          </div>
        </div>

        {/* Metrics Cards */}
        <div className="grid grid-cols-2 gap-3 mb-3">
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">Total Subscriptions</p>
                <p className="text-lg font-bold text-gray-900">1,847</p>
              </div>
              <div className="w-8 h-8 bg-blue-100 rounded-lg flex items-center justify-center">
                <CreditCard className="w-4 h-4 text-blue-600" />
              </div>
            </div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">Active Subscriptions</p>
                <p className="text-lg font-bold text-gray-900">1,723</p>
              </div>
              <div className="w-8 h-8 bg-green-100 rounded-lg flex items-center justify-center">
                <Activity className="w-4 h-4 text-green-600" />
              </div>
            </div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">Trial Subscriptions</p>
                <p className="text-lg font-bold text-gray-900">89</p>
              </div>
              <div className="w-8 h-8 bg-yellow-100 rounded-lg flex items-center justify-center">
                <Clock className="w-4 h-4 text-yellow-600" />
              </div>
            </div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-xs text-gray-600">Churned This Month</p>
                <p className="text-lg font-bold text-gray-900">35</p>
              </div>
              <div className="w-8 h-8 bg-red-100 rounded-lg flex items-center justify-center">
                <TrendingUp className="w-4 h-4 text-red-600" />
              </div>
            </div>
          </div>
        </div>

        {/* Charts Row */}
        <div className="grid grid-cols-2 gap-3 mb-3">
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <h3 className="text-sm font-semibold text-gray-900 mb-2">Subscription Growth</h3>
            <div className="h-16 flex items-end space-x-1">
              {[60, 75, 85, 95, 110, 125, 140, 155].map((height, i) => (
                <div key={i} className="flex-1 bg-purple-600 rounded-t" style={{ height: `${height}%` }}></div>
              ))}
            </div>
            <div className="flex justify-between text-xs text-gray-500 mt-1">
              <span>Jan</span><span>Feb</span><span>Mar</span><span>Apr</span>
              <span>May</span><span>Jun</span><span>Jul</span><span>Aug</span>
            </div>
          </div>
          <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
            <h3 className="text-sm font-semibold text-gray-900 mb-2">Plan Distribution</h3>
            <div className="space-y-2">
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-2">
                  <div className="w-3 h-3 bg-blue-600 rounded"></div>
                  <span className="text-xs">Starter</span>
                </div>
                <div className="flex items-center space-x-2">
                  <div className="w-16 bg-gray-200 rounded-full h-1">
                    <div className="bg-blue-600 h-1 rounded-full" style={{ width: '48%' }}></div>
                  </div>
                  <span className="text-xs font-semibold">892</span>
                </div>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-2">
                  <div className="w-3 h-3 bg-green-600 rounded"></div>
                  <span className="text-xs">Professional</span>
                </div>
                <div className="flex items-center space-x-2">
                  <div className="w-16 bg-gray-200 rounded-full h-1">
                    <div className="bg-green-600 h-1 rounded-full" style={{ width: '35%' }}></div>
                  </div>
                  <span className="text-xs font-semibold">654</span>
                </div>
              </div>
              <div className="flex items-center justify-between">
                <div className="flex items-center space-x-2">
                  <div className="w-3 h-3 bg-purple-600 rounded"></div>
                  <span className="text-xs">Enterprise</span>
                </div>
                <div className="flex items-center space-x-2">
                  <div className="w-16 bg-gray-200 rounded-full h-1">
                    <div className="bg-purple-600 h-1 rounded-full" style={{ width: '17%' }}></div>
                  </div>
                  <span className="text-xs font-semibold">301</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Retention Metrics */}
        <div className="bg-gray-50 border border-gray-200 rounded-lg p-3">
          <h3 className="text-sm font-semibold text-gray-900 mb-2">Retention Metrics</h3>
          <div className="grid grid-cols-3 gap-3">
            <div className="text-center">
              <div className="text-xl font-bold text-green-600 mb-1">97.8%</div>
              <div className="text-xs text-gray-600">Monthly</div>
            </div>
            <div className="text-center">
              <div className="text-xl font-bold text-blue-600 mb-1">94.2%</div>
              <div className="text-xs text-gray-600">Quarterly</div>
            </div>
            <div className="text-center">
              <div className="text-xl font-bold text-purple-600 mb-1">89.5%</div>
              <div className="text-xs text-gray-600">Annual</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )

  const renderScreenshot = () => {
    switch (screenshots[currentIndex].type) {
      case 'dashboard':
        return renderDashboard()
      case 'customers':
        return renderCustomers()
      case 'subscriptions':
        return renderSubscriptions()
      default:
        return renderDashboard()
    }
  }

  return (
    <section className="py-20 bg-white" id="screenshots">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-6"
          >
            See It In Action
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-3xl mx-auto"
          >
            Real screenshots from our unified subscription analytics platform.
            See how we bring together data from multiple payment providers.
          </motion.p>
        </div>

        <div className="grid lg:grid-cols-2 gap-8 lg:gap-12 items-start">
          {/* Screenshot Display */}
          <motion.div
            initial={{ opacity: 0, x: -30 }}
            whileInView={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="relative order-2 lg:order-1"
          >
            <div className="bg-gradient-to-br from-gray-50 to-gray-100 rounded-2xl p-4 sm:p-6 shadow-lg">
              <div className="relative">
                {renderScreenshot()}

                {/* Navigation */}
                <button
                  onClick={prevScreenshot}
                  className="absolute left-2 top-1/2 transform -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 shadow-lg transition-all duration-200"
                >
                  <ChevronLeft className="w-4 h-4 text-gray-600" />
                </button>
                <button
                  onClick={nextScreenshot}
                  className="absolute right-2 top-1/2 transform -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 shadow-lg transition-all duration-200"
                >
                  <ChevronRight className="w-4 h-4 text-gray-600" />
                </button>
              </div>

              {/* Dots indicator */}
              <div className="flex justify-center mt-4 space-x-2">
                {screenshots.map((_, index) => (
                  <button
                    key={index}
                    onClick={() => setCurrentIndex(index)}
                    className={`w-2 h-2 rounded-full transition-all duration-200 ${
                      index === currentIndex ? 'bg-blue-600' : 'bg-gray-300'
                    }`}
                  />
                ))}
              </div>
            </div>
          </motion.div>

          {/* Screenshot Info */}
          <motion.div
            initial={{ opacity: 0, x: 30 }}
            whileInView={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="space-y-6 order-1 lg:order-2"
          >
            <div className="flex flex-col sm:flex-row items-start sm:items-center space-y-4 sm:space-y-0 sm:space-x-4">
              <div className={`p-3 rounded-lg bg-gradient-to-br ${screenshots[currentIndex].color} flex-shrink-0`}>
                {React.createElement(screenshots[currentIndex].icon, { className: "w-8 h-8 text-white" })}
              </div>
              <div className="flex-1 min-w-0">
                <h3 className="text-xl sm:text-2xl font-bold text-gray-900">
                  {screenshots[currentIndex].title}
                </h3>
                <p className="text-gray-600 text-base sm:text-lg mt-1">
                  {screenshots[currentIndex].description}
                </p>
              </div>
            </div>

            <div className="space-y-4">
              <h4 className="font-semibold text-gray-900">Key Features:</h4>
              <ul className="space-y-2">
                {screenshots[currentIndex].type === 'dashboard' && (
                  <>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.1 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-blue-600 rounded-full mr-3 flex-shrink-0"></div>
                      Real-time MRR and ARR calculations
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.2 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-blue-600 rounded-full mr-3 flex-shrink-0"></div>
                      Multi-provider revenue breakdown
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.3 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-blue-600 rounded-full mr-3 flex-shrink-0"></div>
                      Live activity feed and alerts
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.4 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-blue-600 rounded-full mr-3 flex-shrink-0"></div>
                      Growth trend visualization
                    </motion.li>
                  </>
                )}
                {screenshots[currentIndex].type === 'customers' && (
                  <>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.1 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-green-600 rounded-full mr-3 flex-shrink-0"></div>
                      Comprehensive customer database
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.2 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-green-600 rounded-full mr-3 flex-shrink-0"></div>
                      Status tracking and management
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.3 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-green-600 rounded-full mr-3 flex-shrink-0"></div>
                      Payment history and analytics
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.4 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-green-600 rounded-full mr-3 flex-shrink-0"></div>
                      Advanced filtering and search
                    </motion.li>
                  </>
                )}
                {screenshots[currentIndex].type === 'subscriptions' && (
                  <>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.1 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-purple-600 rounded-full mr-3 flex-shrink-0"></div>
                      Subscription lifecycle tracking
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.2 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-purple-600 rounded-full mr-3 flex-shrink-0"></div>
                      Plan distribution analytics
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.3 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-purple-600 rounded-full mr-3 flex-shrink-0"></div>
                      Retention rate calculations
                    </motion.li>
                    <motion.li
                      initial={{ opacity: 0, x: 20 }}
                      animate={{ opacity: 1, x: 0 }}
                      transition={{ duration: 0.3, delay: 0.4 }}
                      className="flex items-center text-gray-700"
                    >
                      <div className="w-2 h-2 bg-purple-600 rounded-full mr-3 flex-shrink-0"></div>
                      Growth trend analysis
                    </motion.li>
                  </>
                )}
              </ul>
            </div>

            <div className="bg-blue-50 rounded-xl p-4 sm:p-6 border border-blue-200">
              <h4 className="font-semibold text-blue-900 mb-2">Why This Matters</h4>
              <p className="text-blue-800 text-sm">
                Get a complete view of your subscription business across all payment providers.
                No more switching between dashboards or manually combining data.
              </p>
            </div>

            <div className="flex flex-col sm:flex-row space-y-3 sm:space-y-0 sm:space-x-4">
              <button className="bg-blue-600 text-white px-6 py-3 rounded-lg font-semibold hover:bg-blue-700 transition-colors">
                View Live Demo
              </button>
              <button className="border border-blue-600 text-blue-600 px-6 py-3 rounded-lg font-semibold hover:bg-blue-50 transition-colors">
                Schedule a Call
              </button>
            </div>
          </motion.div>
        </div>
      </div>
    </section>
  )
}
