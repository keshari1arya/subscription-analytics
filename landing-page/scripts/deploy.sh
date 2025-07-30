#!/bin/bash

echo "🚀 Deploying SubscriptionAnalytics Landing Page to Vercel"

# Check if we're in the right directory
if [ ! -f "package.json" ]; then
    echo "❌ Error: package.json not found. Please run this script from the landing-page directory."
    exit 1
fi

# Build the project
echo "📦 Building project..."
yarn build

if [ $? -ne 0 ]; then
    echo "❌ Build failed. Please fix the errors and try again."
    exit 1
fi

echo "✅ Build completed successfully!"

# Check if Vercel CLI is installed
if ! command -v vercel &> /dev/null; then
    echo "❌ Vercel CLI not found. Please install it first:"
    echo "npm i -g vercel"
    exit 1
fi

# Deploy to Vercel
echo "🌐 Deploying to Vercel..."
vercel --prod

echo "✅ Deployment completed!"
echo "🌍 Your app should be live at the URL provided above."
echo "📊 Check the Vercel dashboard for deployment status and logs."
