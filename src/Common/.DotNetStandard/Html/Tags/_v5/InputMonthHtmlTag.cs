using Common.Html.Attributes;
namespace Common.Html.Tags {
  public interface IInputMonthHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IList, IInputHtmlTag.IMax, IInputHtmlTag.IMin, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.IStep { }

  public record struct InputMonthHtmlTag() : IInputMonthHtmlTag {
    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.month;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.month);
  }

}