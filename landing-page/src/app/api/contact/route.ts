import { NextRequest, NextResponse } from 'next/server'

export async function POST(request: NextRequest) {
  try {
    const { firstName, lastName, email, company, message } = await request.json()

    // Validate required fields
    if (!firstName || !lastName || !email || !message) {
      return NextResponse.json(
        { error: 'Please fill in all required fields' },
        { status: 400 }
      )
    }

    // Validate email
    if (!email.includes('@')) {
      return NextResponse.json(
        { error: 'Please enter a valid email address' },
        { status: 400 }
      )
    }

    // Here you would typically:
    // 1. Save to your database
    // 2. Send to your CRM (HubSpot, Salesforce, etc.)
    // 3. Send notification email to your team
    // 4. Send confirmation email to the user

    // For now, we'll just log it and return success
    console.log('New contact form submission:', {
      firstName,
      lastName,
      email,
      company,
      message
    })

    // Simulate processing time
    await new Promise(resolve => setTimeout(resolve, 500))

    return NextResponse.json(
      {
        success: true,
        message: 'Message sent successfully',
        data: { firstName, lastName, email, company }
      },
      { status: 200 }
    )
  } catch (error) {
    console.error('Contact API error:', error)
    return NextResponse.json(
      { error: 'Internal server error' },
      { status: 500 }
    )
  }
}
