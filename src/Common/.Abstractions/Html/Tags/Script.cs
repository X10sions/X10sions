using System.Collections;

namespace Common.Html.Tags {
  public class Script : HtmlTag5Base<Script> {
    public string? Src { get => Attributes.Get<string?>(nameof(Src)); set => Attributes.Set(nameof(Src), value); }
    public string? Type { get => Attributes.Get<string?>(nameof(Type)); set => Attributes.Set(nameof(Type), value); }
    public string Contents { get; set; } = string.Empty;

    public override string ToHtml() => this.ToHtml(Contents);

    public static string InlineScriptHtmlTag(string contents) => $"<script>{contents}</script>";
    public static string ScriptHtmlTagString(string src) => $"<script src=\"{src}\"></script>";

  }
}
