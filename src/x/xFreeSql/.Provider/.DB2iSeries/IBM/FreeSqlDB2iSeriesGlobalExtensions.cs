namespace FreeSql.DB2iSeries;

public static partial class FreeSqlDB2iSeriesGlobalExtensions {

  /// <summary>
  /// 特殊处理类似 string.Format 的使用方法，防止注入，以及 IS NULL 转换
  /// </summary>
  /// <param name="that"></param>
  /// <param name="args"></param>
  /// <returns></returns>
  public static string FormatDB2iSeries(this string that, params object[] args) => _DB2iSeriesAdo.Addslashes(that, args);
  static FreeSql.DB2iSeries.DB2iSeriesAdo _DB2iSeriesAdo = new FreeSql.DB2iSeries.DB2iSeriesAdo();
}
