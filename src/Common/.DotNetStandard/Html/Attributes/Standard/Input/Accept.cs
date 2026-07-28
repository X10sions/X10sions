namespace Common.Html.Attributes {
  public interface IAcceptHtmlAttribute : IHtmlAttributeString {  };

  /// <summary>Specifies  where to open the linked document</summary>
  public readonly record struct AcceptHtmlAttribute(string?  Value) : IAcceptHtmlAttribute {
    // IANA Media Types : http://www.iana.org/assignments/media-types/
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.accept;

    public static AcceptHtmlAttribute Audio(string suffix) => new($"audio/{suffix}");
    public static AcceptHtmlAttribute Image(string suffix) => new($"image/{suffix}");
    public static AcceptHtmlAttribute Video(string suffix) => new($"video/{suffix}");

  }

}