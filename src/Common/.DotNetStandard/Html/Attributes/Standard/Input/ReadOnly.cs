namespace Common.Html.Attributes {
  public interface IReadOnlyHtmlAttribute : IHtmlAttributeBool { };
  public readonly record struct ReadOnlyHtmlAttribute(bool Value) :  IReadOnlyHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.@readonly;
    public static ReadOnlyHtmlAttribute For(bool value) => value ? True : False;
    public static ReadOnlyHtmlAttribute True { get; } = new(true);
    public static ReadOnlyHtmlAttribute False { get; } = new(false);
    public static implicit operator ReadOnlyHtmlAttribute(bool value) => new(value);
  }
}