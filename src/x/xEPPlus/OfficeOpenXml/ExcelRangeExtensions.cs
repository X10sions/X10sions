using System;
using System.Drawing;

namespace OfficeOpenXml {
  public static class ExcelRangeExtensions {

    public const double PixelsPerExcelColumnWidth = 7.5;
    public const int MaxExcelRowHeightInPoints = 409;
    public static double Dpi72And96PointsPerInch => 72 / 96;

    public static double MeasureTextHeight(this ExcelRange range ) {
      string text = range.Value.ToString();
      var font = range.Style.Font;
      if (string.IsNullOrEmpty(text)) return 0.0;
      var bitmap = new Bitmap(1, 1);
      var graphics = Graphics.FromImage(bitmap);
      var pixelWidth = Convert.ToInt32(range.VisibleWidth() * PixelsPerExcelColumnWidth);
      var drawingFont = new Font(font.Name, font.Size);
      var size = graphics.MeasureString(text, drawingFont, pixelWidth);
      return Math.Min(Convert.ToDouble(size.Height) * Dpi72And96PointsPerInch, MaxExcelRowHeightInPoints);
    }
   

    }
}