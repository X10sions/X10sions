using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper {
  public static class IDbConnectionExtensions {
    public static List<T> QueryToList<T>(this IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
      => dbConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType).ToList();
  }
}
