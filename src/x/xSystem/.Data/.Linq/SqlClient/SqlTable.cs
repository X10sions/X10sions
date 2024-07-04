using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlTable : SqlNode {
    internal MetaTable MetaTable { get; }
    internal string Name => MetaTable.TableName;
    internal List<SqlColumn> Columns { get; }
    internal MetaType RowType { get; }
    internal ProviderType SqlRowType { get; }
    internal SqlTable(MetaTable table, MetaType rowType, ProviderType sqlRowType, Expression sourceExpression)
      : base(SqlNodeType.Table, sourceExpression) {
      MetaTable = table;
      RowType = rowType;
      SqlRowType = sqlRowType;
      Columns = new List<SqlColumn>();
    }
    internal SqlColumn Find(string columnName) {
      foreach (var column in Columns) {
        if (column.Name == columnName) {
          return column;
        }
      }
      return null;
    }
  }

}
