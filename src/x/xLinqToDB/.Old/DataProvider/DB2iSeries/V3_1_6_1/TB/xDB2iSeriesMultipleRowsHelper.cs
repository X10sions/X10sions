using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using LinqToDB.SqlQuery;
using System;
using System.Text;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public class xDB2iSeriesMultipleRowsHelper<T> : MultipleRowsHelper<T> {
    public xDB2iSeriesMultipleRowsHelper(ITable<T> table, BulkCopyOptions options)
      : base(table, options) {
    }

    public override void BuildColumns(object item, Func<ColumnDescriptor, bool>? skipConvert = null) {
      if (skipConvert == null) {
        skipConvert = ((ColumnDescriptor _) => false);
      }
      for (var i = 0; i < Columns.Length; i++) {
        var column = Columns[i];
        var value = column.GetValue(item);
        var columnType = ColumnTypes[i];
        if (column.DbType != null) {
          if (column.DbType!.Equals("time", StringComparison.CurrentCultureIgnoreCase)) {
            columnType = new SqlDataType(DataType.Time);
          } else if (column.DbType!.Equals("date", StringComparison.CurrentCultureIgnoreCase)) {
            columnType = new SqlDataType(DataType.Date);
          }
        }
        if (skipConvert!(column) || value == null || !ValueConverter.TryConvert(StringBuilder, columnType, value)) {
          if (!(ParameterName == "?")) {
            _ = ParameterName + ++ParameterIndex;
          } else {
            _ = ParameterName;
          }
          var parameter = value as DataParameter;
          if (parameter != null) {
            value = parameter.Value;
          }
          var dataParameter = new DataParameter((ParameterName == "?") ? ParameterName : ("p" + ParameterIndex), value, column.DataType);
          Parameters.Add(dataParameter);
          var casttype = ((value == null) ? columnType : DataConnection.MappingSchema.GetDataType(value.GetType())).GetiSeriesType();
          var nameWithCast = "CAST(@" + dataParameter.Name + " AS " + casttype + ")";
          StringBuilder.Append(nameWithCast);
        }
        StringBuilder.Append(",");
      }
      StringBuilder.Length--;
    }
  }

}
