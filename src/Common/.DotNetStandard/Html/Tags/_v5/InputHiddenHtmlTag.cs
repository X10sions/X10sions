using Common.Html.Attributes;
namespace Common.Html.Tags {
  public interface IInputHiddenHtmlTag : IInputHtmlTag.IDirName { }

  public record struct InputHiddenHtmlTag() : IInputHiddenHtmlTag {
    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element => IHtmlTag.ElementType.Void;

    //public IInputTypeHtmlAttribute Type { get => this.GetHtmlAttribute<InputTypeHtmlAttribute>(nameof(Type)); }

    //public IInputTypeHtmlAttribute.Values Type { get => this.GetHtmlAttribute<IInputTypeHtmlAttribute>(nameof(Type)); }// =this.seth  IInputTypeHtmlAttribute.Values.hidden;
    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.hidden;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.hidden);
  }

}