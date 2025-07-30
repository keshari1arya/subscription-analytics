import { NextRequest, NextResponse } from 'next/server';
import { corsMiddleware, createErrorResponse, createSuccessResponse, securityHeadersMiddleware, validateRequest } from './security';

// In-memory store for rate limiting (in production, use Redis or similar)
const rateLimitStore = new Map<string, { count: number; resetTime: number }>();

// Simple rate limiter implementation for Next.js
const rateLimiter = (maxRequests: number = 100, windowMs: number = 15 * 60 * 1000) => {
  return (req: NextRequest) => {
    const ip = req.headers.get('x-forwarded-for') || req.headers.get('x-real-ip') || 'unknown';
    const now = Date.now();

    const record = rateLimitStore.get(ip);

    if (!record || now > record.resetTime) {
      // First request or window expired
      rateLimitStore.set(ip, { count: 1, resetTime: now + windowMs });
      return null; // Allow request
    }

    if (record.count >= maxRequests) {
      // Rate limit exceeded
      return createErrorResponse('Too many requests from this IP, please try again later.', 429);
    }

    // Increment count
    record.count++;
    rateLimitStore.set(ip, record);
    return null; // Allow request
  };
};

// Main middleware wrapper
export const withSecurity = (
  handler: (req: NextRequest) => Promise<NextResponse>,
  options: {
    rateLimit?: { max: number; windowMs: number };
    cors?: boolean;
    securityHeaders?: boolean;
    validateFields?: string[];
  } = {}
) => {
  return async (req: NextRequest): Promise<NextResponse> => {
    try {
      // Apply CORS check
      if (options.cors !== false) {
        const corsError = corsMiddleware(req);
        if (corsError) return corsError;
      }

      // Apply rate limiting
      if (options.rateLimit) {
        const rateLimitError = rateLimiter(options.rateLimit.max, options.rateLimit.windowMs)(req);
        if (rateLimitError) return rateLimitError;
      }

      // Validate request
      if (options.validateFields) {
        const validation = validateRequest(req);
        if (!validation.isValid) {
          return createErrorResponse('Invalid request', 400, validation.errors);
        }
      }

      // Call the original handler
      const response = await handler(req);

      // Apply security headers
      if (options.securityHeaders !== false) {
        return securityHeadersMiddleware(response);
      }

      return response;
    } catch (error) {
      console.error('API Error:', error);
      return createErrorResponse('Internal server error', 500);
    }
  };
};

// Clean up rate limit store periodically (every hour)
setInterval(() => {
  const now = Date.now();
  for (const [ip, record] of rateLimitStore.entries()) {
    if (now > record.resetTime) {
      rateLimitStore.delete(ip);
    }
  }
}, 60 * 60 * 1000); // 1 hour

// Export the response helpers
export { createErrorResponse, createSuccessResponse };
