using Common.Html.Attributes;
namespace Common.Html.Tags {
public interface IInputTimeHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IList, IInputHtmlTag.IMax, IInputHtmlTag.IMin, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.IStep { }

  public record struct InputTimeHtmlTag() : IInputTimeHtmlTag {
    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.time;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.time);
  }

}