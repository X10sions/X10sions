namespace Microsoft.Office.Interop.Access;

public static class ApplicationClassExtensions {

  public static void Dispose(this ApplicationClass app, AcQuitOption quitSaveAll = AcQuitOption.acQuitSaveNone) {
    try {
      app.CloseCurrentDatabase();
    } catch {
    } finally {
      app.Quit(quitSaveAll);
      //System.Runtime.InteropServices.Marshal.ReleaseComObject(this);
    }
  }

  public static object Run<T>(this ApplicationClass app, string procedure, params T[] args) => args.Count() switch {
    0 => app.Run(procedure),
    1 => app.Run(procedure, args[0]),
    2 => app.Run(procedure, args[0], args[1]),
    3 => app.Run(procedure, args[0], args[1], args[2]),
    4 => app.Run(procedure, args[0], args[1], args[2], args[3]),
    5 => app.Run(procedure, args[0], args[1], args[2], args[3], args[4]),
    _ => throw new NotImplementedException($"Too many argmuents")
  };

  public static object Run<T1>(this ApplicationClass app, string procedure, T1 a1) => app.Run(procedure, a1);
  public static object Run<T1, T2>(this ApplicationClass app, string procedure, T1 a1, T2 a2) => app.Run(procedure, a1, a2);
  public static object Run<T1, T2, T3>(this ApplicationClass app, string procedure, T1 a1, T2 a2, T3 a3) => app.Run(procedure, a1, a2, a3);
  public static object Run<T1, T2, T3, T4>(this ApplicationClass app, string procedure, T1 a1, T2 a2, T3 a3, T4 a4) => app.Run(procedure, a1, a2, a3, a4);
  public static object Run<T1, T2, T3, T4, T5>(this ApplicationClass app, string procedure, T1 a1, T2 a2, T3 a3, T4 a4, T5 a5) => app.Run(procedure, a1, a2, a3, a4, a5);

}