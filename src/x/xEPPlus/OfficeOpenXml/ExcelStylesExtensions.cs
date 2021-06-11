using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using System.Drawing;

namespace OfficeOpenXml {

  public static class ExcelStylesExtensions {

    public static ExcelNamedStyleXml MtgPct3(this ExcelStyles styles) {
      var namedStyle = styles.CreateNamedStyle(nameof(MtgPct3));
      namedStyle.Style.Numberformat.Format = "#,##0.000%;[Red]-#,##0.000%";
      return namedStyle;
    }

    public static ExcelNamedStyleXml MtgTable(this ExcelStyles styles) {
      var namedStyle = styles.CreateNamedStyle(nameof(MtgTable));
      namedStyle.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.LightGray);
      namedStyle.Style.Border.Left.Style = ExcelBorderStyle.Thin;
      namedStyle.Style.Border.Left.Color.SetColor(Color.LightGray);
      namedStyle.Style.Border.Top.Style = ExcelBorderStyle.Thin;
      namedStyle.Style.Border.Top.Color.SetColor(Color.LightGray);
      namedStyle.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
      namedStyle.Style.Border.Bottom.Color.SetColor(Color.LightGray);
      namedStyle.Style.Border.Right.Style = ExcelBorderStyle.Thin;
      namedStyle.Style.Border.Right.Color.SetColor(Color.LightGray);
      return namedStyle;
    }

    public static ExcelNamedStyleXml MtgTableHeader(this ExcelStyles styles) {
      var namedStyle = styles.CreateNamedStyle(nameof(MtgTableHeader));
      namedStyle.Style.Fill.SetBackgroundColor(Color.LightBlue, ExcelFillStyle.Solid);
      return namedStyle;
    }

    public static ExcelNamedStyleXml MtgTableTotals(this ExcelStyles styles) {
      var namedStyle = styles.CreateNamedStyle(nameof(MtgTableTotals));
      namedStyle.Style.Fill.SetBackgroundColor(Color.LightBlue, ExcelFillStyle.Solid);
      namedStyle.Style.Border.Top.Style = ExcelBorderStyle.Double;
      namedStyle.Style.Border.Top.Color.SetColor(Color.LightGray);
      return namedStyle;
    }


  }
}