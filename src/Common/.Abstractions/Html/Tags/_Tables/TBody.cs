namespace Common.Html.Tags;

public class TBody : HtmlTag5Base<TBody>, ITableRows {
  public List<TR> TR { get; set; } = new List<TR>();

  public override string ToHtml() => $"<{TagName}>{TR.ToHtml()}</{TagName}>";

}
