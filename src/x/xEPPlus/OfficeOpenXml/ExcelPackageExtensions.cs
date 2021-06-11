using OfficeOpenXml.Table;

namespace OfficeOpenXml {

  public static class ExcelPackageExtensions {

    public static void LoopTables(this ExcelPackage excelPackage) {
      foreach (var sheet in excelPackage.Workbook.Worksheets) {
        foreach (ExcelTable table in sheet.Tables) {
          ExcelCellAddress start = table.Address.Start;
          ExcelCellAddress end = table.Address.End;
          for (int row = start.Row; row <= end.Row; ++row) {
            ExcelRange range = sheet.Cells[row, start.Column, row, end.Column];
            //...
          }
        }
      }
    }

  }
}