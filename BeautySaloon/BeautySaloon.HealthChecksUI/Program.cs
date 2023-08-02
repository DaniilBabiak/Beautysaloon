using BeautySaloon.HealthChecksUI.Extensions;
using Microsoft.IdentityModel.Logging;

namespace BeautySaloon.HealthChecksUI;

public class Program
{
    public static void Main(string[] args)
    {
        IdentityModelEventSource.ShowPII = true;
        var builder = WebApplication.CreateBuilder(args);
        builder.ConfigureServices();

        var app = builder.Build();

        app.ConfigureApplication();

        app.Run();
    }
}
