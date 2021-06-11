using Common.Exceptions;

namespace System.Security.Principal {
  public static class IIdentityExtensions {

    public static bool IsAllowed(this IIdentity identity, string[] allowedUserNames, bool throwIfFalse = false)
      => identity.IsAllowed(allowedUserNames, throwIfFalse ? new PermissionDeniedException() : null);

  }
}
