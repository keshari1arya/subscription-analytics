# 🚀 Vercel Deployment Checklist

## ✅ Pre-Deployment Checklist

### **1. Code Ready**
- [x] Build passes locally (`yarn build`)
- [x] Prisma generate working in build process
- [x] All API endpoints tested
- [x] Database connection working
- [x] Environment variables configured

### **2. Vercel Account**
- [ ] Create Vercel account at [vercel.com](https://vercel.com)
- [ ] Connect GitHub account
- [ ] Verify email address

### **3. Repository Ready**
- [ ] Code pushed to GitHub
- [ ] All changes committed
- [ ] No sensitive data in repository

---

## 🚀 Deployment Steps

### **Step 1: Import Project**
1. Go to [vercel.com/dashboard](https://vercel.com/dashboard)
2. Click "New Project"
3. Click "Import Git Repository"
4. Select your GitHub account
5. Find `subscriptionanalytics` repository
6. Click "Import"

### **Step 2: Configure Project**
```
Framework Preset: Next.js
Root Directory: landing-page
Build Command: yarn build (includes prisma generate)
Output Directory: .next
Install Command: yarn install
```

### **Step 3: Environment Variables**
Add these in Vercel project settings:

```bash
# Required
DATABASE_URL = postgresql://landingpage_straightat:271238ae708062d3a7fd7a9c695231ebed9f25f1@ia56i9.h.filess.io:5434/landingpage_straightat

# Optional (update after deployment)
NEXT_PUBLIC_API_URL = https://your-project-name.vercel.app
NEXT_PUBLIC_GA_MEASUREMENT_ID = G-XXXXXXXXXX
```

### **Step 4: Deploy**
1. Click "Deploy"
2. Wait for build to complete (should include Prisma generate)
3. Note the deployment URL

---

## 🧪 Post-Deployment Testing

### **1. Health Check**
```bash
curl https://your-project-name.vercel.app/api/health
```
Expected: `{"status":"healthy","database":"connected"}`

### **2. Waitlist Test**
```bash
curl -X POST https://your-project-name.vercel.app/api/waitlist \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com"}'
```
Expected: `{"success":true,"message":"Successfully joined waitlist"}`

### **3. Contact Form Test**
```bash
curl -X POST https://your-project-name.vercel.app/api/contact \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "message": "Test message"
  }'
```
Expected: `{"success":true,"message":"Message sent successfully"}`

---

## 🌐 Custom Domain Setup

### **1. Add Domain**
1. Go to Vercel project settings
2. Click "Domains"
3. Add domain: `branddsync.info`
4. Follow DNS configuration instructions

### **2. DNS Records**
```
Type: CNAME
Name: www
Value: cname.vercel-dns.com

Type: CNAME
Name: @
Value: cname.vercel-dns.com
```

### **3. Update Environment**
After domain is active, update:
```
NEXT_PUBLIC_API_URL = https://branddsync.info
```

---

## 📊 Monitoring

### **1. Vercel Dashboard**
- [ ] Check deployment status
- [ ] Monitor function logs
- [ ] Check build logs for errors
- [ ] Verify Prisma generate ran successfully

### **2. Database Monitoring**
- [ ] Test database connection
- [ ] Check for new submissions
- [ ] Monitor performance

### **3. Analytics Setup**
- [ ] Add Google Analytics ID
- [ ] Test conversion tracking
- [ ] Monitor page views

---

## 🔧 Troubleshooting

### **Prisma Issues**
```bash
# If you see "Prisma Client is not generated" error:
# 1. Check that DATABASE_URL is set correctly
# 2. Verify build includes "prisma generate"
# 3. Check Vercel function logs for Prisma errors
```

### **Build Failures**
```bash
# Check build logs in Vercel dashboard
# Common issues:
# - Missing environment variables
# - TypeScript errors
# - Missing dependencies
# - Prisma generate failing
```

### **API Errors**
```bash
# Check function logs in Vercel dashboard
# Common issues:
# - Database connection timeout
# - Missing environment variables
# - CORS issues
# - Prisma client not generated
```

### **Domain Issues**
```bash
# Check DNS propagation
# Verify SSL certificate
# Test domain resolution
```

---

## ✅ Success Criteria

- [ ] Landing page loads correctly
- [ ] All forms submit successfully
- [ ] Database stores submissions
- [ ] Custom domain works
- [ ] SSL certificate active
- [ ] Analytics tracking working
- [ ] Prisma client generated successfully

---

## 📞 Support

If you encounter issues:
1. Check Vercel deployment logs
2. Verify environment variables
3. Test database connectivity
4. Check Prisma generate in build logs
5. Review this checklist

**Ready to deploy! 🚀**
