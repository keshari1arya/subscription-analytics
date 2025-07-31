import { PrismaClient } from '@prisma/client'

const prisma = new PrismaClient()

const sampleJobs = [
  {
    title: 'Senior Full Stack Engineer',
    department: 'Engineering',
    location: 'Remote',
    type: 'Full-time',
    salary: '$120k - $180k',
    description: 'Build the core platform that powers subscription analytics for thousands of businesses. You\'ll work on real-time data processing, API development, and scalable architecture.',
    requirements: [
      '5+ years of experience with React, Node.js, and TypeScript',
      'Experience with real-time data systems and APIs',
      'Strong understanding of database design and optimization',
      'Experience with cloud platforms (AWS, GCP, or Azure)',
      'Experience with microservices architecture',
      'Strong problem-solving and debugging skills'
    ],
    benefits: [
      'Competitive salary and equity',
      'Remote-first culture',
      'Flexible working hours',
      'Health, dental, and vision insurance',
      'Professional development budget',
      'Latest hardware and tools'
    ]
  },
  {
    title: 'Product Manager',
    department: 'Product',
    location: 'Remote',
    type: 'Full-time',
    salary: '$100k - $150k',
    description: 'Lead product strategy and execution for our subscription analytics platform. You\'ll work closely with engineering, design, and customer success teams to deliver exceptional user experiences.',
    requirements: [
      '3+ years of product management experience',
      'Experience in SaaS or fintech products',
      'Strong analytical and data-driven decision making',
      'Excellent communication and stakeholder management',
      'Experience with user research and A/B testing',
      'Technical background or ability to work closely with engineers'
    ],
    benefits: [
      'Competitive salary and equity',
      'Remote-first culture',
      'Flexible working hours',
      'Health, dental, and vision insurance',
      'Professional development budget',
      'Direct impact on product strategy'
    ]
  },
  {
    title: 'Customer Success Manager',
    department: 'Customer Success',
    location: 'Remote',
    type: 'Full-time',
    salary: '$80k - $120k',
    description: 'Help customers maximize the value of SubscriptionAnalytics and drive product adoption. You\'ll be the primary point of contact for our enterprise customers.',
    requirements: [
      '2+ years in customer success or account management',
      'Experience with SaaS products and analytics',
      'Strong problem-solving and communication skills',
      'Data-driven approach to customer success',
      'Experience with enterprise customers',
      'Ability to understand technical concepts'
    ],
    benefits: [
      'Competitive salary and equity',
      'Remote-first culture',
      'Flexible working hours',
      'Health, dental, and vision insurance',
      'Professional development budget',
      'Direct customer impact'
    ]
  },
  {
    title: 'Data Scientist',
    department: 'Data Science',
    location: 'Remote',
    type: 'Full-time',
    salary: '$110k - $160k',
    description: 'Develop predictive models and advanced analytics features for our platform. You\'ll work on machine learning models that help businesses predict churn, optimize pricing, and identify growth opportunities.',
    requirements: [
      '3+ years of experience in data science or machine learning',
      'Strong Python skills and experience with ML frameworks',
      'Experience with subscription business metrics',
      'Ability to translate complex models into business insights',
      'Experience with SQL and data warehousing',
      'Strong statistical background'
    ],
    benefits: [
      'Competitive salary and equity',
      'Remote-first culture',
      'Flexible working hours',
      'Health, dental, and vision insurance',
      'Professional development budget',
      'Access to cutting-edge ML tools'
    ]
  },
  {
    title: 'Frontend Engineer',
    department: 'Engineering',
    location: 'Remote',
    type: 'Full-time',
    salary: '$90k - $140k',
    description: 'Build beautiful, responsive user interfaces for our analytics dashboard. You\'ll work on data visualizations, real-time updates, and creating intuitive user experiences.',
    requirements: [
      '3+ years of experience with React and TypeScript',
      'Experience with data visualization libraries (D3.js, Chart.js)',
      'Strong CSS and responsive design skills',
      'Experience with state management (Redux, Zustand)',
      'Understanding of web performance optimization',
      'Experience with testing frameworks'
    ],
    benefits: [
      'Competitive salary and equity',
      'Remote-first culture',
      'Flexible working hours',
      'Health, dental, and vision insurance',
      'Professional development budget',
      'Latest frontend tools and frameworks'
    ]
  }
]

async function seedJobs() {
  try {
    console.log('🌱 Seeding job listings...')

    for (const job of sampleJobs) {
      await prisma.jobListing.create({
        data: job
      })
      console.log(`✅ Created job: ${job.title}`)
    }

    console.log('🎉 Successfully seeded job listings!')
  } catch (error) {
    console.error('❌ Error seeding jobs:', error)
  } finally {
    await prisma.$disconnect()
  }
}

seedJobs()
