namespace Common.Html.Attributes {
  public interface IPopOverTargetHtmlAttribute : IHtmlAttributeString { };

  public readonly record struct PopOverTargetHtmlAttribute(string? Value) : IPopOverTargetHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.popovertarget;
  }
}