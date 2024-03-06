namespace Microsoft.Office.Interop.Word {
  public class WordApplication : ApplicationClass, IDisposable {
    public WordApplication() { }
    public void Dispose() {
      Quit();
      //System.Runtime.InteropServices.Marshal.ReleaseComObject(this);
    }
  }
}
