using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace LinqToDB.Data {
  public static class DataConnectionExtensions {

    public static void ThrowLastQuery(this DataConnection dc) => throw new Exception($"SQL : {Environment.NewLine}{dc.LastQuery}{Environment.NewLine}");

    public static EntityMappingBuilder<T> GetEntityMappingBuilder<T>(this DataConnection dc, string tableName, Expression<Func<T, object>> primaryKey)
      => dc.MappingSchema.GetFluentMappingBuilder().Entity<T>().HasTableName(tableName).HasPrimaryKey(primaryKey);

    public static ITable<T> GetTable<T>(this DataConnection dc, EntityMappingBuilder<T> map) where T : class => dc.GetTable<T>();

    public static ITable<T> GetTableWithMapping<T, TBuilder>(this DataConnection dc, TBuilder entityMappingBuilder)
      where T : class
      where TBuilder : EntityMappingBuilder<T> =>
      //var e = entityMappingBuilder;
      dc.GetTable<T>();

    public static DataTable LoadDataTable(this DataConnection dc, string sql) {
      var dt = new DataTable();
      dt.Load(dc.ExecuteReader(sql).Reader);
      return dt;
    }

    #region Bulk Copy

    public static BulkCopyRowsCopied MultipleRowsCopy<T>(this DataConnection dataConnection, IEnumerable<T> source, int maxBatchSize = 1000, Action<BulkCopyRowsCopied> rowsCopiedCallback = null) where T : class
      => dataConnection.BulkCopy(new BulkCopyOptions {
        BulkCopyType = BulkCopyType.MultipleRows,
        MaxBatchSize = maxBatchSize,
        RowsCopiedCallback = rowsCopiedCallback
      }, source);

    public static BulkCopyRowsCopied ProviderSpecificBulkCopy<T>(this DataConnection dataConnection, IEnumerable<T> source, int bulkCopyTimeout = 0, bool keepIdentity = false, int notifyAfter = 0, Action<BulkCopyRowsCopied> rowsCopiedCallback = null) where T : class
      => dataConnection.BulkCopy(new BulkCopyOptions {
        BulkCopyType = BulkCopyType.ProviderSpecific,
        BulkCopyTimeout = bulkCopyTimeout,
        KeepIdentity = keepIdentity,
        NotifyAfter = notifyAfter,
        RowsCopiedCallback = rowsCopiedCallback
      }, source);

    #endregion

    #region TempTable
    public const string DB2iTempTableStatementHeaderFormat = "DECLARE GLOBAL TEMPORARY TABLE {0}";

    public static ITable<T> DB2iCreateTempTable<T>(this DataConnection dc, string tableName, bool withReplace, IEnumerable<T> data) => dc.DB2iDeclareTempTable(tableName, withReplace, data);

    public static ITable<T> DB2iDeclareTempTable<T>(this DataConnection dc, string tableName, bool withReplace, IEnumerable<T> data) {
      var footer = withReplace ? " WITH REPLACE" : "";
      var tempTable = dc.CreateTable<T>(tableName, statementHeader: DB2iTempTableStatementHeaderFormat, statementFooter: footer);
      // Dim rows = (From x In data Select x).insert(tempTable, Function(xx) 1)
      if (data != null) {
        foreach (var row in data)
          DataExtensions.Insert((IDataContext)row, tableName);
      }
      return tempTable;
    }

    public static void DB2iDeclareTempTableFromSql(this DataConnection dc, string tableName, string selectSql, bool withData, bool withReplace) {
      var sql = $@"{string.Format(DB2iTempTableStatementHeaderFormat, tableName)} As(
{selectSql}
) {(withData ? " With Data " : "")} {(withReplace ? " With Replace " : "")}";
      dc.Execute(sql);
    }

    public static void DB2iDeclareTempNumbersTable(this DataConnection dc, string tableName, int min, int max, bool withData, bool withReplace) {
      var selectSql = $"Select A.Id + {min} AS Id From Table(Numbers({max} - {min} + 1)) As A";
      dc.DB2iDeclareTempTableFromSql(tableName, selectSql, withData, withReplace);
    }

    #endregion

  }
}