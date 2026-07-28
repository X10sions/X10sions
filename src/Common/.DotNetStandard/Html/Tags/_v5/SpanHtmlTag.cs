#nullable enable

using Common.Html.Attributes;

namespace Common.Html.Tags {
  public interface ISpanHtmlTag : IHtmlTag_v5, IInnerHtml { }

  public static class ISpanHtmlTagExtensions { }

  public record struct SpanHtmlTag(InnerHtml InnerHtml) : ISpanHtmlTag {
    public SpanHtmlTag() : this(InnerHtml.Empty) { }
    public string TagName { get; } = "span";
    public IHtmlTag.ElementType Element { get; } = IHtmlTag.ElementType.Normal;
    public HtmlAttributeDictionary Attributes { get; } = new();
    //public InnerHtml InnerHtml { get; set; } = new();
  }

  public static class SpanHtmlTagExtensions {

  }

}