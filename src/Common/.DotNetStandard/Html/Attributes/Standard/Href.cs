namespace Common.Html.Attributes {
  public interface IHrefHtmlAttribute : IHtmlAttributeString { };

  /// <summary>Specifies whether the content Of an element Is editable Or Not</summary>
  public readonly record struct HrefHtmlAttribute(string? Value) : IHrefHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.href;
  }
}