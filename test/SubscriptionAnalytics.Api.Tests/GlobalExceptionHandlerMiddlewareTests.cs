using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Moq;
using SubscriptionAnalytics.Api.Middleware;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace SubscriptionAnalytics.Api.Tests;

public class GlobalExceptionHandlerMiddlewareTests
{
    private readonly Mock<ILogger<GlobalExceptionHandlerMiddleware>> _loggerMock;
    private readonly Mock<IWebHostEnvironment> _environmentMock;
    private readonly GlobalExceptionHandlerMiddleware _middleware;

    public GlobalExceptionHandlerMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
        _environmentMock = new Mock<IWebHostEnvironment>();
        _middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);
    }

    [Fact]
    public void Constructor_Should_CreateMiddleware()
    {
        // Act & Assert
        _middleware.Should().NotBeNull();
        _middleware.Should().BeOfType<GlobalExceptionHandlerMiddleware>();
    }

    [Fact]
    public async Task InvokeAsync_Should_CallNextDelegate_WhenNoException()
    {
        // Arrange
        var context = CreateHttpContext();
        var nextCalled = false;

        async Task NextDelegate(HttpContext ctx)
        {
            nextCalled = true;
            await Task.CompletedTask;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleArgumentException()
    {
        // Arrange
        var context = CreateHttpContext();
        var argumentException = new ArgumentException("Invalid argument");

        async Task NextDelegate(HttpContext ctx)
        {
            throw argumentException;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody(context);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Message.Should().Be("Invalid argument");
        errorResponse.Type.Should().Be("INVALID_ARGUMENT");
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleInvalidOperationException()
    {
        // Arrange
        var context = CreateHttpContext();
        var invalidOperationException = new InvalidOperationException("Invalid operation");

        async Task NextDelegate(HttpContext ctx)
        {
            throw invalidOperationException;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody(context);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Message.Should().Be("Invalid operation");
        errorResponse.Type.Should().Be("INVALID_OPERATION");
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleBusinessException()
    {
        // Arrange
        var context = CreateHttpContext();
        var businessException = new BusinessException("Business error", "BUSINESS_ERROR");

        async Task NextDelegate(HttpContext ctx)
        {
            throw businessException;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody(context);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Type.Should().Be("BUSINESS_ERROR");
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleGenericException_InDevelopment()
    {
        // Arrange
        var context = CreateHttpContext();
        var genericException = new Exception("Generic error");
        _environmentMock.Setup(x => x.EnvironmentName).Returns("Development");

        async Task NextDelegate(HttpContext ctx)
        {
            throw genericException;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody(context);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Message.Should().Be("Generic error");
        errorResponse.Type.Should().Be("INTERNAL_SERVER_ERROR");
        errorResponse.Details.Should().NotBeNull();
        errorResponse.Extensions.Should().ContainKey("exceptionType");
        errorResponse.Extensions!["exceptionType"].ToString().Should().Be("Exception");
    }

    [Fact]
    public async Task InvokeAsync_Should_HandleGenericException_InProduction()
    {
        // Arrange
        var context = CreateHttpContext();
        var genericException = new Exception("Generic error");
        _environmentMock.Setup(x => x.EnvironmentName).Returns("Production");

        async Task NextDelegate(HttpContext ctx)
        {
            throw genericException;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");

        var responseBody = await GetResponseBody(context);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Message.Should().Be("An unexpected error occurred");
        errorResponse.Type.Should().Be("INTERNAL_SERVER_ERROR");
        errorResponse.Details.Should().BeNull();
        errorResponse.Extensions.Should().BeNull();
    }

    [Fact]
    public async Task InvokeAsync_Should_LogError()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        async Task NextDelegate(HttpContext ctx)
        {
            throw exception;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Should_IncludeTraceId()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        async Task NextDelegate(HttpContext ctx)
        {
            throw exception;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var responseBody = await GetResponseBody(context);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.TraceId.Should().Be(context.TraceIdentifier);
    }

    [Fact]
    public async Task InvokeAsync_Should_IncludeTimestamp()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");

        async Task NextDelegate(HttpContext ctx)
        {
            throw exception;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var responseBody = await GetResponseBody(context);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Production")]
    public async Task InvokeAsync_Should_FormatJsonBasedOnEnvironment(string environmentName)
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");
        _environmentMock.Setup(x => x.EnvironmentName).Returns(environmentName);

        async Task NextDelegate(HttpContext ctx)
        {
            throw exception;
        }

        var middleware = new GlobalExceptionHandlerMiddleware(NextDelegate, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var responseBody = await GetResponseBody(context);
        
        if (environmentName == "Development")
        {
            responseBody.Should().Contain("\n"); // Should be indented
        }
        else
        {
            responseBody.Should().NotContain("\n"); // Should not be indented
        }
    }

    private static HttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.TraceIdentifier = "test-trace-id";
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<string> GetResponseBody(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        return await reader.ReadToEndAsync();
    }

    private static async Task NextDelegate(HttpContext context)
    {
        await Task.CompletedTask;
    }
} 