namespace Common.Html.Tags;
public interface IHtmlTag {
  string TagName { get; }
  Dictionary<string, object> Attributes { get; }
  string ToHtml();
}

public static class IHtmlTagExtensions {

  public static T GetAttribute<T>(this IHtmlTag tag, string key, T defaultValue) => tag.Attributes.Get<string, object, T>(key, defaultValue);
  public static T? GetAttribute<T>(this IHtmlTag tag, string key) => tag.GetAttribute<T>(key, default);

}

