using System;
using System.Runtime.InteropServices;

namespace xWUApiLib {

  [ComImport]
  [Guid("68F1C6F9-7ECC-4666-A464-247FE12496C3")]
  [CoClass(typeof(UpdateDownloaderClass))]
  public interface UpdateDownloader : IUpdateDownloader { }


}
