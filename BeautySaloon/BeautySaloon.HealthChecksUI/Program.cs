using BeautySaloon.HealthChecksUI.Extensions;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace BeautySaloon.HealthChecksUI;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            IdentityModelEventSource.ShowPII = true;
            var builder = WebApplication.CreateBuilder(args);
            builder.ConfigureServices();

            var app = builder.Build();

            app.ConfigureApplication();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Stopping application.");
        }

    }
}
