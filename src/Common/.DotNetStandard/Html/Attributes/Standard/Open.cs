using Common.Html.Tags;

namespace Common.Html.Attributes {
  public interface IOpenHtmlAttribute : IHtmlAttributeBool {  }
  public interface IHtmlTagWithOpenAttribute: IHtmlTag {  }

  public static class IOpenHtmlAttributeExtensions {

    public static IOpenHtmlAttribute Open<T>(this T tag) where T : IHtmlTagWithOpenAttribute => tag.GetHtmlAttribute<IOpenHtmlAttribute>(IHtmlAttribute.Key.open);
    public static T Open<T>(this T tag, IOpenHtmlAttribute value) where T : IHtmlTagWithOpenAttribute => tag.SetHtmlAttribute(value);

  }


  /// <summary>Specifies that the dialog element is active and that the user can interact with it</summary>
  public readonly record struct OpenHtmlAttribute(bool Value) : IOpenHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.open;
  }

}