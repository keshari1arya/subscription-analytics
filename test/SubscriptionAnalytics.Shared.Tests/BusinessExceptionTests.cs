using FluentAssertions;
using SubscriptionAnalytics.Shared.Exceptions;
using Xunit;

namespace SubscriptionAnalytics.Shared.Tests;

public class BusinessExceptionTests
{
    [Fact]
    public void Should_Set_Message_And_ErrorCode()
    {
        var ex = new BusinessException("msg", "ERR");
        ex.Message.Should().Be("msg");
        ex.ErrorCode.Should().Be("ERR");
    }

    [Fact]
    public void NotFoundException_Should_Set_ErrorCode_And_Message()
    {
        var ex = new NotFoundException("not found");
        ex.ErrorCode.Should().Be("NOT_FOUND");
        ex.Message.Should().Be("not found");
    }

    [Fact]
    public void NotFoundException_EntityCtor_Should_Format_Message()
    {
        var ex = new NotFoundException("Entity", 42);
        ex.Message.Should().Contain("Entity with ID 42 was not found");
        ex.ErrorCode.Should().Be("NOT_FOUND");
    }

    [Fact]
    public void ValidationException_Should_Set_ValidationErrors()
    {
        var errors = new Dictionary<string, string[]> { { "field", new[] { "err" } } };
        var ex = new ValidationException("validation failed", errors);
        ex.ErrorCode.Should().Be("VALIDATION_ERROR");
        ex.ValidationErrors.Should().BeSameAs(errors);
    }

    [Fact]
    public void UnauthorizedException_Should_Set_Defaults()
    {
        var ex = new UnauthorizedException();
        ex.ErrorCode.Should().Be("UNAUTHORIZED");
        ex.Message.Should().Be("Unauthorized access");
    }

    [Fact]
    public void ForbiddenException_Should_Set_Defaults()
    {
        var ex = new ForbiddenException();
        ex.ErrorCode.Should().Be("FORBIDDEN");
        ex.Message.Should().Be("Access forbidden");
    }
} 