namespace Common.Html.Attributes {
  public interface ITargetHtmlAttribute : IHtmlAttribute<ITargetHtmlAttribute.Values?> {
    public enum Values { _self, _blank, _parent, _top, _framename }
  };

  /// <summary>Specifies  where to open the linked document</summary>
  public readonly record struct TargetHtmlAttribute(ITargetHtmlAttribute.Values? Value) : ITargetHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.target;

    public static TargetHtmlAttribute Blank { get; } = new(ITargetHtmlAttribute.Values._blank);
    public static TargetHtmlAttribute Parent { get; } = new(ITargetHtmlAttribute.Values._parent);
    public static TargetHtmlAttribute Self { get; } = new(ITargetHtmlAttribute.Values._self);
    public static TargetHtmlAttribute Top { get; } = new(ITargetHtmlAttribute.Values._top);
    public static TargetHtmlAttribute Default { get; } = Self;
  }
}
