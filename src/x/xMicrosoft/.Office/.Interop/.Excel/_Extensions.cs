namespace Microsoft.Office.Interop.Excel;
public static class _Extensions {

  public static XlFileFormat GetExcelFileFormat(this string fileName) => Path.GetExtension(fileName) switch {
    ".xls" => XlFileFormat.xlExcel8,
    ".xlsb" => XlFileFormat.xlExcel12,
    ".xlsm" => XlFileFormat.xlOpenXMLWorkbookMacroEnabled,
    ".xlsx" => XlFileFormat.xlOpenXMLWorkbook,
    _ => XlFileFormat.xlOpenXMLWorkbook
  };

  public static void ExportRecordsetToExcel(this DataTable rs, string exportToFile, int wksIndex = 1, string wksRange = "A2", object? templateFile = null) {
    using (var xlApp = new ExcelApplication()) {
      SetExcelAppUI(xlApp, false);
      var wkb = xlApp.Workbooks.Add(templateFile);
      var wks = (Worksheet)wkb.Sheets[wksIndex];
      wks.Range[wksRange].CopyFromRecordset(rs);
      try {
        wkb.SaveAs(exportToFile, GetExcelFileFormat(exportToFile));
      } catch (Exception ex) {
        Console.WriteLine($"Error: {exportToFile} \n\n {ex.Message}");
      }
      wkb.Close();//: Set wkb = null;
      SetExcelAppUI(xlApp, true);
      xlApp.Quit();//: Set xlApp = null;
    }
  }

  public static void SetExcelAppUI(this Application xlApp, bool b) {
    xlApp.DisplayAlerts = b;
    xlApp.EnableEvents = b;
    xlApp.ScreenUpdating = b;
  }

}
