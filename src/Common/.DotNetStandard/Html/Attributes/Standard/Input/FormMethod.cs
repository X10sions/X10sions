namespace Common.Html.Attributes {
  public interface IFormMethodHtmlAttribute : IHtmlAttribute<IFormMethodHtmlAttribute.Values?> {
    public enum Values { get, post }
  };

  /// <summary>Specifies  where to open the linked document</summary>
  public readonly record struct FormMethodHtmlAttribute(IFormMethodHtmlAttribute.Values? Value) : IFormMethodHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.formmethod;

    public static FormMethodHtmlAttribute Get { get; } = new(IFormMethodHtmlAttribute.Values.get);
    public static FormMethodHtmlAttribute Post { get; } = new(IFormMethodHtmlAttribute.Values.post);
  }
}

