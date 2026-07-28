namespace Common.Html.Attributes {
  public interface IHeightHtmlAttribute : IHtmlAttributeInt{ };

  public readonly record struct HeightHtmlAttribute(int? Value) : IHeightHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.height;

  }
}