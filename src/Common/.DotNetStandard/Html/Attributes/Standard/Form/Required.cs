namespace Common.Html.Attributes {
  public interface IRequiredHtmlAttribute : IHtmlAttributeBool { };
  public readonly record struct RequiredHtmlAttribute(bool Value) : IRequiredHtmlAttribute {
    // https://www.w3schools.com/tags/att_required.asp
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.required;
    public static ReadOnlyHtmlAttribute For(bool value) => value ? True : False;
    public static ReadOnlyHtmlAttribute True { get; } = new(true);
    public static ReadOnlyHtmlAttribute False { get; } = new(false);

  }
}