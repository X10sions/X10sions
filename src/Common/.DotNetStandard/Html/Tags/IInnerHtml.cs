using Common.ValueObjects;

namespace Common.Html {
  public interface IInnerHtml {
    InnerHtml InnerHtml { get; }
  }

  public readonly record struct InnerHtml(string Value) : IValueObject<string> {
    public InnerHtml(object value) : this(value?.ToString() ?? string.Empty) { }
    public static InnerHtml Empty { get; } = new(string.Empty);
  }

  public static class IInnerHtmlExtensions {
    //public static string ToHtml(this IInnerHtml tag) => tag.ToHtml(tag.InnerHtml);
    //public static string ToHtml(this IInnerHtml tag) => tag.ToHtml(tag.InnerHtml.Value);
  }
}
