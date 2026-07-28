namespace Common.Html.Attributes {

  public interface IAccessKeyHtmlAttribute : IHtmlAttribute<char> { };

  /// <summary>Specifies a shortcut key to activate/focus an element</summary>
  public readonly record struct AccessKeyHtmlAttribute(char Value) : IAccessKeyHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.accesskey;
  }

}