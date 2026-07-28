using Common.Html.Attributes;
using Common.Models;
using System.Text;

namespace Common.Html.Tags;

public interface IOptionHtmlTag : IHtmlTag_v5, IInnerText, IHtmlFormElement {
  // https://www.w3schools.com/tags/tag_option.asp
  //IDisabledHtmlAttribute Disabled { get; }
  //ILabelHtmlAttribute xLabel { get; }
  IOptGroupHtmlTag OptGroup { get; }
  //ISelectedHtmlAttribute Selected { get; }
  //IValueHtmlAttribute Value { get; }
}

public static class IOptionHtmlTagExtensions {

  public static IEnumerable<IOptionHtmlTag> AsOptionHtmlTags<T>(params T[] selectedValues) where T : Enum {
    var values = Enum.GetValues(typeof(T)).OfType<T>();
    foreach (var v in values) {
      yield return new OptionHtmlTag(v.ToString(), Convert.ToInt32(v), selectedValues.Contains(v));
    }
  }

  public static IDisabledHtmlAttribute Disabled(this IOptionHtmlTag tag) => tag.GetHtmlAttribute<IDisabledHtmlAttribute>(IHtmlAttribute.Key.disabled);
  public static T Disabled<T>(this T tag, DisabledHtmlAttribute value) where T : IOptionHtmlTag => tag.SetHtmlAttribute(value);

  public static ILabelHtmlAttribute Label(this IOptionHtmlTag tag) => tag.GetHtmlAttribute<ILabelHtmlAttribute>(IHtmlAttribute.Key.label);
  public static T Label<T>(this T tag, ILabelHtmlAttribute value) where T : IOptionHtmlTag => tag.SetHtmlAttribute(value);

  //    public static IOptGroupHtmlTag OptGroup(this IOptionHtmlTag tag) => tag.GetHtmlAttribute<IOptGroupHtmlTag>(nameof(OptGroup));
  //    public static T OptGroup<T>(this T tag, IOptGroupHtmlTag value) where T : IOptionHtmlTag => tag.SetHtmlAttribute(value);

  public static ISelectedHtmlAttribute Selected(this IOptionHtmlTag tag) => tag.GetHtmlAttribute<ISelectedHtmlAttribute>(IHtmlAttribute.Key.selected);
  public static T Selected<T>(this T tag, ISelectedHtmlAttribute value) where T : IOptionHtmlTag => tag.SetHtmlAttribute(value);
  public static IValueHtmlAttribute Value(this IOptionHtmlTag tag) => tag.GetHtmlAttribute<IValueHtmlAttribute>(IHtmlAttribute.Key.value);
  public static T Value<T>(this T tag, IValueHtmlAttribute value) where T : IOptionHtmlTag => tag.SetHtmlAttribute(value);
}

public record struct OptionHtmlTag(InnerText InnerText) : IOptionHtmlTag {
  public OptionHtmlTag() : this(InnerText.Empty) { }
  public OptionHtmlTag(InnerText innerText, IValueHtmlAttribute value, ISelectedHtmlAttribute selected) : this(innerText) {
    this.Value(value);
    this.Selected(selected);
  }

  public OptionHtmlTag(string innerText, object? value = null, bool selected = false) : this(
    new InnerText(innerText),
    new ValueHtmlAttribute(value, IInputTypeHtmlAttribute.Values.blank),
    new SelectedHtmlAttribute(selected)
    ) { }

  //public OptionHtmlTag(string innerText, object? value = null, bool selected = false) : this(new InnerText(innerText)) {
  //  this.Value(new ValueHtmlAttribute(value, IInputTypeHtmlAttribute.Values.blank));
  //  this.Selected(new SelectedHtmlAttribute(selected));
  //}

  public static IEnumerable<IOptionHtmlTag> GetListFromEnum<T>() where T : struct, Enum => from T v in Enum.GetValues(typeof(T)) select v.AsOption(false);


  public string TagName { get; } = "option";
  public IHtmlTag.ElementType Element { get; } = IHtmlTag.ElementType.Normal;
  public HtmlAttributeDictionary Attributes { get; } = new();
  public IOptGroupHtmlTag OptGroup { get; set; } = new OptGroupHtmlTag();

  public static IEnumerable<IOptionHtmlTag> EmptyCollection { get; } = Enumerable.Empty<IOptionHtmlTag>();
  public static IOptionHtmlTag Empty { get; } = new OptionHtmlTag(string.Empty, ValueHtmlAttribute.Empty.Value) { };
  public static IOptionHtmlTag EmptyValue(string? innerText = null) => new OptionHtmlTag(innerText ?? string.Empty, ValueHtmlAttribute.Empty.Value) { };
  //    public IDisabledHtmlAttribute Disabled { get => this.GetHtmlAttribute<IDisabledHtmlAttribute>(nameof(Disabled)); set => this.SetHtmlAttribute(value); }
  //    public ILabelHtmlAttribute Label { get => this.GetHtmlAttribute<ILabelHtmlAttribute>(nameof(Label)); set => this.SetHtmlAttribute(value); }
  //    public ISelectedHtmlAttribute Selected { get => this.GetHtmlAttribute<ISelectedHtmlAttribute>(nameof(Selected)); set => this.SetHtmlAttribute(value); }
  //    public IValueHtmlAttribute Value { get => this.GetHtmlAttribute<IValueHtmlAttribute>(nameof(Value)); set => this.SetHtmlAttribute(value); }

}
public static class OptionHtmlTagExtensions {
  public static IEnumerable<IOptionHtmlTag> AddEmptyValue(this IEnumerable<IOptionHtmlTag> options, string emptyOptionText) => options?.Prepend(OptionHtmlTag.EmptyValue(emptyOptionText));

  public static void AppendLineTo(this IOptionHtmlTag option, StringBuilder sb) => sb.AppendLine($"<option value=\"{option.Value().Value}\" {(option.Selected().Value ? option.Selected().Name : string.Empty)} >{option.InnerText.Value}</option>");
  //public static void AppendLineTo(this Option option, StringBuilder sb) => sb.AppendLine($"<option value=\"{option.Value.Value}\" selected=\"{option.Selected.Value}\">{option.InnerText.Value}</option>");
  //public static StringBuilder AppendLineTo(this IEnumerable<IOptionHtmlTag> options, StringBuilder sb) {
  //  foreach (var option in options) {
  //    //option.AppendLineTo(sb);
  //    sb.Append(option.ToHtmlString());
  //  }
  //  return sb;
  //}

  public static IOptionHtmlTag AsOption<T>(this T enumValue, bool isSelected) where T : struct, Enum => new OptionHtmlTag(enumValue.GetDisplayName(), Convert.ToInt32(enumValue), isSelected);

  public static IEnumerable<IOptionHtmlTag> AsOptions<T>(this T enumValue, bool setSelected) where T : struct, Enum => from T v in Enum.GetValues(typeof(T)) select v.AsOption(setSelected && enumValue.IsSelected(v));
  public static IEnumerable<IOptionHtmlTag> AsOptions<T>(this IEnumerable<T> enumObj, bool setSelected) where T : struct, Enum => from T v in Enum.GetValues(typeof(T)) select v.AsOption(setSelected && enumObj.Contains(v));

  public static bool IsSelected<T>(this T targetValue, object value) where T : struct, Enum {
    var targetIntValue = Convert.ToInt32(targetValue);
    if (Enum.TryParse(value.ToString(), ignoreCase: true, out T parsedValue)) {
      if (Convert.ToInt32(parsedValue) == targetIntValue) {
        return true;
      }
    }
    return false;
  }
  public static string JoinToHtmlString(this IEnumerable<IOptionHtmlTag> options) => options.AppendLineTo(new StringBuilder()).ToString();

}