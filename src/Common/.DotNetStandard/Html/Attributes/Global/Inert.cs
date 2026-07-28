namespace Common.Html.Attributes {
  public interface IInertHtmlAttribute : IHtmlAttributeBool { };

  /// <summary>Disables an element and all the elements inside.</summary>
  public readonly record struct InertHtmlAttribute(bool Value) : IInertHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.inert;
  }

}