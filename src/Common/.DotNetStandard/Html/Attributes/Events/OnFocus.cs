namespace Common.Html.Attributes {
  public interface IOnFocusHtmlAttribute : IHtmlAttributeString {  };

  public readonly record struct OnFocusHtmlAttribute(string? Value) : IOnFocusHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.onfocus;
  }
}