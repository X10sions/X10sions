namespace System.Security.Claims;
public static class ClaimExtensions {

  public static string DebugString(this Claim claim, bool includeChildren = false) {
    var sb = new StringBuilder();
    sb.AppendLine($"  Claim:");
    sb.AppendLine($"    .Issuer: {claim.Issuer}");
    sb.AppendLine($"    .OriginalIssuer: {claim.OriginalIssuer}");
    sb.AppendLine($"    .Properties: {claim.Properties}");
    sb.AppendLine($"    .Type: {claim.Type}");
    sb.AppendLine($"    .Value: {claim.Value}");
    sb.AppendLine($"    .ValueType: {claim.ValueType}");
    if (includeChildren) {
      sb.AppendLine(claim.Subject.DebugString(includeChildren));
    }
    return sb.ToString();
  }

  public static void Add<T>(this List<Claim> claims, string type, T value, string? valueType = null, string? issuer = null, string? orignialIssuer = null) {
    if (valueType == null) {
      valueType = value switch {
        bool => ClaimValueTypes.Boolean,
        DateTime dt => dt.Date == dt ? ClaimValueTypes.Date : ClaimValueTypes.DateTime,
        double => ClaimValueTypes.Double,
        int => ClaimValueTypes.Integer32,
        long => ClaimValueTypes.Integer64,
        string => ClaimValueTypes.String,
        TimeSpan => ClaimValueTypes.Time,
        uint => ClaimValueTypes.UInteger32,
        ulong => ClaimValueTypes.UInteger64,
        _ => null
      };
    }
    claims.Add(new Claim(type, value?.ToString(), valueType, issuer, orignialIssuer));
  }
  public static void AddBoolean(this List<Claim> claims, string type, bool value) => claims.Add(type, value, ClaimValueTypes.Boolean);
  public static void AddDate(this List<Claim> claims, string type, DateTime value) => claims.Add(type, value.Date, ClaimValueTypes.Date);
  public static void AddDateTime(this List<Claim> claims, string type, DateTime value) => claims.Add(type, value, ClaimValueTypes.DateTime);
  public static void AddEmail(this List<Claim> claims, string type, string value) => claims.Add(type, value, ClaimValueTypes.Email);
  public static void AddInteger(this List<Claim> claims, string type, int value) => claims.Add(type, value, ClaimValueTypes.Integer);
  public static void AddString(this List<Claim> claims, string type, string value) => claims.Add(type, value, ClaimValueTypes.String);
  public static void AddTime(this List<Claim> claims, string type, DateTime value) => claims.Add(type, value.TimeOfDay, ClaimValueTypes.Time);

  public static Claim? GetByType(this IEnumerable<Claim> claims, string claimType) => claims.FirstOrDefault(x => x.Type == claimType);

  public static Claim? GetNameIdentifierClaim(this IEnumerable<Claim> claims) => claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
  public static Claim? GetNameClaim(this IEnumerable<Claim> claims) => claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
  public static Claim? GetEmailClaim(this IEnumerable<Claim> claims) => claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

  public static T GetValueByType<T>(this IEnumerable<Claim> claims, string claimType, T defaultValue) {
    var claim = claims.GetByType(claimType);
    return claim == null ? defaultValue : claim.Value.As(defaultValue) ?? defaultValue;
  }

}