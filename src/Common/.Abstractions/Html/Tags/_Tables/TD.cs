namespace Common.Html.Tags;

public class TD : TableCellHtmlTagBase<TD> {
  public override string ToHtml() => $"<{TagName}>{InnerHtml}</{TagName}>";
}
