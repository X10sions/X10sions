using System.Collections.ObjectModel;
using System.Text;

namespace Common.Html.Tags;
public interface IHtmlTag {
  string TagName { get; }
  //ReadOnlyCollection<string, object?> Attributes { get; }
  ReadOnlyDictionary<string, object?> Attributes { get; }
  void AddAttribute<TValue>(string key, TValue value);
  TValue? GetAttribute<TValue>(string key);
  TValue GetAttribute<TValue>(string key, TValue defaultValue);
  void SetAttribute<TValue>(string key, TValue value);

  //GenericDictionary<string, object?> Attributes { get; }
  string ToHtml();
}

public static class IHtmlTagExtensions {

  //public static T GetAttribute<T>(this IHtmlTag tag, string key, T defaultValue) => tag.Attributes.Get(key, defaultValue);
  //public static T? GetAttribute<T>(this IHtmlTag tag, string key) => tag.GetAttribute<T>(key, default);

  //public static T SetAttribute<T>(this IHtmlTag tag, string key, T defaultValue) => tag.Attributes.Add().Set(key, defaultValue);
  //public static T? SetAttribute<T>(this IHtmlTag tag, string key, T? value) => tag.Attributes.Add(key, value);
  //public static T? SetAttribute<T>(this IHtmlTag tag, string key, T? value) { 
  //  tag.Attributes[key] = value;
  //  return value;
  //}

  public static string AttributesHtml(this IHtmlTag tag) {
    var sb = new StringBuilder();
    foreach (var a in tag.Attributes) {
      if (!(a.Value is null)) {
        sb.Append($" {a.Key}=\"{a.Value}\"");
      }
    }
    return sb.ToString();
  }

  public static string ToHtml(this IHtmlTag tag, string innerHtml) => $"<{tag.TagName}{tag.AttributesHtml()}>{innerHtml}</{tag.TagName}>";

}

