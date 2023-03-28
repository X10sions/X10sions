namespace Common.Models.Html.Tags;
// TODO
public class Option : HtmlTag5Base {
  public bool? Disabled { get; set; }
  public string Label { get; set; } // text	Specifies a shorter label For an Option
  public bool? Selected { get; set; } // Specifies that an Option should be pre-selected When the page loads
  public string Value { get; set; } // Specifies the value To be sent To a server
  public OptGroup OptGroup { get; set; }
  public Select Select { get; set; }
  public bool? DefaultSelected { get; set; }
  public Form Form { get; set; }
  public int? Index { get; set; }
  public string Text { get; set; }
  public override string TagName { get; set; } = nameof(Option);
  string SelectedHtml => Selected.HasValue && Selected.Value ? " selected=\"selected\" " : string.Empty;
  public override string ToHtml() => $"<option value=\"{Value}\" {SelectedHtml}>{Text}</option>";

}
