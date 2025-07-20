using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Api.Controllers;
using Xunit;

namespace SubscriptionAnalytics.Api.Tests;

public class TestControllerTests
{
    private readonly Mock<ILogger<TestController>> _loggerMock = new();
    private readonly TestController _controller;

    public TestControllerTests()
    {
        _controller = new TestController(_loggerMock.Object);
    }

    [Fact]
    public void Get_Should_Return_ApiStatus_WithUser()
    {
        // Arrange
        var userName = "testuser";
        var httpContext = new DefaultHttpContext();
        httpContext.User = new System.Security.Claims.ClaimsPrincipal(
            new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, userName)
            }, "mock"));
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        // Act
        var result = _controller.Get() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Message.Should().Be("API is working!");
        value.User.Should().Be(userName);
    }

    [Fact]
    public void Get_Should_Return_Anonymous_IfNoUser()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Get() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.User.Should().Be("Anonymous");
    }

    [Fact]
    public void Post_Should_Echo_Data()
    {
        // Arrange
        var data = new { foo = "bar" };
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Post(data) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Message.Should().Be("POST request received");
        value.Data.Should().BeEquivalentTo(data);
    }
} 