namespace Common.Html.Attributes {
  public interface ITabIndexHtmlAttribute : IHtmlAttributeInt { };

  /// <summary>Specifies the tabbing order Of an element </summary>
  public readonly record struct TabIndexHtmlAttribute(int? Value) : ITabIndexHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.tabindex;
  }
}