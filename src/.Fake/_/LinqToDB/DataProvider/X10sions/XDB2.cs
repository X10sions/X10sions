using LinqToDB.DataProvider.DB2;
using System.Data.Common;
using X10sions.Fake.Data.Enums;

namespace LinqToDB.DataProvider.X10sions {
  public class XDB2LUWDataProvider<TConn, TDataReader> : BaseDataProvider<TConn, TDataReader> where TConn : DbConnection, new() where TDataReader : DbDataReader {
    public XDB2LUWDataProvider(ConnectionStringName name, int id) : base(name.ToString(), id, DB2Tools.GetDataProvider(DB2Version.LUW)) { }
  }
}
