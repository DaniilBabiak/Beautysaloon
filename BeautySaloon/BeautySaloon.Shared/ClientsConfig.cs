using Duende.IdentityServer.Models;

namespace BeautySaloon.Shared;
public static class ClientsConfig
{
    public static readonly Client AngularSpa = new Client
    {
        RequireConsent = false,
        ClientId = "angular_spa",
        ClientName = "Angular SPA",
        AllowedGrantTypes = GrantTypes.Implicit,
        AllowedScopes = { "openid", "profile", "email", "api.read", "api.edit", "role" },
        RedirectUris = { "http://localhost:4200/auth-callback" },
        PostLogoutRedirectUris = { "http://localhost:4200/user/main" },
        AllowedCorsOrigins = { "http://localhost:4200" },
        AllowAccessTokensViaBrowser = true,
        AccessTokenLifetime = 3600,
    };

    public static readonly Client HealthCheckUI = new Client
    {
        ClientId = "HealthCheckUI",
        ClientSecrets = { new Secret("secret".Sha256()) },
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        AllowedScopes = { "health" }
    };

    public static IEnumerable<Client> GetAll()
    {
        yield return AngularSpa;
        yield return HealthCheckUI;
    }
}
