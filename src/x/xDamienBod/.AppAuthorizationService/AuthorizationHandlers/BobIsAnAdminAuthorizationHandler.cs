using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace xDamienBod.AppAuthorizationService {
  public class BobIsAnAdminAuthorizationHandler : AuthorizationHandler<IsAdminRequirement> {
    public BobIsAnAdminAuthorizationHandler(IAppAuthorizationService appAuthorizationService) {
      _appAuthorizationService = appAuthorizationService;
    }

    readonly IAppAuthorizationService _appAuthorizationService;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement) {
      if (_appAuthorizationService.BobIsAnAdmin(context.User.Identity.Name)) {
        context.Succeed(requirement);
      }
      return Task.CompletedTask;
    }

  }
}