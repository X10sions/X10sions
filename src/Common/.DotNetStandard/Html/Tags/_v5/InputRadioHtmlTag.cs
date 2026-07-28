using Common.Html.Attributes;
namespace Common.Html.Tags {
  public interface IInputRadioHtmlTag : IInputHtmlTag.IChecked, IInputHtmlTag.IRequired { }

  public record struct InputRadioHtmlTag() : IInputRadioHtmlTag {

    public static InputRadioHtmlTag New<T>(T? value, bool isChecked = false, bool required = false) where T : struct
      => new InputRadioHtmlTag().Value(value)
      .Checked(new(isChecked))
      .Required(new(required));

    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.radio;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.radio);
  }

}