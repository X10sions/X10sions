using System;

namespace LinqToDB.SqlQuery {

  public static class ISqlTableSourceExtensions {

    public static string GetTableAlias(this ISqlTableSource table) {
      switch (table.ElementType) {
        case QueryElementType.TableSource:
          var ts = (SqlTableSource)table;
          var alias = string.IsNullOrEmpty(ts.Alias) ? GetTableAlias(ts.Source) : ts.Alias;
          return alias != "$" ? alias : null;
        case QueryElementType.SqlTable:
          return ((SqlTable)table).Alias;
        case QueryElementType.SqlCteTable:
          return ((SqlTable)table).Alias;
        default:
          throw new InvalidOperationException();
      }
    }

  }
}
