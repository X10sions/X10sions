using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Data.Common;
using System.Data.Odbc;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Design;

public sealed class DB2iSeriesDatabaseModelFactory : IDatabaseModelFactory {
  public DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options) {
    using var con = new OdbcConnection(connectionString);
    con.Open();
    var model = new DatabaseModel();
    // TODO: Query catalogs (QSYS2.SYSCOLUMNS, SYSTABLES) respecting options.Schemas and options.Tables
    // Populate model.Tables, Columns, PrimaryKeys, ForeignKeys
    return model;
  }

  public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options) => throw new NotImplementedException();

}
