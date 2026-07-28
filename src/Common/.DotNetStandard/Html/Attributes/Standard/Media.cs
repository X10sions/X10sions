namespace Common.Html.Attributes {
  public interface IMediaHtmlAttribute : IHtmlAttributeString { };

  /// <summary>Specifies the language of the linked document</summary>
  public readonly record struct MediaHtmlAttribute(string? Value) : IMediaHtmlAttribute {
    public const string AttributeName = "media";
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.media;
  }
}