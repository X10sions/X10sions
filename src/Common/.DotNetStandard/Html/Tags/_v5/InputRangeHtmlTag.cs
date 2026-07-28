using Common.Html.Attributes;
namespace Common.Html.Tags {
   public interface IInputRangeHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IList, IInputHtmlTag.IMax, IInputHtmlTag.IMin, IInputHtmlTag.IRequired, IInputHtmlTag.IStep { }

  public record struct InputRangeHtmlTag() : IInputRangeHtmlTag {
    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.range;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.range);
  }

}