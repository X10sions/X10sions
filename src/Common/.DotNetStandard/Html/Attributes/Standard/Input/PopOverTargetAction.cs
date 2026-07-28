namespace Common.Html.Attributes {
  public interface IPopOverTargetActionHtmlAttribute : IHtmlAttribute<IPopOverTargetActionHtmlAttribute.Values?> {
    public enum Values { toggle, hide, show }
  };

  /// <summary>Specifies  where to open the linked document</summary>
  public readonly record struct PopOverTargetActionHtmlAttribute(IPopOverTargetActionHtmlAttribute.Values? Value) : IPopOverTargetActionHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.popovertargetaction;
  }
}

