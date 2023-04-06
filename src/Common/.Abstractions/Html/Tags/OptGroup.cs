namespace Common.Html.Tags;
// TODO
public class OptGroup : HtmlTag5Base<OptGroup> {
  public bool? Disabled { get; set; }
  public string Label { get; set; } // text	Specifies a shorter label For an Option
  public List<Option> OptionList { get; set; } = new List<Option>();
  public override string ToHtml() => throw new NotImplementedException();

}
