using Duende.IdentityServer.Models;
using IdentityModel;

namespace BeautySaloon.Identity;
public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "role",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Role
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("api.read"),
            new ApiScope("api.edit"),
            new ApiScope("health")
        };
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client {
                    RequireConsent = false,
                    ClientId = "angular_spa",
                    ClientName = "Angular SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api.read", "api.edit", "role" },
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/user/main"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600,
                },
            new Client
            {
                ClientId = "HealthCheckUI",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string>
                {
                    "health"
                }
            }
        };
}
