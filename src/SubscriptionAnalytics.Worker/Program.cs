using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SubscriptionAnalytics.Worker;
using SubscriptionAnalytics.Worker.Services;
using SubscriptionAnalytics.Shared.Enums;
using System.Text.Json;

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

// Add standalone sync worker
services.AddScoped<StandaloneSyncWorker>();

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
    // Local development mode - run continuous job processor
    Console.WriteLine("🔄 Starting Continuous Job Processor...");
    Console.WriteLine("==========================================");

    using var scope = serviceProvider.CreateScope();
    var syncWorker = scope.ServiceProvider.GetRequiredService<StandaloneSyncWorker>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var pollingInterval = TimeSpan.FromSeconds(10); // Check every 10 seconds
    var isRunning = true;

    Console.WriteLine($"📊 Polling interval: {pollingInterval.TotalSeconds} seconds");
    Console.WriteLine("⏹️  Press Ctrl+C to stop the worker");
    Console.WriteLine("");

    // Handle graceful shutdown
    Console.CancelKeyPress += (sender, e) =>
    {
        e.Cancel = true;
        isRunning = false;
        Console.WriteLine("\n🛑 Shutting down gracefully...");
    };

    try
    {
        while (isRunning)
        {
            try
            {
                // Check for pending jobs
                var pendingJobs = await syncWorker.GetPendingJobsAsync();

                if (pendingJobs.Any())
                {
                    Console.WriteLine($"🔍 Found {pendingJobs.Count()} pending job(s)");

                    foreach (var job in pendingJobs)
                    {
                        Console.WriteLine($"⚡ Processing job {job.Id} for tenant {job.TenantId}, provider {job.ProviderName}");

                        try
                        {
                            var result = await syncWorker.ProcessSyncJobAsync(job);

                            if (result.Status == SyncJobStatus.Completed)
                            {
                                Console.WriteLine($"✅ Job {job.Id} completed successfully! Progress: {result.Progress}%");
                            }
                            else if (result.Status == SyncJobStatus.Failed)
                            {
                                Console.WriteLine($"❌ Job {job.Id} failed: {result.ErrorMessage}");
                            }
                            else if (result.Status == SyncJobStatus.Cancelled)
                            {
                                Console.WriteLine($"⏹️  Job {job.Id} was cancelled");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"💥 Error processing job {job.Id}: {ex.Message}");
                            logger.LogError(ex, "Error processing job {JobId}", job.Id);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("😴 No pending jobs found, waiting...");
                }

                // Wait before next poll
                await Task.Delay(pollingInterval);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Error in job polling loop: {ex.Message}");
                logger.LogError(ex, "Error in job polling loop");
                await Task.Delay(TimeSpan.FromSeconds(30)); // Wait longer on error
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 Fatal error: {ex.Message}");
        logger.LogError(ex, "Fatal error in worker");
    }

    Console.WriteLine("👋 Worker stopped");
}
