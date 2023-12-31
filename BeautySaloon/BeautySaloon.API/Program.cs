using BeautySaloon.API.Extensions;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace BeautySaloon.API;

public class Program
{
    public static void Main(string[] args)
    {
        IdentityModelEventSource.ShowPII = true;
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext());

        builder.ConfigureServices();

        var app = builder.Build();
        app.UseMiddleware<LoggingHeadersMiddleware>();
        app.ConfigureApplication();

        app.Run();
    }
}
