using System.Text;

namespace Common.Html.Tags;

public class TR : HtmlTag5Base<TR> {
  #region Attributes
  #endregion

  public List<ITableCellHtmlTag> Cells { get; set; } = new List<ITableCellHtmlTag>();
  public override string ToHtml() => $"<{TagName}>{Cells.ToHtml()}</{TagName}>";

  public static string ROW_HTML(string th, string td) => $"<tr><th>{th}:</th><td>{td}</td><tr>";
  public static string ROW_WITH_COLSPAN_HTML(string th, int colspan = 2) => $"<tr><th colspan=\"{colspan}\">{th}:</th><tr>";

}

public static class TRExtensions {

  public static string ToHtml(this IEnumerable<TR> rows) {
    var sb = new StringBuilder();
    foreach (var row in rows) {
      sb.AppendLine(row.ToHtml());
    }
    return sb.ToString();
  }

}