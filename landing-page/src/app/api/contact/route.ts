import { prisma } from '@/lib/prisma'
import { contactSchema } from '@/lib/validation'
import { NextRequest, NextResponse } from 'next/server'

export async function POST(request: NextRequest) {
  try {
    const body = await request.json()

    // Validate input
    const validationResult = contactSchema.safeParse(body)
    if (!validationResult.success) {
      return NextResponse.json(
        {
          error: 'Invalid input',
          details: validationResult.error.issues
        },
        { status: 400 }
      )
    }

    const { firstName, lastName, email, company, message, metadata } = validationResult.data

    // Create contact submission
    const contactSubmission = await prisma.contactSubmission.create({
      data: {
        firstName,
        lastName,
        email,
        company,
        message,
        metadata: metadata || {
          userAgent: request.headers.get('user-agent'),
          referer: request.headers.get('referer'),
          timestamp: new Date().toISOString()
        }
      }
    })

    console.log('New contact form submission:', { firstName, lastName, email, company })

    return NextResponse.json(
      {
        success: true,
        message: 'Message sent successfully',
        data: {
          id: contactSubmission.id,
          firstName: contactSubmission.firstName,
          lastName: contactSubmission.lastName,
          email: contactSubmission.email,
          createdAt: contactSubmission.createdAt
        }
      },
      { status: 201 }
    )

  } catch (error) {
    console.error('Contact API error:', error)

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
    const count = await prisma.contactSubmission.count()

    return NextResponse.json(
      {
        success: true,
        data: {
          totalSubmissions: count
        }
      },
      { status: 200 }
    )
  } catch (error) {
    console.error('Contact count error:', error)
    return NextResponse.json(
      { error: 'Internal server error' },
      { status: 500 }
    )
  }
}
