import { NextRequest, NextResponse } from 'next/server';
import { prisma } from '../../../lib/prisma';
import { createErrorResponse, createSuccessResponse, withSecurity } from '../../lib/middleware';

// Handler function
const healthHandler = async (_req: NextRequest): Promise<NextResponse> => {
  try {
    // Test database connection
    const waitlistCount = await prisma.waitlistEntry.count();
    const contactCount = await prisma.contactSubmission.count();

    const healthData = {
      status: 'healthy',
      timestamp: new Date().toISOString(),
      database: 'connected',
      stats: {
        waitlistEntries: waitlistCount,
        contactSubmissions: contactCount
      }
    };

    return createSuccessResponse(healthData, 'Health check successful');
  } catch (error) {
    console.error('Health check error:', error);
    return createErrorResponse('Health check failed', 500);
  }
};

// Export with security middleware
export const GET = withSecurity(healthHandler, {
  rateLimit: { max: 100, windowMs: 15 * 60 * 1000 }, // 100 requests per 15 minutes
  cors: true,
  securityHeaders: true
});
