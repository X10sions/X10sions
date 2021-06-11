using Microsoft.AspNetCore.Authorization;

namespace xDamienBod.AppAuthorizationService {
  public static class MyPolicies {

    static AuthorizationPolicy requireWindowsProviderPolicy;

    public static AuthorizationPolicy GetRequireWindowsProviderPolicy() {
      if (requireWindowsProviderPolicy != null) return requireWindowsProviderPolicy;

      requireWindowsProviderPolicy = new AuthorizationPolicyBuilder()
            .RequireClaim(Constants.IdentityProviderClaim, Constants.IdentityProviderValue)
            .Build();
      return requireWindowsProviderPolicy;
    }

  }
}