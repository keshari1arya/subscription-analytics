import { NextRequest, NextResponse } from 'next/server';
import { prisma } from '../../../lib/prisma';
import { waitlistSchema } from '../../../lib/validation';
import { createErrorResponse, createSuccessResponse, withSecurity } from '../../lib/middleware';
import { sanitizeInput, validateEmail } from '../../lib/security';

// Handler function
const waitlistHandler = async (req: NextRequest): Promise<NextResponse> => {
  if (req.method === 'GET') {
    try {
      const count = await prisma.waitlistEntry.count();
      return createSuccessResponse({ count }, 'Waitlist count retrieved successfully');
    } catch (error) {
      console.error('Error getting waitlist count:', error);
      return createErrorResponse('Failed to get waitlist count', 500);
    }
  }

  if (req.method === 'POST') {
    try {
      const body = await req.json();

      // Validate input
      const validation = waitlistSchema.safeParse(body);
      if (!validation.success) {
        return createErrorResponse('Invalid input data', 400, validation.error.issues);
      }

      const { email } = validation.data;

      // Additional email validation
      if (!validateEmail(email)) {
        return createErrorResponse('Invalid email format', 400);
      }

      // Sanitize email
      const sanitizedEmail = sanitizeInput(email).toLowerCase();

      // Check for existing entry
      const existingEntry = await prisma.waitlistEntry.findUnique({
        where: { email: sanitizedEmail }
      });

      if (existingEntry) {
        return createErrorResponse('Email already registered', 409);
      }

      // Create new entry
      const entry = await prisma.waitlistEntry.create({
        data: {
          email: sanitizedEmail,
          source: 'website',
          metadata: {
            userAgent: req.headers.get('user-agent') || '',
            referer: req.headers.get('referer') || '',
            ip: req.headers.get('x-forwarded-for') || req.headers.get('x-real-ip') || 'unknown',
            timestamp: new Date().toISOString()
          }
        }
      });

      return createSuccessResponse(
        { id: entry.id, email: entry.email },
        'Successfully joined waitlist',
        201
      );
    } catch (error) {
      console.error('Error creating waitlist entry:', error);
      return createErrorResponse('Failed to join waitlist', 500);
    }
  }

  return createErrorResponse('Method not allowed', 405);
};

// Export with security middleware
export const GET = withSecurity(waitlistHandler, {
  rateLimit: { max: 50, windowMs: 15 * 60 * 1000 }, // 50 requests per 15 minutes
  cors: true,
  securityHeaders: true
});

export const POST = withSecurity(waitlistHandler, {
  rateLimit: { max: 10, windowMs: 15 * 60 * 1000 }, // 10 submissions per 15 minutes
  cors: true,
  securityHeaders: true,
  validateFields: ['email']
});
