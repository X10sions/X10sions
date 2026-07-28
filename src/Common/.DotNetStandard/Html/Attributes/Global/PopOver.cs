namespace Common.Html.Attributes {
  public interface IPopOverHtmlAttribute : IHtmlAttributeBool { };

  /// <summary>Add an element with a popover attribute, and a button to show/hide it</summary>
  public readonly record struct PopoverHtmlAttribute(bool Value ) : IPopOverHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.popover;
  }

}