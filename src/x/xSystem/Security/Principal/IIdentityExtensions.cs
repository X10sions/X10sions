namespace System.Security.Principal {
  public static class IIdentityExtensions {

    public static string LogonDomainName(this IIdentity identity) => identity.Name.Split('\\')[0] ?? "-unknown-";
    public static string LogonUserName(this IIdentity identity) => identity.Name.Split('\\').LastOrDefault() ?? "-unknown-";

    //public static string LogonUserNamex(this IIdentity identity) {
    //  var text = identity.Name.ToUpper() ?? "-Unknown-";
    //  return text.Mid(text.IndexOf("\\") + 1);
    //}

    //public static void IsLogonUserAllowed(this IIdentity identity, string[] allowedUsers) {
    //  var logonUserName = identity.LogonUserName();
    //  if (!allowedUsers.Contains(logonUserName)) {
    //    throw new UnauthorizedAccessException(logonUserName);
    //  }
    //}

    public static bool IsAllowed(this IIdentity identity, string[] allowedUserNames, bool throwExceptionIfNotAllowed) {
      var logonUserName = identity.LogonUserName();
      var b = allowedUserNames.Contains(logonUserName, StringComparer.OrdinalIgnoreCase);
      if (!b && throwExceptionIfNotAllowed) throw new UnauthorizedAccessException(logonUserName);
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