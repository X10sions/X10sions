using IBM.Data.DB2.iSeries;

namespace xEFCore.xAS400 {
  public class EFCoreConstants_AS400 : IEFCoreConstants {

    public string IdentityColumnSql => iDB2Constants.IdentityColumnSql;
    public string RowCountColumnSql => throw new System.NotImplementedException();

  }
}
