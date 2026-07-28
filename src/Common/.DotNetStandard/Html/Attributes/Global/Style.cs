using Common.Html.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Common.Html.Attributes {

  public class StyleHtmlAttributeHashSet : HashSet<CssSelector> {
    public StyleHtmlAttributeHashSet() { }
    //public DataAttributeDictionary(IEnumerable<DataItem> items) : base(StringComparer.OrdinalIgnoreCase) {
    //  this.Set(items);
    //}

    public string GetJoinedHtmlKeyValues() {
      var attributes = from item in this
                       where item.Declarations.Count > 0
                       orderby item.Selector
                       select item.ToHtml();
      return string.Join(";", attributes);
    }
  }

  public interface IStyleHtmlAttribute : IHtmlAttribute<StyleHtmlAttributeHashSet> { };

  public static class IStyleHtmlAttributeExtensions {

    public static IStyleHtmlAttribute Set(this IStyleHtmlAttribute attr, params CssSelector[] cssSelectors) {
      if (cssSelectors is not null) {
        foreach (var cssSelector in cssSelectors) {
          //if (dataItem != null) {
          attr.Value.Add(cssSelector);
          //}
        }
      }
      return attr;
    }

    [Obsolete] public static IDataHtmlAttribute Set(this IDataHtmlAttribute attr, string suffix, object value) => attr.Set(new DataItem(suffix, value));

  }


  /// <summary>Specifies an inline CSS style for an element</summary>
  public readonly record struct StyleHtmlAttribute(StyleHtmlAttributeHashSet Value) : IStyleHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.style;

    //public string Style {
    //  get => string.Join(StyleDelimeter, from x in StyleList select x.Key + StyleKeyValueDelimeter + x.Value);
    //  set => StyleList = new HashSet<StyleDefinition>((from x in value.Split(StyleDelimeter) orderby x select new StyleDefinition(x)).Distinct());
    //}  // Specifies an inline CSS style For an element
    //public HashSet<StyleDefinition> StyleList { get; set; } = new HashSet<StyleDefinition>();
  }
}