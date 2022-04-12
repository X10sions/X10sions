using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System.Data {
  public static class DataTableExtensionsX {

    public static List<T> DataTableToObjectList<T>(this DataTable dsTable) where T : class, new() {
      var objectList = new List<T>();
      foreach (DataRow dr in dsTable.Rows) {
        var obj = Activator.CreateInstance<T>();
        dr.CopyObjectFromDataRow(obj);
        objectList.Add(obj);
      }
      return objectList;
    }

    public static List<T?> DataTableToTypedList<T>(this DataTable dsTable) where T : class, new() {
      var objectList = new List<T?>();
      MemberInfo[]? cachedMemberInfo = null;
      foreach (DataRow dr in dsTable.Rows) {
        var obj = default(T); // Activator.CreateInstance<T>();
        //if (obj != null) {
          dr.CopyObjectFromDataRow(obj, cachedMemberInfo);
          objectList.Add(obj);
        //}
      }
      return objectList;
    }

    public static bool HasRows(this DataTable dataTable) => dataTable?.Rows?.Count > 0;

    public static string ToCsv(this DataTable table) {
      var sb = new StringBuilder();
      var iColCount = table.Columns.Count;
      int i;
      for (i = 0; i <= iColCount - 1; i++) {
        sb.Append(table.Columns[i]);
        if (i < iColCount - 1) {
          sb.Append(",");
        }
      }
      sb.AppendLine("");
      i = 0;
      foreach (DataRow row in table.Rows) {
        for (i = 0; i <= iColCount - 1; i++) {
          if (!Convert.IsDBNull(row[i])) {
            sb.Append("\"");
            // see #7 in this article
            //http://tools.ietf.org/html/rfc4180
            sb.Append(row[i].ToString().CsvEscapeQuotes());
            sb.Append("\"");
          } else {
            sb.Append("");
          }
          if (i < iColCount - 1) {
            sb.Append(",");
          }
        }
        sb.AppendLine("");
      }
      return sb.ToString();
    }

    public static string ToHtmlTable(this DataTable table, string tableCssClass = "table") {
      var sb = new StringBuilder($"<table class=\"{ tableCssClass }\"><thead><tr>");
      foreach (DataColumn col in table.Columns) {
        sb.Append($"<th title=\"{col.DataType}{(col.AllowDBNull ? "?" : string.Empty)} \">{col.ColumnName}</th>");
      }
      sb.AppendLine("</tr></thead><tbody>");
      foreach (DataRow row in table.Rows) {
        sb.AppendLine("<tr>");
        foreach (DataColumn col in table.Columns) {
          sb.Append($"<td>{row[col.ColumnName]}</td>");
        }
        sb.Append("</tr>");
      }
      sb.AppendLine("</tbody></table>");
      return sb.ToString();
    }

    public static string ToWordDocString(this DataTable table) {
      var wordString = new StringBuilder();

      wordString.Append(@"<html " +
              "xmlns:o='urn:schemas-microsoft-com:office:office' " +
              "xmlns:w='urn:schemas-microsoft-com:office:word'" +
              "xmlns='http://www.w3.org/TR/REC-html40'>" +
              "<head><title>Time</title>");
      wordString.Append(@"<!--[if gte mso 9]>" +
                               "<xml>" +
                               "<w:WordDocument>" +
                               "<w:View>Print</w:View>" +
                               "<w:Zoom>90</w:Zoom>" +
                               "</w:WordDocument>" +
                               "</xml>" +
                               "<![endif]-->");
      wordString.Append(@"<style>" +
                              "<!-- /* Style Definitions */" +
                              "@page Section1" +
                              "   {size:8.5in 11.0in; " +
                              "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                              "   mso-header-margin:.5in; " +
                              "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                              " div.Section1" +
                              "   {page:Section1;}" +
                              "-->" +
                             "</style></head>");
      wordString.Append(@"<body lang=EN-US style='tab-interval:.5in'>" +
                              "<div class=Section1>" +
                              "<h1>Time and tide wait for none</h1>" +
                              "<p style='color:red'><I>" +
                              DateTime.Now + "</I></p></div><table border='1'>");

      wordString.Append("<tr>");
      for (var i = 0; i < table.Columns.Count; i++) {
        wordString.Append("<td>" + table.Columns[i].ColumnName + "</td>");
      }
      wordString.Append("</tr>");
      //Items
      for (var x = 0; x < table.Rows.Count; x++) {
        wordString.Append("<tr>");
        for (var i = 0; i < table.Columns.Count; i++) {
          wordString.Append("<td>" + table.Rows[x][i] + "</td>");
        }
        wordString.Append("</tr>");
      }
      wordString.Append(@"</table></body></html>");
      return wordString.ToString();
    }

  }
}