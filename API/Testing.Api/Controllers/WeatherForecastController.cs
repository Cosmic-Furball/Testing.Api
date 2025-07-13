using Microsoft.AspNetCore.Mvc;
using Testing.Api.Models;

namespace Testing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets weather forecast data
    /// </summary>
    /// <returns>A list of weather forecasts</returns>
    [HttpGet]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        _logger.LogInformation("Getting weather forecast data");
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        return Ok(forecast);
    }

    /// <summary>
    /// Gets weather forecast for a specific day
    /// </summary>
    /// <param name="days">Number of days from today</param>
    /// <returns>Weather forecast for the specified day</returns>
    [HttpGet("{days:int}")]
    public ActionResult<WeatherForecast> Get(int days)
    {
        if (days < 1 || days > 10)
        {
            return BadRequest("Days must be between 1 and 10");
        }

        _logger.LogInformation("Getting weather forecast for day {Days}", days);

        var forecast = new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(days)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        );

        return Ok(forecast);
    }
}
