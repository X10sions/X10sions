namespace Microsoft.Office.Interop.Excel;
public class ExcelApplication : ApplicationClass, IDisposable {
  public void Dispose() {
    Quit();
    //System.Runtime.InteropServices.Marshal.ReleaseComObject(this);
  }
}

