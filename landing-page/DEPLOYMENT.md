# 🚀 Deployment Guide - SubscriptionAnalytics Landing Page

## 📋 Overview
This guide covers deploying the Next.js landing page with PostgreSQL database and Vercel hosting.

---

## **🔧 Prerequisites**

### **1. Required Accounts**
- [ ] GitHub account
- [ ] Vercel account (free tier)
- [ ] Database: PostgreSQL (currently using filess.io)
- [ ] Domain: branddsync.info

### **2. Required Tools**
- [ ] Node.js 20+
- [ ] Yarn package manager
- [ ] Git

---

## **🗄️ Database Setup (Current: filess.io)**

### **1. Database Configuration**
```bash
# Current database connection
DATABASE_URL="postgresql://landingpage_straightat:271238ae708062d3a7fd7a9c695231ebed9f25f1@ia56i9.h.filess.io:5434/landingpage_straightat"

# Database details
Database name: landingpage_straightat
Username: landingpage_straightat
Schema: landingpage_straightat
```

### **2. Database Schema**
```sql
-- Waitlist entries (landingpage_straightat.waitlist_entries)
CREATE TABLE waitlist_entries (
  id TEXT PRIMARY KEY,
  email TEXT UNIQUE NOT NULL,
  created_at TIMESTAMP DEFAULT NOW(),
  updated_at TIMESTAMP DEFAULT NOW(),
  status TEXT DEFAULT 'pending',
  source TEXT,
  metadata JSONB
);

-- Contact submissions (landingpage_straightat.contact_submissions)
CREATE TABLE contact_submissions (
  id TEXT PRIMARY KEY,
  first_name TEXT NOT NULL,
  last_name TEXT NOT NULL,
  email TEXT NOT NULL,
  company TEXT,
  message TEXT NOT NULL,
  created_at TIMESTAMP DEFAULT NOW(),
  status TEXT DEFAULT 'new',
  metadata JSONB
);
```

---

## **🌐 Vercel Setup**

### **1. Connect GitHub Repository**
```bash
1. Go to Vercel Dashboard
2. Import Git repository
3. Connect to GitHub
4. Select repository: subscriptionanalytics/landing-page
```

### **2. Configure Environment Variables**
```bash
# In Vercel Dashboard > Settings > Environment Variables
DATABASE_URL="postgresql://landingpage_straightat:271238ae708062d3a7fd7a9c695231ebed9f25f1@ia56i9.h.filess.io:5434/landingpage_straightat"
NEXT_PUBLIC_API_URL="https://branddsync.info"
NEXT_PUBLIC_GA_MEASUREMENT_ID="G-XXXXXXXXXX"
```

### **3. Build Settings**
```bash
# Vercel build settings
Framework Preset: Next.js
Build Command: yarn build
Output Directory: .next
Install Command: yarn install
```

---

## **🔑 GitHub Secrets Setup**

### **1. Required Secrets**
```bash
# Go to GitHub > Repository > Settings > Secrets and variables > Actions

VERCEL_TOKEN="your-vercel-token"
VERCEL_ORG_ID="your-vercel-org-id"
VERCEL_PROJECT_ID="your-vercel-project-id"
DATABASE_URL="postgresql://landingpage_straightat:271238ae708062d3a7fd7a9c695231ebed9f25f1@ia56i9.h.filess.io:5434/landingpage_straightat"
```

### **2. Get Vercel Tokens**
```bash
# Install Vercel CLI
npm i -g vercel

# Login to Vercel
vercel login

# Get tokens
vercel whoami
# Note down the tokens from Vercel dashboard
```

---

## **🗄️ Database Migration**

### **1. Local Development**
```bash
# Install dependencies
yarn install

# Generate Prisma client
npx prisma generate

# Push schema to database
npx prisma db push

# View database (optional)
npx prisma studio
```

### **2. Production Migration**
```bash
# The GitHub Actions workflow will run:
npx prisma generate
npx prisma db push
```

---

## **🌐 Domain Configuration**

### **1. DNS Setup**
```bash
# Add to your domain DNS
Type: CNAME
Name: www
Value: cname.vercel-dns.com

Type: CNAME
Name: @
Value: cname.vercel-dns.com
```

### **2. Vercel Domain**
```bash
# In Vercel Dashboard
1. Go to Settings > Domains
2. Add domain: branddsync.info
3. Configure DNS as above
4. Wait for SSL certificate
```

---

## **🚀 Deployment Steps**

### **1. Initial Setup**
```bash
# Clone repository
git clone https://github.com/your-username/subscriptionanalytics.git
cd landing-page

# Install dependencies
yarn install

# Set up environment
cp env.example .env
# Edit .env with your values

# Generate Prisma client
npx prisma generate

# Push schema to database
npx prisma db push
```

### **2. Test Locally**
```bash
# Start development server
yarn dev

# Test endpoints
curl http://localhost:3000/api/health
curl -X POST http://localhost:3000/api/waitlist \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com"}'
```

### **3. Deploy to Production**
```bash
# Push to main branch
git add .
git commit -m "Initial deployment setup"
git push origin main

# GitHub Actions will automatically:
# 1. Run tests
# 2. Build application
# 3. Push database schema
# 4. Deploy to Vercel
```

---

## **📊 Monitoring & Verification**

### **1. Health Check**
```bash
# Check application health
curl https://branddsync.info/api/health

# Expected response:
{
  "status": "healthy",
  "database": "connected",
  "stats": {
    "waitlistSignups": 0,
    "contactSubmissions": 0
  }
}
```

### **2. Test Forms**
```bash
# Test waitlist signup
curl -X POST https://branddsync.info/api/waitlist \
  -H "Content-Type: application/json" \
  -d '{"email":"test@branddsync.info"}'

# Test contact form
curl -X POST https://branddsync.info/api/contact \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "message": "Test message"
  }'
```

---

## **💰 Cost Estimation**

### **Monthly Costs**
```
Database (filess.io): $0-10
Vercel (Hobby): $0
Domain: $10-15/year
Total: ~$10-15/month
```

---

## **🔧 Troubleshooting**

### **1. Database Connection Issues**
```bash
# Check database connectivity
npx prisma db pull

# Check logs
npx prisma studio

# Test connection
npx prisma db execute --url "your-database-url" --stdin <<< "SELECT 1;"
```

### **2. Build Failures**
```bash
# Clear cache
rm -rf .next
rm -rf node_modules
yarn install

# Check environment variables
echo $DATABASE_URL
```

### **3. Deployment Issues**
```bash
# Check Vercel logs
vercel logs

# Check GitHub Actions
# Go to Actions tab in GitHub
```

---

## **📈 Next Steps**

### **1. Analytics Setup**
- [ ] Configure Google Analytics
- [ ] Set up conversion tracking
- [ ] Add event tracking

### **2. Email Notifications (Future)**
- [ ] Set up AWS SES or SendGrid
- [ ] Create email templates
- [ ] Add notification system

### **3. Monitoring**
- [ ] Set up error tracking
- [ ] Add performance monitoring
- [ ] Configure alerts

---

## **✅ Current Status**

### **✅ Backend Features**
- [x] Database persistence for all form submissions
- [x] Input validation with detailed error messages
- [x] Duplicate prevention for waitlist signups
- [x] Metadata tracking (user agent, referrer, timestamp)
- [x] Health monitoring endpoint
- [x] API rate limiting ready

### **✅ API Endpoints**
```bash
POST /api/waitlist     # Join waitlist
GET  /api/waitlist     # Get signup count
POST /api/contact      # Submit contact form
GET  /api/contact      # Get submission count
GET  /api/health       # Health check
```

### **✅ Database Schema**
```sql
-- Tables created in landingpage_straightat schema
waitlist_entries
contact_submissions
```

---

## **📞 Support**

For issues or questions:
1. Check Vercel logs
2. Check GitHub Actions logs
3. Check database connectivity
4. Review this deployment guide

**Ready to deploy! 🚀**
