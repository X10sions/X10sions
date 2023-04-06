using Common.Enums;

namespace Common.Html.Tags {
  public abstract class HtmlTag401Base<T> : IHtmlTag where T : IHtmlTag {
    const string CssClassDelimeter = " ";

    public Dictionary<string, object> Attributes { get; } = new Dictionary<string, object>();

    public string TagName => nameof(T).ToLower();

    public abstract string ToHtml();

    /// <summary>
    /// Specifies a shortcut key To activate/focus an element (<a>, <area>, <button>, <input>, <label>, <legend>, and <textarea>.
    /// </summary>
    public char AccessKey { get => this.GetAttribute<char>(nameof(AccessKey)); set => Attributes[nameof(AccessKey)] = value; }


    /// <summary>
    /// Specifies one Or more classnames For an element (refers To a Class In a style sheet)
    /// </summary>
    public HashSet<string> Class {
      get {
        return this.GetAttribute<HashSet<string>>(nameof(Class));
      }
      set {
        cssClassList = value;
        cssClassList.Clear();
        foreach (var s in value) {
          cssClassList.Add(s);
        }
        Attributes[nameof(Class)] = cssClassList;
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
    public TextDirectionCode Dir { get => this.GetAttribute<TextDirectionCode>(nameof(Dir)); set => Attributes[nameof(Dir)] = value; }

    /// <summary>
    /// Specifies a unique id For an element
    /// </summary>
    public string? Id { get => this.GetAttribute<string>(nameof(Id)); set => Attributes[nameof(Id)] = value; }


    /// <summary>
    /// Specifies the language Of the element's content
    /// </summary>
    public ISO_639_1_LanguageCode Lang { get => this.GetAttribute<ISO_639_1_LanguageCode>(nameof(Lang)); set => Attributes[nameof(Lang)] = value; }


    public HashSet<CssSelctor> Style { get => this.GetAttribute<HashSet<CssSelctor>>(nameof(Style)); set => Attributes[nameof(Style)] = value; }
    //public string Style {
    //  get => string.Join(StyleDelimeter, from x in StyleList select x.Key + StyleKeyValueDelimeter + x.Value);
    //  set => StyleList = new HashSet<StyleDefinition>((from x in value.Split(StyleDelimeter) orderby x select new StyleDefinition(x)).Distinct());
    //}  // Specifies an inline CSS style For an element
    //public HashSet<StyleDefinition> StyleList { get; set; } = new HashSet<StyleDefinition>();

    /// <summary>
    ///  Specifies the tabbing order Of an element
    /// </summary>
    public int TabIndex { get => this.GetAttribute<int>(nameof(TabIndex)); set => Attributes[nameof(TabIndex)] = value; }

    /// <summary>
    /// Specifies extra information about an element
    /// </summary>
    public string Title { get => this.GetAttribute<string>(nameof(Title)); set => Attributes[nameof(Title)] = value; }

    public static string NotNullAttr(string name, object value) => value == null ? string.Empty : $" {name}=\"{value}\"";



  }
}