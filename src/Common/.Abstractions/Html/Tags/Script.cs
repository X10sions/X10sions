namespace Common.Html.Tags {
  public class Script : HtmlTag5Base<Script> {
    //public string? Src { get => Attributes[nameof(Src)]; set => Attributes[nameof(Src)] = value; }
    public string? Type { get => Attributes[nameof(Type)]; set => Attributes[nameof(Type)] = value; }
    public string Contents { get; set; } = string.Empty;

    public override string ToHtml() => $"<{TagName}{NotNullAttr(nameof(Type), Type)");}>{Contents}</{TagName}>";

    public static string InlineScriptHtmlTag(string contents) => $"<script>{contents}</script>";
    public static string ScriptHtmlTagString(string src) => $"<script src=\"{src}\"></script>";

  }
}
