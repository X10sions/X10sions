using LinqToDB.DataProvider;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public class xDB2iSeriesDataProviderOptions {
    public DB2iSeriesIdentifierQuoteMode IdentifierQuoteMode { get; set; } = DB2iSeriesIdentifierQuoteMode.Auto;
    public DB2iSeriesNamingConvention NamingConvention { get; set; } = DB2iSeriesNamingConvention.System;
    public DB2iSeriesVersionRelease VersionRelease { get; set; } = DB2iSeriesVersionRelease.V5R4;
    public bool IsVersion7_2orLater => VersionRelease >= DB2iSeriesVersionRelease.V7_2;
  }

  public interface IDB2iSeriesDataProviderOptions : IDataProvider {
    xDB2iSeriesDataProviderOptions DataProviderOptions { get; }
  }

}
