using System;
using System.Collections.Generic;

namespace Common.Data {
  public class DataSourceProduct : IEquatable<DataSourceProduct> {
    public DataSourceProduct(string name, DbSystem dbSystem) {
      Name = name;
      DbSystem = dbSystem;
      List.Add(this);
    }
    public string Name { get; }
    public DbSystem DbSystem { get; }

    public bool Equals(DataSourceProduct? other) => other != null && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is DataSourceProduct && Equals(obj as DataSourceProduct);
    public override int GetHashCode() => HashCode.Combine(Name);

    public class Names {
      public const string Access = "ACCESS";
      public const string DB2_400_SQL = "DB2/400 SQL";
      public const string Excel = "EXCEL";
      public const string Microsoft_SQL_Server = "Microsoft SQL Server";
    }

    public static HashSet<DataSourceProduct> List = new HashSet<DataSourceProduct>();

    public static DataSourceProduct Access { get; } = new DataSourceProduct(Names.Access, DbSystem.Access);
    public static DataSourceProduct DB2_400_SQL { get; } = new DataSourceProduct(Names.DB2_400_SQL, DbSystem.DB2iSeries);
    public static DataSourceProduct Excel { get; } = new DataSourceProduct(Names.Excel, DbSystem.Excel);
    public static DataSourceProduct Microsoft_SQL_Server { get; } = new DataSourceProduct(Names.Microsoft_SQL_Server, DbSystem.SqlServer);

  }
}