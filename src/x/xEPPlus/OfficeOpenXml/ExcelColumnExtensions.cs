using System;
using System.IO;
using System.Linq;

namespace OfficeOpenXml {
  public static class ExcelColumnExtensions {

    // public static int EndRow(this ExcelColumn col) =>  col.Worksheet.Dimension.End.Row;

    public static void Test(string inFile) {
      var randomCol = DateTime.Now.Second;
      var randomRow = DateTime.Now.Millisecond;
      using (ExcelPackage xl = new ExcelPackage(new FileInfo(inFile))) {
        var wb = xl.Workbook;
        var ws = wb.Worksheets.FirstOrDefault();
        var row = ws.Row(randomRow);
        var col = ws.Column(randomCol);
      }
    }

  }
}