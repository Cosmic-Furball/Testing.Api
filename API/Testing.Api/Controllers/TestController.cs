using Microsoft.AspNetCore.Mvc;

namespace Testing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Simple GET endpoint that returns a greeting message
    /// </summary>
    /// <returns>A greeting message</returns>
    [HttpGet]
    public ActionResult<object> Get()
    {
        _logger.LogInformation("Test GET endpoint called");
        
        return Ok(new 
        { 
            Message = "Hello from Testing API!", 
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        });
    }

    /// <summary>
    /// GET endpoint with a parameter
    /// </summary>
    /// <param name="name">Name to greet</param>
    /// <returns>Personalized greeting</returns>
    [HttpGet("greet/{name}")]
    public ActionResult<object> Greet(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name cannot be empty");
        }

        _logger.LogInformation("Greeting endpoint called for {Name}", name);
        
        return Ok(new 
        { 
            Message = $"Hello, {name}!", 
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// GET endpoint that returns API status
    /// </summary>
    /// <returns>API status information</returns>
    [HttpGet("status")]
    public ActionResult<object> GetStatus()
    {
        _logger.LogInformation("Status endpoint called");
        
        return Ok(new 
        { 
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            MachineName = Environment.MachineName
        });
    }
}
