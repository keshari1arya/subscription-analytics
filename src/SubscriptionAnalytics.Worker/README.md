# SubscriptionAnalytics Worker (AWS Lambda)

This project contains the AWS Lambda function for running background jobs and scheduled tasks for the SubscriptionAnalytics application.

## Features

- **AWS Lambda Function**: Serverless background processing
- **Scheduled Execution**: Runs on a configurable schedule via EventBridge
- **Dependency Injection**: Full DI container support
- **Logging**: Integrated with AWS CloudWatch Logs
- **Configuration**: Support for environment-specific settings

## Prerequisites

- .NET 9 SDK
- AWS CLI configured
- AWS SAM CLI (for local testing)

## Building

```bash
# Build the project
dotnet build -c Release

# Publish for Lambda
dotnet publish -c Release -o ./bin/Release/net9.0/
```

## Local Testing

```bash
# Test locally using SAM
sam local invoke SubscriptionAnalyticsWorkerFunction --event events/test-event.json

# Or run the function directly
dotnet run
```

## Deployment

### Using AWS SAM

```bash
# Build and deploy
sam build
sam deploy --guided

# Deploy to specific environment
sam deploy --parameter-overrides Environment=prod
```

### Using AWS CLI

```bash
# Create deployment package
cd ./bin/Release/net9.0/
zip -r ../../../subscription-analytics-worker.zip .

# Deploy using AWS CLI
aws lambda create-function \
  --function-name subscription-analytics-worker-dev \
  --runtime dotnet9 \
  --role arn:aws:iam::YOUR_ACCOUNT:role/lambda-execution-role \
  --handler SubscriptionAnalytics.Worker::SubscriptionAnalytics.Worker.Program::Main \
  --zip-file fileb://subscription-analytics-worker.zip
```

## Configuration

The Lambda function uses the following configuration sources (in order of precedence):

1. Environment variables
2. `appsettings.json`
3. `appsettings.Development.json`

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Environment name (dev/staging/prod)
- `AWS_LAMBDA_FUNCTION_NAME`: Lambda function name
- Database connection strings and other app-specific settings

## Scheduling

The Lambda function is configured to run every hour by default. You can modify the schedule in `serverless.template`:

```json
"Schedule": "rate(1 hour)"
```

Other schedule examples:
- `rate(5 minutes)` - Every 5 minutes
- `rate(1 day)` - Daily
- `cron(0 12 * * ? *)` - Daily at 12:00 UTC

## Monitoring

- **CloudWatch Logs**: All logs are automatically sent to CloudWatch
- **CloudWatch Metrics**: Lambda execution metrics are available
- **X-Ray**: Enable for distributed tracing

## Development

### Adding New Jobs

1. Create a new job class in the `Jobs/` folder
2. Implement the job logic
3. Register the job in the DI container
4. Call the job from the `LambdaWorker.ExecuteAsync` method

### Testing

```bash
# Run unit tests
dotnet test

# Run integration tests
dotnet test --filter Category=Integration
```

## Troubleshooting

### Common Issues

1. **Timeout**: Increase the timeout in `serverless.template`
2. **Memory**: Increase memory allocation if needed
3. **Cold Start**: Consider using provisioned concurrency for critical functions

### Logs

Check CloudWatch Logs for detailed error information:

```bash
aws logs tail /aws/lambda/subscription-analytics-worker-dev --follow
``` 