namespace Common.NetStandard.Interfaces.Names;

public interface INameFirstLastKnownAs : INameFirstLast {
  string KnownAs { get; set; }
}

public static class INameFirstLastKnownAs_Extensions {

  public static string KnownAsName(this INameFirstLastKnownAs name) => string.IsNullOrWhiteSpace(name.KnownAs) ? name.FirstAndLastName() : $"{name.KnownAs.Trim()} {name.LastName ?? string.Empty}".Trim();
}

