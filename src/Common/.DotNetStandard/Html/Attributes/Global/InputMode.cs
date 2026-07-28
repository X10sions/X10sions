namespace Common.Html.Attributes {
  public interface IInputModeHtmlAttribute : IHtmlAttribute<IInputModeHtmlAttribute.Value> {
    public enum Value { text = 0, @decimal, email, none, numeric, search, tel, url }
  };

  /// <summary>Allows you to change the appearance of the keyboard on a phone or tablet (any device with a virtual keyboard)</summary>
  public readonly record struct InputModeHtmlAttribute(IInputModeHtmlAttribute.Value Value) : IInputModeHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.inputmode;
  }

}