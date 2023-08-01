using BeautySaloon.HealthChecksUI.Extensions;

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
