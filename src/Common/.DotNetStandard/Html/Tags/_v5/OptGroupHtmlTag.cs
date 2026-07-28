using Common.Html.Attributes;
using System.Text;

namespace Common.Html.Tags;

public interface IOptGroupHtmlTag : IHtmlTag_v5, IInnerHtml, IHtmlFormElement {
  IEnumerable<IOptionHtmlTag> Options { get; }
}

public static class IOptGroupHtmlTagExtensions {
  // https://www.w3schools.com/tags/tag_optgroup.asp
  public static IDisabledHtmlAttribute Disabled(this IOptGroupHtmlTag tag) => tag.GetHtmlAttribute<IDisabledHtmlAttribute>(IHtmlAttribute.Key.disabled);
  public static T Disabled<T>(this T tag, DisabledHtmlAttribute value) where T : IOptGroupHtmlTag => tag.SetHtmlAttribute(value);

  public static ILabelHtmlAttribute Label(this IOptGroupHtmlTag tag) => tag.GetHtmlAttribute<ILabelHtmlAttribute>(IHtmlAttribute.Key.label);
  public static T Label<T>(this T tag, ILabelHtmlAttribute value) where T : IOptGroupHtmlTag => tag.SetHtmlAttribute(value);

}

public record struct OptGroupHtmlTag(IEnumerable<IOptionHtmlTag> Options, string? Name = null) : IOptGroupHtmlTag {
  public string TagName { get; } = "optgroup";
  public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Normal;
  public HtmlAttributeDictionary Attributes { get; } = new();
  public InnerHtml InnerHtml => this.GetInnerHtml();
  public OptGroupHtmlTag() : this(OptionHtmlTag.EmptyCollection) { }
  public static IEnumerable<IOptGroupHtmlTag> EmptyCollection { get; } = Enumerable.Empty<IOptGroupHtmlTag>();
}

public static class OptGroupHtmlExtensions {
  public static InnerHtml GetInnerHtml(this IOptGroupHtmlTag tag) {
    var html = new StringBuilder();
    tag.Options.AppendLineTo(html);
    return new InnerHtml(html.ToString());
  }


}
