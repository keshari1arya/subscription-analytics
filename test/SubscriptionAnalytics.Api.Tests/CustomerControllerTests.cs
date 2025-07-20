using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Api.Controllers;
using SubscriptionAnalytics.Shared.DTOs;
using Xunit;

namespace SubscriptionAnalytics.Api.Tests;

public class CustomerControllerTests
{
    private readonly Mock<ILogger<CustomerController>> _loggerMock = new();
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _controller = new CustomerController(_loggerMock.Object);
    }

    [Fact]
    public void GetCustomers_Should_Return_MockCustomerList()
    {
        // Act
        var result = _controller.GetCustomers() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var customers = result!.Value as List<CustomerDto>;
        customers.Should().NotBeNull();
        customers!.Should().HaveCount(2);
        customers[0].CustomerId.Should().Be("cus_123456789");
        customers[1].CustomerId.Should().Be("cus_987654321");
    }

    [Theory]
    [InlineData("cus_123")] 
    [InlineData("cus_abc")] 
    public void GetCustomer_Should_Return_MockCustomer_WithGivenId(string id)
    {
        // Act
        var result = _controller.GetCustomer(id) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var customer = result!.Value as CustomerDto;
        customer.Should().NotBeNull();
        customer!.CustomerId.Should().Be(id);
        customer.Email.Should().Be($"customer{id}@example.com");
        customer.Name.Should().Be($"Customer {id}");
    }
} 