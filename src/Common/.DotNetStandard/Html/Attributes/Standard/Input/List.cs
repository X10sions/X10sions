namespace Common.Html.Attributes {
  public interface IListHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct ListHtmlAttribute(string? Value) : IListHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.list;
  }
}