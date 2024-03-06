using NetOffice.AccessApi.Enums;

namespace NetOffice.AccessApi;

public static class ApplicationExtensions {

  public static void Dispose(this Application app, AcQuitOption quitSaveAll = AcQuitOption.acQuitSaveNone) {
    try {
      app.CloseCurrentDatabase();
    } catch {
    } finally {
      app.Quit(quitSaveAll);
      //System.Runtime.InteropServices.Marshal.ReleaseComObject(this);
    }
  }

  public static string? OutputReportToPdf(this Application accessApp, string reportName, string openArgs, string outputFilePhysicalPath) {
    try {
      var error = (string)accessApp.Run("TryPreviewReport", reportName, openArgs);
      accessApp.DoCmd.OpenReport(reportName, AcView.acViewPreview
        , null /* TODO Change to default(_) if this is not a reference type */
        , null /* TODO Change to default(_) if this is not a reference type */
        , AcWindowMode.acWindowNormal, openArgs);
      if (!string.IsNullOrWhiteSpace(error)) return error;
    } catch (Exception ex) {
      return ex.GetBaseException().Message.Substring(0, 255).Trim();
    }
    accessApp.DoCmd.OutputTo(AcOutputObjectType.acOutputReport
      , objectName: string.Empty
      , outputFormat: Constants.Constants.acFormatPDF
      , outputFile: outputFilePhysicalPath
      , autoStart: false
      , templateFile: null
      , encoding: null
      );
    accessApp.DoCmd.Close(AcObjectType.acReport, reportName, AcCloseSave.acSaveNo);
    return null;
  }

  public static object Run<T>(this Application app, string procedure, params T[] args) => args.Count() switch {
    0 => app.Run(procedure),
    1 => app.Run(procedure, args[0]),
    2 => app.Run(procedure, args[0], args[1]),
    3 => app.Run(procedure, args[0], args[1], args[2]),
    4 => app.Run(procedure, args[0], args[1], args[2], args[3]),
    5 => app.Run(procedure, args[0], args[1], args[2], args[3], args[4]),
    _ => throw new NotImplementedException($"Too many argmuents")
  };

  public static object Run<T1>(this Application app, string procedure, T1 a1) => app.Run(procedure, a1);
  public static object Run<T1, T2>(this Application app, string procedure, T1 a1, T2 a2) => app.Run(procedure, a1, a2);
  public static object Run<T1, T2, T3>(this Application app, string procedure, T1 a1, T2 a2, T3 a3) => app.Run(procedure, a1, a2, a3);
  public static object Run<T1, T2, T3, T4>(this Application app, string procedure, T1 a1, T2 a2, T3 a3, T4 a4) => app.Run(procedure, a1, a2, a3, a4);
  public static object Run<T1, T2, T3, T4, T5>(this Application app, string procedure, T1 a1, T2 a2, T3 a3, T4 a4, T5 a5) => app.Run(procedure, a1, a2, a3, a4, a5);
  public static object Run<T1, T2, T3, T4, T5, T6>(this Application app, string procedure, T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6) => app.Run(procedure, a1, a2, a3, a4, a5, a6);

}