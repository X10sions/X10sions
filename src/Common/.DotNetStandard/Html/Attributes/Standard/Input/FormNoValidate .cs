namespace Common.Html.Attributes {
  public interface IFormNoValidateHtmlAttribute : IHtmlAttributeBool { };

  public readonly record struct FormNoValidateHtmlAttribute(bool Value) : IFormNoValidateHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.formnovalidate;
  }
}