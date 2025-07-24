namespace SubscriptionAnalytics.Api.Configuration;

public class OAuthConfiguration
{
    public string UiCallbackUrl { get; set; } = string.Empty;
}

public static class ApiConfiguration 
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OAuthConfiguration>(configuration.GetSection("OAuth"));
        return services;
    }
} 