namespace xDamienBod.AppAuthorizationService {
  public class AppAuthorizationService : IAppAuthorizationService {

    public bool BobIsAnAdmin(string name) => name.IndexOf("bob", System.StringComparison.OrdinalIgnoreCase) >= 0;
    public bool IsAdmin(string username, string providerClaimValue) => RulesAdmin.IsAdmin(username, providerClaimValue);

  }
}