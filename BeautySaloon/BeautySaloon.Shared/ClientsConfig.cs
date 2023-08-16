using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace BeautySaloon.Shared;
public static class ClientsConfig
{
    public static readonly Client AngularSpa = new Client
    {
        RequireConsent = false,
        ClientId = "angular_spa",
        ClientName = "Angular SPA",
        ClientSecrets = { new Secret("secret".Sha256()) },
        AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
        AllowedScopes = { "openid", "profile", "email", "api.read", "api.edit", "roles", "image.read", "image.edit" },
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
        AllowedGrantTypes = GrantTypes.Code,
        RedirectUris = { "http://localhost:5003/signin-oidc", "http://localhost:5030/signin-oidc" },
        AllowOfflineAccess = true,
        PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },
        AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "roles", "health" }
    };

    public static IEnumerable<Client> GetAll()
    {
        yield return AngularSpa;
        yield return HealthCheckUI;
    }
}
