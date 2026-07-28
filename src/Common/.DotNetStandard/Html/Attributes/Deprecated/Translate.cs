//using Common.Html.Tags;
//using System;

//namespace Common.Html.Attributes.Deprecated {
//  [Obsolete(ObsoleteReason.Not_supported_in_browsers)]
//  public interface ITranslateHtmlAttribute : IHtmlAttribute<ITranslateHtmlAttribute.Value> {
//    public enum Value { yes, no }
//  };

//  /// <summary> Specifies whether the content of an element should be translated or not</summary>
//  [Obsolete(ObsoleteReason.Not_supported_in_browsers)]
//  public readonly record struct TranslateHtmlAttribute(ITranslateHtmlAttribute.Value Value) : ITranslateHtmlAttribute {
//    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.translate;
//  }

//}