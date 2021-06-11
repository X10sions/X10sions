using System.Collections.Generic;

namespace xDamienBod.AppAuthorizationService {
  public static class RulesAdmin {

    static List<string> adminUsers = new List<string>();

    static List<string> adminProviders = new List<string>();

    public static bool IsAdmin(string username, string providerClaimValue) {
      if (adminUsers.Count == 0) {
        AddAllowedUsers();
        AddAllowedProviders();
      }
      if (adminUsers.Contains(username) && adminProviders.Contains(providerClaimValue)) {
        return true;
      }
      return false;
    }

    static void AddAllowedUsers() {
      adminUsers.Add(Constants.AdminUser);
    }

    static void AddAllowedProviders() {
      adminProviders.Add(Constants.IdentityProviderValue);
    }
  }
}
