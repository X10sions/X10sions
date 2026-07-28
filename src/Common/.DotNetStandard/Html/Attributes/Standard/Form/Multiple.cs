namespace Common.Html.Attributes {
  public interface IMultipleHtmlAttribute : IHtmlAttributeBool { };

  /// <summary>Specifies whether the content Of an element Is editable Or Not</summary>
  public readonly record struct MultipleHtmlAttribute(bool Value) : IMultipleHtmlAttribute {
    // https://www.w3schools.com/tags/att_multiple.asp
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.multiple;

    public static MultipleHtmlAttribute For(bool value) => value ? True : False;
    public static MultipleHtmlAttribute True { get; } = new(true);
    public static MultipleHtmlAttribute False { get; } = new(false);
  }

}