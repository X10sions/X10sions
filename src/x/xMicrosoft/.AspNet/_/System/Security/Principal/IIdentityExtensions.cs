using Microsoft.AspNet.Identity;

namespace System.Security.Principal {
  public static class IIdentityExtensions {

    public static bool HasAppUserId(this IIdentity identity) => !string.IsNullOrWhiteSpace(identity.GetUserId());

  }
}