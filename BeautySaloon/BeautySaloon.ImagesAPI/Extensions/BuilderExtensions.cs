using BeautySaloon.ImagesAPI.Services;
using Minio;
using Minio.AspNetCore;
using Minio.AspNetCore.HealthChecks;
using Serilog;

namespace BeautySaloon.ImagesAPI.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(ctx.Configuration));

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });
        builder.Services.AddTransient<IImageService, ImageService>();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        builder.Services.AddMinio(options =>
        {
            if (builder.Environment.IsDevelopment())
            {
                options.Endpoint = "localhost:9000";
            }
            else
            {
                options.Endpoint = "minio:9000";
            }
            options.AccessKey = "0TQQsAlu2FmZoGR2Gdd6";
            options.SecretKey = "oXA1pc73RQFxwyICpxCyTTLxBz0n8OKml0FiAORP";
            options.ConfigureClient(client =>
            {
                client.WithSSL(false);
                client.WithHttpClient(new HttpClient());
            });
        });

        builder.Services.AddHealthChecks()
                        .AddMinio(sp => sp.GetRequiredService<MinioClient>());
        return builder;
    }
}
