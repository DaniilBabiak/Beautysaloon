
using BeautySaloon.API.Extensions;
using Serilog;

namespace BeautySaloon.API;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext());

        builder.ConfigureServices();

        var app = builder.Build();

        await app.ConfigureApplication();

        app.Run();
    }
}
