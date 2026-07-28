namespace Common.Html.Attributes {
  public interface IMaxHtmlAttribute : IHtmlAttributeObject { };

  public readonly record struct MaxHtmlAttribute(object Value) : IMaxHtmlAttribute {
    public MaxHtmlAttribute(object value, IInputTypeHtmlAttribute.Values type = IInputTypeHtmlAttribute.Values.blank) : this(type.GetFormattedValue(value)) { }
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.max;  
  }
}