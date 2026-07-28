namespace Common.Html.Attributes {
  public interface IFormActionHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct FormActionHtmlAttribute(string? Value) : IFormActionHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.formaction;
  }
}