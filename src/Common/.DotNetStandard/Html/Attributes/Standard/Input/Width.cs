namespace Common.Html.Attributes {
  public interface IWidthHtmlAttribute : IHtmlAttributeInt { };

  public readonly record struct WidthHtmlAttribute(int? Value) : IWidthHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.width;
  }
}