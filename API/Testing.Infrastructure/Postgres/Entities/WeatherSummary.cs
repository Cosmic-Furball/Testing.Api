
namespace Testing.Api.Infrastructure.Postgres.Entities
{
    public class WeatherSummary
    {
        public int Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public DateTime CreatedDt { get; set; } = DateTime.UtcNow;
    }
}