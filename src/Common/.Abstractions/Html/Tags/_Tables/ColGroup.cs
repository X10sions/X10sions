using Common.Attributes;

namespace Common.Html.Tags;

public class ColGroup : HtmlTag5Base<ColGroup> {

  #region Attributes
  public int? Span { get => GetAttribute<int?>(nameof(Span)); set => attributes[nameof(Span)] = value; }
  #endregion

  [ToDo] public override string ToHtml() => throw new NotFiniteNumberException("https://www.w3schools.com/tags/tag_colgroup.asp");
}
