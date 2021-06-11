namespace Common.Interfaces {
  public interface IKnownAsName {
    string FirstName { get; set; }
    string LastName { get; set; }
    string PreferedFirstName { get; set; }
  }

  public static class IKnownAsNameExtensions {
    public static string KnownAsName(this IKnownAsName p) => (p.PreferedFirstName ?? p.FirstName).Trim() + " " + p.LastName.Trim();
  }

}

