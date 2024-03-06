namespace Common.Html.Tags;

public class TH : TableCellHtmlTagBase<TH> {
  #region Attributes
  /// <summary>
  /// Specifies an abbreviated version of the content in a header cell
  /// </summary>
  public string? Abbr { get => GetAttribute<string?>(nameof(Abbr)); set => attributes[nameof(Abbr)] = value; }
  /// <summary>
  /// Specifies whether a header cell is a header for a column, row, or group of columns or rows
  /// </summary>
  public TH_Scope? Scope { get => GetAttribute<TH_Scope?>(nameof(Scope)); set => attributes[nameof(Scope)] = value; }

  #endregion

  public enum TH_Scope { col, colgroup, row, rowgroup }
}
