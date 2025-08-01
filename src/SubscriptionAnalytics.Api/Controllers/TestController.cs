using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SubscriptionAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    public class TestResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string User { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    /// <summary>
    /// Test endpoint to verify API is working
    /// </summary>
    /// <returns>Test response</returns>
    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Test endpoint called");
        return Ok(new TestResponseDto
        {
            Message = "API is working!",
            Timestamp = DateTime.UtcNow,
            User = User.Identity?.Name ?? "Anonymous"
        });
    }

    /// <summary>
    /// Test POST endpoint
    /// </summary>
    /// <param name="data">Test data</param>
    /// <returns>Echo response</returns>
    [HttpPost]
    public IActionResult Post([FromBody] object data)
    {
        _logger.LogInformation("Received POST data: {@Data}", data);
        return Ok(new TestResponseDto
        {
            Message = "POST request received",
            Data = data,
            Timestamp = DateTime.UtcNow,
            User = User.Identity?.Name ?? "Anonymous"
        });
    }
} 