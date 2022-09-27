using Ardalis.SmartEnum;

namespace Common.Data;
public sealed class OdbcDriver : SmartEnum<OdbcDriver> {
  public static readonly OdbcDriver Firebird_InterBase = new OdbcDriver("Firebird/InterBase(r) driver", DbSystem.Firebird);
  public static readonly OdbcDriver IBM_DB2 = new OdbcDriver("{IBM DB2 ODBC DRIVER}", DbSystem.DB2);
  // https://www.ibm.com/docs/en/i/7.1?topic=keywords-connection-string-general-properties
  // ;MGDSN=0
  public static readonly OdbcDriver IBM_i_Access = new OdbcDriver("{IBM i Access ODBC Driver}", DbSystem.DB2iSeries);
  public static readonly OdbcDriver Microsoft_Access = new OdbcDriver("{Microsoft Access Driver (*.mdb, *.accdb)}", DbSystem.Access);
  public static readonly OdbcDriver Microsoft_Excel = new OdbcDriver("{Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}", DbSystem.Excel);
  public static readonly OdbcDriver MySQL_8_ANSI = new OdbcDriver("{MySQL ODBC 8.0 ANSI Driver}", DbSystem.MySql);
  public static readonly OdbcDriver MySQL_8_Unicode = new OdbcDriver("{MySQL ODBC 8.0 Unicode Driver}", DbSystem.MySql);
  public static readonly OdbcDriver Microsoft_Oracle = new OdbcDriver("{Microsoft ODBC for Oracle}", DbSystem.Oracle);
  public static readonly OdbcDriver Oracle_in_OraDB18Home1 = new OdbcDriver("{Oracle in OraDB18Home1}", DbSystem.Oracle);
  public static readonly OdbcDriver PostgreSQL = new OdbcDriver("{PostgreSQL}", DbSystem.PostgreSql);
  public static readonly OdbcDriver SQLite3 = new OdbcDriver("SQLite3 ODBC Driver", DbSystem.Sqlite);
  public static readonly OdbcDriver SQLServer_17 = new OdbcDriver("{ODBC Driver 17 for SQL Server}", DbSystem.SqlServer);

  private OdbcDriver(string name, DbSystem dbSystem) : base(name, List.Count) {
    DbSystem = dbSystem;
  }
  public DbSystem DbSystem { get; }

}