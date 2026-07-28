namespace Common.Html.Attributes {
  public interface IPingHtmlAttribute : IHtmlAttributeString { };

  /// <summary>Specifies a space separated list of URLs to be notified if the user follows the hyperlink</summary>
  public readonly record struct PingHtmlAttribute(string? Value) : IPingHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.ping;
  }
}