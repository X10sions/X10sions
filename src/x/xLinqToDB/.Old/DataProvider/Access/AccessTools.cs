//using LinqToDB.Data;
//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace LinqToDB.DataProvider.Access {
//  public static class AccessTools {

//    internal static void CreateFileDatabase(string databaseName, bool deleteIfExists, string extension, Action<string> createDatabase) {
//      databaseName = databaseName.Trim();
//      if(!databaseName.ToLower().EndsWith(extension))
//        databaseName += extension;
//      if(File.Exists(databaseName)) {
//        if(!deleteIfExists)
//          return;
//        File.Delete(databaseName);
//      }
//      createDatabase(databaseName);
//    }

//    internal static void DropFileDatabase(string databaseName, string extension) {
//      databaseName = databaseName.Trim();
//      if(File.Exists(databaseName)) {
//        File.Delete(databaseName);
//      } else if(!databaseName.ToLower().EndsWith(extension)) {
//        databaseName += extension;
//        if(File.Exists(databaseName))
//          File.Delete(databaseName);
//      }
//    }

//    #region BulkCopy

//    public static BulkCopyType DefaultBulkCopyType { get; set; } = BulkCopyType.MultipleRows;

//    public static BulkCopyRowsCopied MultipleRowsCopy<T>(DataConnection dataConnection, IEnumerable<T> source, int maxBatchSize = 1000, Action<BulkCopyRowsCopied> rowsCopiedCallback = null)
//      where T : class => dataConnection.BulkCopy(
//        new BulkCopyOptions {
//          BulkCopyType = BulkCopyType.MultipleRows,
//          MaxBatchSize = maxBatchSize,
//          RowsCopiedCallback = rowsCopiedCallback,
//        }, source);

//    #endregion

//  }
//}