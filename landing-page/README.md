# SubscriptionAnalytics Landing Page

A modern, SEO-optimized landing page for SubscriptionAnalytics - a unified subscription intelligence platform.

## 🚀 Features

- **SEO Optimized**: Comprehensive meta tags, Open Graph, Twitter Cards
- **Modern Design**: Clean, professional design with smooth animations
- **Responsive**: Mobile-first design that works on all devices
- **Performance**: Optimized with Next.js 15 and Tailwind CSS
- **Accessibility**: WCAG compliant with proper semantic HTML
- **Analytics Ready**: Structured for conversion tracking

## 📋 Sections

1. **Hero Section** - Compelling headline with clear value proposition
2. **Problem Section** - Highlights pain points of current solutions
3. **Solution Section** - Showcases unique benefits and features
4. **Pricing Section** - Transparent pricing with three tiers
5. **Waitlist Section** - Email capture with social proof
6. **Contact Section** - Contact form and information
7. **Footer** - Links and company information

## 🛠️ Tech Stack

- **Next.js 15** - React framework with App Router
- **TypeScript** - Type-safe development
- **Tailwind CSS** - Utility-first CSS framework
- **Framer Motion** - Smooth animations and transitions
- **Lucide React** - Beautiful icons
- **ESLint** - Code quality and consistency

## 🚀 Getting Started

### Prerequisites

- Node.js 18+
- Yarn package manager

### Installation

```bash
# Install dependencies
yarn install

# Start development server
yarn dev

# Build for production
yarn build

# Start production server
yarn start
```

The development server will start at `http://localhost:3000`

## 📊 SEO Features

- Comprehensive meta tags
- Open Graph and Twitter Card support
- Structured data markup
- Semantic HTML structure
- Fast loading times
- Mobile-friendly design

## 🎨 Design System

- **Colors**: Blue primary (#2563eb), with supporting grays
- **Typography**: Inter font family
- **Spacing**: Consistent 8px grid system
- **Animations**: Smooth Framer Motion transitions
- **Components**: Reusable, accessible components

## 📱 Responsive Design

- Mobile-first approach
- Breakpoints: sm (640px), md (768px), lg (1024px), xl (1280px)
- Touch-friendly interactions
- Optimized for all screen sizes

## 🔧 Customization

### Content Updates

All content is easily customizable in the component files:
- `src/app/components/Hero.tsx` - Main headline and CTA
- `src/app/components/ProblemSection.tsx` - Pain points
- `src/app/components/SolutionSection.tsx` - Benefits
- `src/app/components/PricingSection.tsx` - Pricing tiers
- `src/app/components/WaitlistSection.tsx` - Email capture

### Styling

- Tailwind CSS classes for styling
- Custom CSS in `src/app/globals.css`
- Component-specific styles in each component

### SEO Configuration

Update SEO settings in `src/app/layout.tsx`:
- Meta tags
- Open Graph data
- Twitter Cards
- Canonical URLs

## 📈 Analytics Integration

Ready for integration with:
- Google Analytics
- Google Tag Manager
- Facebook Pixel
- LinkedIn Insight Tag
- Custom conversion tracking

## 🚀 Deployment

### Vercel (Recommended)

```bash
# Install Vercel CLI
npm i -g vercel

# Deploy
vercel
```

### Netlify

```bash
# Build the project
yarn build

# Deploy to Netlify
netlify deploy --prod --dir=out
```

### Other Platforms

The project can be deployed to any static hosting platform:
- AWS S3 + CloudFront
- Google Cloud Storage
- Azure Static Web Apps
- GitHub Pages

## 📞 Support

For questions or support:
- Email: hello@subscriptionanalytics.com
- Documentation: [Coming Soon]
- Issues: [GitHub Issues]

## 📄 License

This project is proprietary software. All rights reserved.

---

Built with ❤️ for subscription businesses
