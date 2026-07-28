using Common.Html.Attributes;
using System.Text;

namespace Common.Html.Tags;

public interface ISelectHtmlTag : IHtmlTag_v5, IInnerHtml, IHtmlFormElement {
  //IEnumerable<IOptionHtmlTag> Options { get; }
  //IEnumerable<IOptGroupHtmlTag> OptGroups { get; }
}

public static class ISelectHtmlTagExtensions {
  //https://www.w3schools.com/tags/tag_select.asp
  public static IAutoFocusHtmlAttribute AutoFocus(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<IAutoFocusHtmlAttribute>(IHtmlAttribute.Key.autofocus);
  public static T AutoFocus<T>(this T tag, AutoFocusHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);

  public static IDisabledHtmlAttribute Disabled(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<IDisabledHtmlAttribute>(IHtmlAttribute.Key.disabled);
  public static T Disabled<T>(this T tag, DisabledHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);

  public static IFormHtmlAttribute Form(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<IFormHtmlAttribute>(IHtmlAttribute.Key.form);
  public static T Form<T>(this T tag, FormHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);

  public static IMultipleHtmlAttribute Multiple(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<IMultipleHtmlAttribute>(IHtmlAttribute.Key.multiple);
  public static T Multiple<T>(this T tag, MultipleHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);

  public static INameHtmlAttribute Name(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<INameHtmlAttribute>(IHtmlAttribute.Key.name);
  public static T Name<T>(this T tag, NameHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);

  public static IOnChangeHtmlAttribute OnChange(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<IOnChangeHtmlAttribute>(IHtmlAttribute.Key.onchange);
  public static T OnChange<T>(this T tag, OnChangeHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);

  public static IRequiredHtmlAttribute Required(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<IRequiredHtmlAttribute>(IHtmlAttribute.Key.required);
  public static T Required<T>(this T tag, RequiredHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);

  public static ISizeHtmlAttribute Size(this ISelectHtmlTag tag) => tag.GetHtmlAttribute<ISizeHtmlAttribute>(IHtmlAttribute.Key.size);
  public static T Size<T>(this T tag, SizeHtmlAttribute value) where T : ISelectHtmlTag => tag.SetHtmlAttribute(value);
}

public record struct SelectHtmlTag(InnerHtml InnerHtml) : ISelectHtmlTag {
  public string TagName { get; } = "select";
  public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Normal;

  public HtmlAttributeDictionary Attributes { get; } = new();

  //public IEnumerable<IOptionHtmlTag> Options { get; }
  //public IEnumerable<IOptGroupHtmlTag> OptGroups { get; }

  //public InnerHtml InnerHtml => this.GetInnerHtml();

  public SelectHtmlTag() : this(OptionHtmlTag.EmptyCollection, OptGroupHtmlTag.EmptyCollection) { }
  //public SelectHtmlTag(string optionsHtmlRaw, string? emptyOptionText = null, params object[] selectedValues) : this(GetInnerHtml(optionsHtmlRaw, emptyOptionText, selectedValues)) { }
  public SelectHtmlTag(IEnumerable<IOptionHtmlTag> options) : this(options, OptGroupHtmlTag.EmptyCollection) { }
  public SelectHtmlTag(IEnumerable<IOptGroupHtmlTag> optGroups) : this(OptionHtmlTag.EmptyCollection, optGroups) { }
  public SelectHtmlTag(IEnumerable<IOptionHtmlTag> options, IEnumerable<IOptGroupHtmlTag> optGroups) : this(GetInnerHtml(options, optGroups)) { }

  public static SelectHtmlTag New<T>(string optionsHtmlRaw, string? emptyOptionText = null, params IEnumerable<T> selectedValues) => new SelectHtmlTag(GetInnerHtml(optionsHtmlRaw, emptyOptionText, selectedValues));

  public static InnerHtml GetInnerHtml(IEnumerable<IOptionHtmlTag> options, IEnumerable<IOptGroupHtmlTag> optGroups, string? emptyOptionText = null) {
    var html = new StringBuilder();
    if (emptyOptionText is not null) {    
      html.AppendLine(OptionHtmlTag.EmptyValue(emptyOptionText).GetRawHtml());
    }
    optGroups?.AppendLineTo(html);
    options?.AppendLineTo(html);
    return new InnerHtml(html.ToString());
  }


  public static InnerHtml GetInnerHtml<T>(string optionsHtmlRaw, string? emptyOptionText = null, params IEnumerable <T> selectedValues) {
    var html = new StringBuilder();
    if (!string.IsNullOrWhiteSpace(emptyOptionText)) {
      html.AppendLine(OptionHtmlTag.EmptyValue(emptyOptionText).GetRawHtml());
    }
    if (selectedValues is not null) {
      foreach (var s in selectedValues) {
        optionsHtmlRaw = optionsHtmlRaw.Replace($"value=\"{s}\"", $"value=\"{s}\" selected=\"selected\"");//, System.StringComparison.OrdinalIgnoreCase
      }
    }
    html.AppendLine(optionsHtmlRaw);
    return new InnerHtml(html.ToString());
  } 

}

public static class SelectHtmlExtensions {



  //public static InnerHtml GetInnerHtml(this ISelectHtmlTag tag) {
  //  var html = new StringBuilder();
  //  tag.OptGroups.AppendLineTo(html);
  //  tag.Options.AppendLineTo(html);
  //  return new InnerHtml(html.ToString());
  //}

}

