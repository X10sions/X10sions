using Common.Enums;
using System.Collections.ObjectModel;

namespace Common.Html.Tags {
  public abstract class HtmlTag401Base<T> : IHtmlTag where T : IHtmlTag {
    const string CssClassDelimeter = " ";
    protected Dictionary<string, object?> attributes { get; } = new Dictionary<string, object?>();
    public ReadOnlyDictionary<string, object?> Attributes => new ReadOnlyDictionary<string, object?>(attributes);
    public void AddAttribute<TValue>(string key, TValue value) => attributes.Add(key, value);
    public void SetAttribute<TValue>(string key, TValue value) => attributes[key] = value;
    //public TValue? GetAttribute<TValue>(string key) where TValue : class => attributes[key] as TValue;
    public TValue? GetAttribute<TValue>(string key) => attributes.TryGetValue(key, out var value) && value is TValue t ? t : default;
    public TValue GetAttribute<TValue>(string key, TValue defaultValue) => attributes.TryGetValue(key, out var value) && value is TValue t ? t : defaultValue;
    public string TagName => nameof(T).ToLower();

    public abstract string ToHtml();

    /// <summary>
    /// Specifies a shortcut key To activate/focus an element (<a>, <area>, <button>, <input>, <label>, <legend>, and <textarea>.
    /// </summary>
    public char AccessKey { get => GetAttribute<char>(nameof(AccessKey)); set => attributes[nameof(AccessKey)] = value; }

    /// <summary>
    /// Specifies one Or more classnames For an element (refers To a Class In a style sheet)
    /// </summary>
    public HashSet<string> Class {
      get {
        return GetAttribute<HashSet<string>>(nameof(Class));
      }
      set {
        cssClassList = value;
        cssClassList.Clear();
        foreach (var s in value) {
          cssClassList.Add(s);
        }
        attributes[nameof(Class)] = cssClassList;
      }
    }

    //        classList = new HashSet<string>((from x in value.Split(CssClassDelimeter) orderby x select x.Trim()).Distinct());
    //    Attributes[nameof(Class)] = string.Join(CssClassDelimeter, classList);
    private HashSet<string> cssClassList = new HashSet<string>();


    //get => string.Join(CssClassDelimeter, ClassList);
    //      set => ClassList = new HashSet<string>((from x in value.Split(CssClassDelimeter) orderby x select x.Trim()).Distinct());
    //    }


    public void ClassAdd(string cssClass) {
      classList.Add(cssClass);
    }

    public void ClassRemove(string cssClass) {
      classList.Remove(cssClass);
    }

    public void ClassSet(string cssClass) {
      classList.Add(cssClass);
    }

    /// <summary>
    /// Specifies one Or more classnames For an element (refers To a Class In a style sheet)
    /// </summary>
    private HashSet<string> classList = new HashSet<string>();
    //public HashSet<string> ClassList { get; set; } = new HashSet<string>();


    /// <summary>
    ///  Specifies the text direction For the content In an element
    /// </summary>
    public TextDirectionCode Dir { get => GetAttribute<TextDirectionCode>(nameof(Dir)); set => attributes[nameof(Dir)] = value; }

    /// <summary>
    /// Specifies a unique id For an element
    /// </summary>
    public string? Id { get => GetAttribute<string>(nameof(Id)); set => attributes[nameof(Id)] = value; }


    /// <summary>
    /// Specifies the language Of the element's content
    /// </summary>
    public ISO_639_1_LanguageCode Lang { get => GetAttribute<ISO_639_1_LanguageCode>(nameof(Lang)); set => attributes[nameof(Lang)] = value; }

    public HashSet<CssSelctor> Style { get => GetAttribute<HashSet<CssSelctor>>(nameof(Style)); set => attributes[nameof(Style)] = value; }
    //public string Style {
    //  get => string.Join(StyleDelimeter, from x in StyleList select x.Key + StyleKeyValueDelimeter + x.Value);
    //  set => StyleList = new HashSet<StyleDefinition>((from x in value.Split(StyleDelimeter) orderby x select new StyleDefinition(x)).Distinct());
    //}  // Specifies an inline CSS style For an element
    //public HashSet<StyleDefinition> StyleList { get; set; } = new HashSet<StyleDefinition>();

    /// <summary>
    ///  Specifies the tabbing order Of an element
    /// </summary>
    public int TabIndex { get => GetAttribute<int>(nameof(TabIndex)); set => attributes[nameof(TabIndex)] = value; }

    /// <summary>
    /// Specifies extra information about an element
    /// </summary>
    public string? Title { get => GetAttribute<string?>(nameof(Title)); set => attributes[nameof(Title)] = value; }

    //public static string NotNullAttr(string name, object value) => value == null ? string.Empty : $" {name}=\"{value}\"";

  }
}