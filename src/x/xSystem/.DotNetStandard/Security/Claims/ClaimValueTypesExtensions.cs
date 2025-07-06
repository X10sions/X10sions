namespace System.Security.Claims;
public static class ClaimValueTypesExtensions {

  public static string? GetClaimValueTypes<T>(this T value) => value switch {
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
