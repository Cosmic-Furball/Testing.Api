using Microsoft.EntityFrameworkCore;
using Testing.Api.Core;
using Testing.Infrastructure.Postgres;
using Testing.Infrastructure.Postgres.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register SummaryService
builder.Services.AddScoped<ISummaryService, SummaryService>();

builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseNpgsql("Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=postgres", 
        db => db.MigrationsAssembly("Testing.Infrastructure")));

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
