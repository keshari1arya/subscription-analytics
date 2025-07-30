# Standalone Sync Worker

This worker is designed to be completely independent and self-contained. It can run independently without depending on the API's database context.

## 🏗️ Architecture

### Key Features:
- **Independent Database Connection**: Creates its own DbContext instance
- **Self-Contained**: No dependencies on API services
- **Configurable**: Uses its own configuration files
- **Scalable**: Can run multiple instances
- **Queue-Ready**: Designed for future queue integration

### Components:

1. **StandaloneSyncWorker**: Main worker class that processes sync jobs
2. **SyncJobRequest**: Input model for sync job requests
3. **SyncJobResult**: Output model for sync job results
4. **Configuration**: Independent appsettings.json files

## 🚀 Running the Worker

### Prerequisites:
1. Update `appsettings.json` with your database connection string
2. Update `appsettings.json` with your Stripe API key
3. Ensure the database is accessible

### Local Development:
```bash
cd src/SubscriptionAnalytics.Worker
dotnet run
```

### Production (Lambda):
The worker is configured to run as an AWS Lambda function when deployed.

## 📋 Configuration

### Database Connection:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=subscriptionanalytics_dev_v5;Username=postgres;Password=yourpassword"
  }
}
```

### Stripe Configuration:
```json
{
  "Stripe": {
    "ApiKey": "sk_test_...",
    "ConnectClientId": "ca_...",
    "ConnectClientSecret": "sk_test_...",
    "WebhookSecret": "whsec_...",
    "PublishableKey": "pk_test_..."
  }
}
```

### Worker Settings:
```json
{
  "Worker": {
    "MaxRetries": 3,
    "RetryDelaySeconds": 30,
    "BatchSize": 100,
    "TimeoutMinutes": 30
  }
}
```

## 🔄 Sync Process

1. **Job Initialization**: Creates a sync job with status tracking
2. **Provider Detection**: Determines which provider to sync (Stripe, PayPal, etc.)
3. **Data Fetching**: Fetches data from the provider's API
4. **Data Processing**: Processes and transforms the data
5. **Database Operations**: Saves data to the database
6. **Progress Tracking**: Updates progress throughout the process
7. **Error Handling**: Handles errors and retries if needed

## 🎯 Current Implementation

### Stripe Sync:
- ✅ Customer sync (fetch and save customers)
- ✅ Incremental updates (create or update existing customers)
- ✅ Progress tracking
- ✅ Error handling
- 🔄 Subscription sync (TODO)
- 🔄 Payment sync (TODO)

### PayPal Sync:
- 🔄 Customer sync (TODO)
- 🔄 Transaction sync (TODO)

## 🔮 Future Enhancements

### Queue Integration:
- AWS SQS
- Azure Service Bus
- RabbitMQ
- Redis Streams

### Advanced Features:
- Multiple worker instances
- Retry logic with exponential backoff
- Dead letter queues for failed jobs
- Monitoring and alerting
- Horizontal scaling
- Real-time progress updates

### Additional Providers:
- PayPal integration
- Square integration
- Shopify integration
- Custom provider support

## 🧪 Testing

### Local Testing:
```bash
# Run the worker locally
dotnet run --project src/SubscriptionAnalytics.Worker

# The worker will process a test sync job
```

### Integration Testing:
```bash
# Test with real data
# Update the tenant ID and access token in Program.cs
dotnet run --project src/SubscriptionAnalytics.Worker
```

## 📊 Monitoring

The worker includes comprehensive logging:
- Job start/completion
- Progress updates
- Error details
- Performance metrics

## 🔒 Security

- Database connections are isolated
- API keys are stored in configuration
- No shared state between workers
- Secure token handling

## 🚀 Deployment

### AWS Lambda:
The worker is configured for AWS Lambda deployment with:
- Lambda runtime support
- Dependency injection
- Configuration management
- Logging integration

### Docker:
```dockerfile
# Example Dockerfile for containerized deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "SubscriptionAnalytics.Worker.dll"]
```

## 📝 Usage Examples

### Basic Usage:
```csharp
var request = new SyncJobRequest
{
    JobId = Guid.NewGuid(),
    TenantId = tenantId,
    ProviderName = "stripe",
    AccessToken = "access_token_here"
};

var result = await syncWorker.ProcessSyncJobAsync(request);
```

### With Additional Data:
```csharp
var request = new SyncJobRequest
{
    JobId = Guid.NewGuid(),
    TenantId = tenantId,
    ProviderName = "stripe",
    AccessToken = "access_token_here",
    AdditionalData = new Dictionary<string, object>
    {
        ["sync_type"] = "full",
        ["batch_size"] = 100,
        ["force_refresh"] = true
    }
};
```

This standalone worker architecture ensures that your sync operations are completely independent, scalable, and reliable!
