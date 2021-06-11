using BBaithwaite;
using Dapper;
using System.Linq;

namespace System.Data {
  public static class IDbConnectionExtensions {

    public static T DynamicQueryInsert<T>(this IDbConnection cnn, string tableName, object param) => SqlMapper.Query<T>(cnn, DynamicQuery.GetInsertQuery(tableName, param), param).FirstOrDefault();
    public static int DynamicQueryUpdate(this IDbConnection cnn, string tableName, object param) => SqlMapper.Execute(cnn, DynamicQuery.GetUpdateQuery(tableName, param), param);

  }
}
