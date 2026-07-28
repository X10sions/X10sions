namespace Common.Html.Attributes {
  public interface IOnSubmitHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct OnSubmitHtmlAttribute(string? Value) : IOnSubmitHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.onsubmit;
  }
}