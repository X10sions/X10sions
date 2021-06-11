using System;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("07F7438C-7709-4CA5-B518-91279288134E")]
  [CoClass(typeof(UpdateCollectionClass))]
  public interface UpdateCollection : IUpdateCollection { }


}
