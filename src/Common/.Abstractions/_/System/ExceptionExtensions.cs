using Common.Html.Tags;
using System.Diagnostics;
using System.Text;

namespace System {
  public static class ExceptionExtensions {

    public static string ToHtmlTable(this Exception ex) {
      var sb = new StringBuilder("<table><caption>Debug Info</caption><tbody>");
      var exStackTrace = new StackTrace(ex, true).GetFrame(0);
      sb.AppendLine(TR.ROW_WITH_COLSPAN_HTML("StackTrace"));
      sb.AppendLine(TR.ROW_HTML("ExGetFileName", exStackTrace.GetFileName()));
      sb.AppendLine(TR.ROW_HTML("ExGetType", exStackTrace.GetType().ToString()));
      sb.AppendLine(TR.ROW_HTML("ExGetMethod", exStackTrace.GetMethod().ToString()));
      sb.AppendLine(TR.ROW_HTML("ExGetFileLineNumber", exStackTrace.GetFileLineNumber().ToString()));
      sb.AppendLine(TR.ROW_HTML("ExGetFileColumnNumber", exStackTrace.GetFileColumnNumber().ToString()));

      var baseEx = ex.GetBaseException();
      sb.AppendLine(TR.ROW_WITH_COLSPAN_HTML("BaseException"));
      sb.AppendLine(TR.ROW_HTML("Message", baseEx.Message));
      sb.AppendLine(TR.ROW_HTML("Data", baseEx.Data.ToString()));
      sb.AppendLine(TR.ROW_HTML("Source", baseEx.Source));
      sb.AppendLine(TR.ROW_HTML("Last StackTrace", (" " + ex.StackTrace).Replace(" at ", "<br/>at ")));
      sb.AppendLine(TR.ROW_HTML("Base StackTrace", (" " + baseEx.StackTrace).Replace(" at ", "<br/>at ")));

      var BaseStackTrace = new StackTrace(baseEx, true).GetFrame(0);
      sb.AppendLine(TR.ROW_WITH_COLSPAN_HTML("BaseStackTrace"));
      sb.AppendLine(TR.ROW_HTML("BaseGetFileName", BaseStackTrace.GetFileName()));
      sb.AppendLine(TR.ROW_HTML("BaseGetType", BaseStackTrace.GetType().ToString()));
      sb.AppendLine(TR.ROW_HTML("BaseGetMethod", BaseStackTrace.GetMethod().ToString()));
      sb.AppendLine(TR.ROW_HTML("BaseGetFileLineNumber", BaseStackTrace.GetFileLineNumber().ToString()));
      sb.AppendLine(TR.ROW_HTML("BaseGetFileColumnNumber", BaseStackTrace.GetFileColumnNumber().ToString()));
      sb.AppendLine("</tbody></table>");
      return sb.ToString();
    }

  }
}