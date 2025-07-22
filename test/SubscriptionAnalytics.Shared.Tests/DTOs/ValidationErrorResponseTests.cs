using FluentAssertions;
using System.Text.Json;
using SubscriptionAnalytics.Shared.DTOs;
using Xunit;

namespace SubscriptionAnalytics.Shared.Tests.DTOs;

public class ValidationErrorResponseTests
{
    [Fact]
    public void ValidationErrorResponse_Should_InheritFromErrorResponse()
    {
        // Arrange & Act
        var response = new ValidationErrorResponse();

        // Assert
        response.Should().BeAssignableTo<ErrorResponse>();
    }

    [Fact]
    public void ValidationErrorResponse_Should_HaveDefaultValidationErrors()
    {
        // Arrange & Act
        var response = new ValidationErrorResponse();

        // Assert
        response.ValidationErrors.Should().NotBeNull();
        response.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void ValidationErrorResponse_Should_AllowSettingValidationErrors()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email is required", "Email format is invalid" } },
            { "Password", new[] { "Password must be at least 8 characters" } }
        };

        // Act
        var response = new ValidationErrorResponse
        {
            ValidationErrors = validationErrors
        };

        // Assert
        response.ValidationErrors.Should().HaveCount(2);
        response.ValidationErrors["Email"].Should().HaveCount(2);
        response.ValidationErrors["Password"].Should().HaveCount(1);
    }

    [Fact]
    public void ValidationErrorResponse_Should_SerializeCorrectly()
    {
        // Arrange
        var response = new ValidationErrorResponse
        {
            Message = "Validation failed",
            Type = "VALIDATION_ERROR",
            ValidationErrors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "Email is required" } }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<ValidationErrorResponse>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Message.Should().Be("Validation failed");
        deserialized.Type.Should().Be("VALIDATION_ERROR");
        deserialized.ValidationErrors.Should().HaveCount(1);
        deserialized.ValidationErrors["Email"].Should().Contain("Email is required");
    }

    [Fact]
    public void ValidationErrorResponse_Should_DeserializeCorrectly()
    {
        // Arrange
        var json = """
        {
            "Message": "Validation failed",
            "Type": "VALIDATION_ERROR",
            "ValidationErrors": {
                "Email": ["Email is required"],
                "Password": ["Password is required"]
            }
        }
        """;

        // Act
        var response = JsonSerializer.Deserialize<ValidationErrorResponse>(json);

        // Assert
        response.Should().NotBeNull();
        response!.Message.Should().Be("Validation failed");
        response.Type.Should().Be("VALIDATION_ERROR");
        response.ValidationErrors.Should().HaveCount(2);
        response.ValidationErrors["Email"].Should().Contain("Email is required");
        response.ValidationErrors["Password"].Should().Contain("Password is required");
    }

    [Fact]
    public void ValidationErrorResponse_Should_HandleEmptyValidationErrors()
    {
        // Arrange
        var response = new ValidationErrorResponse
        {
            Message = "No validation errors",
            ValidationErrors = new Dictionary<string, string[]>()
        };

        // Act
        var json = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<ValidationErrorResponse>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void ValidationErrorResponse_Should_HandleNullValidationErrors()
    {
        // Arrange
        var response = new ValidationErrorResponse
        {
            Message = "Test message",
            ValidationErrors = null!
        };

        // Act
        var json = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<ValidationErrorResponse>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.ValidationErrors.Should().BeNull();
    }

    [Fact]
    public void ValidationErrorResponse_Should_SupportInheritedProperties()
    {
        // Arrange
        var response = new ValidationErrorResponse
        {
            TraceId = "test-trace-id",
            Message = "Validation failed",
            Details = "Additional details",
            Type = "VALIDATION_ERROR",
            Extensions = new Dictionary<string, object>
            {
                { "field", "email" }
            },
            ValidationErrors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "Invalid format" } }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(response);
        var deserialized = JsonSerializer.Deserialize<ValidationErrorResponse>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.TraceId.Should().Be("test-trace-id");
        deserialized.Message.Should().Be("Validation failed");
        deserialized.Details.Should().Be("Additional details");
        deserialized.Type.Should().Be("VALIDATION_ERROR");
        deserialized.Extensions.Should().ContainKey("field");
        deserialized.Extensions!["field"].ToString().Should().Be("email");
        deserialized.ValidationErrors.Should().HaveCount(1);
    }
} 