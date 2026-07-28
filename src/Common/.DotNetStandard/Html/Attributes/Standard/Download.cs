namespace Common.Html.Attributes {

  public interface IDownloadHtmlAttribute : IHtmlAttributeString { };

  /// <summary>Specifies that the target will be downloaded when a user clicks on the hyperlink</summary>
  public readonly record struct DownloadHtmlAttribute(string? Value) : IDownloadHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.download;
  }
}