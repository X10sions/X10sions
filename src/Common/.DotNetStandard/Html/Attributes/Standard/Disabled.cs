namespace Common.Html.Attributes {
  public interface IDisabledHtmlAttribute : IHtmlAttributeBool { };

  /// <summary>Specifies whether the content Of an element Is editable Or Not</summary>
  public readonly record struct DisabledHtmlAttribute(bool Value) : IDisabledHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.disabled;

    public static DisabledHtmlAttribute For(bool value) => value ? True: False;
    public static DisabledHtmlAttribute True { get; } = new(true);
    public static DisabledHtmlAttribute False { get; } = new(false);
  }
}