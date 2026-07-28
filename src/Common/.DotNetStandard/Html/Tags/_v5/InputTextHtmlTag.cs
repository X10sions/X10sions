using Common.Html.Attributes;
namespace Common.Html.Tags {
  public interface IInputTextHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IDirName, IInputHtmlTag.IList, IInputHtmlTag.IMaxLength, IInputHtmlTag.IMinLength, IInputHtmlTag.IPattern, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.ISize { }

  public record struct InputTextHtmlTag() : IInputTextHtmlTag {
    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.text;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.text);
  }

}