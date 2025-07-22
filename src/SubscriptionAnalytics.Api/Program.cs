using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Infrastructure.Middleware;
using SubscriptionAnalytics.Infrastructure.Services;
using SubscriptionAnalytics.Shared.Interfaces;
using SubscriptionAnalytics.Api.Middleware;
using SubscriptionAnalytics.Connectors.Stripe.Abstractions;
using SubscriptionAnalytics.Connectors.Stripe.Services;
using SubscriptionAnalytics.Connectors.PayPal.Abstractions;
using SubscriptionAnalytics.Connectors.PayPal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SubscriptionAnalytics API",
        Version = "v1",
        Description = "API for Subscription Analytics with multi-tenant support and plugin-based connectors"
    });

    // Add authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Identity API token. Example: \"{token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity Core and Roles
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Configure Identity API
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();

// Configure Authorization
builder.Services.AddAuthorization();

var isTestEnv = builder.Environment.EnvironmentName == "Test";

// Remove test handler registration for Identity scheme in test environment

// Register services
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<IProviderConnectionService, ProviderConnectionService>();

// Connector Services
builder.Services.AddScoped<IStripeConnector, StripeConnector>();
builder.Services.AddScoped<IPayPalConnector, PayPalConnector>();
builder.Services.AddScoped<IConnectorFactory, ConnectorFactory>();

// Legacy Services (for backward compatibility)
builder.Services.AddScoped<IStripeInstallationService, StripeInstallationService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SubscriptionAnalytics API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Add global exception handler middleware (must be first)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Add tenant context middleware
app.UseMiddleware<TenantContextMiddleware>();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Identity API endpoints
var group = app.MapGroup("/api/identity");  
group.MapIdentityApi<IdentityUser>();

app.MapControllers();

// Add some basic endpoints
app.MapGet("/", () => "SubscriptionAnalytics API is running! 🚀");

app.MapGet("/health", () => new { 
    Status = "Healthy", 
    Timestamp = DateTime.UtcNow,
    Version = "1.0.0"
});

app.Run();

// Add this for integration testing
public partial class Program { }