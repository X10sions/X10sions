namespace Common.Html.Attributes {
  public interface IRowsHtmlAttribute : IHtmlAttributeInt { };

  /// <summary>Specifies the visible number of lines in a text area </summary>
  public readonly record struct RowsHtmlAttribute(int? Value) : IRowsHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.rows;
  }
}