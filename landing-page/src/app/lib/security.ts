import rateLimit from 'express-rate-limit';
import { NextRequest, NextResponse } from 'next/server';

// Rate limiting configuration
export const createRateLimiter = (options: {
  windowMs?: number;
  max?: number;
  message?: string;
  keyGenerator?: (req: any) => string;
}) => {
  const limiter = rateLimit({
    windowMs: options.windowMs || 15 * 60 * 1000, // 15 minutes
    max: options.max || 100, // limit each IP to 100 requests per windowMs
    message: options.message || 'Too many requests from this IP, please try again later.',
    keyGenerator: options.keyGenerator || ((req) => req.ip || req.connection.remoteAddress || 'unknown'),
    standardHeaders: true,
    legacyHeaders: false,
  });

  return limiter;
};

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
export const validateRequest = (req: NextRequest, requiredFields: string[]): { isValid: boolean; errors: string[] } => {
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
  data?: any,
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
  errors?: any
): NextResponse => {
  return createApiResponse(false, errors, message, statusCode);
};

// Success response wrapper
export const createSuccessResponse = (
  data: any,
  message?: string,
  statusCode: number = 200
): NextResponse => {
  return createApiResponse(true, data, message, statusCode);
};
