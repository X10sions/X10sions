namespace Microsoft.Office.Interop.Outlook {
  public class OutlookApplication : ApplicationClass, IDisposable {
    public OutlookApplication() { }
    public void Dispose() {
      Quit();
      //System.Runtime.InteropServices.Marshal.ReleaseComObject(this);
    }
  }
}
