using Common.Html.Tags;

namespace Common.Html.Attributes {
  public interface IInputTypeHtmlAttribute :  IHtmlAttribute<IInputTypeHtmlAttribute.Values> {
    public enum Values {
      blank,
      text,
      button,
      checkbox,
      color,
      date,
      datetime_local,
      email,
      file,
      hidden,
      image,
      month,
      number,
      password,
      radio,
      range,
      reset,
      search,
      submit,
      tel,
      time,
      url,
      week,
    }
  };

  public readonly record struct InputTypeHtmlAttribute(IInputTypeHtmlAttribute.Values Value) : IInputTypeHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.type;
  }

  public static class InputTypeHtmlAttributeExtensions {
    public static DateTime? AsDateTime<T>(this T value) => DateTime.TryParse(value.ToString(), out DateTime dt) ? dt : null;
    public static string AsHtmlInputDate(this DateTime? v) => v?.ToString(IInputHtmlTag.Formats.Date) ?? string.Empty;
    public static string AsHtmlInputDateTimeLocal(this DateTime? v) => v?.ToString(IInputHtmlTag.Formats.DateTimeLocal) ?? string.Empty;
    public static string AsHtmlInputMonth(this DateTime? v) => v?.ToString(IInputHtmlTag.Formats.Month) ?? string.Empty;
    public static string AsHtmlInputTime(this DateTime? v) => v?.ToString(IInputHtmlTag.Formats.Time) ?? string.Empty;
    public static string AsHtmlInputTime(this TimeSpan? v) => v?.ToString(IInputHtmlTag.Formats.Time) ?? string.Empty;
    public static string AsHtmlInputWeek(this DateTime? v) => v?.ToString($"yyyy-W{v.Value.GetWeekOfYear().ToString("00")}") ?? string.Empty;

    public static string GetFormattedValue<T>(this IInputTypeHtmlAttribute.Values type, T value) => value is null ? string.Empty : type switch {
      IInputTypeHtmlAttribute.Values.date => value.AsDateTime().AsHtmlInputDate(),
      IInputTypeHtmlAttribute.Values.datetime_local => value.AsDateTime().AsHtmlInputDateTimeLocal(),
      IInputTypeHtmlAttribute.Values.month => value.AsDateTime().AsHtmlInputMonth(),
      IInputTypeHtmlAttribute.Values.time => value.AsDateTime().AsHtmlInputTime(),
      IInputTypeHtmlAttribute.Values.week => value.AsDateTime().AsHtmlInputWeek(),
      _ => value.ToString()
    };

    public static string GetValue(this IInputTypeHtmlAttribute.Values? e) => e switch {
      null => string.Empty,
      IInputTypeHtmlAttribute.Values.blank => string.Empty,
      IInputTypeHtmlAttribute.Values.datetime_local => "datetime-local",
      _ => e.ToString().ToLower()
    };

    public static IInputTypeHtmlAttribute.Values AsInputTypeHtmlAttributeValue(this string type) {
      var inputType = IInputTypeHtmlAttribute.Values.TryParse((type ?? string.Empty).ToLower(), out IInputTypeHtmlAttribute.Values it) ? it : IInputTypeHtmlAttribute.Values.blank;
      return inputType;
    }

    public static IInputTypeHtmlAttribute.Values GetInputTypeHtmlAttributeValue<T>(this T value) => value switch {
      bool b => IInputTypeHtmlAttribute.Values.checkbox,
      DateTime dt => IInputTypeHtmlAttribute.Values.date,
      double dbl => IInputTypeHtmlAttribute.Values.number,
      decimal dec => IInputTypeHtmlAttribute.Values.number,
      int i => IInputTypeHtmlAttribute.Values.number,
      long l => IInputTypeHtmlAttribute.Values.number,
      TimeSpan ts => IInputTypeHtmlAttribute.Values.time,
      _ => IInputTypeHtmlAttribute.Values.blank
    };

    public static InputTypeHtmlAttribute GetInputTypeHtmlAttribute<T>(this T value) => new InputTypeHtmlAttribute(value.GetInputTypeHtmlAttributeValue());

  }

}

