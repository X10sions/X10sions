namespace Common.NetStandard.Interfaces.Names {
  public interface INameFirstLastMiddle : INameFirstLast {
    string MiddleName { get; set; }
  }
  public static class INameFirstLastMiddle_Extensions {
    public static string FullName(this INameFirstLastMiddle @this) => $"{(@this.FirstName ?? string.Empty).Trim()} {(@this.MiddleName ?? string.Empty).Trim()} {(@this.LastName ?? string.Empty).Trim()}".Trim();
  }

}
