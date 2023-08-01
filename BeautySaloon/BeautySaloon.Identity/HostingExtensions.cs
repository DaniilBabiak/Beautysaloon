using BeautySaloon.Identity.Data;
using BeautySaloon.Identity.Models;
using Duende.IdentityServer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BeautySaloon.Identity;
internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
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
                            options.ClientId = "758550067740-2dud263od3thtahp6iv3usl88e9lfi80.apps.googleusercontent.com";
                            options.ClientSecret = "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";
                            options.CorrelationCookie.HttpOnly = true;
                            options.CorrelationCookie.SameSite = SameSiteMode.None;
                            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("HealthPolicy", policy =>
            {
                policy.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
                policy.RequireClaim("scope", "health");
            });
        });

        builder.ConfigureHealthChecks();


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
        app.MapHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).RequireAuthorization("HealthPolicy");
        return app;
    }

    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("BeautysaloonIdentityDbConnection");

        builder.Services.AddHealthChecks()
                        .AddSqlServer(
                            connectionString: connectionString,
                            name: "Identity SQL Server",
                            tags: new[] { "Database" });
    }
}