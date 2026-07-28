using Common.Html.Attributes;
using System.Text;

namespace Common.Html.Tags;

public interface IHtmlTag {
  //HtmlTagName TagName { get; }
  string TagName { get; }
  HtmlAttributeDictionary Attributes { get; }
  ElementType Element { get; }

  public enum ElementType {
    //https://html.spec.whatwg.org/multipage/syntax.html#elements-2
    Normal, // All other allowed HTML elements are normal elements.
    Template, // template element
    RawText, // script, style
    EscapableRawText, // textarea, title
    Foreign, // Elements from the MathML namespace and the SVG namespace.
    Void,   // //area, base, br, col, embed, hr, img, input, link, meta, param, source, track, wbr
  }

  public enum Name {
    // https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements
    a,
    dialog,
    input,
    optgroup,
    option,
    select,
    span,
    textarea
  }
}

public record struct HtmlTag(string TagName, IHtmlTag.ElementType Element = IHtmlTag.ElementType.Normal) : IHtmlTag {//= false
                                                                                                                     //IsVoidElement: cannot have any contents (since there's no end tag, no content can be put between the start tag and the end tag).
                                                                                                                     // area, base, br, col, embed, hr, img, input, link, meta, source, track, wbr
  public HtmlAttributeDictionary Attributes { get; } = new();
}

//public interface IHtmlTag_v1: IHtmlTag { }
//public interface IHtmlTag_v2: IHtmlTag { }
//public interface IHtmlTag_v3_2: IHtmlTag { }
//public interface IHtmlTag_v5_1: IHtmlTag { }
//public interface IHtmlTag_v5_2: IHtmlTag { }

public static class IHtmlTagExtensions {

  public static IHtmlTag.Name GetHtmlTagName<T>(this T tag) where T : IHtmlTag => tag switch {
    IAHtmlTag => IHtmlTag.Name.a,
    IDialogHtmlTag => IHtmlTag.Name.dialog,
    IInputHtmlTag => IHtmlTag.Name.input,
    IOptGroupHtmlTag => IHtmlTag.Name.optgroup,
    IOptionHtmlTag => IHtmlTag.Name.optgroup,
    ISelectHtmlTag => IHtmlTag.Name.select,
    ISpanHtmlTag => IHtmlTag.Name.span,
    ITextAreaHtmlTag => IHtmlTag.Name.textarea,
    _ => throw new NotImplementedException($"{tag.GetType()}")
  };

  public static string Name(this IHtmlTag.Name tag) => tag switch {
    //IHtmlTag.ElementType.Void => true,
    _ => tag.ToString()
  };

  public static bool IsSelfClosing(this IHtmlTag.ElementType element) => element switch {
    IHtmlTag.ElementType.Void => true,
    _ => false
  };

  public static T AddHtmlAttribute<T, TValue>(this T tag, IHtmlAttribute.Key key, TValue value) where T : IHtmlTag where TValue : IHtmlAttribute<TValue> {
    tag.Attributes.Add(key, value);
    return tag;
  }

  public static StringBuilder AppendLineTo<T>(this IEnumerable<T> tags, StringBuilder sb) where T : IHtmlTag {
    foreach (var tag in tags) {
      sb.Append(tag.GetRawHtml());
    }
    return sb;
  }


  public static T SetHtmlAttribute<T, TValue>(this T tag, IHtmlAttribute.Key key, TValue value) where T : IHtmlTag where TValue : IHtmlAttribute {
    //try {
    tag.Attributes[key] = value;
    //} catch(Exception ex){
    //  throw new Exception($"{ex.Message}; key:{key}; tag:{tag is null}; tagAttr:{tag?.Attributes is null}; value:{value}; tagType:{tag?.GetType()}");
    //}
    return tag;
  }

  public static T SetHtmlAttribute<T, TAttribute>(this T tag, TAttribute attribute) where T : IHtmlTag where TAttribute : IHtmlAttribute => tag.SetHtmlAttribute(attribute.Name, attribute);

  public static TAttribute GetHtmlAttribute<TAttribute>(this IHtmlTag tag, IHtmlAttribute.Key key) where TAttribute : IHtmlAttribute => tag.GetHtmlAttribute<TAttribute>(key, default);
  //public static TAttribute GetHtmlAttribute<TAttribute>(this IHtmlTag tag) where TAttribute : IHtmlAttribute => tag.GetHtmlAttribute<TAttribute>(typeof(TAttribute).Name, default);

  public static T GetHtmlAttribute<T>(this IHtmlTag tag, IHtmlAttribute.Key key, T defaultValue) where T : IHtmlAttribute => tag.Attributes.TryGetValue(key, out var value) && value is T t ? t : defaultValue;

  public static string GetRawHtml(this IHtmlTag tag) => tag.Element.IsSelfClosing()
    ? $"<{tag.TagName} {tag.GetAttributesString()} />"
    : $"<{tag.TagName} {tag.GetAttributesString()}>{tag.GetContents()}</{tag.TagName}>";

  public static string GetContents(this IHtmlTag tag) => tag switch {
    IInnerHtml innerHtmlTag => innerHtmlTag.InnerHtml.Value,
    IInnerText innerTextTag => innerTextTag.InnerText.Value,
    _ => string.Empty
  };

  //public static HtmlString GetHtmlString(this IHtmlTag tag, string innerHtml) => new(HtmlRaw(innerHtml));

  public static string GetAttributesString(this IHtmlTag tag) {
    var attributes = from a in tag.Attributes
                     where !string.IsNullOrWhiteSpace(a.Value.GetHtmlKeyValue())
                     orderby a.Key
                     select a.Value.GetHtmlKeyValue();
    return string.Join(" ", attributes);
  }

  #region Global Attributes
  // https://www.w3schools.com/tags/ref_standardattributes.asp
  // https://www.w3schools.com/tags/ref_attributes.asp
  // https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Global_attributes

  public static IAccessKeyHtmlAttribute AccessKey(this IHtmlTag tag) => tag.GetHtmlAttribute<IAccessKeyHtmlAttribute>(IHtmlAttribute.Key.accesskey);
  public static T AccessKey<T>(this T tag, AccessKeyHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IClassHtmlAttribute Class(this IHtmlTag tag) {
    var attribute = tag.GetHtmlAttribute<IClassHtmlAttribute>(IHtmlAttribute.Key.@class);
    if (attribute is null) {
      attribute = new ClassHtmlAttribute(new());
      tag.SetHtmlAttribute(attribute);
      //tag.Attributes[nameof(Class)] = attribute;
    }
    return attribute;
  }
  public static T AddClass<T>(this T tag, params string[] cssClasses) where T : IHtmlTag {
    tag.Class().Add(cssClasses);
    return tag;
  }
  //public static T Class<T>(this T tag, ClassHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IContentEditableHtmlAttribute ContentEditable(this IHtmlTag tag) => tag.GetHtmlAttribute<IContentEditableHtmlAttribute>(IHtmlAttribute.Key.contenteditable);
  public static T ContentEditable<T>(this T tag, ContentEditableHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IDataHtmlAttribute Data(this IHtmlTag tag) {
    var attribute = tag.GetHtmlAttribute<IDataHtmlAttribute>(IHtmlAttribute.Key.data);
    if (attribute is null) {
      attribute = new DataHtmlAttribute(new());
      tag.SetHtmlAttribute(attribute);
      //tag.Attributes[nameof(Data)] = attribute;
    }
    return attribute;
  }
  public static T SetData<T>(this T tag, params DataItem[] dataItems) where T : IHtmlTag {
    tag.Data().Set(dataItems);
    return tag;
  }
  //public static T Data<T>(this T tag, DataHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IDirHtmlAttribute Dir(this IHtmlTag tag) => tag.GetHtmlAttribute<IDirHtmlAttribute>(IHtmlAttribute.Key.dir);
  public static T Dir<T>(this T tag, DirHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IDraggableHtmlAttribute Draggable(this IHtmlTag tag) => tag.GetHtmlAttribute<IDraggableHtmlAttribute>(IHtmlAttribute.Key.draggable);
  public static T Draggable<T>(this T tag, DraggableHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IEnterKeyHintHtmlAttribute EnterKeyHint(this IHtmlTag tag) => tag.GetHtmlAttribute<IEnterKeyHintHtmlAttribute>(IHtmlAttribute.Key.enterkeyhint);
  public static T EnterKeyHint<T>(this T tag, EnterKeyHintHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IHiddenHtmlAttribute Hidden(this IHtmlTag tag) => tag.GetHtmlAttribute<IHiddenHtmlAttribute>(IHtmlAttribute.Key.hidden);
  public static T Hidden<T>(this T tag, HiddenHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IIdHtmlAttribute Id(this IHtmlTag tag) => tag.GetHtmlAttribute<IIdHtmlAttribute>(IHtmlAttribute.Key.id);
  public static T Id<T>(this T tag, IdHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);
  public static T EmptyId<T>(this T tag) where T : IHtmlTag => tag.SetHtmlAttribute(IdHtmlAttribute.Empty);
  //public static T Id<T>(this T tag, IdHtmlAttribute value) where T : IHtmlTag => tag.Id(value);

  public static IInertHtmlAttribute Inert(this IHtmlTag tag) => tag.GetHtmlAttribute<IInertHtmlAttribute>(IHtmlAttribute.Key.inert);
  public static T Inert<T>(this T tag, InertHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IInputModeHtmlAttribute InputMode(this IHtmlTag tag) => tag.GetHtmlAttribute<IInputModeHtmlAttribute>(IHtmlAttribute.Key.inputmode);
  public static T InputMode<T>(this T tag, InputModeHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static ILangHtmlAttribute Lang(this IHtmlTag tag) => tag.GetHtmlAttribute<ILangHtmlAttribute>(IHtmlAttribute.Key.lang);
  public static T Lang<T>(this T tag, LangHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IPopOverHtmlAttribute PopOver(this IHtmlTag tag) => tag.GetHtmlAttribute<IPopOverHtmlAttribute>(IHtmlAttribute.Key.popover);
  public static T PopOver<T>(this T tag, PopoverHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);
  public static T RemoveClass<T>(this T tag, params string[] cssClasses) where T : IHtmlTag {
    tag.Class().Remove(cssClasses);
    return tag;
  }

  public static ISpellCheckHtmlAttribute SpellCheck(this IHtmlTag tag) => tag.GetHtmlAttribute<ISpellCheckHtmlAttribute>(IHtmlAttribute.Key.spellcheck);
  public static T SpellCheck<T>(this T tag, SpellCheckHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static IStyleHtmlAttribute Style(this IHtmlTag tag) {
    var attribute = tag.GetHtmlAttribute<IStyleHtmlAttribute>(IHtmlAttribute.Key.style);
    if (attribute is null) {
      attribute = new StyleHtmlAttribute(new());
      tag.SetHtmlAttribute(attribute);
      //tag.Attributes[nameof(Data)] = attribute;
    }
    return attribute;
  }
  public static T SetStyle<T>(this T tag, params CssSelector[] cssSelectors) where T : IHtmlTag {
    tag.Style().Set(cssSelectors);
    return tag;
  }
  //    public static T Style<T>(this T tag, StyleHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);



  public static ITabIndexHtmlAttribute TabIndex(this IHtmlTag tag) => tag.GetHtmlAttribute<ITabIndexHtmlAttribute>(IHtmlAttribute.Key.tabindex);
  public static T TabIndex<T>(this T tag, TabIndexHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  public static ITitleHtmlAttribute Title(this IHtmlTag tag) => tag.GetHtmlAttribute<ITitleHtmlAttribute>(IHtmlAttribute.Key.title);
  public static T Title<T>(this T tag, TitleHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  //public static ITranslateHtmlAttribute Translate(this IHtmlTag tag) => tag.GetHtmlAttribute<ITranslateHtmlAttribute>(IHtmlAttribute.Key.translate);
  //public static T Translate<T>(this T tag, TranslateHtmlAttribute value) where T : IHtmlTag => tag.SetHtmlAttribute(value);

  #endregion

}