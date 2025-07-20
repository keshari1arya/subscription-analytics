using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SubscriptionAnalytics.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;

namespace SubscriptionAnalytics.Api.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            // Seed test user as before
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var testUser = new IdentityUser {
                Id = "test-user-id-123",
                UserName = "TestUser",
                Email = "TestUser@example.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true
            };
            var existing = userManager.FindByNameAsync(testUser.UserName).GetAwaiter().GetResult();
            if (existing == null)
            {
                userManager.CreateAsync(testUser, "TestPassword123!").GetAwaiter().GetResult();
            }

            // Remove all authentication handlers and add only the test handler
            services.AddAuthentication(TestAuthHandler.Scheme)
                .AddScheme<AuthenticationSchemeOptions, SubscriptionAnalytics.Api.TestAuthHandler>(TestAuthHandler.Scheme, options => { });
        });
    }
} 