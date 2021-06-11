using System;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("918EFD1E-B5D8-4C90-8540-AEB9BDC56F9D")]
  [CoClass(typeof(UpdateSessionClass))]
  public interface UpdateSession : IUpdateSession3 {
  }

  [ComImport]
  [Guid("0BB8531D-7E8D-424F-986C-A0B8F60A3E7B")]
  [CoClass(typeof(UpdateServiceManagerClass))]
  public interface UpdateServiceManager : IUpdateServiceManager2 {
  }

}
