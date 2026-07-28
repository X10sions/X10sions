namespace Common.Html.Attributes {
  public interface ICaptureHtmlAttribute : IHtmlAttributeString { };

  /// <summary>Specifies which camera to use for capture of image or video data</summary>
  public readonly record struct CaptureHtmlAttribute(string? Value) : ICaptureHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.capture;

  }
}