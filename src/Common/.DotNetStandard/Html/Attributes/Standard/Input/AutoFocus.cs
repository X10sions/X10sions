namespace Common.Html.Attributes {
  public interface IAutoFocusHtmlAttribute : IHtmlAttributeBool { };

  public readonly record struct AutoFocusHtmlAttribute(bool Value) : IAutoFocusHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.autofocus;
  }
}