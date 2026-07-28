using Common.Html.Attributes;
namespace Common.Html.Tags {
  public interface IInputCheckboxHtmlTag : IInputHtmlTag.IChecked, IInputHtmlTag.IRequired { }

  public record struct InputCheckboxHtmlTag() : IInputCheckboxHtmlTag {

    public static InputCheckboxHtmlTag New<T>(T? value, bool isChecked = false, bool required = false) where T : struct
      => new InputCheckboxHtmlTag().Value(value)
      .Checked(new(isChecked))
      .Required(new(required));

    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.checkbox;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.checkbox);
  }

}