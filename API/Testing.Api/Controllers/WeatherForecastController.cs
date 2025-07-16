using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Testing.Api.Models;
using Testing.Api.Core;

namespace Testing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISummaryService _summaryService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ISummaryService summaryService)
    {
        _logger = logger;
        _summaryService = summaryService;
    }

    /// <summary>
    /// Gets weather forecast data
    /// </summary>
    /// <returns>A list of weather forecasts</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
    {
        _logger.LogInformation("Getting weather forecast data");

        var summaries = await _summaryService.GetAllSummariesAsync();
        if (!summaries.Any())
        {
            return Problem("No weather summaries available");
        }

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Count)]
            ))
            .ToArray();

        return Ok(forecast);
    }

    /// <summary>
    /// Gets weather forecast data
    /// </summary>
    /// <returns>A list of all possible weather forecasts</returns>
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<string>>> GetAllSummaries()
    {
        _logger.LogInformation("Getting all weather forecast data");

        var summaries = await _summaryService.GetAllSummariesAsync();
        return Ok(summaries);
    }

    /// <summary>
    /// Gets weather forecast for a specific day
    /// </summary>
    /// <param name="days">Number of days from today</param>
    /// <returns>Weather forecast for the specified day</returns>
    [HttpGet("{days:int}")]
    public async Task<ActionResult<WeatherForecast>> Get(int days)
    {
        if (days < 1 || days > 10)
        {
            return BadRequest("Days must be between 1 and 10");
        }

        _logger.LogInformation("Getting weather forecast for day {Days}", days);

        var summaries = await _summaryService.GetAllSummariesAsync();
        if (!summaries.Any())
        {
            return Problem("No weather summaries available");
        }

        var forecast = new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(days)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Count)]
        );

        return Ok(forecast);
    }
    
    /// <summary>
    /// Adds a new weather summary
    /// </summary>
    /// <param name="model">The summary to add</param>
    /// <returns>Success or error response</returns>
    [HttpPost("add")]
    public async Task<ActionResult> AddNewSummary([FromBody] AddSummaryModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Summary))
        {
            return BadRequest("Summary cannot be empty");
        }

        _logger.LogInformation("Adding new weather forecast data: {summary}", model.Summary);

        var success = await _summaryService.AddSummaryAsync(model.Summary);
        
        if (!success)
        {
            return Conflict($"Summary '{model.Summary}' already exists or could not be added");
        }

        return Ok(new { message = $"Summary '{model.Summary}' added successfully" });
    }

    /// <summary>
    /// Removes a weather summary
    /// </summary>
    /// <param name="summary">The summary to remove</param>
    /// <returns>Success or error response</returns>
    [HttpDelete("remove/{summary}")]
    public async Task<ActionResult> RemoveSummary(string summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            return BadRequest("Summary cannot be empty");
        }

        _logger.LogInformation("Removing weather forecast data: {summary}", summary);

        var success = await _summaryService.RemoveSummaryAsync(summary);
        
        if (!success)
        {
            return NotFound($"Summary '{summary}' not found");
        }

        return Ok(new { message = $"Summary '{summary}' removed successfully" });
    }
}
