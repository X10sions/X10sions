namespace Common.Html.Attributes {
  public interface IColsHtmlAttribute : IHtmlAttributeInt{ };

  /// <summary>Specifies the visible width of a text area</summary>
  public readonly record struct ColsHtmlAttribute(int? Value) : IColsHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.cols;
  }
}