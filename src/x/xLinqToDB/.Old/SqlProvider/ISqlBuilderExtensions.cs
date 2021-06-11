using LinqToDB.Data;
using System;
using System.Text;

namespace LinqToDB.SqlProvider {
  public static class ISqlBuilderExtensions {

    [Obsolete]
    public static string GetTableName<T>(this ISqlBuilder sqlBuilder, BulkCopyOptions options, ITable<T> table, TableOptions tableOptions) {
      var databaseName = options.DatabaseName ?? table.DatabaseName;
      var schemaName = options.SchemaName ?? table.SchemaName;
      var tableName = options.TableName ?? table.TableName;
      var serverName = options.ServerName ?? table.ServerName;
      return sqlBuilder.BuildTableName(
        new StringBuilder(),
        serverName == null ? null : sqlBuilder.ConvertInline(serverName, ConvertType.NameToServer),
        databaseName == null ? null : sqlBuilder.ConvertInline(databaseName, ConvertType.NameToDatabase),
        schemaName == null ? null : sqlBuilder.ConvertInline(schemaName, ConvertType.NameToSchema),
        tableName == null ? null : sqlBuilder.ConvertInline(tableName, ConvertType.NameToQueryTable)        ,
        tableOptions)
      .ToString();
    }

  }
}
