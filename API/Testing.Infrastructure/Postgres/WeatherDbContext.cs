using Microsoft.EntityFrameworkCore;
using Testing.Api.Infrastructure.Postgres.Entities;
using Testing.Infrastructure.Postgres.Configuration;

namespace Testing.Infrastructure.Postgres;

public class WeatherDbContext : DbContext
{
    public DbSet<WeatherSummary> WeatherSummaries { get; set; }
    
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeatherTypeConfiguration).Assembly);
    }
}