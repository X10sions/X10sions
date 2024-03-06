namespace Common.Html.Tags;

public class THead : HtmlTag5Base<THead>, ITableRows {
  #region Attributes
  #endregion

  public List<TR> TR { get; set; } = new List<TR>();
  public override string ToHtml() => $"<{TagName}>{TR.ToHtml()}</{TagName}>";
}
