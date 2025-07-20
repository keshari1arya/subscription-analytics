using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SubscriptionAnalytics.Api.IntegrationTests;

public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // Helper to get an authenticated client (simulate Admin or User)
    private HttpClient GetAuthenticatedClient(string role = "Admin")
    {
        var client = _factory.CreateClient();
        // Simulate JWT or cookie auth as needed. For now, set a fake header.
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", role);
        return client;
    }

    private async Task<HttpClient> RegisterAndLoginUserAsync(string email, string password, string role = null)
    {
        var client = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        // Register
        var registerPayload = new { Email = email, Password = password };
        var registerResponse = await client.PostAsJsonAsync("/identity/register", registerPayload);
        registerResponse.EnsureSuccessStatusCode();
        // Login
        var loginPayload = new { Email = email, Password = password };
        var loginResponse = await client.PostAsJsonAsync("/identity/login", loginPayload);
        loginResponse.EnsureSuccessStatusCode();
        // If using JWT, extract and set Authorization header here
        // If using cookies, HttpClientHandler will handle it
        return client;
    }

    [Fact]
    public async Task CreateTenant_ShouldReturnCreated_WhenAdminAndValidRequest()
    {
        var client = GetAuthenticatedClient("Admin");
        var request = new { Name = "TestTenant" };
        var response = await client.PostAsJsonAsync("/api/tenant", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("TestTenant");
    }

    [Fact]
    public async Task CreateTenant_ShouldReturnUnauthorized_WhenNoAuth()
    {
        var client = _factory.CreateClient();
        var request = new { Name = "TestTenant" };
        var response = await client.PostAsJsonAsync("/api/tenant", request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateTenant_ShouldReturnForbidden_WhenNotAdmin()
    {
        var client = GetAuthenticatedClient("User");
        var request = new { Name = "TestTenant" };
        var response = await client.PostAsJsonAsync("/api/tenant", request);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CreateTenant_ShouldReturnBadRequest_WhenInvalidInput()
    {
        var client = GetAuthenticatedClient("Admin");
        var request = new { Name = "" }; // Invalid: Name required
        var response = await client.PostAsJsonAsync("/api/tenant", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnOk_WhenAuthenticated()
    {
        var client = GetAuthenticatedClient();
        var response = await client.GetAsync("/api/customer");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("john.doe@example.com");
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnUnauthorized_WhenNoAuth()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/customer");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetTenant_ShouldReturnOk_WhenValidIdAndAuthenticated()
    {
        var client = GetAuthenticatedClient();
        // First, create a tenant to get a valid ID
        var createRequest = new { Name = "TestTenant2" };
        var createResponse = await client.PostAsJsonAsync("/api/tenant", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var jsonString = await createResponse.Content.ReadAsStringAsync();
        using var doc = System.Text.Json.JsonDocument.Parse(jsonString);
        string id = doc.RootElement.GetProperty("id").GetString();

        var response = await client.GetAsync($"/api/tenant/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("TestTenant2");
    }

    [Fact]
    public async Task GetTenant_ShouldReturnNotFound_WhenNonExistentId()
    {
        var client = GetAuthenticatedClient();
        var response = await client.GetAsync($"/api/tenant/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTenant_ShouldReturnUnauthorized_WhenNoAuth()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/api/tenant/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllTenants_ShouldReturnOk_WhenAdmin()
    {
        var client = GetAuthenticatedClient("Admin");
        // Create a tenant to ensure there is at least one
        var createRequest = new { Name = "AllTenantsTest" };
        await client.PostAsJsonAsync("/api/tenant", createRequest);
        var response = await client.GetAsync("/api/tenant");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("AllTenantsTest");
    }

    [Fact]
    public async Task GetAllTenants_ShouldReturnUnauthorized_WhenNoAuth()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/tenant");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllTenants_ShouldReturnForbidden_WhenNotAdmin()
    {
        var client = GetAuthenticatedClient("User");
        var response = await client.GetAsync("/api/tenant");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AssignUserToTenant_ShouldReturnOk_WhenAdminAndValidRequest()
    {
        var client = GetAuthenticatedClient("Admin");
        // Create a tenant to assign a user to
        var createTenant = new { Name = "AssignUserTest" };
        var createResponse = await client.PostAsJsonAsync("/api/tenant", createTenant);
        createResponse.EnsureSuccessStatusCode();
        var jsonString = await createResponse.Content.ReadAsStringAsync();
        using var doc = System.Text.Json.JsonDocument.Parse(jsonString);
        string tenantId = doc.RootElement.GetProperty("id").GetString();
        var request = new { UserEmail = "user@example.com", TenantId = tenantId, Role = "User" };
        var response = await client.PostAsJsonAsync("/api/tenant/assign-user", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseJson = await response.Content.ReadAsStringAsync();
        responseJson.Should().Contain("user@example.com");
    }

    [Fact]
    public async Task AssignUserToTenant_ShouldReturnBadRequest_WhenInvalidInput()
    {
        var client = GetAuthenticatedClient("Admin");
        var request = new { UserEmail = "", TenantId = "", Role = "" }; // Invalid
        var response = await client.PostAsJsonAsync("/api/tenant/assign-user", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AssignUserToTenant_ShouldReturnUnauthorized_WhenNoAuth()
    {
        var client = _factory.CreateClient();
        var request = new { UserEmail = "user@example.com", TenantId = Guid.NewGuid().ToString(), Role = "User" };
        var response = await client.PostAsJsonAsync("/api/tenant/assign-user", request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AssignUserToTenant_ShouldReturnForbidden_WhenNotAdmin()
    {
        var client = GetAuthenticatedClient("User");
        var request = new { UserEmail = "user@example.com", TenantId = Guid.NewGuid().ToString(), Role = "User" };
        var response = await client.PostAsJsonAsync("/api/tenant/assign-user", request);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    // TODO: Add a proper integration test for /api/tenant/my-tenants using real Identity registration and login flow.
} 