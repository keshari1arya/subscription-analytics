import { prisma } from '@/lib/prisma'
import { NextRequest, NextResponse } from 'next/server'

export async function POST(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const body = await request.json()
    const {
      firstName,
      lastName,
      email,
      phone,
      linkedin,
      github,
      portfolio,
      coverLetter,
      resumeUrl
    } = body

    // Validate that the job listing exists and is active
    const jobListing = await prisma.jobListing.findUnique({
      where: { id: params.id }
    })

    if (!jobListing) {
      return NextResponse.json(
        { error: 'Job listing not found' },
        { status: 404 }
      )
    }

    if (!jobListing.isActive) {
      return NextResponse.json(
        { error: 'This position is no longer accepting applications' },
        { status: 400 }
      )
    }

    // Check if user has already applied
    const existingApplication = await prisma.jobApplication.findFirst({
      where: {
        jobListingId: params.id,
        email: email
      }
    })

    if (existingApplication) {
      return NextResponse.json(
        { error: 'You have already applied for this position' },
        { status: 400 }
      )
    }

    const application = await prisma.jobApplication.create({
      data: {
        jobListingId: params.id,
        firstName,
        lastName,
        email,
        phone,
        linkedin,
        github,
        portfolio,
        coverLetter,
        resumeUrl,
        metadata: {
          userAgent: request.headers.get('user-agent'),
          referer: request.headers.get('referer'),
          ip: request.headers.get('x-forwarded-for') || request.headers.get('x-real-ip')
        }
      }
    })

    return NextResponse.json({ application }, { status: 201 })
  } catch (error) {
    console.error('Error submitting application:', error)
    return NextResponse.json(
      { error: 'Failed to submit application' },
      { status: 500 }
    )
  }
}

export async function GET(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const applications = await prisma.jobApplication.findMany({
      where: {
        jobListingId: params.id
      },
      orderBy: {
        createdAt: 'desc'
      }
    })

    return NextResponse.json({ applications })
  } catch (error) {
    console.error('Error fetching applications:', error)
    return NextResponse.json(
      { error: 'Failed to fetch applications' },
      { status: 500 }
    )
  }
}
