using Common.Features.DummyFakeExamples.WeatherForecast;
using Microsoft.EntityFrameworkCore;
using X10sions.Wsus.Api;
using X10sions.Wsus.Api.Endpoints;
using X10sions.Wsus.Data;

var builder = WebApplication.CreateBuilder(args);
var appSettings = AppSettings.Configure(builder);

builder.Services.AddDbContext<SusdbDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.SUSDB ?? throw new InvalidOperationException("Connection string 'X10sionsWsusContext' not found.")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async () => {
  var forecast = await WeatherForecast.GetRandomListAsync(5);
  return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapComputerTargetEndpoints();

app.Run();


