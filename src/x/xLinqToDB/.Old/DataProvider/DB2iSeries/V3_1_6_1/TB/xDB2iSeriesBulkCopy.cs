using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using System;
using System.Collections.Generic;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {

  public class xDB2iSeriesBulkCopy : BasicBulkCopy {
    public xDB2iSeriesBulkCopy(DB2iSeriesNamingConvention naming) {
      this.naming = naming;
    }
    private readonly DB2iSeriesNamingConvention naming;
    private const int MAX_ALLOWABLE_BATCH_SIZE = 100;

    protected override BulkCopyRowsCopied ProviderSpecificCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) => throw new NotImplementedException("Not able to do bulk copy in DB2iSeries Provider.");

    protected override BulkCopyRowsCopied MultipleRowsCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) {
      if ((options.MaxBatchSize ?? int.MaxValue) > MAX_ALLOWABLE_BATCH_SIZE) {
        options.MaxBatchSize = MAX_ALLOWABLE_BATCH_SIZE;
      }
      return MultipleRowsCopy2(new xDB2iSeriesMultipleRowsHelper<T>(table, options), source, " FROM " + naming.DummyTableWithSchema());
    }
  }

}