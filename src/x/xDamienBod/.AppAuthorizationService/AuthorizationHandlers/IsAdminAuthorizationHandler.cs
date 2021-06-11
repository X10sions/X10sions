using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace xDamienBod.AppAuthorizationService {
  public class IsAdminAuthorizationHandler : AuthorizationHandler<IsAdminRequirement> {
    public IsAdminAuthorizationHandler(IAppAuthorizationService appAuthorizationService) {
      _appAuthorizationService = appAuthorizationService;
    }

    IAppAuthorizationService _appAuthorizationService;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement) {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      if (requirement == null)
        throw new ArgumentNullException(nameof(requirement));

      var claimIdentityprovider = context.User.Claims.FirstOrDefault(t => t.Type == Constants.IdentityProviderClaim);

      if (claimIdentityprovider != null && _appAuthorizationService.IsAdmin(context.User.Identity.Name, claimIdentityprovider.Value)) {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}