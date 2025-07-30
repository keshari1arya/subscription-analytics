import { prisma } from '@/lib/prisma'
import { NextResponse } from 'next/server'

export async function GET() {
  try {
    // Test database connection
    await prisma.$queryRaw`SELECT 1`

    // Get basic stats
    const [waitlistCount, contactCount] = await Promise.all([
      prisma.waitlistEntry.count(),
      prisma.contactSubmission.count()
    ])

    return NextResponse.json(
      {
        status: 'healthy',
        timestamp: new Date().toISOString(),
        database: 'connected',
        stats: {
          waitlistSignups: waitlistCount,
          contactSubmissions: contactCount
        }
      },
      { status: 200 }
    )
  } catch (error) {
    console.error('Health check error:', error)

    return NextResponse.json(
      {
        status: 'unhealthy',
        timestamp: new Date().toISOString(),
        database: 'disconnected',
        error: error instanceof Error ? error.message : 'Unknown error'
      },
      { status: 503 }
    )
  }
}
