using System;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [CoClass(typeof(UpdateSearcherClass))]
  [Guid("04C6895D-EAF2-4034-97F3-311DE9BE413A")]
  public interface UpdateSearcher : IUpdateSearcher3 { }


}
