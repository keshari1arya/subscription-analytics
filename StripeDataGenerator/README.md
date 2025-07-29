# Stripe Data Generator

A standalone C# console application that generates realistic SaaS business data in your Stripe connected account.

## 🎯 Features

- **2000+ Customers** with realistic names, emails, and addresses
- **10000+ Subscriptions** spread over 5 years
- **3 SaaS Products** with multiple pricing tiers
- **US-based data** with USD currency
- **Realistic business patterns** including cancellations and upgrades

## 📦 Products Generated

### 1. ProjectFlow Pro
- **Basic**: $29/month (5 users)
- **Pro**: $79/month (25 users)
- **Enterprise**: $199/month (unlimited users)

### 2. DataInsight Analytics
- **Starter**: $49/month (3 users)
- **Professional**: $129/month (10 users)
- **Enterprise**: $299/month (unlimited users)

### 3. SecureCloud Backup
- **Basic**: $19/month (1 user)
- **Professional**: $59/month (5 users)
- **Enterprise**: $149/month (unlimited users)

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- Stripe CLI (for verification)

### Installation

1. **Navigate to the project directory:**
   ```bash
   cd StripeDataGenerator
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the project:**
   ```bash
   dotnet build
   ```

4. **Run the data generator:**
   ```bash
   dotnet run
   ```

## ⚙️ Configuration

Edit `appsettings.json` to customize the data generation:

```json
{
  "Stripe": {
    "ApiKey": "",
    "ConnectedAccountId": "your_connected_account_id"
  },
  "DataGeneration": {
    "CustomerCount": 2500,
    "SubscriptionCount": 12000,
    "StartDate": "2020-01-01",
    "EndDate": "2025-01-01"
  }
}
```

### Environment Variables

Set your Stripe API key as an environment variable:

```bash
export STRIPE_API_KEY="your_stripe_test_api_key"
```

Or run with the environment variable:

```bash
STRIPE_API_KEY="your_stripe_test_api_key" dotnet run
```

## 📊 Data Distribution

### Customer Generation
- **Realistic names** using Bogus library
- **Valid email addresses** with proper domains
- **US addresses** with proper formatting
- **Company information** and industry tags
- **Phone numbers** in US format

### Subscription Patterns
- **70% monthly** vs **30% yearly** subscriptions
- **15% cancellation rate** for realistic churn
- **Random quantities** (1-5) per subscription
- **Spread over 5 years** for historical analysis

### Business Intelligence
- **Industry distribution** across Technology, Healthcare, Finance, Education, Retail, Manufacturing
- **Geographic distribution** across US states
- **Company size variation** through subscription quantities
- **Realistic pricing** with proper decimal handling

## 🔍 Verification

After running the generator, verify the data in your Stripe dashboard:

```bash
# List customers
stripe customers list --limit 10

# List subscriptions
stripe subscriptions list --limit 10

# List products
stripe products list

# List prices
stripe prices list
```

## 🛡️ Safety Features

- **Test mode only** - Uses your test API key
- **Error handling** - Continues on individual failures
- **Progress logging** - Shows real-time progress
- **Validation** - Ensures data quality

## 📈 Expected Results

After running the generator, you should see:

- **~2500 customers** with complete profiles
- **~12000 subscriptions** with realistic patterns
- **9 products** (3 products × 3 tiers each)
- **18 prices** (monthly and yearly for each tier)
- **Realistic revenue data** for analytics

## 🔧 Troubleshooting

### Common Issues

1. **API Key Error**
   - Ensure your Stripe API key is correct
   - Verify it's a test key (starts with `sk_test_`)

2. **Connected Account Error**
   - Verify your connected account ID
   - Ensure the account is active

3. **Rate Limiting**
   - The script includes delays to respect Stripe's rate limits
   - If you encounter rate limits, wait and retry

### Logs

The application provides detailed logging:
- **Information level**: Progress updates
- **Warning level**: Non-critical errors
- **Error level**: Critical failures

## 📝 Notes

- **Test Data Only**: This generates test data in your Stripe test environment
- **No Production Impact**: Safe to run multiple times
- **Standalone**: No dependencies on your main project
- **Configurable**: Easy to modify for different scenarios

## 🎯 Use Cases

- **Development Testing**: Generate realistic data for development
- **Analytics Testing**: Test your analytics dashboard with varied data
- **Demo Purposes**: Create impressive demos with substantial data
- **Performance Testing**: Test your application with large datasets
