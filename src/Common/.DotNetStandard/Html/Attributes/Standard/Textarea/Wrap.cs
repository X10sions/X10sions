namespace Common.Html.Attributes {
  public interface IWrapHtmlAttribute : IHtmlAttribute<IWrapHtmlAttribute.Values?> {
    public enum Values {
      soft,  // The text in the textarea is not wrapped when submitted in a form. This is default
      hard  // The text in the textarea is wrapped (contains newlines) when submitted in a form. When "hard" is used, the cols attribute must be specified
    }
  };

  /// <summary>Specifies whether the dragged data Is copied, moved, Or linked, When droppedt</summary>
  public readonly record struct WrapHtmlAttribute(IWrapHtmlAttribute.Values? Value) : IWrapHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.wrap;
  }

}