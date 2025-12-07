using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Data.Odbc;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;

public class DB2iSeriesOdbcRelationalConnection : RelationalConnection {
  public DB2iSeriesOdbcRelationalConnection(RelationalConnectionDependencies dependencies) : base(dependencies) { }

  protected override DbConnection CreateDbConnection() => new OdbcConnection(ConnectionString);

}