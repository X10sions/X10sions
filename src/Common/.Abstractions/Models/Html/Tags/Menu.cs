using System.Collections.Generic;
namespace Common.Models.Html.Tags {
  // TODO
  public class Menu : _BaseHtml5 {
    public string Label { get; set; } // Specifies whether the dragged data Is copied, moved, Or linked, When dropped
    public Menu_Type Type { get; set; }  // Specifies that an element Is Not yet, Or Is no longer, relevant
    public List<Menu> MenuItems { get; set; } = new List<Menu>();
    public override string TagName { get; set; } = nameof(Menu);
    public override string ToHtml() => $"<{TagName}{NotNullAttr(nameof(Type), Type)}></{TagName}>";
    public string NotNullAttr(string name, object value) => value == null ? string.Empty : $" {name}=\"{value.ToString()}\"";
    public enum Menu_Type {
      List,
      Toolbar,
      Context
    }
  }
}
