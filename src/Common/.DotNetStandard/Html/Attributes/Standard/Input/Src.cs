namespace Common.Html.Attributes {
  public interface ISrcHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct SrcHtmlAttribute(string? Value) : ISrcHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.src;
  }
}