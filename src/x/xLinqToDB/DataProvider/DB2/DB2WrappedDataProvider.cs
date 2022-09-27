using Common.Data;
using LinqToDB.DataProvider.DB2;
using System.Data.Common;

namespace LinqToDB.DataProvider.X10sions {
  public class DB2WrappedDataProvider<TConn, TDataReader> : WrappedDataProvider<TConn, TDataReader> where TConn : DbConnection, new() where TDataReader : DbDataReader {
    public DB2WrappedDataProvider(int id) : base(DbSystem.DB2, id, DB2Tools.GetDataProvider(DB2Version.LUW)) { }
  }
}
