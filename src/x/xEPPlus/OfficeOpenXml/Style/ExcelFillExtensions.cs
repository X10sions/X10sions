using System.Drawing;

namespace OfficeOpenXml.Style {
  public static class ExcelFillExtensions {

    public static ExcelFill SetBackgroundColor(this ExcelFill fill, Color color, ExcelFillStyle patternType = ExcelFillStyle.Solid) {
      fill.PatternType = patternType;
      fill.BackgroundColor.SetColor(color);
      return fill;
    }

  }
}