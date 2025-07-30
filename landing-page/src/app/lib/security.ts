import { NextRequest, NextResponse } from 'next/server';

// Rate limiting configuration for Next.js
export interface RateLimitConfig {
  windowMs?: number;
  max?: number;
  message?: string;
}

// CORS configuration
export const corsOptions = {
  origin: [
    'http://localhost:3000',
    'https://branddsync.info',
    'https://*.vercel.app',
    'https://*.netlify.app'
  ],
  methods: ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS'],
  allowedHeaders: ['Content-Type', 'Authorization', 'X-Requested-With'],
  credentials: true,
  maxAge: 86400 // 24 hours
};

// Security headers middleware
export const securityHeaders = {
  'X-Frame-Options': 'DENY',
  'X-Content-Type-Options': 'nosniff',
  'X-XSS-Protection': '1; mode=block',
  'Referrer-Policy': 'strict-origin-when-cross-origin',
  'Permissions-Policy': 'camera=(), microphone=(), geolocation=()',
  'Content-Security-Policy': "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://www.googletagmanager.com https://www.google-analytics.com; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; font-src 'self' https://fonts.gstatic.com; img-src 'self' data: https:; connect-src 'self' https://www.google-analytics.com;"
};

// Input validation and sanitization
export const validateEmail = (email: string): boolean => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email) && email.length <= 254;
};

export const sanitizeInput = (input: string): string => {
  return input
    .trim()
    .replace(/[<>]/g, '') // Remove potential HTML tags
    .substring(0, 1000); // Limit length
};

// Request validation middleware
export const validateRequest = (req: NextRequest): { isValid: boolean; errors: string[] } => {
  const errors: string[] = [];

  // Check if request is too large
  const contentLength = req.headers.get('content-length');
  if (contentLength && parseInt(contentLength) > 1024 * 1024) { // 1MB limit
    errors.push('Request too large');
  }

  // Check content type for POST requests
  if (req.method === 'POST') {
    const contentType = req.headers.get('content-type');
    if (!contentType || !contentType.includes('application/json')) {
      errors.push('Invalid content type. Expected application/json');
    }
  }

  return { isValid: errors.length === 0, errors };
};

// API response wrapper
export const createApiResponse = (
  success: boolean,
  data?: unknown,
  message?: string,
  statusCode: number = 200
): NextResponse => {
  const response = {
    success,
    message: message || (success ? 'Success' : 'Error'),
    data,
    timestamp: new Date().toISOString()
  };

  return NextResponse.json(response, { status: statusCode });
};

// Error response wrapper
export const createErrorResponse = (
  message: string,
  statusCode: number = 400,
  errors?: unknown
): NextResponse => {
  return createApiResponse(false, errors, message, statusCode);
};

// Success response wrapper
export const createSuccessResponse = (
  data: unknown,
  message?: string,
  statusCode: number = 200
): NextResponse => {
  return createApiResponse(true, data, message, statusCode);
};

// CORS middleware for Next.js
export const corsMiddleware = (req: NextRequest): NextResponse | null => {
  const origin = req.headers.get('origin');
  const allowedOrigins = corsOptions.origin;

  // Check if origin is allowed
  const isAllowedOrigin = allowedOrigins.some(allowedOrigin => {
    if (allowedOrigin.includes('*')) {
      return origin?.includes(allowedOrigin.replace('*', ''));
    }
    return origin === allowedOrigin;
  });

  if (!isAllowedOrigin && origin) {
    return createErrorResponse('CORS: Origin not allowed', 403);
  }

  return null; // Allow request
};

// Security headers middleware
export const securityHeadersMiddleware = (response: NextResponse): NextResponse => {
  Object.entries(securityHeaders).forEach(([key, value]) => {
    response.headers.set(key, value);
  });
  return response;
};
