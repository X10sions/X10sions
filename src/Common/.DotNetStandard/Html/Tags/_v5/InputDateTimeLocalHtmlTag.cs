using Common.Html.Attributes;
namespace Common.Html.Tags {
  public interface IInputDateTimeLocalHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IList, IInputHtmlTag.IMax, IInputHtmlTag.IMin, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.IStep { }

  public record struct InputDateTimeLocalHtmlTag() : IInputDateTimeLocalHtmlTag {
    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.datetime_local;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.datetime_local);
  }

}