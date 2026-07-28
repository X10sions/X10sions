namespace Common.Html.Attributes {
  public interface IMaxLengthHtmlAttribute : IHtmlAttributeInt { };

  public readonly record struct MaxLengthHtmlAttribute(int? Value) : IMaxLengthHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.maxlength;
  }
}