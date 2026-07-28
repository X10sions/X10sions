using Common.ValueObjects;

namespace Common.Html;

public interface IInnerText {
  InnerText InnerText { get; }
}

public readonly record struct InnerText(string Value) : IValueObject<string> {
  public InnerText(object value) : this(value?.ToString() ?? string.Empty) { }
  public static InnerText Empty { get; } = new(string.Empty);
}

public static class IInnerTextExtensions {
  //public static string ToHtml(this IInnerText tag) => tag.ToHtml(tag.InnerText);
  //public static string ToHtml(this IInnerText tag) => tag.ToHtml(tag.InnerText.Value);
}
