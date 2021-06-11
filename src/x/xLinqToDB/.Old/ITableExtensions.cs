using LinqToDB.Data;
using System;
using System.Collections.Generic;

namespace LinqToDB {
  public static class ITableExtensions {

    public static BulkCopyRowsCopied ProviderSpecificBulkCopy<T>(this ITable<T> table, IEnumerable<T> source, int bulkCopyTimeout = 0, bool keepIdentity = false, int notifyAfter = 0, Action<BulkCopyRowsCopied> rowsCopiedCallback = null)
      => table.BulkCopy(new BulkCopyOptions {
        BulkCopyType = BulkCopyType.ProviderSpecific,
        BulkCopyTimeout = bulkCopyTimeout,
        KeepIdentity = keepIdentity,
        NotifyAfter = notifyAfter,
        RowsCopiedCallback = rowsCopiedCallback
      }, source);

    public static BulkCopyRowsCopied MultipleRowsCopy<T>(ITable<T> table, IEnumerable<T> source, int maxBatchSize = 1000, Action<BulkCopyRowsCopied> rowsCopiedCallback = null)
      => table.BulkCopy(new BulkCopyOptions {
        BulkCopyType = BulkCopyType.MultipleRows,
        MaxBatchSize = maxBatchSize,
        RowsCopiedCallback = rowsCopiedCallback
      }, source);

  }
}