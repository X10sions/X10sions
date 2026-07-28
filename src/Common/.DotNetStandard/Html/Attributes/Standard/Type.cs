namespace Common.Html.Attributes {
  public interface ITypeHtmlAttribute : IHtmlAttributeString {  };

  /// <summary>Specifies the type of element</summary>
  public readonly record struct TypeHtmlAttribute(string? Value) : ITypeHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.type;
    // IANA Media Types
  }
}