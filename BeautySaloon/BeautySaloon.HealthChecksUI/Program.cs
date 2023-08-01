using BeautySaloon.HealthChecksUI.Extensions;
using BeautySaloon.HealthChecksUI.Helpers;
using Serilog;

namespace BeautySaloon.HealthChecksUI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.ConfigureServices();

        var app = builder.Build();

        app.ConfigureApplication();

        app.Run();
    }
}
