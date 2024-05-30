namespace Common.Html.Tags;

public interface IInnerHtml : IHtmlTag {
  string InnerHtml { get; set; }
}

public static class IInnerHtmlExtensions {
  public static string ToHtml(this IInnerHtml tag) => tag.ToHtml(tag.InnerHtml);
}
