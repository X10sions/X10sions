using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Data.Odbc;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;

public class DB2iSeriesOdbcRelationalConnection : RelationalConnection {
  public DB2iSeriesOdbcRelationalConnection(RelationalConnectionDependencies dependencies) : base(dependencies) { }

  protected override DbConnection CreateDbConnection() {
    var ext = Dependencies.ContextOptions.FindExtension<DB2iSeriesOptionsExtension>() ?? throw new InvalidOperationException("DB2iSeriesOptionsExtension missing.");
    if (ext.ExistingConnection != null) return ext.ExistingConnection;
    var cs = ext.ConnectionString ?? throw new InvalidOperationException("ConnectionString not set.");
    return new OdbcConnection(cs);
  }

}

