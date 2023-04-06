using System.Text;

namespace Common.Html.Tags;

public interface ITableCellHtmlTag : IHtmlTag {
  public string InnerHtml { get; set; }
}

public static class ITableCellHtmlTagExtensions {

  public static string ToHtml(this IEnumerable<ITableCellHtmlTag> cells) {
    var sb = new StringBuilder();
    foreach (var cell in cells) {
      sb.AppendLine(cell.ToHtml());
    }
    return sb.ToString();
  }

}

public abstract class TableCellHtmlTagBase<T> : HtmlTag5Base<T>, ITableCellHtmlTag where T : IHtmlTag {
  public string InnerHtml { get; set; } = string.Empty;
}