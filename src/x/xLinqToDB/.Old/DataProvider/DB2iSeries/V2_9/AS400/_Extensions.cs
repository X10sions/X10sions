using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.DB2iSeries.V2_9.RoyChase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400{
  public static class DB2iSeriesExtensions {

    public static BulkCopyRowsCopied BulkCopy_DB2iSeries<T>(this ITable<T> table, BulkCopyOptions options, IEnumerable<T> source, DB2iSeriesConfiguration dB2ISeriesConfiguration, DB2iSeriesBulkCopy bulkCopy = null) {
      if (bulkCopy == null)
        bulkCopy = new DB2iSeriesBulkCopy(dB2ISeriesConfiguration);
      return bulkCopy.BulkCopy(options.BulkCopyType == BulkCopyType.Default ?  DB2iSeriesTools.DefaultBulkCopyType : options.BulkCopyType, table, options, source);
    }

    public static void InitCommand_DB2iSeries(this DataProviderBase dataProviderBase, DataConnection dataConnection, CommandType commandType, string commandText, DataParameter[] parameters, bool withParameters) {
      dataConnection.DisposeCommand();
      dataProviderBase.InitCommand(dataConnection, commandType, commandText, parameters, withParameters);
    }

    public static int Merge_DB2iSeries<T>(this DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source,
      string tableName, string databaseName, string schemaName) where T : class {
      if (delete) throw new LinqToDBException("DB2 iSeries MERGE statement does not support DELETE by source.");
      return new DB2iSeriesMerge().Merge(dataConnection, deletePredicate, delete, source, tableName, databaseName, schemaName);
    }

    public static Task<int> MergeAsync_DB2iSeries<T>(this DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source,
      string tableName, string databaseName, string schemaName, CancellationToken token) where T : class {
      if (delete) throw new LinqToDBException("DB2 iSeries MERGE statement does not support DELETE by source.");
      return new DB2iSeriesMerge().MergeAsync(dataConnection, deletePredicate, delete, source, tableName, databaseName, schemaName, token);
    }

    //public static BasicMergeBuilder<TTarget, TSource> GetMergeBuilder_DB2iSeries<TTarget, TSource>(this DataProviderBase dataProviderBase, DataConnection connection, IMergeable<TTarget, TSource> merge) => new DB2iSeriesMergeBuilder<TTarget, TSource>(connection, merge);

  }
}
