
namespace X10sions.Fake.Data.Enums {
  //public enum DbTypes {
  //  Access_Odbc,
  //  Access_Oledb,
  //  DB2_IBM,
  //  DB2_Odbc,
  //  DB2iSeries_IBM,
  //  DB2iSeries_Odbc,
  //  DB2iSeries_OleDb,
  //  Firebird,
  //  MariaDb,
  //  MySql,
  //  MySql_Connector,
  //  PostgreSql,
  //  Oracle,
  //  SqlServer,
  //  Sqlite,
  //}

  public static class DbTypesExtensions{

    public static string GetConnectionStrings(this DbTypes dbType) => DbTypes switch { };
  
  }

  //public enum DbConnectionTypes { 
  //  OleDb,
  //  Odbc
  
  //}
}
