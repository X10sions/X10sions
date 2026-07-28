namespace Common.Html.Attributes {
  public interface ICheckedHtmlAttribute : IHtmlAttributeBool { };

  public readonly record struct CheckedHtmlAttribute(bool Value = false) : ICheckedHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.@checked;
  }
}