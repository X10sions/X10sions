namespace Common.Html.Tags {
  public interface IHtmlTag {
    string TagName { get; }
    string ToHtml();
  }
}

