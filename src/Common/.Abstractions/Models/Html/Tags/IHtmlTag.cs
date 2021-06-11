namespace Common.Models.Html.Tags {
  public interface IHtmlTag {
    string TagName { get; set; }
    string ToHtml();
  }
}

