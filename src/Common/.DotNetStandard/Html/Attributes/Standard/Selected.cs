namespace Common.Html.Attributes {
  public interface ISelectedHtmlAttribute : IHtmlAttributeBool { }

  /// <summary>Specifies that an Option should be pre-selected When the page loads</summary>
  public readonly record struct SelectedHtmlAttribute(bool Value) : ISelectedHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.selected;
 
    public static SelectedHtmlAttribute For(bool value) => value ? True : False;
    public static SelectedHtmlAttribute True { get; } = new(true);
    public static SelectedHtmlAttribute False { get; } = new(false);

  }
}