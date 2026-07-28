namespace Common.Html.Attributes {
  public interface IOnClickHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct OnClickHtmlAttribute(string? Value) : IOnClickHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.onclick;
  }
}