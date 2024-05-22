using System.Security.Principal;

namespace System.Security.Claims;
public static class ClaimsPrincipalExtensions {

  /// <summary> https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/PrincipalExtensions.cs </summary>
  public static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) 
    => principal is null ? throw new ArgumentNullException(nameof(principal)) : principal.FindFirst(claimType)?.Value;

  public static string GetClaimEmail(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Email);
  public static string GetClaimNameIdentifier(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.NameIdentifier);
  public static string GetClaimName(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Name);

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