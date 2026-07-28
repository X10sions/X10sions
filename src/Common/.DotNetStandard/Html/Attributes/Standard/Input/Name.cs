namespace Common.Html.Attributes {
  public interface INameHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct NameHtmlAttribute(string? Value) : INameHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.name;
    
    public static NameHtmlAttribute Empty { get; } = new(string.Empty);
  }
}