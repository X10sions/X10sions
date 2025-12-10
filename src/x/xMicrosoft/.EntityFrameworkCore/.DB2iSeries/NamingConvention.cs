namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

public enum NamingConvention {
  SYS = 0,
  SQL = 1,
}

public static class NamingConventionExtensions {
  public static string GetQualifiedPrefix(this NamingConvention naming) => naming switch {
        NamingConvention.SYS => "/",
        NamingConvention.SQL => ".",
        _ => throw new ArgumentOutOfRangeException(nameof(naming), naming, null),
      };
  public static string GetQualifiedName(this NamingConvention naming, string schema, string table) => $"{schema}{naming.GetQualifiedPrefix()}{table}";

}