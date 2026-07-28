using System.Collections.Generic;

namespace Common.Html.Attributes{
  public class HtmlAttributeDictionary : Dictionary<IHtmlAttribute.Key, IHtmlAttribute> {
    public HtmlAttributeDictionary(params IHtmlAttribute[] defaultAttributes) {
      //this[nameof(Class)] = new ClassHtmlAttribute(new());
      foreach (var attr in defaultAttributes) {
        this[attr.Name] = attr;
      }
    }
    public HtmlAttributeDictionary(IInputTypeHtmlAttribute.Values type) : this(new InputTypeHtmlAttribute(type)) { }
    //public ClassHtmlAttribute Class { get => (ClassHtmlAttribute)this[nameof(Class)]; }
  }

}