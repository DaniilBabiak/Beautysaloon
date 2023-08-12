using BeautySaloon.Identity.Models;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BeautySaloon.Identity.Services;

public class ProfileService : DefaultProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
                          UserManager<ApplicationUser> userManager,
                          ILogger<DefaultProfileService> logger)
        :base(logger)
    {
        _claimsFactory = claimsFactory;
        _userManager = userManager;
    }

    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaimValue = string.Join(" ", roles);
        context.IssuedClaims.AddRange(new []{ new Claim(ClaimTypes.Role, roleClaimValue), new Claim("role", roleClaimValue)});
        await base.GetProfileDataAsync(context);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}