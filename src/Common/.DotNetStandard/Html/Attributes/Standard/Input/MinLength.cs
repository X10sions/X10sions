namespace Common.Html.Attributes {
  public interface IMinLengthHtmlAttribute : IHtmlAttributeInt { };

  public readonly record struct MinLengthHtmlAttribute(int? Value) : IMinLengthHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.minlength;
  }
}