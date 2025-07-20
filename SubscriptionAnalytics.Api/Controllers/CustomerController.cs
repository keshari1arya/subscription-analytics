using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ILogger<CustomerController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get all customers for the current tenant
    /// </summary>
    /// <returns>List of customers</returns>
    [HttpGet]
    public IActionResult GetCustomers()
    {
        _logger.LogInformation("Getting customers for tenant");
        
        // Mock data for now
        var customers = new List<CustomerDto>
        {
            new CustomerDto
            {
                CustomerId = "cus_123456789",
                Email = "john.doe@example.com",
                Name = "John Doe",
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                SyncedAt = DateTime.UtcNow.AddHours(-2),
                Livemode = true
            },
            new CustomerDto
            {
                CustomerId = "cus_987654321",
                Email = "jane.smith@example.com",
                Name = "Jane Smith",
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                SyncedAt = DateTime.UtcNow.AddHours(-1),
                Livemode = true
            }
        };

        return Ok(customers);
    }

    /// <summary>
    /// Get a specific customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id}")]
    public IActionResult GetCustomer(string id)
    {
        _logger.LogInformation("Getting customer with ID: {CustomerId}", id);
        
        // Mock data for now
        var customer = new CustomerDto
        {
            CustomerId = id,
            Email = $"customer{id}@example.com",
            Name = $"Customer {id}",
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            SyncedAt = DateTime.UtcNow.AddHours(-2),
            Livemode = true
        };

        return Ok(customer);
    }
} 