using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.TagHelpers {

  public static class TagHelperOutputExtensions {

    public static TagHelperOutput AddCssClass(this TagHelperOutput output, string cssClass)
      => AddCssClass(output, new[] { cssClass });

    public static TagHelperOutput AddCssClass(this TagHelperOutput output, IEnumerable<string> cssClasses) {
      if (output.Attributes.ContainsName("class") && output.Attributes["class"] != null) {
        var classes = output.Attributes["class"].Value.ToString().Split(' ').ToList();
        foreach (string cssClass in cssClasses.Where(cssClass => !classes.Contains(cssClass)))
          classes.Add(cssClass);
        output.Attributes.SetAttribute("class", classes.Aggregate((s, s1) => s + " " + s1));
      } else if (output.Attributes.ContainsName("class"))
        output.Attributes.SetAttribute("class", cssClasses.Aggregate((s, s1) => s + " " + s1));
      else
        output.Attributes.Add("class", cssClasses.Aggregate((s, s1) => s + " " + s1));
      return output;
    }

    public static TagHelperOutput RemoveCssClass(this TagHelperOutput output, string cssClass) {
      if (output.Attributes.ContainsName("class")) {
        var classes = output.Attributes["class"].Value.ToString().Split(' ').ToList();
        classes.Remove(cssClass);
        if (classes.Count == 0)
          output.Attributes.RemoveAll("class");
        else
          output.Attributes.SetAttribute("class", classes.Aggregate((s, s1) => s + " " + s1));
      }
      return output;
    }

    public static TagHelperOutput AddCssStyle(this TagHelperOutput output, string name, string value) {
      if (output.Attributes.ContainsName("style")) {
        if (string.IsNullOrEmpty(output.Attributes["style"].Value.ToString())) {
          output.Attributes.SetAttribute("style", name + ": " + value + ";");
        } else {
          output.Attributes.SetAttribute("style", (output.Attributes["style"].Value.ToString().EndsWith(";", System.StringComparison.Ordinal)
                                                   ? " "
                                                   : "; ") + name + ": " + value + ";");
        }
      } else {
        output.Attributes.Add("style", name + ": " + value + ";");
      }
      return output;
    }

    public static async Task LoadChildContentAsync(this TagHelperOutput output) {
      output.Content.SetHtmlContent(await output.GetChildContentAsync() ?? new DefaultTagHelperContent());
    }

    public static TagHelperContent ToTagHelperContent(this TagHelperOutput output) {
      var content = new DefaultTagHelperContent();
      content.AppendHtml(output.PreElement);
      var builder = new TagBuilder(output.TagName);
      foreach (TagHelperAttribute attribute in output.Attributes) {
        builder.Attributes.Add(attribute.Name, attribute.Value?.ToString());
      }
      if (output.TagMode == TagMode.SelfClosing) {
        builder.TagRenderMode = TagRenderMode.SelfClosing;
        content.AppendHtml(builder);
      } else {
        builder.TagRenderMode = TagRenderMode.StartTag;
        content.AppendHtml(builder);
        content.AppendHtml(output.PreContent);
        content.AppendHtml(output.Content);
        content.AppendHtml(output.PostContent);
        if (output.TagMode == TagMode.StartTagAndEndTag) {
          content.AppendHtml($"</{output.TagName}>");
        }
      }
      content.AppendHtml(output.PostElement);
      return content;
    }

    public static TagHelperOutput AddAriaAttribute(this TagHelperOutput output, string name, object value) => output.MergeAttribute("aria-" + name, value);

    public static TagHelperOutput AddDataAttribute(this TagHelperOutput output, string name, object value) => output.MergeAttribute("data-" + name, value);

    public static string GetAttributeValue(this TagHelperOutput output, string attrName) {
      if (string.IsNullOrEmpty(attrName) || !output.Attributes.TryGetAttribute(attrName, out var attr)) {
        return null;
      }
      if (attr.Value is string stringValue) {
        return stringValue;
      } else if (attr.Value is IHtmlContent content) {
        if (content is HtmlString htmlString) {
          return htmlString.ToString();
        }
        using (var writer = new StringWriter()) {
          content.WriteTo(writer, HtmlEncoder.Default);
          return writer.ToString();
        }
      }
      return null;
    }


    public static TagHelperOutput MergeAttribute(this TagHelperOutput output, string key, object value) {
      output.Attributes.SetAttribute(key, value);
      return output;
    }

    public static TagHelperOutput WrapContentOutside(this TagHelperOutput output, TagBuilder builder) {
      builder.TagRenderMode = TagRenderMode.StartTag;
      return WrapContentOutside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
    }

    public static TagHelperOutput WrapContentOutside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag) {
      output.PreContent.Prepend(startTag);
      output.PostContent.AppendHtml(endTag);
      return output;
    }

    public static TagHelperOutput WrapContentOutside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreContent.Prepend(startTag);
      output.PostContent.Append(endTag);
      return output;
    }

    public static TagHelperOutput WrapHtmlContentOutside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreContent.PrependHtml(startTag);
      output.PostContent.AppendHtml(endTag);
      return output;
    }

    public static TagHelperOutput WrapContentInside(this TagHelperOutput output, TagBuilder builder) {
      builder.TagRenderMode = TagRenderMode.StartTag;
      return WrapContentInside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
    }

    public static TagHelperOutput WrapContentInside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag) {
      output.PreContent.AppendHtml(startTag);
      output.PostContent.Prepend(endTag);
      return output;
    }

    public static TagHelperOutput WrapContentInside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreContent.Append(startTag);
      output.PostContent.Prepend(endTag);
      return output;
    }

    public static TagHelperOutput WrapHtmlContentInside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreContent.AppendHtml(startTag);
      output.PostContent.PrependHtml(endTag);
      return output;
    }

    public static TagHelperOutput WrapOutside(this TagHelperOutput output, TagBuilder builder) {
      builder.TagRenderMode = TagRenderMode.StartTag;
      return WrapOutside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
    }

    public static TagHelperOutput WrapOutside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag) {
      output.PreElement.Prepend(startTag);
      output.PostElement.AppendHtml(endTag);
      return output;
    }

    public static TagHelperOutput WrapOutside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreElement.Prepend(startTag);
      output.PostElement.Append(endTag);
      return output;
    }

    public static TagHelperOutput WrapHtmlOutside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreElement.PrependHtml(startTag);
      output.PostElement.AppendHtml(endTag);
      return output;
    }

    public static TagHelperOutput WrapInside(this TagHelperOutput output, TagBuilder builder) {
      builder.TagRenderMode = TagRenderMode.StartTag;
      return WrapInside(output, builder, new TagBuilder(builder.TagName) { TagRenderMode = TagRenderMode.EndTag });
    }

    public static TagHelperOutput WrapInside(this TagHelperOutput output, IHtmlContent startTag, IHtmlContent endTag) {
      output.PreElement.AppendHtml(startTag);
      output.PostElement.Prepend(endTag);
      return output;

    }

    public static TagHelperOutput WrapInside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreElement.Append(startTag);
      output.PostElement.Prepend(endTag);
      return output;
    }


    public static TagHelperOutput WrapHtmlInside(this TagHelperOutput output, string startTag, string endTag) {
      output.PreElement.AppendHtml(startTag);
      output.PostElement.PrependHtml(endTag);
      return output;
    }

  }
}