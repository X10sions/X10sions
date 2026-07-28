using System.ComponentModel;

namespace Common.Html.Attributes {
 public interface IRelHtmlAttribute : IHtmlAttribute<IRelHtmlAttribute.Values?> {
    public enum Values {
      [Description("A,AREA,LINK")] alternate,
      [Description("A,AREA,LINK")] author,
      [Description("A,AREA")] bookmark,
      [Description(",LINK")] dns_prefetch,
      [Description("A,FORM")] external,
      [Description("A,AREA,LINK,FORM")] help,
      [Description(",LINK")] icon,
      [Description("A,AREA,LINK,FORM")] license,
      [Description("A,AREA,LINK,FORM")] next,
      [Description("A,AREA,FORM")] nofollow,
      [Description("A,FORM")] noopener,
      [Description("A,AREA,FORM")] noreferrer,
      [Description(" ,FORM")] opener,
      [Description(" ,LINK")] pingback,
      [Description(" ,LINK")] preconnect,
      [Description(" ,AREA,LINK")] prefetch,
      [Description(" ,LINK")] preload,
      [Description(" ,LINK")] prerender,
      [Description("A,AREA,LINK,FORM")] prev,
      [Description("A,AREA,LINK,FORM")] search,
      [Description("A,AREA")] tag,
      [Description(",LINK")] stylesheet,
    }
  };

  /// <summary>Specifies the relationship between the current document and the linked document/resource</summary>
  public readonly record struct RelHtmlAttribute(IRelHtmlAttribute.Values? Value) : IRelHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.rel;
  }
}