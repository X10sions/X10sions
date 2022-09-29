using Ardalis.SmartEnum;

namespace Common.Data;
public sealed class OleDbProvider : SmartEnum<OleDbProvider> {
  public static readonly OleDbProvider DB2OLEDB = new OleDbProvider("DB2OLEDB", DbSystem.DB2);
  public static readonly OleDbProvider IBMDADB2 = new OleDbProvider("IBMDADB2", DbSystem.DB2);
  // https://www.ibm.com/support/pages/access-client-solutions-ole-db-custom-connection-properties
  public static readonly OleDbProvider IBMDA400 = new OleDbProvider("IBMDA400", DbSystem.DB2iSeries);
  public static readonly OleDbProvider Microsoft_ACE_OLEDB_12_0_Access = new OleDbProvider("Microsoft.ACE.OLEDB.12.0", DbSystem.Access);
  public static readonly OleDbProvider Microsoft_ACE_OLEDB_12_0_Excel = new OleDbProvider("Microsoft.ACE.OLEDB.12.0", DbSystem.Excel);
  public static readonly OleDbProvider MySQLProv = new OleDbProvider("MySQLProv", DbSystem.MySql);
  public static readonly OleDbProvider MSDAORA = new OleDbProvider("MSDAORA", DbSystem.Oracle);
  public static readonly OleDbProvider OraOLEDB_Oracle = new OleDbProvider("OraOLEDB.Oracle", DbSystem.Oracle);
  public static readonly OleDbProvider PostgreSQL_OLE_DB_Provider = new OleDbProvider("PostgreSQL OLE DB Provider", DbSystem.PostgreSql);
  public static readonly OleDbProvider MSOLEDBSQL = new OleDbProvider("MSOLEDBSQL", DbSystem.SqlServer);

  private OleDbProvider(string name, DbSystem dbSystem) : base(name, List.Count) {
    DbSystem = dbSystem;
  }

  public DbSystem DbSystem { get; }

}