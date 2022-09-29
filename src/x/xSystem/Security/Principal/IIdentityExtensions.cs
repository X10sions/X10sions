namespace System.Security.Principal {
  public static class IIdentityExtensions {

    public static string LogonDomainName(this IIdentity identity) => identity.Name.Split('\\')[0] ?? "unknown";
    public static string LogonUserName(this IIdentity identity) => identity.Name.Split('\\').LastOrDefault() ?? "unknown";

    public static bool IsAllowed(this IIdentity identity, string[] allowedUserNames, Exception? throwIfFalseException = null) {
      var b = allowedUserNames.Contains(identity.LogonUserName(), StringComparer.OrdinalIgnoreCase);
      if (!b && throwIfFalseException != null)
        //throw new PermissionDeniedException();
        throw throwIfFalseException;
      return b;
    }

    public static string DebugString(this IIdentity identity, bool includeChildren = false) {
      var sb = new StringBuilder();
      sb.AppendLine($"    .Identity");
      sb.AppendLine($"      .AuthenticationType: {identity.AuthenticationType}");
      sb.AppendLine($"      .Name: {identity.Name}");
      sb.AppendLine($"      .IsAuthenticated: {identity.IsAuthenticated}");
      return sb.ToString();
    }


  }
}