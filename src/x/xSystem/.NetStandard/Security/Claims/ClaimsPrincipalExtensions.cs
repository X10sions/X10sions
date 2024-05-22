using System.Security.Principal;
using xSystem.NetStandard;

namespace System.Security.Claims;
public static class ClaimsPrincipalExtensions {

  public static string? GetClaimEmail(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Email);
  public static string? GetClaimNameIdentifier(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.NameIdentifier);
  public static string? GetClaimName(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Name);

  //public static bool IsCurrentUser(this ClaimsPrincipal principal, string id) => string.Equals(GetClaimUserId(principal), id, StringComparison.OrdinalIgnoreCase);

  public static string DebugString(this ClaimsPrincipal claimsPrincipal, bool includeChildren = false) {
    var sb = new StringBuilder();
    sb.AppendLine($"  ClaimsPrincipal:");
    sb.AppendLine(claimsPrincipal.Identity.DebugString(includeChildren));
    sb.AppendLine($"    .Claims: {claimsPrincipal.Claims.DebugString(x => x.DebugString(includeChildren))}");
    sb.AppendLine($"    .Identities: {claimsPrincipal.Identities.DebugString(x => x.DebugString(includeChildren))}");
    return sb.ToString();
  }

  //public static T NameIdentifierClaimValue<T>(this ClaimsPrincipal cp) => cp.Claims.FirstOrDefault(x=> x.Type== ClaimTypes.NameIdentifier)?.Value.As(-1);
  //public static T NameIdentifierClaim<T>(this ClaimsPrincipal cp) => cp.Claims.FirstOrDefault(x=> x.Type== ClaimTypes.NameIdentifier)?.Value.As(-1);

  public static T GetClaimValueByType<T>(this ClaimsPrincipal cp, string claimType, T defaultValue) => cp.Claims.GetValueByType(claimType, defaultValue);

}