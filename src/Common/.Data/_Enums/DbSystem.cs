using Ardalis.SmartEnum;

namespace Common.Data;
public abstract class DbSystem : SmartEnum<DbSystem> {
  public static readonly DbSystem Access = new SealedDbSystem(nameof(Access));
  public static readonly DbSystem DB2 = new SealedDbSystem(nameof(DB2));
  //public static readonly DbSystem DB2iSeries = new SealedDbSystem(nameof(DB2iSeries));
  public static readonly DbSystem DB2iSeries = new DB2iSeriesDbSystem();
  public static readonly DbSystem Excel = new SealedDbSystem(nameof(Excel));
  public static readonly DbSystem Firebird = new SealedDbSystem(nameof(Firebird));
  //public static readonly DbSystem Informix = new SealedDbSystem(nameof(Informix));
  public static readonly DbSystem MariaDb = new SealedDbSystem(nameof(MariaDb));
  public static readonly DbSystem MongoDb = new SealedDbSystem(nameof(MongoDb));
  public static readonly DbSystem MySql = new SealedDbSystem(nameof(MySql));
  public static readonly DbSystem Oracle = new SealedDbSystem(nameof(Oracle));
  public static readonly DbSystem PostgreSql = new SealedDbSystem(nameof(PostgreSql));
  public static readonly DbSystem Redis = new SealedDbSystem(nameof(Redis));
  //public static readonly DbSystem SapHana = new SealedDbSystem(nameof(SapHana));
  public static readonly DbSystem Sqlite = new SealedDbSystem(nameof(Sqlite));
  //public static readonly DbSystem SqlCE = new SealedDbSystem(nameof(SqlCE));
  //public static readonly DbSystem SqlServer = new SealedDbSystem(nameof(SqlServer));
  public static readonly DbSystem SqlServer = new SqlServerDbSystem();
  //public static readonly DbSystem Sybase = new SealedDbSystem(nameof(Sybase));

  private DbSystem(string name) : base(name, count++) { }
  //private DbSystem(string name) : base(name, List == null ? 0 : List.Count) { }

  static int count = -1;

  #region SealedClasses

  public sealed class SealedDbSystem : DbSystem {
    public SealedDbSystem(string name) : base(name) { }
  }

  public sealed class DB2iSeriesDbSystem : DbSystem {
    public DB2iSeriesDbSystem() : base(nameof(DB2iSeries)) { }

    public enum ReleaseVerion {
      V1 = 19880826,
      V2R1 = 19910524,
      V2R1M1 = 19920306,
      V2R2 = 19920918,
      V2R3 = 19931717,
      /*
       * R7.5 SSP	= 19960220

       */
      V3R0M5 = 19940603,
      V3R1 = 19941125,
      V3R2 = 19960621,
      V3R6 = 19951222,
      V3R7 = 19961108,
      V4R1 = 19970829,
      V4R2 = 19980227,
      V4R3 = 19980911,
      V4R4 = 19990521,
      V4R5 = 20000728,
      V5R1 = 20010525,
      V5R2 = 20020830,
      V5R3 = 20040611,
      V5R4 = 20060214,
      V6R1 = 20080321,
      V7R1 = 20100423,
      V7R2 = 20140502,
      V7R3 = 20160415,
      V7R4 = 20190621
    }

    public static ReleaseVerion GetReleaseVerion(Version version) {
      // https://wiki.midrange.com/index.php/History_of_OS/400
      // https://www.ibm.com/support/pages/release-life-cycle
      return version switch {
        //{ Major: 15 } => ReleaseVerion.v2019,
        //{ Major: 14 } => ReleaseVerion.v2017,
        //{ Major: 13 } => ReleaseVerion.v2016,
        //{ Major: 12 } => ReleaseVerion.v2014,
        //{ Major: 11 } => ReleaseVerion.v2012,
        //{ Major: 10 } => version.Minor > 0 ? ReleaseVerion.v2008_R2 : ReleaseVerion.v2008,
        //{ Major: 9 } => ReleaseVerion.v2005,
        //{ Major: 8 } => ReleaseVerion.v2000,
        //{ Major: 7 } => ReleaseVerion.v7,
        //{ Major: 6 } => version.Minor > 0 ? ReleaseVerion.v6_5 : ReleaseVerion.v6,
        _ => throw new NotImplementedException($"Unknown version: {version}")
      };
    }
  }

  public sealed class SqlServerDbSystem : DbSystem {
    public SqlServerDbSystem() : base(nameof(SqlServer)) { }

    public enum ReleaseVerion { v6, v6_5, v7, v2000, v2005, v2008, v2008_R2, v2012, v2014, v2016, v2017, v2019 }

    public static ReleaseVerion GetReleaseVerion(Version version) {
      //https://sqlserverbuilds.blogspot.com/
      return version switch { { Major: 15 } => ReleaseVerion.v2019, { Major: 14 } => ReleaseVerion.v2017, { Major: 13 } => ReleaseVerion.v2016, { Major: 12 } => ReleaseVerion.v2014, { Major: 11 } => ReleaseVerion.v2012, { Major: 10 } => version.Minor > 0 ? ReleaseVerion.v2008_R2 : ReleaseVerion.v2008, { Major: 9 } => ReleaseVerion.v2005, { Major: 8 } => ReleaseVerion.v2000, { Major: 7 } => ReleaseVerion.v7, { Major: 6 } => version.Minor > 0 ? ReleaseVerion.v6_5 : ReleaseVerion.v6,
        _ => throw new NotImplementedException($"Unknown version: {version}")
      };
    }
  }

  #endregion

}
public static class DbSystemExtensions {
  public static List<DataSourceProduct> DataSourceProductList(this DbSystem dbSystem) => DataSourceProduct.List.Where(x => x.DbSystem.Name == dbSystem.Name).ToList();
}