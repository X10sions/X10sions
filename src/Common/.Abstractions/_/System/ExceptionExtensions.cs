using Common.Constants;
using System.Diagnostics;
using System.Text;

namespace System {
  public static class ExceptionExtensions {

    public static string ToHtmlTable(this Exception ex) {
      var sb = new StringBuilder("<table><caption>Debug Info</caption><tbody>");
      var exStackTrace = new StackTrace(ex, true).GetFrame(0);
      sb.AppendLine(HtmlConstants.ROW_WITH_COLSPAN_HTML("StackTrace"));
      sb.AppendLine(HtmlConstants.ROW_HTML("ExGetFileName", exStackTrace.GetFileName()));
      sb.AppendLine(HtmlConstants.ROW_HTML("ExGetType", exStackTrace.GetType().ToString()));
      sb.AppendLine(HtmlConstants.ROW_HTML("ExGetMethod", exStackTrace.GetMethod().ToString()));
      sb.AppendLine(HtmlConstants.ROW_HTML("ExGetFileLineNumber", exStackTrace.GetFileLineNumber().ToString()));
      sb.AppendLine(HtmlConstants.ROW_HTML("ExGetFileColumnNumber", exStackTrace.GetFileColumnNumber().ToString()));

      var baseEx = ex.GetBaseException();
      sb.AppendLine(HtmlConstants.ROW_WITH_COLSPAN_HTML("BaseException"));
      sb.AppendLine(HtmlConstants.ROW_HTML("Message", baseEx.Message));
      sb.AppendLine(HtmlConstants.ROW_HTML("Data", baseEx.Data.ToString()));
      sb.AppendLine(HtmlConstants.ROW_HTML("Source", baseEx.Source));
      sb.AppendLine(HtmlConstants.ROW_HTML("Last StackTrace", (" " + ex.StackTrace).Replace(" at ", "<br/>at ")));
      sb.AppendLine(HtmlConstants.ROW_HTML("Base StackTrace", (" " + baseEx.StackTrace).Replace(" at ", "<br/>at ")));

      var BaseStackTrace = new StackTrace(baseEx, true).GetFrame(0);
      sb.AppendLine(HtmlConstants.ROW_WITH_COLSPAN_HTML("BaseStackTrace"));
      sb.AppendLine(HtmlConstants.ROW_HTML("BaseGetFileName", BaseStackTrace.GetFileName()));
      sb.AppendLine(HtmlConstants.ROW_HTML("BaseGetType", BaseStackTrace.GetType().ToString()));
      sb.AppendLine(HtmlConstants.ROW_HTML("BaseGetMethod", BaseStackTrace.GetMethod().ToString()));
      sb.AppendLine(HtmlConstants.ROW_HTML("BaseGetFileLineNumber", BaseStackTrace.GetFileLineNumber().ToString()));
      sb.AppendLine(HtmlConstants.ROW_HTML("BaseGetFileColumnNumber", BaseStackTrace.GetFileColumnNumber().ToString()));
      sb.AppendLine("</tbody></table>");
      return sb.ToString();
    }

  }
}