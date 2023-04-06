namespace Common.Html.Tags;

public class TH : TableCellHtmlTagBase<TH> {
  public override string ToHtml() => $"<{TagName}>{InnerHtml}</{TagName}>";
}
