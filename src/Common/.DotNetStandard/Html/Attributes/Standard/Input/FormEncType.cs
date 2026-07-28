namespace Common.Html.Attributes {
  public interface IFormEncTypeHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct FormEncTypeHtmlAttribute(string? Value) : IFormEncTypeHtmlAttribute {
    // IANA Media Types : http://www.iana.org/assignments/media-types/
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.formenctype;

    public static FormEncTypeHtmlAttribute ApplicationXWwwFormUrlencoded { get; } = new("application/x-www-form-urlencoded");
    public static FormEncTypeHtmlAttribute MultipartFormData { get; } = new("multipart/form-data");
    public static FormEncTypeHtmlAttribute TextPlain { get; } = new("text/plain");
    public static FormEncTypeHtmlAttribute Default { get; } = ApplicationXWwwFormUrlencoded;
  }
}
