namespace Common.Html.Tags;

public class TFoot : HtmlTag5Base<TFoot>, ITableRows {
  public List<TR> TR { get; set; } = new List<TR>();
  public override string ToHtml() => $"<{TagName}>{TR.ToHtml()}</{TagName}>";
}
