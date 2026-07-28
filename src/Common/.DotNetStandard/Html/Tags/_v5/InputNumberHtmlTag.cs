using Common.Html.Attributes;
using System;
namespace Common.Html.Tags {
  public interface IInputNumberHtmlTag : IInputHtmlTag.IAutoComplete, IInputHtmlTag.IList, IInputHtmlTag.IMax, IInputHtmlTag.IMin, IInputHtmlTag.IPlaceHolder, IInputHtmlTag.IReadOnly, IInputHtmlTag.IRequired, IInputHtmlTag.IStep { }

  public record struct InputNumberHtmlTag() : IInputNumberHtmlTag {

    [Obsolete]
    public static InputNumberHtmlTag New<T>(T? value, T? max = null, T? min = null, string? placeHolder = null, bool readOnly = false, decimal? step = null) where T : struct
      => new InputNumberHtmlTag().Value(value)
      .Max(max)
      .Min(min)
      .PlaceHolder(new(placeHolder))
      .ReadOnly(new(readOnly))
      .Step(new(step))
      ;

    public string TagName { get; } = "input";
    public IHtmlTag.ElementType Element { get; } = IHtmlTag.ElementType.Void;

    public IInputTypeHtmlAttribute.Values Type { get; } = IInputTypeHtmlAttribute.Values.number;
    public HtmlAttributeDictionary Attributes { get; } = new(IInputTypeHtmlAttribute.Values.number);
  }

}