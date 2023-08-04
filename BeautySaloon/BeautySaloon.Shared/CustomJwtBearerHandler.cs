using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace BeautySaloon.Shared;
public class CustomJwtBearerHandler : JwtBearerHandler
{
    public CustomJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string authorization = Request.Headers.Authorization;

        // If no authorization header found, nothing to process further
        if (string.IsNullOrEmpty(authorization))
        {
            return AuthenticateResult.NoResult();
        }

        if (authorization.StartsWith("BearerHttp ", StringComparison.OrdinalIgnoreCase))
        {
            // Extract the token for BearerHttp
            var token = authorization.Substring("BearerHttp ".Length).Trim();
            // Set the token in the Authorization header
            Request.Headers.Authorization = $"Bearer {token}";
        }
        else if (authorization.StartsWith("BearerHttps ", StringComparison.OrdinalIgnoreCase))
        {
            // Extract the token for BearerHttps
            var token = authorization.Substring("BearerHttps ".Length).Trim();
            // Set the token in the Authorization header
            Request.Headers.Authorization = $"Bearer {token}";
        }
        else
        {
            // If no valid token found, no further work possible
            return AuthenticateResult.NoResult();
        }

        // Call the base method to continue token processing
        return await base.HandleAuthenticateAsync();
    }
}