namespace Microsoft.Graph.Models;

public static class EmailAddressExtensions {

  public static string LocalPart(this EmailAddress emailAddress) => emailAddress.Address?.Split('@')[0] ?? string.Empty;
  public static string DomainPart(this EmailAddress emailAddress) => emailAddress.Address?.Split('@')[1] ?? string.Empty;

}