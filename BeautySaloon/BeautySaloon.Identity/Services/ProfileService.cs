using BeautySaloon.Identity.Models;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BeautySaloon.Identity.Services;

public class ProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
                          UserManager<ApplicationUser> userManager)
    {
        _claimsFactory = claimsFactory;
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaimValue = string.Join(" ", roles);
        context.IssuedClaims.AddRange(new []{ new Claim(ClaimTypes.Role, roleClaimValue), new Claim("role", roleClaimValue)});
        context.IssuedClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
        var principal = await _claimsFactory.CreateAsync(user);
        context.IssuedClaims.AddRange(principal.Claims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}