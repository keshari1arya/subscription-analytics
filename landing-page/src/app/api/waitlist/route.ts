import { prisma } from '@/lib/prisma'
import { waitlistSchema } from '@/lib/validation'
import { NextRequest, NextResponse } from 'next/server'

export async function POST(request: NextRequest) {
  try {
    const body = await request.json()

    // Validate input
    const validationResult = waitlistSchema.safeParse(body)
    if (!validationResult.success) {
      return NextResponse.json(
        {
          error: 'Invalid input',
          details: validationResult.error.issues
        },
        { status: 400 }
      )
    }

    const { email, source, metadata } = validationResult.data

    // Check if email already exists
    const existingEntry = await prisma.waitlistEntry.findUnique({
      where: { email }
    })

    if (existingEntry) {
      return NextResponse.json(
        {
          error: 'Email already registered',
          message: 'This email is already on our waitlist'
        },
        { status: 409 }
      )
    }

    // Create waitlist entry
    const waitlistEntry = await prisma.waitlistEntry.create({
      data: {
        email,
        source: source || 'website',
        metadata: metadata || {
          userAgent: request.headers.get('user-agent'),
          referer: request.headers.get('referer'),
          timestamp: new Date().toISOString()
        }
      }
    })

    console.log('New waitlist signup:', email)

    return NextResponse.json(
      {
        success: true,
        message: 'Successfully joined waitlist',
        data: {
          id: waitlistEntry.id,
          email: waitlistEntry.email,
          createdAt: waitlistEntry.createdAt
        }
      },
      { status: 201 }
    )

  } catch (error) {
    console.error('Waitlist API error:', error)

    // Handle Prisma errors
    if (error instanceof Error && error.message.includes('prisma')) {
      return NextResponse.json(
        { error: 'Database error. Please try again later.' },
        { status: 500 }
      )
    }

    return NextResponse.json(
      { error: 'Internal server error' },
      { status: 500 }
    )
  }
}

export async function GET() {
  try {
    const count = await prisma.waitlistEntry.count()

    return NextResponse.json(
      {
        success: true,
        data: {
          totalSignups: count
        }
      },
      { status: 200 }
    )
  } catch (error) {
    console.error('Waitlist count error:', error)
    return NextResponse.json(
      { error: 'Internal server error' },
      { status: 500 }
    )
  }
}
