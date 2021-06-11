using System;
using System.Data;
using System.IO;
using UnitsNet;

namespace OfficeOpenXml {

  public static class ExcelWorksheetExtensions {

    public static double PointsToMillimetres(double points) => Length.FromDtpPoints(points).Millimeters;
    public static double MillimetresToPoints(double mm) => Length.FromMillimeters(mm).DtpPoints;


    public static int ColumnCount(this ExcelWorksheet ws) => ws.Dimension.End.Column;



    public static ExcelWorksheet DeleteEndEmptyColumnsAndRows(this ExcelWorksheet ws) => ws.DeleteEndEmptyColumns().DeleteEndEmptyRows();

    public static ExcelWorksheet DeleteEndEmptyColumns(this ExcelWorksheet ws) {
      while (ws.IsEndColumnEmpty()) {
        ws.DeleteColumn(ws.Dimension.End.Column, 1);
      }
      return ws;
    }


    public static ExcelWorksheet DeleteEndEmptyRows(this ExcelWorksheet ws) {
      while (ws.IsEndRowEmpty()) {
        ws.DeleteRow(ws.Dimension.End.Row, 1);
      }
      return ws;
    }

    public static eOrientation GuessPageOrientation(this ExcelWorksheet ws) {
      double paperWidth = ws.PrinterSettings.PaperSize.PaperWidthInMillimetres();
      double leftMargin = Length.FromInches(ws.PrinterSettings.LeftMargin).Millimeters;
      double rightMargin = Length.FromInches(ws.PrinterSettings.RightMargin).Millimeters;
      double usedRangeWidth = PointsToMillimetres(ws.UsedRange(false).VisibleWidth());
      if ((paperWidth - leftMargin - rightMargin - usedRangeWidth) > 1) {
        return eOrientation.Portrait;
      }
      return eOrientation.Landscape;
    }

    public static bool IsEndColumnEmpty(this ExcelWorksheet worksheet) {
      for (var index = 1; index <= worksheet.Dimension.End.Row; index++) {
        var value = worksheet.Cells[index, worksheet.Dimension.End.Column].Value;
        if (!string.IsNullOrWhiteSpace(value.ToString())) {
          return false;
        }
      }
      return true;
    }

    public static bool IsEndRowEmpty(this ExcelWorksheet worksheet) {
      for (var index = 1; index <= worksheet.Dimension.End.Column; index++) {
        var value = worksheet.Cells[worksheet.Dimension.End.Row, index].Value;
        if (string.IsNullOrWhiteSpace(value.ToString())) {
          return false;
        }
      }
      return true;
    }

    public static int EndColumn(this ExcelWorksheet ws) => ws.Dimension.End.Column;
    public static int EndRow(this ExcelWorksheet ws) => ws.Dimension.End.Row;

    public static DataTable ToDataTable(this ExcelWorksheet ws, bool hasHeader = true) {
      var tbl = new DataTable();
      foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column]) {
        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
      }
      var startRow = hasHeader ? 2 : 1;
      for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++) {
        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
        DataRow row = tbl.Rows.Add();
        foreach (var cell in wsRow) {
          row[cell.Start.Column - 1] = cell.Text;
        }
      }
      return tbl;
    }

    public static ExcelRange UsedRange(this ExcelWorksheet ws, bool doDeleteEndEmptyColumnsAndRows = false) {
      if (doDeleteEndEmptyColumnsAndRows) {
        ws.DeleteEndEmptyColumnsAndRows();
      }
      return ws.Cells[1, 1, ws.EndRow(), ws.EndColumn()];
    }

    public static double UsedRangeVisibleWidth(this ExcelWorksheet ws, bool doDeleteEndEmptyColumnsAndRows = false) {
      double width = 0;
      for (var i = 1; i <= ws.Dimension.End.Column; i++) {
        var column = ws.Column(i);
        if (!column.Hidden) {
          width += ws.Column(i).Width;
        }
      }
      return width;
    }

    public static void WorksheetSaveCopyAs(this ExcelWorksheet wks, string fileName, OfficeProperties props) {
      var fi = new FileInfo(fileName);
      using (var xl = new ExcelPackage(fi)) {
        var ws = xl.Workbook.Worksheets.Add(wks.Name, wks);
        xl.Workbook.Properties.Clone(props);
        xl.SaveAs(fi);
      }
    }

    public static void FormatMulti_Data_xslx(this ExcelWorksheet ws, string reportLayoutFilterId) {
      ws.Cells[1, 1, 1, ws.Dimension.End.Column].Merge = true;
      ws.Cells[2, 1, 2, ws.Dimension.End.Column].Merge = true;

      ws.Cells.AutoFitColumns();

      ws.PrinterSettings.Orientation = ws.GuessPageOrientation();
      ws.PrinterSettings.RepeatRows = ws.Cells["1:5"];
      ws.HeaderFooter.OddFooter.LeftAlignedText = $"&6 {DateTime.Now.ToString("dd-mmm-yyyy hh:nn:ss")} ({reportLayoutFilterId})";
      ws.HeaderFooter.OddFooter.CenteredText = $"&6{ExcelHeaderFooter.FilePath}{ExcelHeaderFooter.FileName}!{ExcelHeaderFooter.SheetName}";
      ws.HeaderFooter.OddFooter.RightAlignedText = $"&6Page {ExcelHeaderFooter.PageNumber} of {ExcelHeaderFooter.NumberOfPages}";

    }

    public static double ColumnsUsedWidth(this ExcelWorksheet ws) {
      double w = 0;
      for (var i = 1; i <= ws.ColumnCount(); i++)
        w += ws.Column(i).Width;
      return w;
    }

    public static int RowCount(this ExcelWorksheet ws) => ws.Dimension.End.Row;

    public static double RowsUsedHeight(this ExcelWorksheet ws) {
      double h = 0;
      for (var i = 1; i <= ws.RowCount(); i++)
        h += ws.Row(i).Height;
      return h;
    }

    public static ExcelRange UsedRange(this ExcelWorksheet ws) => ws.Cells[1, 1, ws.RowCount(), ws.ColumnCount()];

    public static void SetPageSetupToDefault(this ExcelWorksheet ws) {
      ws.HeaderFooter.OddFooter.LeftAlignedText = "&6&[Date] &[Time]";
      ws.HeaderFooter.OddFooter.CenteredText = "&6&[Path]&[File]!&[Tab]";
      ws.HeaderFooter.OddFooter.RightAlignedText = "&6Page &[Page] of &[Pages]";
      ws.PrinterSettings.BottomMargin = 1.3m;
      ws.PrinterSettings.FitToWidth = 1;
      ws.PrinterSettings.FooterMargin = 0.5m;
      ws.PrinterSettings.HeaderMargin = 0.5m;
      ws.PrinterSettings.HorizontalCentered = true;
      ws.PrinterSettings.LeftMargin = 0.5m;
      ws.PrinterSettings.Orientation = ws.GuessPageOrientation();
      ws.PrinterSettings.RightMargin = 0.5m;
      ws.PrinterSettings.TopMargin = 0.5m;
    }

  }
}