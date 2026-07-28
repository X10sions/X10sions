namespace Common.Html.Tags;

public interface IInnerHtmlOld : IHtmlTagOld {
  string InnerHtml { get; set; }
}

public static class IInnerHtmlOldExtensions {
  public static string ToHtml(this IInnerHtmlOld tag) => tag.ToHtml(tag.InnerHtml);
}
