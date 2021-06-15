using DatabaseSchemaReader;
using System.Data.Common;

namespace System.Data {
  public static class IDbConnectionExtensions {

    public static DatabaseReader DatabaseSchemaReader(this IDbConnection conn) => new DatabaseReader((DbConnection)conn);

  }
}
