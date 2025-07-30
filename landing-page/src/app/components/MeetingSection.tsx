'use client'

import { motion } from 'framer-motion'
import { Calendar, Clock, MessageSquare, User, Video } from 'lucide-react'

const meetingTypes = [
  {
    id: 'demo',
    title: 'Product Demo',
    duration: '30 min',
    description: 'See the platform in action',
    icon: Video,
    color: 'from-blue-600 to-indigo-700'
  },
  {
    id: 'pain-points',
    title: 'Pain Point Discussion',
    duration: '45 min',
    description: 'Deep dive into your challenges',
    icon: MessageSquare,
    color: 'from-green-600 to-emerald-700'
  },
  {
    id: 'implementation',
    title: 'Implementation Planning',
    duration: '60 min',
    description: 'Technical setup strategy',
    icon: Calendar,
    color: 'from-purple-600 to-pink-700'
  },
  {
    id: 'executive',
    title: 'Executive Overview',
    duration: '30 min',
    description: 'Business case and ROI',
    icon: User,
    color: 'from-orange-600 to-red-700'
  }
]

export default function MeetingSection() {
  return (
    <section className="py-20 bg-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-12">
          <motion.h2
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="text-3xl md:text-4xl font-bold text-gray-900 mb-4"
          >
            Schedule a Meeting
          </motion.h2>
          <motion.p
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            viewport={{ once: true }}
            className="text-xl text-gray-600 max-w-2xl mx-auto"
          >
            Get personalized insights for your business
          </motion.p>
        </div>

        <div className="grid lg:grid-cols-2 gap-8 items-start">
          {/* Meeting Types */}
          <motion.div
            initial={{ opacity: 0, x: -30 }}
            whileInView={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="space-y-4"
          >
            <h3 className="text-xl font-bold text-gray-900 mb-4">
              Choose Your Meeting Type
            </h3>
            <div className="space-y-3">
              {meetingTypes.map((meeting, index) => (
                <motion.div
                  key={meeting.id}
                  initial={{ opacity: 0, x: -20 }}
                  whileInView={{ opacity: 1, x: 0 }}
                  transition={{ duration: 0.5, delay: index * 0.1 }}
                  viewport={{ once: true }}
                  className="bg-white border border-gray-200 rounded-lg p-4 hover:shadow-md transition-all duration-200 cursor-pointer group"
                >
                  <div className="flex items-center space-x-3">
                    <div className={`p-2 rounded-lg bg-gradient-to-br ${meeting.color} flex-shrink-0`}>
                      <meeting.icon className="w-4 h-4 text-white" />
                    </div>
                    <div className="flex-1">
                      <div className="flex items-center justify-between">
                        <h4 className="font-semibold text-gray-900 group-hover:text-blue-600 transition-colors">
                          {meeting.title}
                        </h4>
                        <span className="text-sm text-gray-500 flex items-center">
                          <Clock className="w-3 h-3 mr-1" />
                          {meeting.duration}
                        </span>
                      </div>
                      <p className="text-gray-600 text-sm mt-1">
                        {meeting.description}
                      </p>
                    </div>
                  </div>
                </motion.div>
              ))}
            </div>
          </motion.div>

          {/* Booking */}
          <motion.div
            initial={{ opacity: 0, x: 30 }}
            whileInView={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8 }}
            viewport={{ once: true }}
            className="space-y-4"
          >
            <div className="bg-gradient-to-br from-gray-50 to-gray-100 rounded-xl p-6">
              <div className="text-center mb-4">
                <div className="w-10 h-10 bg-blue-600 rounded-full flex items-center justify-center mx-auto mb-3">
                  <Calendar className="w-5 h-5 text-white" />
                </div>
                <h3 className="text-lg font-bold text-gray-900 mb-2">
                  Book Your Meeting
                </h3>
                <p className="text-gray-600 text-sm">
                  Select a time that works for you
                </p>
              </div>

              {/* Cal.com Widget Placeholder */}
              <div className="bg-white rounded-lg p-4 border border-gray-200">
                <div className="text-center py-6">
                  <div className="w-6 h-6 bg-gray-200 rounded-full flex items-center justify-center mx-auto mb-2">
                    <Calendar className="w-3 h-3 text-gray-500" />
                  </div>
                  <h4 className="font-semibold text-gray-900 mb-1">
                    Cal.com Integration
                  </h4>
                  <p className="text-gray-600 text-sm mb-3">
                    Calendar widget will be embedded here
                  </p>
                  <button className="bg-blue-600 text-white px-4 py-2 rounded-lg font-semibold hover:bg-blue-700 transition-colors text-sm">
                    Open Calendar
                  </button>
                </div>
              </div>

              <div className="mt-3 text-center text-xs text-gray-500">
                Available Monday - Friday, 9 AM - 6 PM EST
              </div>
            </div>

            {/* <div className="bg-blue-50 rounded-lg p-4 border border-blue-200">
              <h4 className="font-semibold text-blue-900 mb-2">What You'll Get</h4>
              <ul className="space-y-1 text-sm text-blue-800">
                <li>• Personalized demo based on your business</li>
                <li>• Direct access to our founder</li>
                <li>• Custom implementation recommendations</li>
                <li>• Pricing and ROI analysis</li>
              </ul>
            </div> */}
          </motion.div>
        </div>
      </div>
    </section>
  )
}
