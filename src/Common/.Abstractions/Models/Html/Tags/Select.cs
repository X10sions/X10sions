namespace Common.Models.Html.Tags;
// TODO
public class Select : HtmlTag5Base {
  public override string TagName { get; set; } = nameof(Select);
  public override string ToHtml() => throw new NotImplementedException();
  public bool? AutoFocus { get; set; } // Specifies that the drop-down list should automatically Get focus When the page loads
  public Form Form { get; set; } // form_id'	Defines one Or more forms the Select field belongs To
  public bool? Required { get; set; }  // Specifies that the user Is required To Select a value before submitting the form
  public bool? Disabled { get; set; }  // Specifies that a drop-down list should be disabled
  public bool? Multiple { get; set; } // Specifies that multiple options can be selected at once
  public string Name { get; set; }  // Defines a name For the drop-down list
  public int Size { get; set; } // Defines the number Of visible options In a drop-down list
  public List<Option> OptionList { get; set; } = new List<Option>();
}
