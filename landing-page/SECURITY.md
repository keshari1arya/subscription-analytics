# 🔒 Security Implementation

## Overview
This document outlines the security measures implemented in the SubscriptionAnalytics landing page API endpoints using Next.js-specific patterns.

## 🛡️ Security Features

### 1. Rate Limiting
- **Waitlist API**: 10 submissions per 15 minutes per IP
- **Contact API**: 5 submissions per 15 minutes per IP
- **Health API**: 100 requests per 15 minutes per IP
- **Debug API**: 10 requests per 15 minutes per IP (limited for security)

### 2. CORS (Cross-Origin Resource Sharing)
- **Allowed Origins**:
  - `http://localhost:3000` (development)
  - `https://branddsync.info` (production domain)
  - `https://*.vercel.app` (Vercel deployments)
  - `https://*.netlify.app` (Netlify deployments)

- **Allowed Methods**: GET, POST, PUT, DELETE, OPTIONS
- **Allowed Headers**: Content-Type, Authorization, X-Requested-With
- **Credentials**: Enabled
- **Max Age**: 24 hours

### 3. Security Headers
- **X-Frame-Options**: DENY (prevents clickjacking)
- **X-Content-Type-Options**: nosniff (prevents MIME type sniffing)
- **X-XSS-Protection**: 1; mode=block (XSS protection)
- **Referrer-Policy**: strict-origin-when-cross-origin
- **Permissions-Policy**: camera=(), microphone=(), geolocation=() (restricts permissions)
- **Content-Security-Policy**: Comprehensive CSP for script, style, font, and image sources

### 4. Input Validation & Sanitization
- **Email Validation**: Regex pattern + length check (max 254 characters)
- **Input Sanitization**:
  - Removes HTML tags (`<`, `>`)
  - Trims whitespace
  - Limits length to 1000 characters
- **Request Size Limit**: 1MB maximum
- **Content-Type Validation**: Ensures JSON for POST requests

### 5. Database Security
- **SQL Injection Prevention**: Using Prisma ORM with parameterized queries
- **Input Sanitization**: All user inputs are sanitized before database storage
- **Schema Isolation**: Using dedicated schema (`landingpage_straightat`) for data isolation

### 6. Error Handling
- **Generic Error Messages**: No sensitive information leaked in error responses
- **Structured Error Responses**: Consistent error format with status codes
- **Logging**: Errors are logged for debugging without exposing sensitive data

## 🔧 Implementation Details

### Rate Limiting (Next.js Implementation)
```typescript
// In-memory store (for production, use Redis or similar)
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
```

### CORS Implementation (Next.js)
```typescript
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
```

### Input Sanitization
```typescript
export const sanitizeInput = (input: string): string => {
  return input
    .trim()
    .replace(/[<>]/g, '') // Remove potential HTML tags
    .substring(0, 1000); // Limit length
};
```

## 🚀 Production Recommendations

### 1. Rate Limiting
- **Use Redis**: Replace in-memory store with Redis for distributed rate limiting
- **Configure Limits**: Adjust rate limits based on expected traffic
- **Monitor**: Set up alerts for rate limit violations

### 2. CORS
- **Restrict Origins**: Limit to only necessary domains
- **Review Headers**: Only allow required headers
- **HTTPS Only**: Ensure all origins use HTTPS in production

### 3. Security Headers
- **CSP**: Review and adjust Content Security Policy based on your needs
- **HSTS**: Add Strict-Transport-Security header for HTTPS enforcement
- **Monitoring**: Set up monitoring for security header violations

### 4. Database
- **Connection Pooling**: Configure Prisma connection pooling
- **Backup Strategy**: Implement regular database backups
- **Encryption**: Ensure database connections use TLS

### 5. Monitoring
- **Error Tracking**: Implement error tracking (Sentry, etc.)
- **Security Monitoring**: Set up alerts for suspicious activity
- **Performance Monitoring**: Monitor API response times

## 🔍 Testing Security

### Rate Limiting Test
```bash
# Test rate limiting
for i in {1..15}; do
  curl -X POST https://your-domain.vercel.app/api/waitlist \
    -H "Content-Type: application/json" \
    -d '{"email":"test@example.com"}'
done
```

### CORS Test
```bash
# Test CORS from different origin
curl -H "Origin: https://malicious-site.com" \
  https://your-domain.vercel.app/api/health
```

### Input Validation Test
```bash
# Test XSS attempt
curl -X POST https://your-domain.vercel.app/api/contact \
  -H "Content-Type: application/json" \
  -d '{"firstName":"<script>alert(\"xss\")</script>","lastName":"Test","email":"test@example.com","message":"test"}'
```

## 📊 Security Metrics

Track these metrics to ensure security effectiveness:
- Rate limit violations per day
- CORS rejections per day
- Invalid input attempts per day
- Error response rates
- API response times

## 🔄 Updates

This security implementation should be reviewed and updated regularly:
- Monthly: Review rate limits and CORS settings
- Quarterly: Update security headers and CSP
- Annually: Comprehensive security audit

---

**Note**: This security implementation provides a solid foundation for Next.js API routes and should be enhanced based on your specific requirements and threat model.
