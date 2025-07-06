using LinqToDB;
using LinqToDB.Data;
using System.Data.Common;

namespace RCommon.Persistence.Linq2Db;

public class RCommonDataConnection : DataConnection, IDataStore {
  public RCommonDataConnection(DataOptions linq2DbOptions) : base(linq2DbOptions) { }
  public DbConnection GetDbConnection() => this.Connection;
}
