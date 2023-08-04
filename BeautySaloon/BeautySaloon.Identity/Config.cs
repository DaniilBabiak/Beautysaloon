using BeautySaloon.Shared;
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
        ScopesConfig.GetAll();

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
        };

    public static IEnumerable<Client> Clients =>
        ClientsConfig.GetAll();
}
