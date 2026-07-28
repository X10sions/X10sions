namespace Common.Html.Attributes {
  public interface IOnInputHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct OnInputHtmlAttribute(string? Value) : IOnInputHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.oninput;
  }
}