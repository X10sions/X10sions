using Common.Html.Attributes;

namespace Common.Html.Tags;

[Obsolete]
public record struct xInputHtmlTag(IInputTypeHtmlAttribute.Values Type) : IInputHtmlTag {
  public string TagName { get; } = "input";
  public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

  public HtmlAttributeDictionary Attributes { get; } = new();

  //public static IInputHtmlTag New<T>(IInputTypeHtmlAttribute.Values type, T value) => new xInputHtmlTag(type).Value(new(value, type));
  //public static InputHtmlTag New<T>(T value) where T : struct => New(value.GetInputTypeHtmlAttributeValue(), value);

  #region Buttons

  //public static InputHtmlTag Button(string value, string? popOverTarget = null, IPopOverTargetActionHtmlAttribute.Values? popOverTargetAction = null)
  //  => New(IInputTypeHtmlAttribute.Values.button, value).pop.PopOvert() with {
  //    PopOverTarget = new(popOverTarget),
  //    PopOverTargetAction = new(popOverTargetAction)
  //  };

  //public static InputHtmlTag File(bool multiple = false, bool readOnly = false)
  //  => new InputHtmlTag(IInputTypeHtmlAttribute.Values.file) with {
  //    Multiple = new(multiple),
  //    ReadOnly = new(readOnly)
  //  };
  //public static InputHtmlTag Image<T>(T value, string src, string? formAction = null, string? formEncType = null, IFormMethodHtmlAttribute.Values? formMethod = null, ITargetHtmlAttribute.Values? formTarget = null, int? height = null, int? width = null)
  //  => New(IInputTypeHtmlAttribute.Values.image, value) with {
  //    FormAction = new(formAction),
  //    FormEncType = new(formEncType),
  //    FormMethod = new(formMethod),
  //    FormTarget = new(formTarget),
  //    Height = new(height),
  //    Src = new(src),
  //    Width = new(width)
  //  };

  //public static InputHtmlTag Reset(string value = "Reset") => new(IInputTypeHtmlAttribute.Values.reset, new(value));

  //public static InputHtmlTag Submit(string value = "Submit", string? formAction = null, string? formEncType = null, IFormMethodHtmlAttribute.Values? formMethod = null, bool formNoValidate = false, ITargetHtmlAttribute.Values? formTarget = null)
  //  => New(IInputTypeHtmlAttribute.Values.submit, value) with {
  //    FormAction = new(formAction),
  //    FormEncType = new(formEncType),
  //    FormMethod = new(formMethod),
  //    FormNoValidate = new(formNoValidate),
  //    FormTarget = new(formTarget)
  //  };
  #endregion

  #region Date/Time
  //public static InputHtmlTag Date(DateTime? value, DateTime? max = null, DateTime? min = null, string? pattern = null, bool readOnly = false, DateTime? step = null)
  //  => New(IInputTypeHtmlAttribute.Values.date, value.AsInputDate()) with {
  //    Max = new(max.AsInputDate()),
  //    Min = new(min.AsInputDate()),
  //    Pattern = new(pattern),
  //    ReadOnly = new(readOnly),
  //    Step = new(step)
  //  };
  //public static InputHtmlTag DateTimeLocal(DateTime? value, DateTime? max = null, DateTime? min = null, bool readOnly = false, DateTime? step = null)
  //  => New(IInputTypeHtmlAttribute.Values.datetime_local, value.AsInputDateTimeLocal()) with {
  //    Max = new(max.AsInputDateTimeLocal()),
  //    Min = new(min.AsInputDateTimeLocal()),
  //    ReadOnly = new(readOnly),
  //    Step = new(step)
  //  };
  //public static InputHtmlTag Month(DateTime? value, DateTime? max = null, DateTime? min = null, DateTime? step = null)
  //  => New(IInputTypeHtmlAttribute.Values.month, value.AsInputMonth()) with {
  //    Max = new(max.AsInputMonth()),
  //    Min = new(min.AsInputMonth()),
  //    Step = new(step)
  //  };
  //public static InputHtmlTag Time(TimeSpan? value, TimeSpan? max = null, TimeSpan? min = null, TimeSpan? step = null)
  //  => New(IInputTypeHtmlAttribute.Values.time, value.AsInputTime()) with {
  //    Max = new(max.AsInputTime()),
  //    Min = new(min.AsInputTime()),
  //    Step = new(step)
  //  };
  //public static InputHtmlTag Week(DateTime? value, DateTime? max = null, DateTime? min = null, DateTime? step = null)
  //  => New(IInputTypeHtmlAttribute.Values.week, value.AsInputWeek()) with {
  //    Max = new(max.AsInputWeek()),
  //    Min = new(min.AsInputWeek()),
  //    Step = new(step)
  //  };
  #endregion

  #region Numbers

  //public static InputNumberHtmlTag Number<T>(T? value, T? max = null, T? min = null, string? placeHolder = null, bool readOnly = false, T? step = null) where T : struct
  //  => new InputNumberHtmlTag().Value(value).Max(new(max)).Min(new(min)).PlaceHolder(new(placeHolder)).ReadOnly(new(readOnly)).Step(new(step));

  //public static InputRangeHtmlTag Range<T>(T? value, T? max = null, T? min = null, T? step = null) where T : struct
  //  => new InputRangeHtmlTag().Value(value).Max(new(max)).Min(new(min)).Step(new(step));

  #endregion

  #region String
  //public static InputHtmlTag Email(string? value, int? minLength = null, bool multiple = false, string? pattern = null, string? placeHolder = null, bool readOnly = false, int? size = null)
  //  => New(IInputTypeHtmlAttribute.Values.email, value) with {
  //    MinLength = new(minLength),
  //    Multiple = new(multiple),
  //    Pattern = new(pattern),
  //    PlaceHolder = new(placeHolder),
  //    ReadOnly = new(readOnly),
  //    Size = new(size)
  //  };

  public static InputHiddenHtmlTag Hidden<T>(T? value, string? id = null, string? name = null) where T : struct => new InputHiddenHtmlTag()
    .Value(value)
    .Id(new IdHtmlAttribute(id))
    .Name(new(name ?? id));

  //public static InputHtmlTag Password(string? value, int? minLength = null, string? pattern = null, string? placeHolder = null, bool readOnly = false, int? size = null)
  //  => New(IInputTypeHtmlAttribute.Values.password, value) with {
  //    MinLength = new(minLength),
  //    Pattern = new(pattern),
  //    PlaceHolder = new(placeHolder),
  //    ReadOnly = new(readOnly),
  //    Size = new(size)
  //  };
  //public static InputHtmlTag Search(string? value, int? minLength = null, string? pattern = null, string? placeHolder = null, bool readOnly = false, int? size = null)
  //  => New(IInputTypeHtmlAttribute.Values.search, value) with {
  //    MinLength = new(minLength),
  //    Pattern = new(pattern),
  //    PlaceHolder = new(placeHolder),
  //    ReadOnly = new(readOnly),
  //    Size = new(size)
  //  };

  //public static InputHtmlTag Tel(string? value, int? minLength = null, string? pattern = null, string? placeHolder = null, bool readOnly = false, int? size = null)
  //  => New(IInputTypeHtmlAttribute.Values.tel, value) with {
  //    MinLength = new(minLength),
  //    Pattern = new(pattern),
  //    PlaceHolder = new(placeHolder),
  //    ReadOnly = new(readOnly),
  //    Size = new(size)
  //  };

  //public static InputHtmlTag Text(string? value, int? minLength = null, string? pattern = null, string? placeHolder = null, bool readOnly = false, int? size = null)
  //  => New(IInputTypeHtmlAttribute.Values.text, value) with {
  //    MinLength = new(minLength),
  //    Pattern = new(pattern),
  //    PlaceHolder = new(placeHolder),
  //    ReadOnly = new(readOnly),
  //    Size = new(size)
  //  };

  //public static InputHtmlTag Url(string? value, int? minLength = null, string? pattern = null, string? placeHolder = null, bool readOnly = false, int? size = null)
  //  => New(IInputTypeHtmlAttribute.Values.url, value) with {
  //    MinLength = new(minLength),
  //    Pattern = new(pattern),
  //    PlaceHolder = new(placeHolder),
  //    ReadOnly = new(readOnly),
  //    Size = new(size)
  //  };
  #endregion

  //public static InputHtmlTag CheckBox<T>(T? value, bool isChecked = false, bool readOnly = false) where T : struct
  //  => New(IInputTypeHtmlAttribute.Values.checkbox, value) with { Checked = new(isChecked), ReadOnly = new(readOnly) };
  //public static InputHtmlTag Radio<T>(T? value, bool isChecked = false, bool readOnly = false) where T : struct
  //  => New(IInputTypeHtmlAttribute.Values.radio, value) with { Checked = new(isChecked), ReadOnly = new(readOnly) };

}

[Obsolete]
public static class xInputHtmlTagExtensions {

  //public readonly record struct HtmlInputValue {
  //  //[Obsolete("Use HtmlInputValue(object value, string type)")]
  //  //public HtmlInputValue(object value) {
  //  //  InputType = GetInputType(value);
  //  //  FormattedValue = GetInputFormattedValue(value);
  //  //}
  //  public HtmlInputValue(object value, IInputTypeHtmlAttribute.Values? type = IInputTypeHtmlAttribute.Values.blank) {
  //    var inputType = type == null ? GetInputType(value) : type.GetValue();
  //    InputType = inputType;
  //    FormattedValue = GetInputFormattedValue(value, inputType);
  //  }
  //  //public T Value { init; }
  //  public string InputType { get; }
  //  public string FormattedValue { get; }
  //}

  //public static string? GetInputType<T>(T value) => value switch {
  //  bool b => "checkbox",
  //  DateOnly d => "date",
  //  DateTime dt => "datetime-local",
  //  //DateTime? dt => "date",
  //  //Nullable<DateTime> dt => "date",
  //  double d => "number",
  //  decimal dec => "number",
  //  int i => "number",
  //  long l => "number",
  //  TimeOnly t => "time",
  //  TimeSpan ts => "time",
  //  _ => null
  //};

}

