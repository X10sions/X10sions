#nullable enable
using Common.Html.Attributes;
using System;

namespace Common.Html.Tags {

  public interface IInputHtmlTag : IHtmlTag_v5, IHtmlFormElement {
    public struct Formats {
      public const string Date = "yyyy-MM-dd";
      public const string DateTime = Date + " " + Time;
      public const string DateTimeLocal = Date + "T" + Time;
      public const string Month = "yyyy-MM";
      public const string Time = "HH:mm:ss";
      public const string Week = "yyyy-W00";
    }

    IInputTypeHtmlAttribute.Values Type { get; }

    // https://www.w3schools.com/tags/tag_input.asp
    public interface IAccept : IInputHtmlTag { }
    public interface IAlt : IInputHtmlTag { }
    public interface IAutoComplete : IInputHtmlTag { }
    //public interface IAutoFocus : IInputHtmlTag { }
    public interface ICapture : IInputHtmlTag { }
    public interface IChecked : IInputHtmlTag { }
    public interface IDirName : IInputHtmlTag { }
    //public interface IDisabled : IInputHtmlTag { }
    //public interface IForm : IInputHtmlTag { }
    public interface IFormAction : IInputHtmlTag { }
    public interface IFormEncType : IInputHtmlTag { }
    public interface IFormMethod : IInputHtmlTag { }
    public interface IFormNoValidate : IInputHtmlTag { }
    public interface IFormTarget : IInputHtmlTag { }
    public interface IHeight : IInputHtmlTag { }
    public interface IList : IInputHtmlTag { }
    public interface IMax : IInputHtmlTag { }
    public interface IMaxLength : IInputHtmlTag { }
    public interface IMin : IInputHtmlTag { }
    public interface IMinLength : IInputHtmlTag { }
    public interface IMultiple : IInputHtmlTag { }
    //public interface IName : IInputHtmlTag { }
    public interface IPattern : IInputHtmlTag { }
    public interface IPlaceHolder : IInputHtmlTag { }
    public interface IPopOverTarget : IInputHtmlTag { }
    public interface IPopOverTargetAction : IInputHtmlTag { }
    public interface IReadOnly : IInputHtmlTag { }
    public interface IRequired : IInputHtmlTag { }
    public interface ISize : IInputHtmlTag { }
    public interface IStep : IInputHtmlTag { }
    public interface ISrc : IInputHtmlTag { }
    //public interface IType : IInputHtmlTag { }
    //public interface IValue : IInputHtmlTag { }
    public interface IWidth : IInputHtmlTag { }
  }

  public static class IInputHtmlTagExtensions {
    // https://www.w3schools.com/tags/tag_input.asp

    #region All Inputs
    public static IAutoFocusHtmlAttribute AutoFocus(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IAutoFocusHtmlAttribute>(IHtmlAttribute.Key.autofocus);
    public static T AutoFocus<T>(this T tag, AutoFocusHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    //public static IAutoFocusHtmlAttribute AutoFocus(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IAutoFocusHtmlAttribute>(nameof(AutoFocus));
    //public static T AutoFocus<T>(this T tag, AutoFocusHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IDisabledHtmlAttribute Disabled(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IDisabledHtmlAttribute>(IHtmlAttribute.Key.disabled);
    public static T Disabled<T>(this T tag, DisabledHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IFormHtmlAttribute Form(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IFormHtmlAttribute>(IHtmlAttribute.Key.form);
    public static T Form<T>(this T tag, FormHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static INameHtmlAttribute Name(this IInputHtmlTag tag) => tag.GetHtmlAttribute<INameHtmlAttribute>(IHtmlAttribute.Key.name);
    public static T Name<T>(this T tag, NameHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);
    public static T EmptyName<T>(this T tag) where T : IInputHtmlTag => tag.SetHtmlAttribute(NameHtmlAttribute.Empty);

    public static IOnBlurHtmlAttribute OnBlur(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IOnBlurHtmlAttribute>(IHtmlAttribute.Key.onblur);
    public static T OnBlur<T>(this T tag, OnBlurHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);


    public static IOnChangeHtmlAttribute OnChange(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IOnChangeHtmlAttribute>(IHtmlAttribute.Key.onchange);
    public static T OnChange<T>(this T tag, OnChangeHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IOnClickHtmlAttribute OnClick(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IOnClickHtmlAttribute>(IHtmlAttribute.Key.onclick);
    public static T OnClick<T>(this T tag, OnClickHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IOnFocusHtmlAttribute OnFocus(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IOnFocusHtmlAttribute>(IHtmlAttribute.Key.onfocus);
    public static T OnFocus<T>(this T tag, OnFocusHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IOnInputHtmlAttribute OnInput(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IOnInputHtmlAttribute>(IHtmlAttribute.Key.oninput);
    public static T OnInput<T>(this T tag, OnInputHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IOnSubmitHtmlAttribute OnSubmit(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IOnSubmitHtmlAttribute>(IHtmlAttribute.Key.onsubmit);
    public static T OnSubmit<T>(this T tag, OnSubmitHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);


    public static IInputTypeHtmlAttribute Type(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IInputTypeHtmlAttribute>(IHtmlAttribute.Key.type);
    public static T Type<T>(this T tag, InputTypeHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IValueHtmlAttribute Value(this IInputHtmlTag tag) => tag.GetHtmlAttribute<IValueHtmlAttribute>(IHtmlAttribute.Key.value);
    public static T Value<T>(this T tag, IValueHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);
    public static T Value<T>(this T tag, object value) where T : IInputHtmlTag => tag.Value(new ValueHtmlAttribute(value, tag.Type));
    //public static T Value<T>(this T tag, object value) where T : IInputHtmlTag =>  tag.Value(new ValueHtmlAttribute(value, tag.Type));

    public static string HtmlFormattedValue(this IInputHtmlTag tag) => tag.Type.GetFormattedValue(tag.Value());

    #endregion

    public static IAcceptHtmlAttribute Accept(this IInputHtmlTag.IAccept tag) => tag.GetHtmlAttribute<IAcceptHtmlAttribute>(IHtmlAttribute.Key.accept);
    public static T Accept<T>(this T tag, AcceptHtmlAttribute value) where T : IInputHtmlTag.IAccept => tag.SetHtmlAttribute(value);

    public static IAltHtmlAttribute Alt(this IInputHtmlTag.IAlt tag) => tag.GetHtmlAttribute<IAltHtmlAttribute>(IHtmlAttribute.Key.alt);
    public static T Alt<T>(this T tag, AltHtmlAttribute value) where T : IInputHtmlTag.IAlt => tag.SetHtmlAttribute(value);

    public static IAutoCompleteHtmlAttribute AutoComplete(this IInputHtmlTag.IAutoComplete tag) => tag.GetHtmlAttribute<IAutoCompleteHtmlAttribute>(IHtmlAttribute.Key.autocomplete);
    public static T AutoComplete<T>(this T tag, AutoCompleteHtmlAttribute value) where T : IInputHtmlTag.IAutoComplete => tag.SetHtmlAttribute(value);

    public static ICaptureHtmlAttribute Capture(this IInputHtmlTag.ICapture tag) => tag.GetHtmlAttribute<ICaptureHtmlAttribute>(IHtmlAttribute.Key.capture);
    public static T Capture<T>(this T tag, CaptureHtmlAttribute value) where T : IInputHtmlTag.ICapture => tag.SetHtmlAttribute(value);


    public static ICheckedHtmlAttribute Checked(this IInputHtmlTag.IChecked tag) => tag.GetHtmlAttribute<ICheckedHtmlAttribute>(IHtmlAttribute.Key.@checked);
    public static T Checked<T>(this T tag, CheckedHtmlAttribute value) where T : IInputHtmlTag => tag.SetHtmlAttribute(value);

    public static IDirNameHtmlAttribute DirName(this IInputHtmlTag.IDirName tag) => tag.GetHtmlAttribute<IDirNameHtmlAttribute>(IHtmlAttribute.Key.dirname);
    public static T DirName<T>(this T tag, DirNameHtmlAttribute value) where T : IInputHtmlTag.IDirName => tag.SetHtmlAttribute(value);

    public static IFormActionHtmlAttribute FormAction(this IInputHtmlTag.IFormAction tag) => tag.GetHtmlAttribute<IFormActionHtmlAttribute>(IHtmlAttribute.Key.formaction);
    public static T FormAction<T>(this T tag, FormActionHtmlAttribute value) where T : IInputHtmlTag.IFormAction => tag.SetHtmlAttribute(value);

    public static IFormEncTypeHtmlAttribute FormEncType(this IInputHtmlTag.IFormEncType tag) => tag.GetHtmlAttribute<IFormEncTypeHtmlAttribute>(IHtmlAttribute.Key.formenctype);
    public static T FormEncType<T>(this T tag, FormEncTypeHtmlAttribute value) where T : IInputHtmlTag.IFormEncType => tag.SetHtmlAttribute(value);

    public static IFormMethodHtmlAttribute FormMethod(this IInputHtmlTag.IFormMethod tag) => tag.GetHtmlAttribute<IFormMethodHtmlAttribute>(IHtmlAttribute.Key.formmethod);
    public static T FormMethod<T>(this T tag, FormMethodHtmlAttribute value) where T : IInputHtmlTag.IFormMethod => tag.SetHtmlAttribute(value);

    public static IFormNoValidateHtmlAttribute FormNoValidate(this IInputHtmlTag.IFormNoValidate tag) => tag.GetHtmlAttribute<IFormNoValidateHtmlAttribute>(IHtmlAttribute.Key.formnovalidate);
    public static T FormNoValidate<T>(this T tag, FormNoValidateHtmlAttribute value) where T : IInputHtmlTag.IFormNoValidate => tag.SetHtmlAttribute(value);

    //    public static IFormTargetHtmlAttribute FormTarget(this IInputHtmlTag.IFormTarget tag) => tag.GetHtmlAttribute<IFormTargetHtmlAttribute>(nameof(FormTarget));
    //    public static T FormTarget<T>(this T tag, FormTargetHtmlAttribute value) where T : IInputHtmlTag.IFormTarget => tag.SetHtmlAttribute(value);

    public static IHeightHtmlAttribute Height(this IInputHtmlTag.IHeight tag) => tag.GetHtmlAttribute<IHeightHtmlAttribute>(IHtmlAttribute.Key.height);
    public static T Height<T>(this T tag, HeightHtmlAttribute value) where T : IInputHtmlTag.IHeight => tag.SetHtmlAttribute(value);

    public static IListHtmlAttribute List(this IInputHtmlTag.IList tag) => tag.GetHtmlAttribute<IListHtmlAttribute>(IHtmlAttribute.Key.list);
    public static T List<T>(this T tag, ListHtmlAttribute value) where T : IInputHtmlTag.IList => tag.SetHtmlAttribute(value);

    public static IMaxHtmlAttribute Max(this IInputHtmlTag.IMax tag) => tag.GetHtmlAttribute<IMaxHtmlAttribute>(IHtmlAttribute.Key.max);
    public static T Max<T>(this T tag, MaxHtmlAttribute value) where T : IInputHtmlTag.IMax => tag.SetHtmlAttribute(value);
    public static T Max<T>(this T tag, object value) where T : IInputHtmlTag.IMax => tag.Max(new(value, tag.Type));
    public static T MaxUseNowDate<T>(this T tag, bool value) where T : IInputDateHtmlTag.IMax => value ? tag.Max(new(DateTime.Now)).OnBlur(new("setMaxDate_yyyyMMdd(this)")) : tag;
    public static T MaxUseNowDateTimeLocal<T>(this T tag, bool value) where T : IInputDateTimeLocalHtmlTag.IMax => value ? tag.Max(new(DateTime.Now)).OnBlur(new("setMaxDate_yyyyMMdd_HHmmss(this)")) : tag;
    public static T MaxUseNowTime<T>(this T tag, bool value) where T : IInputTimeHtmlTag.IMax => value ? tag.Max(new(DateTime.Now)).OnBlur(new("setMaxDate_HHmmss(this)")) : tag;

    public static IMaxLengthHtmlAttribute MaxLength(this IInputHtmlTag.IMaxLength tag) => tag.GetHtmlAttribute<IMaxLengthHtmlAttribute>(IHtmlAttribute.Key.maxlength);
    public static T MaxLength<T>(this T tag, MaxLengthHtmlAttribute value) where T : IInputHtmlTag.IMaxLength => tag.SetHtmlAttribute(value);

    public static IMinHtmlAttribute Min(this IInputHtmlTag.IMin tag) => tag.GetHtmlAttribute<IMinHtmlAttribute>(IHtmlAttribute.Key.min);
    public static T Min<T>(this T tag, MinHtmlAttribute value) where T : IInputHtmlTag.IMin => tag.SetHtmlAttribute(value);
    public static T Min<T>(this T tag, object value) where T : IInputHtmlTag.IMin => tag.Min(new(value, tag.Type));

    public static IMinLengthHtmlAttribute MinLength(this IInputHtmlTag.IMinLength tag) => tag.GetHtmlAttribute<IMinLengthHtmlAttribute>(IHtmlAttribute.Key.minlength);
    public static T MinLength<T>(this T tag, MinLengthHtmlAttribute value) where T : IInputHtmlTag.IMinLength => tag.SetHtmlAttribute(value);

    public static IMultipleHtmlAttribute Multiple(this IInputHtmlTag.IMultiple tag) => tag.GetHtmlAttribute<IMultipleHtmlAttribute>(IHtmlAttribute.Key.multiple);
    public static T Multiple<T>(this T tag, MultipleHtmlAttribute value) where T : IInputHtmlTag.IMultiple => tag.SetHtmlAttribute(value);

    public static IPatternHtmlAttribute Pattern(this IInputHtmlTag.IPattern tag) => tag.GetHtmlAttribute<IPatternHtmlAttribute>(IHtmlAttribute.Key.pattern);
    public static T Pattern<T>(this T tag, PatternHtmlAttribute value) where T : IInputHtmlTag.IPattern => tag.SetHtmlAttribute(value);

    public static IPlaceHolderHtmlAttribute PlaceHolder(this IInputHtmlTag.IPlaceHolder tag) => tag.GetHtmlAttribute<IPlaceHolderHtmlAttribute>(IHtmlAttribute.Key.placeholder);
    public static T PlaceHolder<T>(this T tag, PlaceHolderHtmlAttribute value) where T : IInputHtmlTag.IPlaceHolder => tag.SetHtmlAttribute(value);

    public static IPopOverTargetHtmlAttribute PopOverTarget(this IInputHtmlTag.IPopOverTarget tag) => tag.GetHtmlAttribute<IPopOverTargetHtmlAttribute>(IHtmlAttribute.Key.popovertarget);
    public static T PopOverTarget<T>(this T tag, PopOverTargetHtmlAttribute value) where T : IInputHtmlTag.IPopOverTarget => tag.SetHtmlAttribute(value);

    public static IPopOverTargetActionHtmlAttribute PopOverTargetAction(this IInputHtmlTag.IPopOverTargetAction tag) => tag.GetHtmlAttribute<IPopOverTargetActionHtmlAttribute>(IHtmlAttribute.Key.popovertargetaction);
    public static T PopOverTargetAction<T>(this T tag, PopOverTargetActionHtmlAttribute value) where T : IInputHtmlTag.IPopOverTargetAction => tag.SetHtmlAttribute(value);

    public static IReadOnlyHtmlAttribute ReadOnly(this IInputHtmlTag.IReadOnly tag) => tag.GetHtmlAttribute<IReadOnlyHtmlAttribute>(IHtmlAttribute.Key.@readonly);
    public static T ReadOnly<T>(this T tag, ReadOnlyHtmlAttribute value) where T : IInputHtmlTag.IReadOnly => tag.SetHtmlAttribute(value);

    public static IRequiredHtmlAttribute Required(this IInputHtmlTag.IRequired tag) => tag.GetHtmlAttribute<IRequiredHtmlAttribute>(IHtmlAttribute.Key.required);
    public static T Required<T>(this T tag, RequiredHtmlAttribute value) where T : IInputHtmlTag.IRequired => tag.SetHtmlAttribute(value);

    public static ISizeHtmlAttribute Size(this IInputHtmlTag.ISize tag) => tag.GetHtmlAttribute<ISizeHtmlAttribute>(IHtmlAttribute.Key.size);
    public static T Size<T>(this T tag, SizeHtmlAttribute value) where T : IInputHtmlTag.ISize => tag.SetHtmlAttribute(value);

    public static IStepHtmlAttribute Step(this IInputHtmlTag.IStep tag) => tag.GetHtmlAttribute<IStepHtmlAttribute>(IHtmlAttribute.Key.step);
    public static T Step<T>(this T tag, StepHtmlAttribute value) where T : IInputHtmlTag.IStep => tag.SetHtmlAttribute(value);

    public static ISrcHtmlAttribute Src(this IInputHtmlTag.ISrc tag) => tag.GetHtmlAttribute<ISrcHtmlAttribute>(IHtmlAttribute.Key.src);
    public static T Src<T>(this T tag, SrcHtmlAttribute value) where T : IInputHtmlTag.ISrc => tag.SetHtmlAttribute(value);

    public static IWidthHtmlAttribute Width(this IInputHtmlTag.IWidth tag) => tag.GetHtmlAttribute<IWidthHtmlAttribute>(IHtmlAttribute.Key.width);
    public static T Width<T>(this T tag, WidthHtmlAttribute value) where T : IInputHtmlTag.IWidth => tag.SetHtmlAttribute(value);

  }

  //#region button types
  //public interface IInputButtonHtmlTag : IInputHtmlTag.IDirName, IInputHtmlTag.IPopOverTarget, IInputHtmlTag.IPopOverTargetAction { }
  //public interface IInputImageHtmlTag : IInputHtmlTag.IAlt, IInputHtmlTag.IFormAction, IInputHtmlTag.IFormEncType, IInputHtmlTag.IFormMethod, IInputHtmlTag.IFormTarget, IInputHtmlTag.IHeight, IInputHtmlTag.ISrc, IInputHtmlTag.IWidth { }
  //public interface IInputResetHtmlTag : IInputHtmlTag.IDirName { }
  //public interface IInputSubmitHtmlTag : IInputHtmlTag.IDirName, IInputHtmlTag.IFormAction, IInputHtmlTag.IFormEncType, IInputHtmlTag.IFormMethod, IInputHtmlTag.IFormNoValidate, IInputHtmlTag.IFormTarget, IInputHtmlTag.IRequired { }
  //#endregion

  //#region select types
  //public interface IInputCheckboxHtmlTag : IInputHtmlTag.IChecked, IInputHtmlTag.IRequired { }
  //public interface IInputColorHtmlTag : IInputHtmlTag.IList, IInputHtmlTag.IRequired { }
  //public interface IInputFileHtmlTag : IInputHtmlTag.IAccept, IInputHtmlTag.IMultiple, IInputHtmlTag.IRequired { }
  //public interface IInputRadioHtmlTag : IInputHtmlTag.IChecked, IInputHtmlTag.IRequired { }
  //#endregion

  //#region string types
  //public interface IInputEmailHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IDirName, IInputHtmlTag.IList, IInputHtmlTag.IMaxLength, IInputHtmlTag.IMinLength, IInputHtmlTag.IMultiple, IInputHtmlTag.IPattern, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.ISize { }
  //public interface IInputPasswordHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IDirName, IInputHtmlTag.IList, IInputHtmlTag.IMaxLength, IInputHtmlTag.IMinLength, IInputHtmlTag.IPattern, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.ISize { }
  //public interface IInputSearchHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IDirName, IInputHtmlTag.IList, IInputHtmlTag.IMaxLength, IInputHtmlTag.IMinLength, IInputHtmlTag.IPattern, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.ISize { }
  //public interface IInputTelHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IDirName, IInputHtmlTag.IList, IInputHtmlTag.IMaxLength, IInputHtmlTag.IMinLength, IInputHtmlTag.IPattern, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.ISize { }
  //public interface IInputUrlHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IDirName, IInputHtmlTag.IList, IInputHtmlTag.IMaxLength, IInputHtmlTag.IMinLength, IInputHtmlTag.IPattern, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.ISize { }
  //#endregion

  //#region date/time types
  //public interface IInputWeekHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IList, IInputHtmlTag.IMax, IInputHtmlTag.IMin, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.IStep { }
  //#endregion

}