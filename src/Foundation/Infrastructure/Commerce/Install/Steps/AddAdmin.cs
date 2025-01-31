using EPiServer.Authorization;
using EPiServer.Shell.Security;
using Foundation.Infrastructure.Cms.Users;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Foundation.Infrastructure.Commerce.Install.Steps;

public class AddAdmin : BaseInstallStep
{
    private readonly UserManager<SiteUser> _userManager;
    private readonly UIRoleProvider _uiRoleProvider;

    public AddAdmin(IContentRepository contentRepository, ReferenceConverter referenceConverter,
        IMarketService marketService, IWebHostEnvironment webHostEnvironment, UserManager<SiteUser> userManager,
        UIRoleProvider uiRoleProvider) : base(contentRepository,
        referenceConverter, marketService, webHostEnvironment)
    {
        _userManager = userManager;
        _uiRoleProvider = uiRoleProvider;
    }

    public override int Order => 1;

    public override string Description => "Adds administrator";

    protected override void ExecuteInternal(IProgressMessenger progressMessenger) =>
        AsyncHelper.RunSync(Execute);

    private async Task Execute()
    {
        var email = "admin@example.com";
        var password = "Episerver123!";
        var roles = new List<string> { Roles.Administrators, Roles.WebAdmins };

        await CreateOrUpdateUser(email, password);
        await ActivateUser(email);
        await ConfirmUserEmail(email);
        await EnsureRolesExist(roles);
        await RemoveRoles(email);
        await AssignRoles(roles, email);
    }

    private async Task<IdentityResult> CreateOrUpdateUser(string email, string password)
    {
        var username = email;
        var user = await _userManager.FindByNameAsync(username);

        IdentityResult identityResult;

        if (user == null)
        {
            user = new SiteUser { Username = username, };

            var result = await _userManager.CreateAsync(user, password);

            identityResult = result;
        }
        else
        {
            identityResult = await _userManager.RemovePasswordAsync(user);
            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddPasswordAsync(user, password);
            }
        }

        return identityResult;
    }

    private async Task<IdentityResult> ActivateUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user != null)
        {
            if (!user.IsApproved)
            {
                user.IsApproved = true;
                var result = await _userManager.UpdateAsync(user);

                return result;
            }
        }

        return IdentityResult.Success;
    }

    private async Task<IdentityResult> ConfirmUserEmail(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user != null)
        {
            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);

                return result;
            }
        }

        return IdentityResult.Success;
    }

    private async Task EnsureRolesExist(List<string> roles)
    {
        foreach (var role in roles)
        {
            var exists = await _uiRoleProvider.RoleExistsAsync(role);
            if (!exists)
            {
                await _uiRoleProvider.CreateRoleAsync(role);
            }
        }
    }

    private async Task RemoveRoles(string username)
    {
        var userRoles = await _uiRoleProvider.GetRolesForUserAsync(username).ToListAsync();
        await _uiRoleProvider.RemoveUserFromRolesAsync(username, userRoles);
    }

    private async Task AssignRoles(List<string> roles, string username)
    {
        await _uiRoleProvider.AddUserToRolesAsync(username, roles);
    }
}