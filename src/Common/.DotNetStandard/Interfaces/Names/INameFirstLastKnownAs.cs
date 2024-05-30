namespace Common.NetStandard.Interfaces.Names {
  public interface INameFirstLastKnownAs : INameFirstLast {
    string KnownAs { get; set; }
  }

  public static class INameFirstLastKnownAs_Extensions {
    public static string KnownAsName(this INameFirstLastKnownAs @this) => string.IsNullOrWhiteSpace(KnownAsName(@this))
        ? INameFirstLast_Extensions.FirstAndLastName(@this)
        : $"{(@this.KnownAs ?? string.Empty).Trim()} {@this.LastName ?? string.Empty}".Trim();
  }

}
