using System;
using OfficeOpenXml.Style;

namespace System.Data {
  public static class DataColumnExtensions {

    public static ExcelHorizontalAlignment GuessHorizontalAlignment(this DataColumn dataColumn) {
      var type = dataColumn.DataType;
      if (type.IsDateOrTime()) {
        return ExcelHorizontalAlignment.Center;
      }
      if (type.IsNumeric()) {
        return ExcelHorizontalAlignment.Right;
      }
      if (type.IsText()) {
        return (dataColumn.MaxLength < 10) ? ExcelHorizontalAlignment.Center : ExcelHorizontalAlignment.Left;
      }
      throw new Exception("Field '" + dataColumn.ColumnName + "' has unknown type: " + dataColumn.DataType);
    }

  }
}
