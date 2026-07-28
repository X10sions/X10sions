#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Html.Attributes {

  public interface IHtmlAttribute { //}: IHtmlAttribute<object?> {
    IHtmlAttribute.Key Name { get; }
    //public string AttributeKey = "hx-swap-oob";
    //public string xHtmlKeyValue { get; }// => $"{Swap.ToString().ToLower()}{(string.IsNullOrWhiteSpace(CssSelector.Value) ? string.Empty : ":" + CssSelector.Value)}";

    public enum Key {
      accept,    // <input>
      accept_charset,    // <form>
      accesskey,   // Global Attributes
      action,    // <form>
      [Obsolete] align,   // Not supported in HTML 5.
      alt,   // <area>, <img>, <input>
      async,   // <script>
      autocomplete,    // <form>, <input>
      autofocus,   // <button>, <input>, <select>, <textarea>
      autoplay,    // <audio>, <video>
      [Obsolete] bgcolor,   // Not supported in HTML 5.
      [Obsolete] border,    // Not supported in HTML 5.
      capture,
      charset,   // <meta>, <script>
      @checked,   // <input>
      cite,    // <blockquote>, <del>, <ins>, <q>
      @class,    // Global Attributes
      [Obsolete] color,   // Not supported in HTML 5.
      cols,    // <textarea>
      colspan,   // <td>, <th>
      content,   // <meta>
      contenteditable,   // Global Attributes
      controls,    // <audio>, <video>
      coords,    // <area>
      data,    // <object>
               //      data-*,    // Global Attributes
      datetime,    // <del>, <ins>, <time>
      @default,   // <track>
      defer,   // <script>
      dir,   // Global Attributes
      dirname,   // <input>, <textarea>
      disabled,    // <button>, <fieldset>, <input>, <optgroup>, <option>, <select>, <textarea>
      download,    // <a>, <area>
      draggable,   // Global Attributes
      dropzone,
      enctype,   // <form>
      enterkeyhint,    // Global Attributes
      @for,   // <label>, <output>
      form,    // <button>, <fieldset>, <input>, <label>, <meter>, <object>, <output>, <select>, <textarea>
      formaction,    // <button>, <input>
      formenctype,
      formmethod,
      formnovalidate,
      headers,   // <td>, <th>
      height,    // <canvas>, <embed>, <iframe>, <img>, <input>, <object>, <video>
      hidden,    // Global Attributes
      high,    // <meter>
      href,    // <a>, <area>, <base>, <link>
      hreflang,    // <a>, <area>, <link>
      http_equiv,    // <meta>
      id,    // Global Attributes
      inert,   // Global Attributes
      inputmode,   // Global Attributes
      ismap,   // <img>
      kind,    // <track>
      label,   // <track>, <option>, <optgroup>
      lang,    // Global Attributes
      list,    // <input>
      loop,    // <audio>, <video>
      low,   // <meter>
      max,   // <input>, <meter>, <progress>
      maxlength,   // <input>, <textarea>
      media,   // <a>, <area>, <link>, <source>, <style>
      method,    // <form>
      min,   // <input>, <meter>
      minlength,
      multiple,    // <input>, <select>
      muted,   // <video>, <audio>
      name,    // <button>, <fieldset>, <form>, <iframe>, <input>, <map>, <meta>, <object>, <output>, <param>, <select>, <textarea>
      novalidate,    // <form>
      onabort,   // <audio>, <embed>, <img>, <object>, <video>
      onafterprint,    // <body>
      onbeforeprint,   // <body>
      onbeforeunload,    // <body>
      onblur,    // All visible elements.
      oncanplay,   // <audio>, <embed>, <object>, <video>
      oncanplaythrough,    // <audio>, <video>
      onchange,    // All visible elements.
      onclick,   // All visible elements.
      oncontextmenu,   // All visible elements.
      oncopy,    // All visible elements.
      oncuechange,   // <track>
      oncut,   // All visible elements.
      ondblclick,    // All visible elements.
      ondrag,    // All visible elements.
      ondragend,   // All visible elements.
      ondragenter,   // All visible elements.
      ondragleave,   // All visible elements.
      ondragover,    // All visible elements.
      ondragstart,   // All visible elements.
      ondrop,    // All visible elements.
      ondurationchange,    // <audio>, <video>
      onemptied,   // <audio>, <video>
      onended,   // <audio>, <video>
      onerror,   // <audio>, <body>, <embed>, <img>, <object>, <script>, <style>, <video>
      onfocus,   // All visible elements.
      onhashchange,    // <body>
      oninput,   // All visible elements.
      oninvalid,   // All visible elements.
      onkeydown,   // All visible elements.
      onkeypress,    // All visible elements.
      onkeyup,   // All visible elements.
      onload,    // <body>, <iframe>, <img>, <input>, <link>, <script>, <style>
      onloadeddata,    // <audio>, <video>
      onloadedmetadata,    // <audio>, <video>
      onloadstart,   // <audio>, <video>
      onmousedown,   // All visible elements.
      onmousemove,   // All visible elements.
      onmouseout,    // All visible elements.
      onmouseover,   // All visible elements.
      onmouseup,   // All visible elements.
      onmousewheel,    // All visible elements.
      onoffline,   // <body>
      ononline,    // <body>
      onpagehide,    // <body>
      onpageshow,    // <body>
      onpaste,   // All visible elements.
      onpause,   // <audio>, <video>
      onplay,    // <audio>, <video>
      onplaying,   // <audio>, <video>
      onpopstate,    // <body>
      onprogress,    // <audio>, <video>
      onratechange,    // <audio>, <video>
      onreset,   // <form>
      onresize,    // <body>
      onscroll,    // All visible elements.
      onsearch,    // <input>
      onseeked,    // <audio>, <video>
      onseeking,   // <audio>, <video>
      onselect,    // All visible elements.
      onstalled,   // <audio>, <video>
      onstorage,   // <body>
      onsubmit,    // <form>
      onsuspend,   // <audio>, <video>
      ontimeupdate,    // <audio>, <video>
      ontoggle,    // <details>
      onunload,    // <body>
      onvolumechange,    // <audio>, <video>
      onwaiting,   // <audio>, <video>
      onwheel,   // All visible elements.
      open,    // <details>
      optimum,   // <meter>
      pattern,   // <input>
      ping,
      placeholder,   // <input>, <textarea>
      popover,   // Global Attributes
      popovertarget,   // <button>, <input>
      popovertargetaction,   // <button>, <input>
      poster,    // <video>
      preload,   // <audio>, <video>
      @readonly,   // <input>, <textarea>
      referrerpolicy,
      rel,   // <a>, <area>, <form>, <link>
      required,    // <input>, <select>, <textarea>
      reversed,    // <ol>
      rows,    // <textarea>
      rowspan,   // <td>, <th>
      sandbox,   // <iframe>
      scope,   // <th>
      selected,    // <option>
      shape,   // <area>
      size,    // <input>, <select>
      sizes,   // <img>, <link>, <source>
      span,    // <col>, <colgroup>
      spellcheck,    // Global Attributes
      src,   // <audio>, <embed>, <iframe>, <img>, <input>, <script>, <source>, <track>, <video>
      srcdoc,    // <iframe>
      srclang,   // <track>
      srcset,    // <img>, <source>
      start,   // <ol>
      step,    // <input>
      style,   // Global Attributes
      tabindex,    // Global Attributes
      target,    // <a>, <area>, <base>, <form>
      title,   // Global Attributes
      [Obsolete] translate,   // Global Attributes
      type,    // <a>, <button>, <embed>, <input>, <link>, <menu>, <object>, <script>, <source>, <style>
      usemap,    // <img>, <object>
      value,   // <button>, <input>, <li>, <option>, <meter>, <progress>, <param>
      width,   // <canvas>, <embed>, <iframe>, <img>, <input>, <object>, <video>
      wrap,	 // <textarea>
    }

  }

  public interface IHtmlAttribute<TValue> : IHtmlAttribute {
    //public string Name { get; }
    public TValue Value { get; }
  }
  public interface IHtmlAttributeBool : IHtmlAttribute<bool> { }
  public interface IHtmlAttributeDecimal : IHtmlAttribute<decimal?> { }
  public interface IHtmlAttributeInt : IHtmlAttribute<int?> { }
  public interface IHtmlAttributeObject : IHtmlAttribute<object?> { }
  public interface IHtmlAttributeString : IHtmlAttribute<string?> { }

  public interface IHtmlAttribute<TTag, TValue> : IHtmlAttribute<TValue> {
    public TTag Tag { get; }
  }

  public static class IHtmlAttributeExtensions {

    public static string ToHtmlAttributeValue<T>(this T value) => value switch {
      null => string.Empty,
      bool b => b.ToString().ToLower(),
      HashSet<string> hashSet => string.Join(" ", hashSet.OrderBy(x => x)),
      SortedSet<string> sortedSet => string.Join(" ", sortedSet),
      string s => s,
      _ => value.ToString(),
    };

    //public static string GetHtmlKeyOnly<T>(this IHtmlAttribute<T> attr, bool hasValue) => hasValue ? attr.Name : string.Empty;
    //public static string GetHtmlKeyValue(this IHtmlAttribute attr, string? value) => string.IsNullOrWhiteSpace(value) ? string.Empty : $"{attr.Name}=\"{value.ToHtmlAttributeValue()}\"";

    public static IHtmlAttribute.Key GetHtmlKey<T>(this T attr) where T : IHtmlAttribute => attr switch {
      IAcceptHtmlAttribute => IHtmlAttribute.Key.accept,
      //IacceptcharsetHtmlAttribute => IHtmlAttribute.Key.accept_charset,
      IAccessKeyHtmlAttribute => IHtmlAttribute.Key.accesskey,
      //IactionHtmlAttribute => IHtmlAttribute.Key.action,
      //Obsolete IAlignHtmlAttribute => IHtmlAttribute.Key.align,
      IAltHtmlAttribute => IHtmlAttribute.Key.alt,
      //IasyncHtmlAttribute => IHtmlAttribute.Key.async,
      IAutoCompleteHtmlAttribute => IHtmlAttribute.Key.autocomplete,
      IAutoFocusHtmlAttribute => IHtmlAttribute.Key.autofocus,
      //IautoplayHtmlAttribute => IHtmlAttribute.Key.autoplay,
      //IbgcolorHtmlAttribute => IHtmlAttribute.Key.bgcolor,
      //IborderHtmlAttribute => IHtmlAttribute.Key.border,
      ICaptureHtmlAttribute => IHtmlAttribute.Key.capture,
      //IcharsetHtmlAttribute => IHtmlAttribute.Key.charset,
      ICheckedHtmlAttribute => IHtmlAttribute.Key.@checked,
      //IciteHtmlAttribute => IHtmlAttribute.Key.cite,
      IClassHtmlAttribute => IHtmlAttribute.Key.@class,
      //IcolorHtmlAttribute => IHtmlAttribute.Key.color,
      IColsHtmlAttribute => IHtmlAttribute.Key.cols,
      //IcolspanHtmlAttribute => IHtmlAttribute.Key.colspan,
      //IcontentHtmlAttribute => IHtmlAttribute.Key.content,
      IContentEditableHtmlAttribute => IHtmlAttribute.Key.contenteditable,
      //IcontrolsHtmlAttribute => IHtmlAttribute.Key.controls,
      //IcoordsHtmlAttribute => IHtmlAttribute.Key.coords,
      //IdataHtmlAttribute => IHtmlAttribute.Key.data,
      //Idata-*HtmlAttribute => IHtmlAttribute.Key.data-*,
      //IdatetimeHtmlAttribute => IHtmlAttribute.Key.datetime,
      //IdefaultHtmlAttribute => IHtmlAttribute.Key.@default,
      //IdeferHtmlAttribute => IHtmlAttribute.Key.defer,
      IDirHtmlAttribute => IHtmlAttribute.Key.dir,
      IDirNameHtmlAttribute => IHtmlAttribute.Key.dirname,
      IDisabledHtmlAttribute => IHtmlAttribute.Key.disabled,
      IDownloadHtmlAttribute => IHtmlAttribute.Key.download,
      IDraggableHtmlAttribute => IHtmlAttribute.Key.draggable,
      //IenctypeHtmlAttribute => IHtmlAttribute.Key.enctype,
      IEnterKeyHintHtmlAttribute => IHtmlAttribute.Key.enterkeyhint,
      //IforHtmlAttribute => IHtmlAttribute.Key.@for,
      IFormHtmlAttribute => IHtmlAttribute.Key.form,
      IFormActionHtmlAttribute => IHtmlAttribute.Key.formaction,
      //IheadersHtmlAttribute => IHtmlAttribute.Key.headers,
      IHeightHtmlAttribute => IHtmlAttribute.Key.height,
      IHiddenHtmlAttribute => IHtmlAttribute.Key.hidden,
      //IhighHtmlAttribute => IHtmlAttribute.Key.high,
      IHrefHtmlAttribute => IHtmlAttribute.Key.href,
      IHrefLangHtmlAttribute => IHtmlAttribute.Key.hreflang,
      //IhttpequivHtmlAttribute => IHtmlAttribute.Key.http_equiv,
      IIdHtmlAttribute => IHtmlAttribute.Key.id,
      IInertHtmlAttribute => IHtmlAttribute.Key.inert,
      IInputModeHtmlAttribute => IHtmlAttribute.Key.inputmode,
      //IismapHtmlAttribute => IHtmlAttribute.Key.ismap,
      //IkindHtmlAttribute => IHtmlAttribute.Key.kind,
      ILabelHtmlAttribute => IHtmlAttribute.Key.label,
      ILangHtmlAttribute => IHtmlAttribute.Key.lang,
      IListHtmlAttribute => IHtmlAttribute.Key.list,
      //IloopHtmlAttribute => IHtmlAttribute.Key.loop,
      //IlowHtmlAttribute => IHtmlAttribute.Key.low,
      IMaxHtmlAttribute => IHtmlAttribute.Key.max,
      IMaxLengthHtmlAttribute => IHtmlAttribute.Key.maxlength,
      IMediaHtmlAttribute => IHtmlAttribute.Key.media,
      //ImethodHtmlAttribute => IHtmlAttribute.Key.method,
      IMinHtmlAttribute => IHtmlAttribute.Key.min,
      IMultipleHtmlAttribute => IHtmlAttribute.Key.multiple,
      //ImutedHtmlAttribute => IHtmlAttribute.Key.muted,
      INameHtmlAttribute => IHtmlAttribute.Key.name,
      //InovalidateHtmlAttribute => IHtmlAttribute.Key.novalidate,
      //IonabortHtmlAttribute => IHtmlAttribute.Key.onabort,
      //IonafterprintHtmlAttribute => IHtmlAttribute.Key.onafterprint,
      //IonbeforeprintHtmlAttribute => IHtmlAttribute.Key.onbeforeprint,
      //IonbeforeunloadHtmlAttribute => IHtmlAttribute.Key.onbeforeunload,
      //IonblurHtmlAttribute => IHtmlAttribute.Key.onblur,
      //IoncanplayHtmlAttribute => IHtmlAttribute.Key.oncanplay,
      //IoncanplaythroughHtmlAttribute => IHtmlAttribute.Key.oncanplaythrough,
      IOnChangeHtmlAttribute => IHtmlAttribute.Key.onchange,
      //IonclickHtmlAttribute => IHtmlAttribute.Key.onclick,
      //IoncontextmenuHtmlAttribute => IHtmlAttribute.Key.oncontextmenu,
      //IoncopyHtmlAttribute => IHtmlAttribute.Key.oncopy,
      //IoncuechangeHtmlAttribute => IHtmlAttribute.Key.oncuechange,
      //IoncutHtmlAttribute => IHtmlAttribute.Key.oncut,
      //IondblclickHtmlAttribute => IHtmlAttribute.Key.ondblclick,
      //IondragHtmlAttribute => IHtmlAttribute.Key.ondrag,
      //IondragendHtmlAttribute => IHtmlAttribute.Key.ondragend,
      //IondragenterHtmlAttribute => IHtmlAttribute.Key.ondragenter,
      //IondragleaveHtmlAttribute => IHtmlAttribute.Key.ondragleave,
      //IondragoverHtmlAttribute => IHtmlAttribute.Key.ondragover,
      //IondragstartHtmlAttribute => IHtmlAttribute.Key.ondragstart,
      //IondropHtmlAttribute => IHtmlAttribute.Key.ondrop,
      //IondurationchangeHtmlAttribute => IHtmlAttribute.Key.ondurationchange,
      //IonemptiedHtmlAttribute => IHtmlAttribute.Key.onemptied,
      //IonendedHtmlAttribute => IHtmlAttribute.Key.onended,
      //IonerrorHtmlAttribute => IHtmlAttribute.Key.onerror,
      //IonfocusHtmlAttribute => IHtmlAttribute.Key.onfocus,
      //IonhashchangeHtmlAttribute => IHtmlAttribute.Key.onhashchange,
      //IoninputHtmlAttribute => IHtmlAttribute.Key.oninput,
      //IoninvalidHtmlAttribute => IHtmlAttribute.Key.oninvalid,
      //IonkeydownHtmlAttribute => IHtmlAttribute.Key.onkeydown,
      //IonkeypressHtmlAttribute => IHtmlAttribute.Key.onkeypress,
      //IonkeyupHtmlAttribute => IHtmlAttribute.Key.onkeyup,
      //IonloadHtmlAttribute => IHtmlAttribute.Key.onload,
      //IonloadeddataHtmlAttribute => IHtmlAttribute.Key.onloadeddata,
      //IonloadedmetadataHtmlAttribute => IHtmlAttribute.Key.onloadedmetadata,
      //IonloadstartHtmlAttribute => IHtmlAttribute.Key.onloadstart,
      //IonmousedownHtmlAttribute => IHtmlAttribute.Key.onmousedown,
      //IonmousemoveHtmlAttribute => IHtmlAttribute.Key.onmousemove,
      //IonmouseoutHtmlAttribute => IHtmlAttribute.Key.onmouseout,
      //IonmouseoverHtmlAttribute => IHtmlAttribute.Key.onmouseover,
      //IonmouseupHtmlAttribute => IHtmlAttribute.Key.onmouseup,
      //IonmousewheelHtmlAttribute => IHtmlAttribute.Key.onmousewheel,
      //IonofflineHtmlAttribute => IHtmlAttribute.Key.onoffline,
      //IononlineHtmlAttribute => IHtmlAttribute.Key.ononline,
      //IonpagehideHtmlAttribute => IHtmlAttribute.Key.onpagehide,
      //IonpageshowHtmlAttribute => IHtmlAttribute.Key.onpageshow,
      //IonpasteHtmlAttribute => IHtmlAttribute.Key.onpaste,
      //IonpauseHtmlAttribute => IHtmlAttribute.Key.onpause,
      //IonplayHtmlAttribute => IHtmlAttribute.Key.onplay,
      //IonplayingHtmlAttribute => IHtmlAttribute.Key.onplaying,
      //IonpopstateHtmlAttribute => IHtmlAttribute.Key.onpopstate,
      //IonprogressHtmlAttribute => IHtmlAttribute.Key.onprogress,
      //IonratechangeHtmlAttribute => IHtmlAttribute.Key.onratechange,
      //IonresetHtmlAttribute => IHtmlAttribute.Key.onreset,
      //IonresizeHtmlAttribute => IHtmlAttribute.Key.onresize,
      //IonscrollHtmlAttribute => IHtmlAttribute.Key.onscroll,
      //IonsearchHtmlAttribute => IHtmlAttribute.Key.onsearch,
      //IonseekedHtmlAttribute => IHtmlAttribute.Key.onseeked,
      //IonseekingHtmlAttribute => IHtmlAttribute.Key.onseeking,
      //IonselectHtmlAttribute => IHtmlAttribute.Key.onselect,
      //IonstalledHtmlAttribute => IHtmlAttribute.Key.onstalled,
      //IonstorageHtmlAttribute => IHtmlAttribute.Key.onstorage,
      //IonsubmitHtmlAttribute => IHtmlAttribute.Key.onsubmit,
      //IonsuspendHtmlAttribute => IHtmlAttribute.Key.onsuspend,
      //IontimeupdateHtmlAttribute => IHtmlAttribute.Key.ontimeupdate,
      //IontoggleHtmlAttribute => IHtmlAttribute.Key.ontoggle,
      //IonunloadHtmlAttribute => IHtmlAttribute.Key.onunload,
      //IonvolumechangeHtmlAttribute => IHtmlAttribute.Key.onvolumechange,
      //IonwaitingHtmlAttribute => IHtmlAttribute.Key.onwaiting,
      //IonwheelHtmlAttribute => IHtmlAttribute.Key.onwheel,
      IOpenHtmlAttribute => IHtmlAttribute.Key.open,
      //IoptimumHtmlAttribute => IHtmlAttribute.Key.optimum,
      IPatternHtmlAttribute => IHtmlAttribute.Key.pattern,
      IPlaceHolderHtmlAttribute => IHtmlAttribute.Key.placeholder,
      IPopOverHtmlAttribute => IHtmlAttribute.Key.popover,
      IPopOverTargetHtmlAttribute => IHtmlAttribute.Key.popovertarget,
      IPopOverTargetActionHtmlAttribute => IHtmlAttribute.Key.popovertargetaction,
      //IposterHtmlAttribute => IHtmlAttribute.Key.poster,
      //IpreloadHtmlAttribute => IHtmlAttribute.Key.preload,
      IReadOnlyHtmlAttribute => IHtmlAttribute.Key.@readonly,
      IRelHtmlAttribute => IHtmlAttribute.Key.rel,
      IRequiredHtmlAttribute => IHtmlAttribute.Key.required,
      //IreversedHtmlAttribute => IHtmlAttribute.Key.reversed,
      IRowsHtmlAttribute => IHtmlAttribute.Key.rows,
      //IrowspanHtmlAttribute => IHtmlAttribute.Key.rowspan,
      //IsandboxHtmlAttribute => IHtmlAttribute.Key.sandbox,
      //IscopeHtmlAttribute => IHtmlAttribute.Key.scope,
      ISelectedHtmlAttribute => IHtmlAttribute.Key.selected,
      //IshapeHtmlAttribute => IHtmlAttribute.Key.shape,
      ISizeHtmlAttribute => IHtmlAttribute.Key.size,
      //IsizesHtmlAttribute => IHtmlAttribute.Key.sizes,
      //IspanHtmlAttribute => IHtmlAttribute.Key.span,
      ISpellCheckHtmlAttribute => IHtmlAttribute.Key.spellcheck,
      ISrcHtmlAttribute => IHtmlAttribute.Key.src,
      //IsrcdocHtmlAttribute => IHtmlAttribute.Key.srcdoc,
      //IsrclangHtmlAttribute => IHtmlAttribute.Key.srclang,
      //IsrcsetHtmlAttribute => IHtmlAttribute.Key.srcset,
      //IstartHtmlAttribute => IHtmlAttribute.Key.start,
      IStepHtmlAttribute => IHtmlAttribute.Key.step,
      IStyleHtmlAttribute => IHtmlAttribute.Key.style,
      ITabIndexHtmlAttribute => IHtmlAttribute.Key.tabindex,
      ITargetHtmlAttribute => IHtmlAttribute.Key.target,
      ITitleHtmlAttribute => IHtmlAttribute.Key.title,
      //Obsolete  ITranslateHtmlAttribute => IHtmlAttribute.Key.translate,
      ITypeHtmlAttribute => IHtmlAttribute.Key.type,
      //IusemapHtmlAttribute => IHtmlAttribute.Key.usemap,
      IValueHtmlAttribute => IHtmlAttribute.Key.value,
      IWidthHtmlAttribute => IHtmlAttribute.Key.width,
      IWrapHtmlAttribute => IHtmlAttribute.Key.wrap,

      _ => throw new NotImplementedException($"{attr.GetType()}")
    };

    public static string GetHtmlKeyValue<T>(this T attr) where T : IHtmlAttribute {
      return attr is null ? string.Empty
      //:  attr.Value is null ? string.Empty
      : attr switch {
        null => string.Empty,
        // Specific Html Attributes
        IAutoCompleteHtmlAttribute autoComplete => autoComplete.GetHtmlKeyValue(autoComplete.Value.ToString().Replace("_", "-")),
        IClassHtmlAttribute cssClass => cssClass.GetHtmlKeyValue(string.Join(ClassHtmlAttribute.CssClassDelimeter, cssClass.Value)),
        IDataHtmlAttribute data => data.Value.GetJoinedHtmlKeyValues(),
        IFormActionHtmlAttribute form => form.GetHtmlKeyValue(form.Value?.ToString().Replace("_", "-")) ?? string.Empty,
        IInputTypeHtmlAttribute inputType => inputType.GetHtmlKeyValue(inputType.Value.ToString().Replace("_", "-")),
        IPopOverTargetActionHtmlAttribute popOverTargetAction => popOverTargetAction.GetHtmlKeyValue(popOverTargetAction.Value.ToString().Replace("_", "-")),
        IReferrerPolicyHtmlAttribute referrerPolicy => referrerPolicy.GetHtmlKeyValue(referrerPolicy.Value.ToString().Replace("_", "-")),
        IRelHtmlAttribute rel => rel.GetHtmlKeyValue(rel.Value.ToString().Replace("_", "-")),
        ITargetHtmlAttribute target => target.GetHtmlKeyValue(target.Value.ToString().Replace("_", "-")),
        IValueHtmlAttribute value => $"{attr.Name}=\"{value.Value.ToHtmlAttributeValue()}\"",
        // By Data Type
        IHtmlAttributeBool b => b.Value ? $"{attr.Name}" : string.Empty,
        IHtmlAttributeDecimal dec => dec.GetHtmlKeyValue($"{dec.Value}"),
        IHtmlAttributeInt i => i.GetHtmlKeyValue($"{i.Value}"),
        IHtmlAttributeObject o => o.GetHtmlKeyValue($"{o.Value}"),
        IHtmlAttributeString s => s.GetHtmlKeyValue(s.Value),
        //_ => string.IsNullOrWhiteSpace($"{attr.Value}") ? string.Empty : $"{attr.Name}=\"{attr.Value.ToHtmlAttributeValue()}\""
        //_ =>  $"{attr.Name}=\"{attr.Value.ToHtmlAttributeValue()}\""
        _ => throw new NotSupportedException($"The attribute '{attr.Name}' with type '{attr.GetType().Name}' is not supported.")
      };
    }

    public static string GetHtmlKeyValue<T>(this IHtmlAttribute<T> attr, string? value)
      => string.IsNullOrWhiteSpace(value) ? string.Empty : $"{attr.Name}=\"{value.ToHtmlAttributeValue()}\"";

    //[Obsolete("Use GetHtmlKeyValue<T>(this IHtmlAttribute<T> attr)")] public static string GetHtmlKeyValue<T>(this IHtmlAttribute<T> attr, T value) => attr.GetHtmlKeyValue($"{value}");

    //[Obsolete]public static T GetValue<T>(this IHtmlAttribute<T> attr, IHtmlTag tag) => tag.GetAttributeValue<T>(attr.Name);
    //[Obsolete] public static TValue GetValue<TTag, TValue>(this IHtmlAttribute<TTag, TValue> attr, IHtmlTag tag) => tag.GetAttributeValue<TValue>(attr.Name);
    //[Obsolete] public static TTag SetValue<TTag, TValue>(this IHtmlAttribute<TValue> attr, TTag tag, TValue value) where TTag : IHtmlTag  => tag.SetAttributeValue(attr.Name, value);
    //[Obsolete] public static T GetAttributeValue<T>(this IHtmlTag tag, IHtmlAttribute<T> attr) => tag.GetAttributeValue<T>(attr.Name);
    //[Obsolete] public static TTag SetAttributeValue<TTag, TValue>(this TTag tag, IHtmlAttribute<TValue> attr, TValue value) where TTag : IHtmlTag where TValue: IHtmlAttribute   => tag.SetAttributeValue(attr.Name, value);

  }
}