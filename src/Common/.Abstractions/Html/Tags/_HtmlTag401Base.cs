using Common.Enums;

namespace Common.Html.Tags {
  public abstract class HtmlTag401Base<T> : IHtmlTag where T : IHtmlTag {
    const string ClassDelimeter = " ";
    const string StyleDelimeter = ";";
    const string StyleKeyValueDelimeter = ":";

    public string TagName => nameof(T).ToLower();

    public abstract string ToHtml();

    public char AccessKey { get; set; } // Specifies a shortcut key To activate/focus an element (<a>, <area>, <button>, <input>, <label>, <legend>, and <textarea>.)
    public string Class {
      get => string.Join(ClassDelimeter, ClassList);
      set => ClassList = new HashSet<string>((from x in value.Split(ClassDelimeter) orderby x select x.Trim()).Distinct());
    }
    // Specifies one Or more classnames For an element (refers To a Class In a style sheet)
    public HashSet<string> ClassList { get; set; } = new HashSet<string>();
    public TextDirectionCode Dir { get; set; } // Specifies the text direction For the content In an element
    public string Id { get; set; } // Specifies a unique id For an element
    public ISO_639_1_LanguageCode Lang { get; set; } // Specifies the language Of the element's content
    public string Style {
      get => string.Join(StyleDelimeter, from x in StyleList select x.Key + StyleKeyValueDelimeter + x.Value);
      set => StyleList = new HashSet<StyleDefinition>((from x in value.Split(StyleDelimeter) orderby x select new StyleDefinition(x)).Distinct());
    }  // Specifies an inline CSS style For an element
    public HashSet<StyleDefinition> StyleList { get; set; } = new HashSet<StyleDefinition>();
    public int TabIndex { get; set; } // Specifies the tabbing order Of an element
    public string Title { get; set; } // Specifies extra information about an element

    public static string NotNullAttr(string name, object value) => value == null ? string.Empty : $" {name}=\"{value}\"";

    public class StyleDefinition {
      public string Key { get; set; }
      public string Value { get; set; }
      public StyleDefinition(string keyValue) {
        var a = keyValue.Split(StyleKeyValueDelimeter);
        Key = a[0].Trim();
        Value = a[1].Trim();
      }
    }

  }
}