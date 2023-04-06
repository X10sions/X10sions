namespace Common.Html.Tags {
  // TODO
  public class Menu : HtmlTag5Base<Menu> {
    public string Label { get; set; } // Specifies whether the dragged data Is copied, moved, Or linked, When dropped
    public Menu_Type Type { get; set; }  // Specifies that an element Is Not yet, Or Is no longer, relevant
    public List<Menu> MenuItems { get; set; } = new List<Menu>();
    public override string ToHtml() => $"<{TagName}{NotNullAttr(nameof(Type), Type)}></{TagName}>";
    public enum Menu_Type {
      List,
      Toolbar,
      Context
    }
  }
}
