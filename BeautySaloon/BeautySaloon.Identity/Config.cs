using BeautySaloon.Shared;
using Duende.IdentityServer.Models;

namespace BeautySaloon.Identity;
public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", new[] { "role" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        ScopesConfig.GetAll();

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource(ScopesConfig.ApiRead.Name, " ", new[] { "rolse" }),
            new ApiResource(ScopesConfig.ApiEdit.Name, " ", new[] { "roles" })
        };

    public static IEnumerable<Client> Clients =>
        ClientsConfig.GetAll();
}
