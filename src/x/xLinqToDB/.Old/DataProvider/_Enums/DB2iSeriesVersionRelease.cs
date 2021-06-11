using System;

namespace xLinqToDB.DataProvider.DB2iSeries {
  public enum DB2iSeriesVersionRelease {
    // https://www.ibm.com/support/pages/release-life-cycle
    V5R1 = 20010525,
    V5R2 = 20020830,
    V5R3 = 20040611,
    V5R4 = 20060214,
    V6_1 = 20080321,
    V7_1 = 20100423,
    V7_2 = 20140502,
    V7_3 = 20160415,
    V7_4 = 20190621
  }

  public static class DB2iSeriesVersionExtensions {

    public static bool SupportsOffsetClause(this DB2iSeriesVersionRelease version) => version >= DB2iSeriesVersionRelease.V7_3;
    public static bool SupportsTruncateTable(this DB2iSeriesVersionRelease version, DB2iSeriesConnectionType connectionType) => version >= DB2iSeriesVersionRelease.V7_2 && connectionType != DB2iSeriesConnectionType.Odbc;
    public static bool SupportsMergeStatement(this DB2iSeriesVersionRelease version) => version >= DB2iSeriesVersionRelease.V7_1;
    public static bool SupportsNCharTypes(this DB2iSeriesVersionRelease version) => version >= DB2iSeriesVersionRelease.V7_1;
   
    public static DB2iSeriesVersionRelease GetDB2iSeriesVersionRelease(this Version version) => version switch {
      var x when x.Major == 7 && x.Minor == 4 => DB2iSeriesVersionRelease.V7_4,
      var x when x.Major == 7 && x.Minor == 3 => DB2iSeriesVersionRelease.V7_3,
      var x when x.Major == 7 && x.Minor == 2 => DB2iSeriesVersionRelease.V7_2,
      var x when x.Major == 7 && x.Minor == 1 => DB2iSeriesVersionRelease.V7_1,
      var x when x.Major == 6 && x.Minor == 1 => DB2iSeriesVersionRelease.V6_1,
      var x when x.Major == 5 && x.Minor == 4 => DB2iSeriesVersionRelease.V5R4,
      _ => throw new Exception($"Unknown version: {version}")
    };

  }
}