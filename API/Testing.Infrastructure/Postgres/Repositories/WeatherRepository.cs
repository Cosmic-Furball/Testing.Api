using Microsoft.Extensions.Logging;
using Testing.Api.Infrastructure.Postgres.Entities;

namespace Testing.Infrastructure.Postgres.Repositories;

public interface IWeatherRepository
{
     Task CreateAsync(WeatherSummary weatherSummaryEntity, CancellationToken cancellationToken = default);
     Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class WeatherRepository : IWeatherRepository
{
    private readonly WeatherDbContext _dbContext;
    private readonly ILogger<WeatherRepository> _logger;
    
    public WeatherRepository(WeatherDbContext dbContext, ILogger<WeatherRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task CreateAsync(WeatherSummary weatherSummaryEntity, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[Listing V2] CreateListings started");
        await _dbContext.Set<WeatherSummary>().AddAsync(weatherSummaryEntity, cancellationToken);
        _logger.LogInformation("[Listing V2] CreateListings finished");
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken);
}