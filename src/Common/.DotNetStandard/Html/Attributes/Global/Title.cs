namespace Common.Html.Attributes {
  public interface ITitleHtmlAttribute : IHtmlAttributeString { };

  /// <summary> Specifies extra information about an element </summary>
  public readonly record struct TitleHtmlAttribute(string Value) : ITitleHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.title;
  }

}