using System.Collections.Generic;
using System.Linq;

namespace System.Data {
  public static class DataTableExtensions {

    public static IEnumerable<DataRow> DataRows(this DataTable dataTable) => dataTable.Rows.OfType<DataRow>();
    public static IEnumerable<T> DataRowsAs<T>(this DataTable dataTable, Func<DataRow, T> builder) => dataTable.DataRows().Select(builder);

  }
}
