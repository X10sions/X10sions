using Common.Enums;

namespace Common.Html.Attributes;

public interface ILangHtmlAttribute : IHtmlAttributeString {
  public LanguageIsoCode639 LanguageCode { get; }
  public CountryIsoCode3166? CountryCode { get; }
};

/// <summary>Specifies the language Of the element's content</summary>
public readonly record struct LangHtmlAttribute(LanguageIsoCode639 LanguageCode, CountryIsoCode3166? CountryCode = null) : ILangHtmlAttribute {
  public string Value => LanguageCode.ToString().ToLower() + CountryCode is null ? string.Empty : $"-{CountryCode.ToString().ToUpper()}";
  public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.lang;

}