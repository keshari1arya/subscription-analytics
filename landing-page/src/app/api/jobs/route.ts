import { prisma } from '@/lib/prisma'
import { NextRequest, NextResponse } from 'next/server'

export async function GET(request: NextRequest) {
  try {
    const { searchParams } = new URL(request.url)
    const department = searchParams.get('department')
    const type = searchParams.get('type')
    const location = searchParams.get('location')

    const where: Record<string, unknown> = {
      isActive: true
    }

    if (department) {
      where.department = department
    }

    if (type) {
      where.type = type
    }

    if (location) {
      where.location = location
    }

    const jobs = await prisma.jobListing.findMany({
      where,
      orderBy: {
        createdAt: 'desc'
      },
      include: {
        _count: {
          select: {
            applications: true
          }
        }
      }
    })

    return NextResponse.json({ jobs })
  } catch (error) {
    console.error('Error fetching jobs:', error)
    return NextResponse.json(
      { error: 'Failed to fetch jobs' },
      { status: 500 }
    )
  }
}

export async function POST(request: NextRequest) {
  try {
    const body = await request.json()
    const { title, department, location, type, salary, description, requirements, benefits } = body

    const job = await prisma.jobListing.create({
      data: {
        title,
        department,
        location,
        type,
        salary,
        description,
        requirements,
        benefits
      }
    })

    return NextResponse.json({ job }, { status: 201 })
  } catch (error) {
    console.error('Error creating job:', error)
    return NextResponse.json(
      { error: 'Failed to create job' },
      { status: 500 }
    )
  }
}
