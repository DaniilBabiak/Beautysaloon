using BeautySaloon.ImagesAPI.Extensions;
using BeautySaloon.ImagesAPI.HealthChecks;
using BeautySaloon.ImagesAPI.RabbitMQ;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
builder.Services.Configure<HealthChecksSettings>(configuration.GetSection("HealthChecksSettings"));

builder.ConfigureServices();

var app = builder.Build();

app.ConfigureApplication();

app.Run();
