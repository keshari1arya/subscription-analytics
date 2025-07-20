using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;

namespace SubscriptionAnalytics.Worker;

public interface ILambdaWorker
{
    Task<Stream> ExecuteAsync(Stream inputStream, ILambdaContext context);
}

public class LambdaWorker : ILambdaWorker
{
    private readonly ILogger<LambdaWorker> _logger;

    public LambdaWorker(ILogger<LambdaWorker> logger)
    {
        _logger = logger;
    }

    public async Task<Stream> ExecuteAsync(Stream inputStream, ILambdaContext context)
    {
        try
        {
            _logger.LogInformation("Lambda Worker started at: {time}", DateTimeOffset.Now);
            _logger.LogInformation("Lambda function name: {functionName}", context.FunctionName);
            _logger.LogInformation("Lambda function version: {functionVersion}", context.FunctionVersion);
            _logger.LogInformation("Lambda request ID: {requestId}", context.AwsRequestId);

            // TODO: Add your sync/analytics logic here
            // This could include:
            // - Loading plugins
            // - Running scheduled sync jobs
            // - Processing analytics
            // - Database operations

            _logger.LogInformation("Lambda Worker completed successfully");

            // Return empty response stream
            var responseStream = new MemoryStream();
            await responseStream.FlushAsync();
            responseStream.Position = 0;
            return responseStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Lambda Worker");
            throw;
        }
    }
}
