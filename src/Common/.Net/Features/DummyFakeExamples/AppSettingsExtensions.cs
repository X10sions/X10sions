using Common.App.Settings;

namespace Common.Features.DummyFakeExamples {
  public static class AppSettingsExtensions {
    public static string Access_OleDb(this ConnectionStringsAppSettings cs) => cs[nameof(Access_OleDb)];
    public static string DB2_IBM(this ConnectionStringsAppSettings cs) => cs[nameof(DB2_IBM)];
    public static string DB2_Odbc(this ConnectionStringsAppSettings cs) => cs[nameof(DB2_Odbc)];
    public static string DB2_OleDb(this ConnectionStringsAppSettings cs) => cs[nameof(DB2_OleDb)];
    public static string DB2iSeries_IBM(this ConnectionStringsAppSettings cs) => cs[nameof(DB2iSeries_IBM)];
    public static string DB2iSeries_Odbc(this ConnectionStringsAppSettings cs) => cs[nameof(DB2iSeries_Odbc)];
    public static string DB2iSeries_OleDb(this ConnectionStringsAppSettings cs) => cs[nameof(DB2iSeries_OleDb)];
    public static string MariaDb(this ConnectionStringsAppSettings cs) => cs[nameof(MariaDb)];
    public static string MySql(this ConnectionStringsAppSettings cs) => cs[nameof(MySql)];
    public static string PostgreSql(this ConnectionStringsAppSettings cs) => cs[nameof(PostgreSql)];
    public static string Oracle(this ConnectionStringsAppSettings cs) => cs[nameof(Oracle)];
    public static string SqlServer(this ConnectionStringsAppSettings cs) => cs[nameof(SqlServer)];
    public static string Sqlite(this ConnectionStringsAppSettings cs) => cs[nameof(Sqlite)];
  }

}
