using Common.Html.Attributes;
using Common.Javascript;
using System.Text;

namespace Common.Html.Tags;

public interface IDialogHtmlTag : IHtmlTag_v5, IInnerHtml, IHtmlTagWithOpenAttribute {
  string UniqueId { get; }
}

public static class IDialogExtensions {

  public static string CloseCssClass(this IDialogHtmlTag dialog) => $"{dialog.UniqueId}-close";
  public static string SubmitCssClass(this IDialogHtmlTag dialog) => $"{dialog.UniqueId}-submit";

  //public static string ToHtml(this IDialog dialog) => $"<{dialog.TagName} {dialog.Open().HtmlKeyValue}>{dialog.InnerHtml.Value}</{dialog.TagName}>";
  public static string JavascriptClose(this IDialogHtmlTag dialog) => $"{dialog.Id()?.Value}.close()";
  public static string JavascriptShowModal(this IDialogHtmlTag dialog) => $"{dialog.Id()?.Value}.showModal();";
}

public record struct DialogHtmlTag(InnerHtml InnerHtml) : IDialogHtmlTag {
  public DialogHtmlTag() : this(InnerHtml.Empty) { }

  public const string tag = "dialog";
  public string TagName { get; } = tag;
  public IHtmlTag.ElementType Element { get; } = IHtmlTag.ElementType.Normal;
  public HtmlAttributeDictionary Attributes { get; } = new();
  public string UniqueId { get; } = $"{tag}{uniqueIdCount++}";
  static int uniqueIdCount = 0;

  public DialogHtmlTag(InnerHtml innerHtml, IdHtmlAttribute id) : this(innerHtml) {
    this.Id(id);
  }

  //public DialogHtmlTag(string innerHtml, string? id = null) : this(
  //  new InnerHtml(innerHtml),
  //  new IdHtmlAttribute(id)
  //) { }

}

public static class DialogExtensions {

  public static string GetFormControlName(this IDialogHtmlTag dialog, string name) => $"{dialog.UniqueId}_{name}";

  public static void JavascriptShowModal(this IDialogHtmlTag dialog, IJavascriptInput[] inputs, StringBuilder sb) {
    sb.AppendLine($"  function {dialog.Id()?.Value}_ShowModal(tr) {{");
    sb.AppendLine($@"
    if (tr.content) {{
      tr = tr.content.querySelector(""*"");
    }}
");
    foreach (var input in inputs) {
      sb.AppendLine($"    {dialog.GetFormControlName(input.Name)}.value = {input.ValueSetter};");
    }
    sb.AppendLine($"    {dialog.JavascriptShowModal()}");
    sb.AppendLine($"  }}");
  }

}