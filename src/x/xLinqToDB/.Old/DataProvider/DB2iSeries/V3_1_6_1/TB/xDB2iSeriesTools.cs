using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using LinqToDB.SqlQuery;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public static class xDB2iSeriesTools {

    public const string MapGuidAsString = "MapGuidAsString";

    public static BulkCopyType DefaultBulkCopyType = BulkCopyType.MultipleRows;

    public static bool AutoDetectProvider { get; set; } = true;

  }
}