using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testing.Api.Infrastructure.Postgres.Entities;

namespace Testing.Infrastructure.Postgres.Configuration;

public class WeatherTypeConfiguration : IEntityTypeConfiguration<WeatherSummary>
{
    public void Configure(EntityTypeBuilder<WeatherSummary> builder)
    {
        builder.HasKey(ws => ws.Id);
        builder.Property(ws => ws.Id)
            .HasIdentityOptions(startValue: 1000, incrementBy: 1);

        builder.Property(ws => ws.Summary)
            .HasMaxLength(100);
    }
}
