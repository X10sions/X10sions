namespace Common.Html.Attributes {
  public interface IOnBlurHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct OnBlurHtmlAttribute(string? Value) : IOnBlurHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.onblur;
  }
}