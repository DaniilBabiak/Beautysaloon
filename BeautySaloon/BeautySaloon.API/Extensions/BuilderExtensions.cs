using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.HealthChecks;
using BeautySaloon.API.RabbitMQ;
using BeautySaloon.API.Services;
using BeautySaloon.API.Services.Interfaces;
using BeautySaloon.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using BeautySaloon.API.Validators;
using FluentValidation;

namespace BeautySaloon.API.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        builder.Services.Configure<HealthChecksSettings>(configuration.GetSection("HealthChecksSettings"));

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Используем Preserve, чтобы сохранить ссылки на объекты
            });

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();

        builder.ConfigureSwagger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        builder.ConfigureSqlContexts();

        builder.ConfigureAuthentication();
        builder.ConfigureAuthorization();

        builder.ConfigureCors();

        builder.ConfigureHealthChecks();

        builder.Services.AddTransient<IServiceService, ServiceService>();
        builder.Services.AddTransient<IServiceCategoryService, ServiceCategoryService>();
        builder.Services.AddTransient<IBestWorkService, BestWorkService>();
        builder.Services.AddTransient<IReservationService, ReservationService>();
        builder.Services.AddTransient<ICustomerService, CustomerService>();
        builder.Services.AddHostedService<CustomerListener>();
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
                            options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                            options.TokenValidationParameters.ValidateAudience = false;
                            options.RequireHttpsMetadata = false;
                        });
    }

    private static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("api.read", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.ApiRead.Name);
            });
            options.AddPolicy("api.edit", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.ApiEdit.Name);
            });
            options.AddPolicy("customer", policy =>
            {
                policy.RequireClaim("scope", ScopesConfig.ApiRead.Name);
                policy.RequireClaim("scope", ScopesConfig.ApiEdit.Name);
                policy.RequireAuthenticatedUser();
            });
            options.AddPolicy("admin", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "api.read");
                policy.RequireClaim("scope", "api.edit");
                policy.RequireRole(RolesConfig.Admin);
            });
        });
    }
    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        string connectionString = configuration.GetConnectionString("BeautysaloonDbConnection");

        builder.Services.AddHostedService<RabbitMQHealthCheckListener>();

        builder.Services.AddHealthChecks()
                        .AddSqlServer(
                            connectionString: connectionString,
                            name: "API SQL Server",
                            tags: new[] { "Database" })
                        .AddRabbitMQ(configuration.GetSection("RabbitMQSettings")["Uri"],
                                     name: "RabbitMQ");
    }
    private static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(setup =>
        {
            setup.AddPolicy("AllowAllPolicy", options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });
        });
    }
    private static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter 'Bearer {token}'",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
                    {
                        { securityScheme, new[] { "Bearer" } }
                    };
            c.AddSecurityRequirement(securityRequirement);
        });
    }
    private static void ConfigureSqlContexts(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("BeautysaloonDbConnection");

        builder.Services.AddDbContext<BeautySaloonContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
}
