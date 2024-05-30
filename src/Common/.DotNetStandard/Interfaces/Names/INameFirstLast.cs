namespace Common.NetStandard.Interfaces.Names {
  public interface INameFirstLast {
    string FirstName { get; set; }

    string LastName { get; set; }
  }

  public static class INameFirstLast_Extensions {
    public static string FirstAndLastName(this INameFirstLast @this) => $"{(@this.FirstName ?? string.Empty).Trim()} {@this.LastName ?? string.Empty}".Trim();
  }

}
