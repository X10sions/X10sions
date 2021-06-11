using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microsoft.AspNetCore.Razor.TagHelpers {
  public static class TagHelperContentExtensions {

    public static TagHelperContent Prepend(this TagHelperContent content, string value)
      => content.SetContent(value + (content.IsEmptyOrWhiteSpace ? string.Empty : content.GetContent()));

    public static TagHelperContent PrependHtml(this TagHelperContent content, string value)
      => content.SetHtmlContent(value + (content.IsEmptyOrWhiteSpace ? string.Empty : content.GetContent()));

    public static TagHelperContent Prepend(this TagHelperContent content, IHtmlContent value) {
      if (content.IsEmptyOrWhiteSpace) {
        content.SetHtmlContent(value);
        content.AppendLine();
      } else {
        string currentContent = content.GetContent();
        content.SetHtmlContent(value);
        content.AppendHtml(currentContent);
      }
      return content;
    }

    public static TagHelperContent Prepend(this TagHelperContent content, TagHelperOutput output) => content.Prepend(output.ToTagHelperContent());

    public static TagHelperContent Wrap(this TagHelperContent content, TagBuilder builder) {
      builder.TagRenderMode = TagRenderMode.StartTag;
      return Wrap(content, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
    }

    public static TagHelperContent Wrap(this TagHelperContent content, IHtmlContent contentStart, IHtmlContent contentEnd) => content.Prepend(contentStart).AppendHtml(contentEnd);

    public static TagHelperContent Wrap(this TagHelperContent content, string contentStart, string contentEnd) => content.Prepend(contentStart).Append(contentEnd);

    public static TagHelperContent WrapHtml(this TagHelperContent content, string contentStart, string contentEnd) => content.PrependHtml(contentStart).AppendHtml(contentEnd);

  }
}