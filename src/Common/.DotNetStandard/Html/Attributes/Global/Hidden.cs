namespace Common.Html.Attributes {

  public interface IHiddenHtmlAttribute : IHtmlAttributeBool { };

  /// <summary>Specifies that an element Is Not yet, Or Is no longer, relevant</summary>
  public readonly record struct HiddenHtmlAttribute(bool Value) : IHiddenHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.hidden;

    public static HiddenHtmlAttribute For(bool value) => value ? True : False;
    public static HiddenHtmlAttribute True { get; } = new(true);
    public static HiddenHtmlAttribute False { get; } = new(false);

  }
}