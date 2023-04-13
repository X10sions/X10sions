namespace Common.Html.Tags;

public class Caption : HtmlTag5Base<Caption>, IInnerText {
  #region Attributes
  #endregion

  public string InnerText { get; set; } = string.Empty;
  public override string ToHtml() => this.ToHtml(InnerText);
}
