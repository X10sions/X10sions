using OfficeOpenXml.Style.XmlAccess;
using System.Linq;

namespace OfficeOpenXml {

  public static class ExcelWorkBookExtensions {

    public static ExcelNamedStyleXml GetNamedStyle(this ExcelWorkbook wb, string styleName) {
      var namedStyle = wb.Styles.NamedStyles.FirstOrDefault(c=> c.Name == styleName);
      if (namedStyle == null) {
        namedStyle = wb.Styles.CreateNamedStyle(styleName);
      }
      return namedStyle;
    }

    public static bool NamedStyleExists(this ExcelWorkbook wb, string styleName)=> wb.Styles.NamedStyles.FirstOrDefault(c => c.Name == styleName) != null;

    public static void SetProperties(this ExcelWorkbook wb, OfficeProperties props) => wb.Properties.Clone(props);

  }
}