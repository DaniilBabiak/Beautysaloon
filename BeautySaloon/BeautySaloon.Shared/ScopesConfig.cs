using Duende.IdentityServer.Models;

namespace BeautySaloon.Shared;
public static class ScopesConfig
{
    public static readonly ApiScope ApiRead = new ApiScope("api.read");
    public static readonly ApiScope ApiEdit = new ApiScope("api.edit");
    public static readonly ApiScope Health = new ApiScope("health");
    public static readonly ApiScope ImageRead = new ApiScope("image.read");
    public static readonly ApiScope ImageEdit = new ApiScope("image.edit");

    public static IEnumerable<ApiScope> GetAll()
    {
        yield return ApiRead;
        yield return ApiEdit;
        yield return Health;
        yield return ImageRead;
        yield return ImageEdit;
    }
}
