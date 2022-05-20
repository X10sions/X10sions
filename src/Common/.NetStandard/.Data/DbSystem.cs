using System.Data;

namespace Common.Data {
  public class DbSystem : IEquatable<DbSystem> {
    public DbSystem(string name) {
      Name = name;
      List.Add(this);
    }

    public string Name { get; }
    public List<DataSourceProduct> DataSourceProductList => DataSourceProduct.List.Where(x => x.DbSystem.Name == Name).ToList();

    public bool Equals(DbSystem? other) => other != null && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is DbSystem && Equals(obj as DbSystem);
    public override int GetHashCode() => HashCode.Combine(Name);

    public enum Enum {
      _Unkown,
      Access,
      DB2,
      DB2iSeries,
      Excel,
      Firebird,
      Informix,
      MySql,
      Oracle,
      PostgreSql,
      SapHana,
      Sqlite,
      SqlCE,
      SqlServer,
      Sybase
    }

    public class Names {
      public const string Access = nameof(Access);
      public const string DB2 = nameof(DB2);
      public const string DB2iSeries = nameof(DB2iSeries);
      public const string Excel = nameof(Excel);
      public const string Firebird = nameof(Firebird);
      //public const string Informix = nameof(Informix);
      public const string MySql = nameof(MySql);
      public const string Oracle = nameof(Oracle);
      public const string PostgreSql = nameof(PostgreSql);
      public const string SapHana = nameof(SapHana);
      public const string Sqlite = nameof(Sqlite);
      //public const string SqlCE = nameof(SqlCE);
      public const string SqlServer = nameof(SqlServer);
      //public const string Sybase = nameof(Sybase);
    }

    public static HashSet<DbSystem> List = new HashSet<DbSystem>();

    public static DbSystem Access { get; } = new DbSystem(Names.Access);
    public static DbSystem DB2 { get; } = new DbSystem(Names.DB2);
    public static DbSystem DB2iSeries { get; } = new DB2iSeriesDbSystem();
    public static DbSystem Excel { get; } = new DbSystem(Names.Excel);
    public static DbSystem SqlServer { get; } = new SqlServerDbSystem();

  }

  public class DB2iSeriesDbSystem : DbSystem {
    public DB2iSeriesDbSystem() : base(Names.DB2iSeries) { }

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

  public class SqlServerDbSystem : DbSystem {
    public SqlServerDbSystem() : base(Names.SqlServer) { }

    public enum ReleaseVerion { v6, v6_5, v7, v2000, v2005, v2008, v2008_R2, v2012, v2014, v2016, v2017, v2019 }

    public static ReleaseVerion GetReleaseVerion(Version version) {
      //https://sqlserverbuilds.blogspot.com/
      return version switch { { Major: 15 } => ReleaseVerion.v2019, { Major: 14 } => ReleaseVerion.v2017, { Major: 13 } => ReleaseVerion.v2016, { Major: 12 } => ReleaseVerion.v2014, { Major: 11 } => ReleaseVerion.v2012, { Major: 10 } => version.Minor > 0 ? ReleaseVerion.v2008_R2 : ReleaseVerion.v2008, { Major: 9 } => ReleaseVerion.v2005, { Major: 8 } => ReleaseVerion.v2000, { Major: 7 } => ReleaseVerion.v7, { Major: 6 } => version.Minor > 0 ? ReleaseVerion.v6_5 : ReleaseVerion.v6,
        _ => throw new NotImplementedException($"Unknown version: {version}")
      };
    }
  }

}