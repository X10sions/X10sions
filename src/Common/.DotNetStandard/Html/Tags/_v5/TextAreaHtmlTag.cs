using Common.Html.Attributes;

namespace Common.Html.Tags {

  public interface ITextAreaHtmlTag : IHtmlTag_v5, IHtmlFormElement, IInnerText {
    //public interface ICols : ITextAreaHtmlTag { }
    //public interface IDirName : ITextAreaHtmlTag { }
    //public interface IMaxLength : ITextAreaHtmlTag { }
    //public interface IPlaceHolder : ITextAreaHtmlTag { }
    //public interface IReadOnly : ITextAreaHtmlTag { }
    //public interface IRows : ITextAreaHtmlTag { }
    //public interface IWrap : ITextAreaHtmlTag { }
  }

  public static class ITextAreaHtmlTagExtensions {
    // https://www.w3schools.com/tags/tag_textarea.asp


    #region TextArea Specific Attributes

    public static IColsHtmlAttribute Cols(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IColsHtmlAttribute>(IHtmlAttribute.Key.cols);
    public static T Cols<T>(this T tag, ColsHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IDirNameHtmlAttribute DirName(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IDirNameHtmlAttribute>(IHtmlAttribute.Key.dirname);
    public static T DirName<T>(this T tag, DirNameHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IMaxLengthHtmlAttribute MaxLength(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IMaxLengthHtmlAttribute>(IHtmlAttribute.Key.maxlength);
    public static T MaxLength<T>(this T tag, MaxLengthHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IPlaceHolderHtmlAttribute PlaceHolder(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IPlaceHolderHtmlAttribute>(IHtmlAttribute.Key.placeholder);
    public static T PlaceHolder<T>(this T tag, PlaceHolderHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IReadOnlyHtmlAttribute ReadOnly(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IReadOnlyHtmlAttribute>(IHtmlAttribute.Key.@readonly);
    public static T ReadOnly<T>(this T tag, ReadOnlyHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IRowsHtmlAttribute Rows(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IRowsHtmlAttribute>(IHtmlAttribute.Key.rows);
    public static T Rows<T>(this T tag, RowsHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IWrapHtmlAttribute Wrap(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IWrapHtmlAttribute>(IHtmlAttribute.Key.wrap);
    public static T Wrap<T>(this T tag, WrapHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    #endregion

    public static IAutoFocusHtmlAttribute AutoFocus(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IAutoFocusHtmlAttribute>(IHtmlAttribute.Key.autofocus);
    public static T AutoFocus<T>(this T tag, AutoFocusHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IDisabledHtmlAttribute Disabled(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IDisabledHtmlAttribute>(IHtmlAttribute.Key.disabled);
    public static T Disabled<T>(this T tag, DisabledHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IFormHtmlAttribute Form(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IFormHtmlAttribute>(IHtmlAttribute.Key.form);
    public static T Form<T>(this T tag, FormHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static INameHtmlAttribute Name(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<INameHtmlAttribute>(IHtmlAttribute.Key.name);
    public static T Name<T>(this T tag, NameHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IOnChangeHtmlAttribute OnChange(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IOnChangeHtmlAttribute>(IHtmlAttribute.Key.onchange);
    public static T OnChange<T>(this T tag, OnChangeHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

    public static IRequiredHtmlAttribute Required(this ITextAreaHtmlTag tag) => tag.GetHtmlAttribute<IRequiredHtmlAttribute>(IHtmlAttribute.Key.required);
    public static T Required<T>(this T tag, RequiredHtmlAttribute value) where T : ITextAreaHtmlTag => tag.SetHtmlAttribute(value);

  }

  public record struct TextAreaHtmlTag(InnerText InnerText) : ITextAreaHtmlTag {
    public TextAreaHtmlTag(object innerText): this(new InnerText(innerText.ToString())) { }
    public TextAreaHtmlTag() : this(InnerText.Empty) { }

    public string TagName { get; } = "textarea";
    public IHtmlTag.ElementType Element { get; } = IHtmlTag.ElementType.EscapableRawText;

    public HtmlAttributeDictionary Attributes { get; } = new();


    //public static ITextAreaHtmlTag New<T>( T value) => new TextAreaHtmlTag().Value(new(value));
  }

  public static class TextAreaHtmlExtensions {

  }

}