using FluentAssertions;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Validation;
using Xunit;

namespace SubscriptionAnalytics.Shared.Tests;

public class CustomerDtoValidatorTests
{
    private readonly CustomerDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_CustomerId_And_ValidEmail()
    {
        var dto = new CustomerDto { CustomerId = "cus_123", Email = "test@example.com" };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_CustomerId_Is_Empty()
    {
        var dto = new CustomerDto { CustomerId = "", Email = "test@example.com" };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "CustomerId");
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var dto = new CustomerDto { CustomerId = "cus_123", Email = "not-an-email" };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Email");
    }

    [Fact]
    public void Should_Pass_When_Email_Is_Empty()
    {
        var dto = new CustomerDto { CustomerId = "cus_123", Email = "" };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }
} 