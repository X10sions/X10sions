namespace FreeSql;
public static partial class FreeSqlOdbcGlobalExtensions {

  internal static string FormatOdbcDB2iSeries(this string that, params object[] args) => _odbcDB2iSeriesAdo.Addslashes(that, args);
  static Odbc.DB2iSeries.OdbcDB2iSeriesAdo _odbcDB2iSeriesAdo = new Odbc.DB2iSeries.OdbcDB2iSeriesAdo();
}