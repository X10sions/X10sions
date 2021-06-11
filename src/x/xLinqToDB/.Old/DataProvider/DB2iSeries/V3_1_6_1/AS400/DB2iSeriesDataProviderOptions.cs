//using LinqToDB.DataProvider;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.AS400 {

//  //public interface IDB2iSeriesDataProviderOptions : IDB2iSeriesNamingConvention {
//  //  public DB2iSeriesIdentifierQuoteMode IdentifierQuoteMode { get; set; }
//  //  public LinqToDBVersion LinqToDBVersion { get; set; }
//  //  public DB2iSeriesVersion Version { get; set; }
//  //}

//  public interface IDB2iSeriesDataProviderOptions : IDataProvider {
//    public DB2iSeriesDataProviderOptions DataProviderOptions { get; }
//  }

//  //public static class IDB2iSeriesDataProviderOptionsExtensions {
//  //  public static bool IsVersion7_2orLater(this DB2iSeriesDataProviderOptions options) => options.Version >= DB2iSeriesVersion.v7r2;
//  //  public static bool IsMapGuidAsString(this DB2iSeriesDataProviderOptions options) => options.IsVersion7_2orLater();
//  //  public static bool IsOffsetFirst(this DB2iSeriesDataProviderOptions options, bool defaultValue) => options.LinqToDBVersion == LinqToDBVersion.V2_9 ? options.IsVersion7_2orLater() ? true : false : defaultValue;
//  //  public static bool CanBuildTruncateTableStatement(this DB2iSeriesDataProviderOptions options) => options.IsVersion7_2orLater();
//  //}

//  public class DB2iSeriesDataProviderOptions {
//    public DB2iSeriesIdentifierQuoteMode IdentifierQuoteMode { get; set; } = DB2iSeriesIdentifierQuoteMode.Auto;
//    public DB2iSeriesNamingConvention NamingConvention { get; set; } = DB2iSeriesNamingConvention.System;
//    public DB2iSeriesVersion Version { get; set; } = DB2iSeriesVersion.v5r4;

//    public bool IsVersion7_2orLater => (int)Version >= (int)DB2iSeriesVersion.v7r2;
//    //IsMapGuidAsString public bool IsMapGuidAsString => !IsVersion7_2orLater;
//    //public bool IsOffsetFirst( bool defaultValue) => LinqToDBVersion == LinqToDBVersion.V2_9 ? IsVersion7_2orLater ? true : false : defaultValue;
//    //public bool CanBuildTruncateTableStatement => IsVersion7_2orLater;
//  }

//}