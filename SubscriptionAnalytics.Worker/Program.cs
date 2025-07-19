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
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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

// Start the Lambda runtime
await LambdaBootstrapBuilder.Create(FunctionHandler, new DefaultLambdaJsonSerializer())
    .Build()
    .RunAsync();
