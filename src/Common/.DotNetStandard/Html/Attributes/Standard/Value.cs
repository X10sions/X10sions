using Common.Html.Tags;

namespace Common.Html.Attributes {
  public interface IValueHtmlAttribute<T> : IHtmlAttribute<T> {
    //IInputTypeHtmlAttribute.Values Type { get; }
  }
  public interface IValueHtmlAttribute : IValueHtmlAttribute<object> { }

  /// <summary>Specifies the value To be sent To a server</summary>
  public readonly record struct ValueHtmlAttribute<T>(T Value) : IValueHtmlAttribute<T> {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.value;
  }

  /// <summary>Specifies the value To be sent To a server</summary>
  public readonly record struct ValueHtmlAttribute(object Value) : IValueHtmlAttribute {
    public ValueHtmlAttribute(object value, IInputTypeHtmlAttribute.Values type = IInputTypeHtmlAttribute.Values.blank) : this(type.GetFormattedValue(value)) { }
    public ValueHtmlAttribute New<T>(T value, IInputTypeHtmlAttribute.Values type) => new(value, type);
    //public ValueHtmlAttribute New<T>(T tag, IInputTypeHtmlAttribute.Values type) where T: IInputHtmlTag => new(value, type);
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.value;
//    public string HtmlKeyValue => this.GetHtmlKeyValue(Type.GetFormattedValue(Value));
    public static ValueHtmlAttribute Empty { get; } = new(string.Empty, IInputTypeHtmlAttribute.Values.blank);
  }

  public static class IValueExtensions {

  }

}