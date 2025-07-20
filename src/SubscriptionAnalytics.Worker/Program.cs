using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SubscriptionAnalytics.Worker;

// Configure dependency injection
var services = new ServiceCollection();

// Add configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

services.AddSingleton<IConfiguration>(configuration);

// Add logging
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
});

// Add application services
services.AddScoped<ILambdaWorker, LambdaWorker>();

var serviceProvider = services.BuildServiceProvider();

// Create Lambda function handler
async Task<Stream> FunctionHandler(Stream inputStream, ILambdaContext context)
{
    using var scope = serviceProvider.CreateScope();
    var worker = scope.ServiceProvider.GetRequiredService<ILambdaWorker>();
    return await worker.ExecuteAsync(inputStream, context);
}

// Check if running in Lambda environment
if (Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME") != null)
{
    // Start the Lambda runtime
    await LambdaBootstrapBuilder.Create(FunctionHandler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();
}
else
{
    // Local development mode - run a simple test
    Console.WriteLine("Running Worker in local development mode...");
    
    using var scope = serviceProvider.CreateScope();
    var worker = scope.ServiceProvider.GetRequiredService<ILambdaWorker>();
    
    // Create a simple test stream
    var testInput = "{\"test\": \"data\"}";
    var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testInput));
    
    // Create a mock Lambda context
    var context = new MockLambdaContext();
    
    try
    {
        var result = await worker.ExecuteAsync(stream, context);
        Console.WriteLine("Worker executed successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Worker execution failed: {ex.Message}");
    }
    
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

// Mock Lambda context for local development
public class MockLambdaContext : ILambdaContext
{
    public string AwsRequestId => "local-test-request-id";
    public IClientContext ClientContext => null!;
    public string FunctionName => "local-test-function";
    public string FunctionVersion => "local-test-version";
    public ICognitoIdentity Identity => null!;
    public string InvokedFunctionArn => "local-test-arn";
    public ILambdaLogger Logger => new MockLambdaLogger();
    public string LogGroupName => "local-test-log-group";
    public string LogStreamName => "local-test-log-stream";
    public int MemoryLimitInMB => 512;
    public TimeSpan RemainingTime => TimeSpan.FromMinutes(5);
}

public class MockLambdaLogger : ILambdaLogger
{
    public void Log(string message) => Console.WriteLine($"[LAMBDA] {message}");
    public void LogLine(string message) => Console.WriteLine($"[LAMBDA] {message}");
}
