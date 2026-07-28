namespace Common.Html.Attributes {
  public interface IFormHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct FormHtmlAttribute(string? Value) : IFormHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.form;
  }
}