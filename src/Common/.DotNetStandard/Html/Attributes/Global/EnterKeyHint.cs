namespace Common.Html.Attributes {
  public interface IEnterKeyHintHtmlAttribute : IHtmlAttribute<IEnterKeyHintHtmlAttribute.Value> {
    public enum Value { done, enter, go, next, previous, search, send }
  };

  /// <summary>Specify a virtual keyboard's "Enter" button with the enterkeyhint attribute.</summary>
  public readonly record struct EnterKeyHintHtmlAttribute(IEnterKeyHintHtmlAttribute.Value Value) : IEnterKeyHintHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.enterkeyhint;
  
  }

}