namespace Common.Html.Attributes {
  public interface IAltHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct AltHtmlAttribute(string?  Value) : IAltHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.alt;

  }
}