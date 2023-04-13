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
  #region Attributes

  /// <summary>
  /// Specifies the number of columns a cell should span
  /// </summary>
  public int? ColSpan { get => GetAttribute<int?>(nameof(ColSpan)); set => attributes[nameof(ColSpan)] = value; }
  /// <summary>
  /// Specifies one or more header cells a cell is related to
  /// 
  /// </summary>
  public string? Headers { get => GetAttribute<string?>(nameof(Headers)); set => attributes[nameof(Headers)] = value; }
  /// <summary>
  /// Specifies the number of rows a header cell should span
  /// Specifies one or more header cells a cell is related to
  /// </summary>
  public int? RowSpan { get => GetAttribute<int?>(nameof(RowSpan)); set => attributes[nameof(RowSpan)] = value; }

  #endregion

  public string InnerHtml { get; set; } = string.Empty;
  public override string ToHtml() => this.ToHtml(InnerHtml);
}