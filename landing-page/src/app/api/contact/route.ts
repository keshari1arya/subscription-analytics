import { NextRequest, NextResponse } from 'next/server';
import { prisma } from '../../../lib/prisma';
import { contactSchema } from '../../../lib/validation';
import { createErrorResponse, createSuccessResponse, withSecurity } from '../../lib/middleware';
import { sanitizeInput } from '../../lib/security';

// Handler function
const contactHandler = async (req: NextRequest): Promise<NextResponse> => {
  if (req.method === 'GET') {
    try {
      const count = await prisma.contactSubmission.count();
      return createSuccessResponse({ count }, 'Contact submissions count retrieved successfully');
    } catch (error) {
      console.error('Error getting contact count:', error);
      return createErrorResponse('Failed to get contact count', 500);
    }
  }

  if (req.method === 'POST') {
    try {
      const body = await req.json();

      // Validate input
      const validation = contactSchema.safeParse(body);
      if (!validation.success) {
        return createErrorResponse('Invalid input data', 400, validation.error.issues);
      }

      const { firstName, lastName, email, company, message } = validation.data;

      // Sanitize inputs
      const sanitizedData = {
        firstName: sanitizeInput(firstName),
        lastName: sanitizeInput(lastName),
        email: sanitizeInput(email).toLowerCase(),
        company: company ? sanitizeInput(company) : null,
        message: sanitizeInput(message)
      };

      // Create contact submission
      const submission = await prisma.contactSubmission.create({
        data: {
          ...sanitizedData,
          metadata: {
            userAgent: req.headers.get('user-agent') || '',
            referer: req.headers.get('referer') || '',
            ip: req.headers.get('x-forwarded-for') || req.headers.get('x-real-ip') || 'unknown',
            timestamp: new Date().toISOString()
          }
        }
      });

      return createSuccessResponse(
        { id: submission.id, email: submission.email },
        'Contact form submitted successfully',
        201
      );
    } catch (error) {
      console.error('Error creating contact submission:', error);
      return createErrorResponse('Failed to submit contact form', 500);
    }
  }

  return createErrorResponse('Method not allowed', 405);
};

// Export with security middleware
export const GET = withSecurity(contactHandler, {
  rateLimit: { max: 50, windowMs: 15 * 60 * 1000 }, // 50 requests per 15 minutes
  cors: true,
  securityHeaders: true
});

export const POST = withSecurity(contactHandler, {
  rateLimit: { max: 5, windowMs: 15 * 60 * 1000 }, // 5 submissions per 15 minutes
  cors: true,
  securityHeaders: true,
  validateFields: ['firstName', 'lastName', 'email', 'message']
});
