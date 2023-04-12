using Common.Constants;
using System.Text;

namespace Common.Html.Tags;
public class Style : HtmlTag5Base<Style> {
  public const string TypeDefault = "text/css";

  public Style() {
    Type = TypeDefault;
  }

  public override string ToHtml() => this.ToHtml(CssSelctorHtml());

  /// <summary>
  /// Media type 
  /// </summary>
  public bool? Media { get => GetAttribute<bool?>(nameof(Media)); set => attributes[nameof(Media)] = value; }

  public string Type { get => GetAttribute<string>(nameof(Type)); set => attributes[nameof(Type)] = value.IfNullOrWhiteSpace(TypeDefault); }

  public HashSet<CssSelctor> Selctors { get; set; } = new HashSet<CssSelctor>();
  public string CssSelctorHtml() {
    var sb = new StringBuilder();
    foreach (var s in Selctors) {
      sb.AppendLine(s.ToHtml());
    }
    return sb.ToString();
  }

  public static string InlineStylesheetHtmlTagString(string contents) => $"<style type=\"{MediaTypeNames.Text.Css}\">{contents}</style>";
  public static string StylesheetHtmlTagString(string href) => $"<link rel=\"stylesheet\" href=\"{href}\" />";


}
