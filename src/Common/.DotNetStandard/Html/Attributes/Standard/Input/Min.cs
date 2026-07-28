namespace Common.Html.Attributes {
  public interface IMinHtmlAttribute : IHtmlAttributeObject { };

  public readonly record struct MinHtmlAttribute(object Value) : IMinHtmlAttribute {
    public MinHtmlAttribute(object value, IInputTypeHtmlAttribute.Values type = IInputTypeHtmlAttribute.Values.blank) : this(type.GetFormattedValue(value)) { }
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.min;
  }
}