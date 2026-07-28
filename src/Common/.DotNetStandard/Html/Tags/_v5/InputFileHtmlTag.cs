using Common.Html.Attributes;
namespace Common.Html.Tags {
  public interface IInputFileHtmlTag : IInputHtmlTag.IAccept, IInputHtmlTag.ICapture, IInputHtmlTag.IMultiple, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.ISize { }

  public record struct InputFileHtmlTag() : IInputFileHtmlTag {
    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.file;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.file);
  }

}