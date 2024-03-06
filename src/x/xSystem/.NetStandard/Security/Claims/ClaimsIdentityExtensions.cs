using System.Collections.Generic;
using System.Text;

namespace System.Security.Claims {
  public static class ClaimsIdentityExtensions {

    public static string DebugString(this ClaimsIdentity claimsIdentity, bool includeChildren = false) {
      var sb = new StringBuilder();
      sb.AppendLine($"  ClaimsIdentity:");
      sb.AppendLine($"    .Actor: {claimsIdentity.Actor.DebugString(includeChildren)}");
      sb.AppendLine($"    .AuthenticationType: {claimsIdentity.AuthenticationType}");
      sb.AppendLine($"    .BootstrapContext: {claimsIdentity.BootstrapContext}");
      sb.AppendLine($"    .Claims: {claimsIdentity.Claims.DebugString(x => x.DebugString(includeChildren))}");
      sb.AppendLine($"    .IsAuthenticated: {claimsIdentity.IsAuthenticated}");
      sb.AppendLine($"    .Label: {claimsIdentity.Label}");
      sb.AppendLine($"    .Name: {claimsIdentity.Name}");
      sb.AppendLine($"    .NameClaimType: {claimsIdentity.NameClaimType}");
      sb.AppendLine($"    .RoleClaimType: {claimsIdentity.RoleClaimType}");
      if (includeChildren) {
      }
      return sb.ToString();
    }

  }
}
