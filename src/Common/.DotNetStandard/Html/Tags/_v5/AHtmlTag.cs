using Common.Html.Attributes;

namespace Common.Html.Tags;

public interface IAHtmlTag : IHtmlTag_v5, IInnerHtml {
  IDownloadHtmlAttribute Download { get; }
  IHrefHtmlAttribute Href { get; }
  IHrefLangHtmlAttribute HrefLang { get; }
  IMediaHtmlAttribute Media { get; }
  IPingHtmlAttribute Ping { get; }
  IReferrerPolicyHtmlAttribute ReferrerPolicy { get; }
  IRelHtmlAttribute Rel { get; }
  ITargetHtmlAttribute Target { get; }
  ITypeHtmlAttribute Type { get; }
}

public record struct AHtmlTag(InnerHtml InnerHtml) : IAHtmlTag {
  public AHtmlTag() : this(InnerHtml.Empty) { }
  public AHtmlTag(string? href = null, string? innerText = null) : this() {
    if (!string.IsNullOrWhiteSpace(href)) {
      Href = new HrefHtmlAttribute(href);
    }
    if (!string.IsNullOrWhiteSpace(innerText)) {
      InnerHtml = new InnerHtml(innerText);
    }
  }

  public string TagName { get; } = "a";
  public IHtmlTag.ElementType Element { get; } = IHtmlTag.ElementType.Normal;
  public HtmlAttributeDictionary Attributes { get; } = new();

  public IDownloadHtmlAttribute Download { get => this.GetHtmlAttribute<IDownloadHtmlAttribute>(IHtmlAttribute.Key.download); set => this.SetHtmlAttribute(value); }
  public IHrefHtmlAttribute Href { get => this.GetHtmlAttribute<IHrefHtmlAttribute>(IHtmlAttribute.Key.href); set => this.SetHtmlAttribute(value); }
  public IHrefLangHtmlAttribute HrefLang { get => this.GetHtmlAttribute<IHrefLangHtmlAttribute>(IHtmlAttribute.Key.hreflang); set => this.SetHtmlAttribute(value); }
  public IMediaHtmlAttribute Media { get => this.GetHtmlAttribute<IMediaHtmlAttribute>(IHtmlAttribute.Key.media); set => this.SetHtmlAttribute(value); }
  public IPingHtmlAttribute Ping { get => this.GetHtmlAttribute<IPingHtmlAttribute>(IHtmlAttribute.Key.ping); set => this.SetHtmlAttribute(value); }
  public IReferrerPolicyHtmlAttribute ReferrerPolicy { get => this.GetHtmlAttribute<IReferrerPolicyHtmlAttribute>(IHtmlAttribute.Key.referrerpolicy); set => this.SetHtmlAttribute(value); }
  public IRelHtmlAttribute Rel { get => this.GetHtmlAttribute<IRelHtmlAttribute>(IHtmlAttribute.Key.rel); set => this.SetHtmlAttribute(value); }
  public ITargetHtmlAttribute Target { get => this.GetHtmlAttribute<ITargetHtmlAttribute>(IHtmlAttribute.Key.target); set => this.SetHtmlAttribute(value); }
  public ITypeHtmlAttribute Type { get => this.GetHtmlAttribute<ITypeHtmlAttribute>(IHtmlAttribute.Key.type); set => this.SetHtmlAttribute(value); }
  //    public string ToHtml() => $"<a href=\"{this.Href().Value}\">{InnerHtml.Value}</a>";
}


