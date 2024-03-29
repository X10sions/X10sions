﻿namespace Microsoft.Office.Interop.Access;
public class AccessApplication : ApplicationClass, IDisposable {
  public AccessApplication(AcQuitOption quitSaveAll = AcQuitOption.acQuitSaveNone) {
    QuitSaveAll = quitSaveAll;
  }
  //public AccessApplication(string filerPath, AcQuitOption quitSaveAll = AcQuitOption.acQuitSaveNone):this(quitSaveAll) {
  //  OpenCurrentDatabase(filerPath);
  //}

  public AcQuitOption QuitSaveAll { get; set; }
  public void Dispose() => this.Dispose(QuitSaveAll);
}
