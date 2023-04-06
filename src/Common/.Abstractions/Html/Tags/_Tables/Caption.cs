namespace Common.Html.Tags;

public class Caption : HtmlTag5Base<Caption> {
  public string InnerText { get; set; }
  public override string ToHtml() => $"<{TagName}{InnerText}></{TagName}>";
}
