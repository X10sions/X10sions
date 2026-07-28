namespace Common.Html.Attributes {
  public interface ILabelHtmlAttribute : IHtmlAttributeString { };

  /// <summary>Specifies a shorter label For an Option</summary>
  public readonly record struct LabelHtmlAttribute(string? Value) : ILabelHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.label;
  }
}