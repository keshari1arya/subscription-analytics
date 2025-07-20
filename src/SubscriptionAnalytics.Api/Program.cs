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
using Microsoft.AspNetCore.Authentication;
using SubscriptionAnalytics.Api;
using SubscriptionAnalytics.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;

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

var app = builder.Build();

// Seed roles and default AppAdmin user at startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string[] roles = new[]
    {
        Roles.AppAdmin,
        Roles.TenantAdmin,
        Roles.TenantUser,
        Roles.SupportUser,
        Roles.ReadOnlyUser
    };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    // Seed default AppAdmin user
    var adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var password = "Admin!" + Guid.NewGuid().ToString("N").Substring(0, 8) + "!";
        var result = await userManager.CreateAsync(admin, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, Roles.AppAdmin);
            Console.WriteLine($"Seeded AppAdmin user: {adminEmail} with password: {password}");
        }
        else
        {
            Console.WriteLine($"Failed to seed AppAdmin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

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
app.MapIdentityApi<IdentityUser>();

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