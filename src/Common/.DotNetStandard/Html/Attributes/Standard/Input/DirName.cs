namespace Common.Html.Attributes {
  public interface IDirNameHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct DirNameHtmlAttribute(string? Value) : IDirNameHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.dirname;
  }
}