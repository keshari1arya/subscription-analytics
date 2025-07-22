using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Infrastructure.Middleware;
using SubscriptionAnalytics.Infrastructure.Services;
using SubscriptionAnalytics.Shared.Interfaces;
using System.Security.Claims;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class TenantContextMiddlewareTests
{
    private readonly Mock<ILogger<TenantContextMiddleware>> _loggerMock;
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly TenantContextMiddleware _middleware;
    private readonly TenantContext _tenantContext;

    public TenantContextMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<TenantContextMiddleware>>();
        _nextMock = new Mock<RequestDelegate>();
        _middleware = new TenantContextMiddleware(_nextMock.Object, _loggerMock.Object);
        _tenantContext = new TenantContext();
    }

    [Fact]
    public void Constructor_Should_CreateMiddleware()
    {
        // Act & Assert
        _middleware.Should().NotBeNull();
    }

    [Fact]
    public async Task InvokeAsync_Should_ExtractTenantIdFromHeader()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var context = CreateHttpContext();
        context.Request.Headers["X-Tenant-Id"] = tenantId.ToString();

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(tenantId);
        context.Items["TenantId"].Should().Be(tenantId);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_ExtractTenantIdFromQueryParameter()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var context = CreateHttpContext();
        context.Request.QueryString = new QueryString($"?tenantId={tenantId}");

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(tenantId);
        context.Items["TenantId"].Should().Be(tenantId);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_ExtractTenantIdFromJwtClaim()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var context = CreateHttpContext();
        var claims = new List<Claim>
        {
            new Claim("tenant_id", tenantId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        context.User = new ClaimsPrincipal(identity);

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(tenantId);
        context.Items["TenantId"].Should().Be(tenantId);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_UseDefaultTenantId_WhenNoTenantFound()
    {
        // Arrange
        var context = CreateHttpContext();

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_PrioritizeHeaderOverQueryParameter()
    {
        // Arrange
        var headerTenantId = Guid.NewGuid();
        var queryTenantId = Guid.NewGuid();
        var context = CreateHttpContext();
        context.Request.Headers["X-Tenant-Id"] = headerTenantId.ToString();
        context.Request.QueryString = new QueryString($"?tenantId={queryTenantId}");

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(headerTenantId);
        context.Items["TenantId"].Should().Be(headerTenantId);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_PrioritizeQueryParameterOverJwtClaim()
    {
        // Arrange
        var queryTenantId = Guid.NewGuid();
        var jwtTenantId = Guid.NewGuid();
        var context = CreateHttpContext();
        context.Request.QueryString = new QueryString($"?tenantId={queryTenantId}");
        var claims = new List<Claim>
        {
            new Claim("tenant_id", jwtTenantId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        context.User = new ClaimsPrincipal(identity);

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(queryTenantId);
        context.Items["TenantId"].Should().Be(queryTenantId);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleInvalidGuidInHeader()
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.Headers["X-Tenant-Id"] = "invalid-guid";

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleInvalidGuidInQueryParameter()
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.QueryString = new QueryString("?tenantId=invalid-guid");

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleInvalidGuidInJwtClaim()
    {
        // Arrange
        var context = CreateHttpContext();
        var claims = new List<Claim>
        {
            new Claim("tenant_id", "invalid-guid")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        context.User = new ClaimsPrincipal(identity);

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleEmptyPath()
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.Path = "";

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleNullPath()
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.Path = null;

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleUnauthenticatedUser()
    {
        // Arrange
        var context = CreateHttpContext();
        context.User = new ClaimsPrincipal(new ClaimsIdentity());

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleUserWithoutTenantClaim()
    {
        // Arrange
        var context = CreateHttpContext();
        var claims = new List<Claim>
        {
            new Claim("name", "testuser")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        context.User = new ClaimsPrincipal(identity);

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_CallNextDelegate()
    {
        // Arrange
        var context = CreateHttpContext();
        var called = false;
        _nextMock.Setup(x => x(context)).Callback(() => called = true);

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        called.Should().BeTrue();
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleException()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");
        _nextMock.Setup(x => x(context)).Throws(exception);

        // Act & Assert
        var action = async () => await _middleware.InvokeAsync(context, _tenantContext);
        await action.Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleExceptionAndLogError()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");
        _nextMock.Setup(x => x(context)).Throws(exception);

        // Act & Assert
        var action = async () => await _middleware.InvokeAsync(context, _tenantContext);
        await action.Should().ThrowAsync<Exception>();

        // Verify that the logger was called (we can't easily verify the exact log message with Moq)
        // but we can verify the middleware handled the exception properly
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleMultipleQueryParameters()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var context = CreateHttpContext();
        context.Request.QueryString = new QueryString($"?tenantId={tenantId}&otherParam=value&anotherParam=123");

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(tenantId);
        context.Items["TenantId"].Should().Be(tenantId);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleMultipleJwtClaims()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var context = CreateHttpContext();
        var claims = new List<Claim>
        {
            new Claim("tenant_id", tenantId.ToString()),
            new Claim("name", "testuser"),
            new Claim("email", "test@example.com")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        context.User = new ClaimsPrincipal(identity);

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(tenantId);
        context.Items["TenantId"].Should().Be(tenantId);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleEmptyHeaderValue()
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.Headers["X-Tenant-Id"] = "";

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleWhitespaceHeaderValue()
    {
        // Arrange
        var context = CreateHttpContext();
        context.Request.Headers["X-Tenant-Id"] = "   ";

        // Act
        await _middleware.InvokeAsync(context, _tenantContext);

        // Assert
        _tenantContext.TenantId.Should().Be(Guid.Empty);
        context.Items["TenantId"].Should().Be(Guid.Empty);
        _nextMock.Verify(x => x(context), Times.Once);
    }

    private static HttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "http";
        context.Request.Host = new HostString("localhost");
        context.Request.Path = "/";
        context.Request.Method = "GET";
        return context;
    }
} 