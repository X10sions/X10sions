namespace LinqToDB.DataProvider {
  /// <summary>
  /// Source code reference as at given date.
  /// </summary>
  public class UrlAsAt : Attribute {
    public UrlAsAt(DateTime asAt, string url) {
      AsAt = asAt;
      Url = url;
    }
    public UrlAsAt(int year, int month, int day, string url) : this(new DateTime(year, month, day), url) { }

    public DateTime AsAt { get; set; }
    public string Url { get; set; }

    public class AccessBulkCopy_2021_06_04 : UrlAsAt { public AccessBulkCopy_2021_06_04() : base(2021, 6, 4, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/Access/AccessBulkCopy.cs") { } }
    public class AccessMappingSchema_2021_05_07 : UrlAsAt { public AccessMappingSchema_2021_05_07() : base(2021, 5, 7, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/Access/AccessMappingSchema.cs") { } }
    public class AccessOdbcDataProviderDataProvider_2021_03_14 : UrlAsAt { public AccessOdbcDataProviderDataProvider_2021_03_14() : base(2021, 3, 14, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/Access/AccessODBCDataProvider.cs") { } }
    public class AccessOleDbDataProviderDataProvider_2021_03_14 : UrlAsAt { public AccessOleDbDataProviderDataProvider_2021_03_14() : base(2021, 3, 14, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/Access/AccessOleDbDataProvider.cs") { } }

    public class DB2DataProvider_2021_03_14 : UrlAsAt { public DB2DataProvider_2021_03_14() : base(2021, 3, 14, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/DB2/DB2DataProvider.cs") { } }
    public class DB2MappingSchema_2021_05_07 : UrlAsAt { public DB2MappingSchema_2021_05_07() : base(2021, 5, 7, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/DB2/DB2MappingSchema.cs") { } }

    public class SapHanaMappingSchema_2021_05_07 : UrlAsAt { public SapHanaMappingSchema_2021_05_07() : base(2021, 5, 7, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/SapHana/SapHanaMappingSchema.cs") { } }

    public class SqlServerMappingSchema_2021_05_07 : UrlAsAt { public SqlServerMappingSchema_2021_05_07() : base(2021, 5, 7, "https://github.com/linq2db/linq2db/blob/master/Source/LinqToDB/DataProvider/SqlServer/SqlServerMappingSchema.cs") { } }

  }
}