using System.Text;

namespace Common.Html.Tags;
public class Table : HtmlTag5Base<Table> {
  public const string DefaultCss = "table{border-collapse:collapse;}";

  public Caption Caption { get; set; } = new Caption();
  public ColGroup? ColGroup { get; set; }
  public THead? THead { get; set; }
  public List<TBody> TBody { get; set; } = new List<TBody>();
  public TFoot? TFoot { get; set; }

  public override string ToHtml() {
    var sb = new StringBuilder();
    sb.AppendLine($"<{TagName}>");
    sb.AppendLine(Caption.ToHtml());
    if (ColGroup != null) sb.AppendLine(ColGroup.ToHtml());
    if (THead != null) sb.AppendLine(THead.ToHtml());
    foreach (var tbody in TBody) {
      sb.AppendLine(tbody.ToHtml());
    }
    if (TFoot != null) sb.AppendLine(TFoot.ToHtml());
    sb.AppendLine($"</{TagName}>");
    return sb.ToString();
  }
}

public static class TableExtensions {

}