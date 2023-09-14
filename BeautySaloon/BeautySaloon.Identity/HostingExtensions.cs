using BeautySaloon.Identity.Data;
using BeautySaloon.Identity.EventHandlers;
using BeautySaloon.Identity.HealthChecks;
using BeautySaloon.Identity.Models;
using BeautySaloon.Identity.RabbitMQ;
using BeautySaloon.Identity.Services;
using BeautySaloon.Shared;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BeautySaloon.Identity;
internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQSettings"));
        builder.Services.Configure<HealthChecksSettings>(configuration.GetSection("HealthChecksSettings"));
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BeautysaloonIdentityDbConnection")));

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddAspNetIdentity<ApplicationUser>();

        builder.Services.AddAuthentication()
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

                            options.TokenValidationParameters.ValidIssuers = new[] { "https://localhost:5001", "http://identity" };
                            options.TokenValidationParameters.ValidateAudience = false;
                            options.RequireHttpsMetadata = false;
                        })
                        .AddGoogle(options =>
                        {
                            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                            options.ForwardSignOut = IdentityServerConstants.DefaultCookieAuthenticationScheme;

                            // register your IdentityServer with Google at https://console.developers.google.com
                            // enable the Google+ API
                            // set the redirect URI to https://localhost:5001/signin-google
                            options.ClientId = GoogleConfig.ClientId;
                            options.ClientSecret = GoogleConfig.ClientSecret;
                            options.CorrelationCookie.HttpOnly = true;
                            options.CorrelationCookie.SameSite = SameSiteMode.None;
                            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                        });

        builder.Services.AddAuthorization();

        builder.ConfigureHealthChecks();
        builder.Services.AddTransient<IProfileService, ProfileService>();
        builder.Services.AddTransient<IEventSink, UserLoginSuccessEventHandler>();
        builder.Services.AddTransient<CustomerPublisher>();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.EnsureSeedData();
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();
        //app.MapHealthChecks("/api/health", new HealthCheckOptions
        //{
        //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //}).RequireAuthorization("HealthPolicy");
        return app;
    }

    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        string connectionString = configuration.GetConnectionString("BeautysaloonIdentityDbConnection");

        builder.Services.AddHealthChecks()
                        .AddSqlServer(
                            connectionString: connectionString,
                            name: "Identity SQL Server",
                            tags: new[] { "Database" })
                        .AddRabbitMQ(configuration.GetSection("RabbitMQSettings")["Uri"],
                                     name: "RabbitMQ",
                                     tags: new[] { "Queue services" });

        builder.Services.AddHostedService<RabbitMQHealthCheckListener>();
    }
}