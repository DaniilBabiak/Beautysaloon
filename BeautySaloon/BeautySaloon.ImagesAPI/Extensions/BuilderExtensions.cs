using BeautySaloon.ImagesAPI.HealthChecks;
using BeautySaloon.ImagesAPI.Services;
using BeautySaloon.Shared;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Minio.AspNetCore;
using Minio.AspNetCore.HealthChecks;
using Serilog;

namespace BeautySaloon.ImagesAPI.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureSerilog();
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

        builder.ConfigureAuthentication();
        builder.ConfigureAuthorization();

        builder.ConfigureMinio();

        return builder;
    }

    private static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
                                .AddJwtBearer(options =>
                                {
                                    if (builder.Environment.IsDevelopment())
                                    {
                                        options.Authority = "https://localhost:5001";
                                    }
                                    else
                                    {
                                        options.Authority = "http://identity";
                                    }

                                    options.TokenValidationParameters = new TokenValidationParameters()
                                    {
                                        ClockSkew = TimeSpan.FromMinutes(0)
                                    };
                                    options.TokenValidationParameters.ValidIssuers = new[] { "https://localhost:5001", "http://identity" };
                                    options.TokenValidationParameters.ValidateAudience = false;
                                    options.RequireHttpsMetadata = false;
                                });
    }

    private static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("image.read", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.ImageRead.Name);
            });
            options.AddPolicy("image.edit", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.ImageEdit.Name);
            });
            options.AddPolicy("health", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.Health.Name);
            });
        });
    }

    private static void ConfigureMinio(this WebApplicationBuilder builder)
    {
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
        builder.Services.AddHostedService<RabbitMQHealthCheckListener>();
    }

    private static void ConfigureSerilog(this WebApplicationBuilder builder)
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
    }
}
