using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using SubscriptionAnalytics.Api.Controllers;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Api.Tests;

public class CustomerControllerTests
{
    private readonly CustomerController _controller;
    private readonly Mock<ILogger<CustomerController>> _loggerMock;

    public CustomerControllerTests()
    {
        _loggerMock = new Mock<ILogger<CustomerController>>();
        _controller = new CustomerController(_loggerMock.Object);
    }

    [Fact]
    public void Constructor_Should_CreateController()
    {
        // Act & Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<CustomerController>();
    }

    [Fact]
    public void GetCustomers_Should_ReturnOkResult()
    {
        // Act
        var result = _controller.GetCustomers();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public void GetCustomers_Should_ReturnListOfCustomers()
    {
        // Act
        var result = _controller.GetCustomers();

        // Assert
        var okResult = result as OkObjectResult;
        var customers = okResult!.Value as List<CustomerDto>;
        customers.Should().NotBeNull();
        customers!.Count.Should().Be(2);
    }

    [Fact]
    public void GetCustomers_Should_ReturnValidCustomerData()
    {
        // Act
        var result = _controller.GetCustomers();

        // Assert
        var okResult = result as OkObjectResult;
        var customers = okResult!.Value as List<CustomerDto>;
        
        var firstCustomer = customers!.First();
        firstCustomer.CustomerId.Should().Be("cus_123456789");
        firstCustomer.Email.Should().Be("john.doe@example.com");
        firstCustomer.Name.Should().Be("John Doe");
        firstCustomer.Livemode.Should().BeTrue();
        firstCustomer.CreatedAt.Should().BeBefore(DateTime.UtcNow);
        firstCustomer.SyncedAt.Should().BeBefore(DateTime.UtcNow);
    }

    [Fact]
    public void GetCustomers_Should_ReturnCustomersWithValidIds()
    {
        // Act
        var result = _controller.GetCustomers();

        // Assert
        var okResult = result as OkObjectResult;
        var customers = okResult!.Value as List<CustomerDto>;
        
        customers!.Should().OnlyContain(c => !string.IsNullOrEmpty(c.CustomerId));
        customers.Should().OnlyContain(c => c.CustomerId.StartsWith("cus_"));
    }

    [Fact]
    public void GetCustomers_Should_ReturnCustomersWithValidEmails()
    {
        // Act
        var result = _controller.GetCustomers();

        // Assert
        var okResult = result as OkObjectResult;
        var customers = okResult!.Value as List<CustomerDto>;
        
        customers!.Should().OnlyContain(c => !string.IsNullOrEmpty(c.Email));
        customers.Should().OnlyContain(c => c.Email.Contains("@"));
    }

    [Theory]
    [InlineData("cus_123456789")]
    [InlineData("cus_987654321")]
    [InlineData("cus_test123")]
    [InlineData("cus_abc123")]
    public void GetCustomer_Should_ReturnOkResult_ForValidId(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
    }

    [Theory]
    [InlineData("cus_123456789")]
    [InlineData("cus_987654321")]
    [InlineData("cus_test123")]
    public void GetCustomer_Should_ReturnCustomerWithCorrectId(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        var okResult = result as OkObjectResult;
        var customer = okResult!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.CustomerId.Should().Be(customerId);
    }

    [Theory]
    [InlineData("cus_123456789")]
    [InlineData("cus_test123")]
    public void GetCustomer_Should_ReturnCustomerWithValidEmail(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        var okResult = result as OkObjectResult;
        var customer = okResult!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.Email.Should().Be($"customer{customerId}@example.com");
    }

    [Theory]
    [InlineData("cus_123456789")]
    [InlineData("cus_test123")]
    public void GetCustomer_Should_ReturnCustomerWithValidName(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        var okResult = result as OkObjectResult;
        var customer = okResult!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.Name.Should().Be($"Customer {customerId}");
    }

    [Theory]
    [InlineData("cus_123456789")]
    [InlineData("cus_test123")]
    public void GetCustomer_Should_ReturnCustomerWithValidProperties(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        var okResult = result as OkObjectResult;
        var customer = okResult!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.Livemode.Should().BeTrue();
        customer.CreatedAt.Should().BeBefore(DateTime.UtcNow);
        customer.SyncedAt.Should().BeBefore(DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void GetCustomer_Should_HandleEmptyOrNullId(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var customer = okResult!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.CustomerId.Should().Be(customerId);
    }

    [Theory]
    [InlineData("invalid_id")]
    [InlineData("123456789")]
    [InlineData("cus_")]
    [InlineData("customer_123")]
    public void GetCustomer_Should_HandleInvalidIdFormat(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var customer = okResult!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.CustomerId.Should().Be(customerId);
    }

    [Theory]
    [InlineData("cus_very_long_customer_id_that_exceeds_normal_length")]
    [InlineData("cus_aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void GetCustomer_Should_HandleVeryLongIds(string customerId)
    {
        // Act
        var result = _controller.GetCustomer(customerId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var customer = okResult!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.CustomerId.Should().Be(customerId);
    }

    [Fact]
    public void GetCustomers_Should_LogInformation()
    {
        // Act
        _controller.GetCustomers();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Getting customers for tenant")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData("cus_123456789")]
    [InlineData("cus_test123")]
    public void GetCustomer_Should_LogInformation_WithCustomerId(string customerId)
    {
        // Act
        _controller.GetCustomer(customerId);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Getting customer with ID: {customerId}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void GetCustomers_Should_ReturnConsistentData()
    {
        // Act
        var result1 = _controller.GetCustomers();
        var result2 = _controller.GetCustomers();

        // Assert
        var okResult1 = result1 as OkObjectResult;
        var okResult2 = result2 as OkObjectResult;
        var customers1 = okResult1!.Value as List<CustomerDto>;
        var customers2 = okResult2!.Value as List<CustomerDto>;
        
        customers1!.Count.Should().Be(customers2!.Count);
        customers1.First().CustomerId.Should().Be(customers2.First().CustomerId);
    }

    [Fact]
    public void GetCustomer_Should_ReturnConsistentData_ForSameId()
    {
        // Arrange
        var customerId = "cus_123456789";

        // Act
        var result1 = _controller.GetCustomer(customerId);
        var result2 = _controller.GetCustomer(customerId);

        // Assert
        var okResult1 = result1 as OkObjectResult;
        var okResult2 = result2 as OkObjectResult;
        var customer1 = okResult1!.Value as CustomerDto;
        var customer2 = okResult2!.Value as CustomerDto;
        
        customer1!.CustomerId.Should().Be(customer2!.CustomerId);
        customer1.Email.Should().Be(customer2.Email);
        customer1.Name.Should().Be(customer2.Name);
    }
} 