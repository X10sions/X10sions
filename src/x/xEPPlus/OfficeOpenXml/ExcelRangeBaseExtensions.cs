using System.Collections.Generic;
using System.Linq;

namespace OfficeOpenXml {

  public static class ExcelRangeBaseExtensions {

    public static List<ExcelColumn> ColumnList(this ExcelRangeBase range) {
      var list = new List<ExcelColumn>();
      for (var i = 1; i <= range.Columns; i++) {
        var col = range.Worksheet.Column(range.Start.Column + i - 1);
        list.Add(col);
      }
      return list;
    }
    
    public static ExcelRangeBase EndCell(this ExcelRangeBase range) => range.Worksheet.Cells[range.End.Row, range.End.Column];
    public static ExcelColumn EndColumn(this ExcelRangeBase range) => range.Worksheet.Column(range.End.Column);
    public static ExcelRow EndRow(this ExcelRangeBase range) => range.Worksheet.Row(range.End.Row);

    public static ExcelRangeBase Cell(this ExcelRangeBase range, int row, int column) => range.Worksheet.Cells[range.Start.Row + row - 1, range.Start.Column + column - 1];
    public static ExcelColumn Column(this ExcelRangeBase range, int column) => range.Worksheet.Column(range.Start.Column + column - 1);
    public static ExcelRow Row(this ExcelRangeBase range, int row) => range.Worksheet.Row(range.Start.Row + row - 1);

    //public static int ColumnNumber(this ExcelRangeBase range, string columnName) => range.First(c => c.Value.ToString() == columnName).Start.Column;

    public static ExcelRangeBase StartCell(this ExcelRangeBase range) => range.Worksheet.Cells[range.Start.Row, range.Start.Column];
    public static ExcelColumn StartColumn(this ExcelRangeBase range) => range.Worksheet.Column(range.Start.Column);
    public static ExcelRow StartRow(this ExcelRangeBase range) => range.Worksheet.Row(range.Start.Row);

    public static List<ExcelRow> RowList(this ExcelRangeBase range) {
      var list = new List<ExcelRow>();
      for (var i = 1; i <= range.Rows; i++) {
        var row = range.Worksheet.Row(range.Start.Row + i - 1);
        list.Add(row);
      }
      return list;
    }

    public static double VisibleHeight(this ExcelRangeBase range) => (from x in range.RowList() where !x.Hidden select x.Height).Sum();
    public static double VisibleWidth(this ExcelRangeBase range) => (from x in range.ColumnList() where !x.Hidden select x.Width).Sum();

  }
}