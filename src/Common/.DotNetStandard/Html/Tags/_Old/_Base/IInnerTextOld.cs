namespace Common.Html.Tags;

public interface IInnerTextOld : IHtmlTagOld {
  string InnerText { get; set; }
}

public static class IInnerTextOldExtensions {
  public static string ToHtml(this IInnerTextOld tag) => tag.ToHtml(tag.InnerText);
}