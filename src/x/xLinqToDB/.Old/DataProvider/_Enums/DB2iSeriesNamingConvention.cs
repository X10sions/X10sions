namespace xLinqToDB.DataProvider.DB2iSeries {
  public enum DB2iSeriesNamingConvention {
    Sql,
    System
  }

  public interface IDB2iSeriesNamingConvention {
    DB2iSeriesNamingConvention NamingConvention { get; set; }
  }

  public static class DB2iSeriesNamingConventionExtensions {
    public static string DummyTableWithSchema(this DB2iSeriesNamingConvention naming) => DB2iSeriesConstants.DummyTableSchema + naming.Separator() + DB2iSeriesConstants.DummyTableName;
    public static string QualifiedObject(this DB2iSeriesNamingConvention naming, string prefix, string suffix) => prefix + naming.Separator() + suffix;
    public static string SelectIdentityFromDummyTableSql(this DB2iSeriesNamingConvention naming) => $"SELECT {DB2iSeriesConstants.IdentityColumnSql} FROM {naming.DummyTableWithSchema()}";
    public static string Separator(this DB2iSeriesNamingConvention naming) => naming != DB2iSeriesNamingConvention.System ? "." : "/";

  }

}
