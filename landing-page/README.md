# 🚀 SubscriptionAnalytics Landing Page

A modern, responsive landing page for SubscriptionAnalytics with server-side functionality and database persistence.

## ✨ Features

### **Frontend**
- 🎨 Modern, responsive design with Tailwind CSS
- ⚡ Next.js 15 with App Router
- 🎭 Smooth animations with Framer Motion
- 📱 Mobile-first responsive design
- 🎯 SEO optimized with meta tags
- 📊 Google Analytics integration ready

### **Backend**
- 🗄️ PostgreSQL database with Prisma ORM
- ✅ Form validation with Zod
- 🔄 Real-time database persistence
- 🛡️ Duplicate prevention for waitlist
- 📈 Health monitoring endpoint
- 📊 Analytics tracking ready

### **API Endpoints**
```bash
POST /api/waitlist     # Join waitlist with validation
GET  /api/waitlist     # Get signup count
POST /api/contact      # Submit contact form
GET  /api/contact      # Get submission count
GET  /api/health       # Health check with stats
```

## 🛠️ Tech Stack

- **Frontend**: Next.js 15, React 19, TypeScript
- **Styling**: Tailwind CSS 4, Framer Motion
- **Database**: PostgreSQL with Prisma ORM
- **Validation**: Zod
- **Deployment**: Vercel
- **CI/CD**: GitHub Actions

## 🚀 Quick Start

### **1. Clone Repository**
```bash
git clone https://github.com/your-username/subscriptionanalytics.git
cd landing-page
```

### **2. Install Dependencies**
```bash
yarn install
```

### **3. Environment Setup**
```bash
# Copy environment file
cp env.example .env

# Update with your database URL
DATABASE_URL="postgresql://landingpage_straightat:271238ae708062d3a7fd7a9c695231ebed9f25f1@ia56i9.h.filess.io:5434/landingpage_straightat"
```

### **4. Database Setup**
```bash
# Generate Prisma client
npx prisma generate

# Push schema to database
npx prisma db push
```

### **5. Start Development Server**
```bash
yarn dev
```

Visit [http://localhost:3000](http://localhost:3000) to see the application.

## 🗄️ Database Schema

### **Waitlist Entries**
```sql
CREATE TABLE waitlist_entries (
  id TEXT PRIMARY KEY,
  email TEXT UNIQUE NOT NULL,
  created_at TIMESTAMP DEFAULT NOW(),
  updated_at TIMESTAMP DEFAULT NOW(),
  status TEXT DEFAULT 'pending',
  source TEXT,
  metadata JSONB
);
```

### **Contact Submissions**
```sql
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

## 🧪 Testing

### **Health Check**
```bash
curl http://localhost:3000/api/health
```

### **Waitlist Signup**
```bash
curl -X POST http://localhost:3000/api/waitlist \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com"}'
```

### **Contact Form**
```bash
curl -X POST http://localhost:3000/api/contact \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "message": "Test message"
  }'
```

## 🚀 Deployment

### **Vercel Deployment**
1. Connect GitHub repository to Vercel
2. Add environment variables:
   - `DATABASE_URL`
   - `NEXT_PUBLIC_API_URL`
   - `NEXT_PUBLIC_GA_MEASUREMENT_ID`
3. Deploy automatically on push to main branch

**Note**: The build process includes `prisma generate` to ensure the Prisma client is properly generated for Vercel deployment.

### **Domain Setup**
- Configure custom domain: `branddsync.info`
- Set up DNS records for Vercel
- SSL certificate will be auto-generated

## 📊 Monitoring

### **Health Endpoint**
```bash
GET /api/health
```
Returns application status and database statistics.

### **Analytics**
- Google Analytics integration ready
- Form submission tracking
- Conversion rate monitoring

## 🔧 Development

### **Available Scripts**
```bash
yarn dev          # Start development server
yarn build        # Build for production (includes prisma generate)
yarn start        # Start production server
yarn lint         # Run ESLint
```

### **Database Commands**
```bash
npx prisma generate    # Generate Prisma client
npx prisma db push     # Push schema to database
npx prisma studio      # Open database GUI
```

## 📁 Project Structure

```
landing-page/
├── src/
│   ├── app/
│   │   ├── api/              # API routes
│   │   │   ├── waitlist/     # Waitlist endpoint
│   │   │   ├── contact/      # Contact form endpoint
│   │   │   └── health/       # Health check endpoint
│   │   ├── components/       # React components
│   │   └── lib/              # Utilities
│   │       ├── prisma.ts     # Database client
│   │       └── validation.ts # Zod schemas
├── prisma/
│   └── schema.prisma         # Database schema
├── .github/
│   └── workflows/            # CI/CD pipeline
└── public/                   # Static assets
```

## 🎯 Business Features

### **Lead Generation**
- Email capture for waitlist
- Contact form for inquiries
- Meeting scheduling integration
- Analytics tracking

### **User Experience**
- Fast loading times
- Mobile responsive design
- Smooth animations
- Clear call-to-actions

### **Analytics & Tracking**
- Form submission tracking
- Page view analytics
- Conversion rate monitoring
- User behavior insights

## 🔒 Security

- Input validation with Zod
- SQL injection prevention (Prisma)
- Rate limiting ready
- Secure database connections
- Environment variable protection

## 📈 Performance

- Next.js 15 with App Router
- Optimized images and assets
- CDN distribution via Vercel
- Database connection pooling
- Efficient API responses

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 📞 Support

For questions or issues:
1. Check the [deployment guide](./DEPLOYMENT.md)
2. Review API documentation
3. Check database connectivity
4. Contact the development team

---

**Built with ❤️ for SubscriptionAnalytics**
