namespace Common.Html.Attributes {
  public interface IOnChangeHtmlAttribute : IHtmlAttributeString {  };

  public readonly record struct OnChangeHtmlAttribute(string? Value) : IOnChangeHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.onchange;
  }
}