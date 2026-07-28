namespace Common.Html.Attributes {

  public interface IContentEditableHtmlAttribute : IHtmlAttributeBool { };

  /// <summary>Specifies whether the content Of an element Is editable Or Not</summary>
  public readonly record struct ContentEditableHtmlAttribute(bool Value) : IContentEditableHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.contenteditable;
  }

}