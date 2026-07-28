namespace Common.Html.Attributes {
  public interface IPlaceHolderHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct PlaceHolderHtmlAttribute(string? Value) : IPlaceHolderHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.placeholder;
  }
}