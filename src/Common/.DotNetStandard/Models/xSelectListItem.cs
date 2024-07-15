namespace Common.Models;
public class xSelectListItem {
  //public xSelectListItem() { }
  public xSelectListItem(string text) : this(text, text) { }
  public xSelectListItem(string text, string value, bool selected = false, bool disabled = false) {
    Text = text;
    Value = value;
    Selected = selected;
    Disabled = disabled;
  }

  public bool Disabled { get; set; }
  public xSelectListGroup Group { get; set; }
  public bool Selected { get; set; }
  public string Text { get; set; }
  public string Value { get; set; }
}