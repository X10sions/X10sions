using Common.Enums;

namespace Common.Html.Attributes {
  public interface IHrefLangHtmlAttribute : IHtmlAttribute<LanguageIsoCode639?> { };

  /// <summary>Specifies the language of the linked document</summary>
  public readonly record struct HrefLangHtmlAttribute(LanguageIsoCode639? Value) : IHrefLangHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.hreflang;
  }
}