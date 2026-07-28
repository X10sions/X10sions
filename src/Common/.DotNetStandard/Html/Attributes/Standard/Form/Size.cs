namespace Common.Html.Attributes {
  public interface ISizeHtmlAttribute : IHtmlAttributeInt { };

  public readonly record struct SizeHtmlAttribute(int? Value) : ISizeHtmlAttribute {
    // https://www.w3schools.com/tags/att_size.asp
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.size;
  }
}