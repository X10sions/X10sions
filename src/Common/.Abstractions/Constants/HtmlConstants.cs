namespace Common.Constants {
  public static class HtmlConstants {

    public static string InlineStylesheetHtmlTagString(string contents) => $"<style type=\"{MediaTypeNames.Text.Css}\">{contents}</style>";
    public static string InlineScriptHtmlTag(string contents) => $"<script>{contents}</script>";

    public static string StylesheetHtmlTagString(string href) => $"<link rel=\"stylesheet\" href=\"{href}\" />";
    public static string ScriptHtmlTagString(string src) => $"<script src=\"{src}\"></script>";

    public static string ROW_HTML(string th, string td) => $"<tr><th>{th}:</th><td>{td}</td><tr>";
    public static string ROW_WITH_COLSPAN_HTML(string th, int colspan = 2) => $"<tr><th colspan=\"{colspan}\">{th}:</th><tr>";

  }
}