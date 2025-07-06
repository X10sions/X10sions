namespace Common.Html.Tags;

public interface IInnerText : IHtmlTag {
  string InnerText { get; set; }
}

public static class IInnerTextExtensions {
  public static string ToHtml(this IInnerText tag) => tag.ToHtml(tag.InnerText);
}