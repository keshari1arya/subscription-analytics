using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StripeDataGenerator.Services;

namespace StripeDataGenerator;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Stripe Data Generator");
        Console.WriteLine("=========================");
        Console.WriteLine();

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Setup logging
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        services.AddSingleton<IConfiguration>(configuration);
        services.AddTransient<Services.StripeDataGenerator>();

        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Starting Stripe data generation...");

            var dataGenerator = serviceProvider.GetRequiredService<Services.StripeDataGenerator>();
            await dataGenerator.GenerateDataAsync();

            Console.WriteLine();
            Console.WriteLine("✅ Data generation completed successfully!");
            Console.WriteLine("📊 Check your Stripe dashboard to see the generated data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate data");
            Console.WriteLine();
            Console.WriteLine("❌ Data generation failed. Check the logs for details.");
            Environment.Exit(1);
        }
    }
}
