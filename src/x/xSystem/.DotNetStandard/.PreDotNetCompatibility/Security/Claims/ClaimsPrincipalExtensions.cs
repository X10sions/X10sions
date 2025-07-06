namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions {

    [PreDotNetCompatibility("https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/PrincipalExtensions.cs")]
  public static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
    => principal is null ? throw new ArgumentNullException(nameof(principal)) : principal.FindFirst(claimType)?.Value;

  public static string? GetClaimEmail(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Email);
  public static string? GetClaimNameIdentifier(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.NameIdentifier);
  public static string? GetClaimName(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Name);

}