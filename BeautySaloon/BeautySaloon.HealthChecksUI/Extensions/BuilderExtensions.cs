using BeautySaloon.HealthChecksUI.HealthChecks;
using BeautySaloon.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Net.Http;

namespace BeautySaloon.HealthChecksUI.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.Configure<HealthChecksSettings>(configuration.GetSection("HealthChecksSettings"));
        builder.Services.AddMemoryCache();
        // Add services to the container.
        builder.ConfigureSerilog();
        builder.Services.AddRazorPages();
        builder.ConfigureHealthChecks();

        builder.ConfigureHealthChecksUI();

        builder.Services.AddAuthentication(options =>
                        {
                            options.DefaultScheme = "Cookies";
                            options.DefaultChallengeScheme = "oidc";
                        })
                        .AddCookie("Cookies", options =>
                        {
                            //options.CookieManager = new ChunkingCookieManager();

                            ////options.Cookie.HttpOnly = true;
                            //options.Cookie.SameSite = SameSiteMode.None;
                            //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        })
                        .AddOpenIdConnect("oidc", options =>
                        {
                            if (builder.Environment.IsDevelopment())
                            {
                                options.Authority = "https://localhost:5001";
                            }
                            else
                            {
                                options.Authority = "http://identity";
                                options.Events.OnRedirectToIdentityProvider = context =>
                                {
                                    // Intercept the redirection so the browser navigates to the right URL in your host
                                    context.ProtocolMessage.IssuerAddress = "https://localhost:5001/connect/authorize";
                                    context.ProtocolMessage.RedirectUri = "http://localhost:5030/signin-oidc";
                                    return Task.CompletedTask;
                                };
                            }
                            options.TokenValidationParameters.ValidIssuers = new[] { "https://localhost:5001", "http://identity" };
                            options.RequireHttpsMetadata = false;
                            options.ClientId = ClientsConfig.HealthCheckUI.ClientId;
                            options.ClientSecret = "secret";
                            options.ResponseType = "code";
                            options.SaveTokens = true;

                            options.Scope.Clear();
                            options.Scope.Add("openid");
                            options.Scope.Add("roles");
                            options.Scope.Add("health");
                            options.ClaimActions.MapJsonKey("role", "role", "role");
                            options.TokenValidationParameters.RoleClaimType = "role";

                            options.Events.OnTokenResponseReceived = context =>
                            {
                                // Логирование токена
                                var accessToken = context.TokenEndpointResponse.AccessToken;
                                Log.Information(accessToken);

                                return Task.CompletedTask;
                            };

                            options.Events.OnUserInformationReceived = context =>
                            {
                                return Task.CompletedTask;
                            };

                            options.GetClaimsFromUserInfoEndpoint = true;
                        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireRole(RolesConfig.Admin);
            });
        });
        return builder;
    }

    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        var rabbitUri = string.Empty;

        if (builder.Environment.IsDevelopment())
        {
            rabbitUri = "amqp://guest:guest@localhost:5672";
        }
        else
        {
            rabbitUri = "amqp://guest:guest@RabbitMQ";
        }
        builder.Services.AddHealthChecks()
                        .AddRabbitMQHealthCheck("API", new HealthChecksSettings
                        {
                            RequestQueueName = "request-health-api",
                            ReplyQueueName = "reply-health-api",
                            HostName = "beautysaloon-rabbit",
                            UserName = "guest",
                            Password = "guest",
                            Uri = rabbitUri
                        })
                        .AddRabbitMQHealthCheck("Identity", new HealthChecksSettings
                        {
                            RequestQueueName = "request-health-identity",
                            ReplyQueueName = "reply-health-identity",
                            HostName = "beautysaloon-rabbit",
                            UserName = "guest",
                            Password = "guest",
                            Uri = rabbitUri
                        })
                        .AddRabbitMQHealthCheck("Images", new HealthChecksSettings
                        {
                            RequestQueueName = "request-health-images",
                            ReplyQueueName = "reply-health-images",
                            HostName = "beautysaloon-rabbit",
                            UserName = "guest",
                            Password = "guest",
                            Uri = rabbitUri
                        })
                        .AddSqlServer(configuration.GetConnectionString("HealthChecksDb"),
                                      name: "Health Checks SQL Server",
                                      tags: new[] { "database" });
    }

    private static void ConfigureHealthChecksUI(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(5);
            options.SetMinimumSecondsBetweenFailureNotifications(5);
            options.UseApiEndpointHttpMessageHandler(sp =>
            {
                return new HttpClientHandler
                {
                    UseCookies = true,
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
                };
            });

            if (builder.Environment.IsDevelopment())
            {
                options.AddHealthCheckEndpoint("Healthcheck API", "/health");
            }
            else
            {
                options.AddHealthCheckEndpoint("Healthcheck API", "http://localhost/health");
            }
        }).AddSqlServerStorage(configuration.GetConnectionString("HealthChecksDb"), options =>
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }

    private static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
               .ReadFrom.Configuration(context.Configuration)
               .Enrich.FromLogContext());
    }
}
