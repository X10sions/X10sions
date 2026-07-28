namespace Common.Html.Attributes {
  public interface IPatternHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct PatternHtmlAttribute(string? Value) : IPatternHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.pattern;
  }
}