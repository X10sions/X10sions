using Common.Attributes;
using Common.Constants;
using System.Text;

namespace Common.Html.Tags;

public class Style : HtmlTag5Base<Style> {
  //public const string StartTag = "<style>";
  //public const string EndTag = "</style>";

  public override string ToHtml() {

    var sb = new StringBuilder();
    sb.AppendLine($"<{TagName}");
    sb.Append($" {NotNullAttr(nameof(Type), Type)}");
    sb.Append(">");
    foreach (var s in Selctors) {
      sb.AppendLine(s.ToHtml());
    }
    sb.AppendLine($"</{TagName}>");

    //sb.AppendLine(StartTag);
    //sb.AppendLine($" type='{Type}'");
    //sb.AppendLine(EndTag);
    return sb.ToString();
  }

  [ToDo] public bool? Media { get; set; }


  /// <summary>
  /// Media type 
  /// </summary>
  [ToDo] public string Type { get; set; } = "text/css";

  public HashSet<StyleSelctor> Selctors { get; set; } = new HashSet<StyleSelctor>();
  //public Dictionary<string, string> Selectors { get; } = new Dictionary<string, string>();




  public static string InlineStylesheetHtmlTagString(string contents) => $"<style type=\"{MediaTypeNames.Text.Css}\">{contents}</style>";
  public static string StylesheetHtmlTagString(string href) => $"<link rel=\"stylesheet\" href=\"{href}\" />";


}
