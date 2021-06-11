using System;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("3442D4FE-224D-4CEE-98CF-30E0C4D229E6")]
  [CoClass(typeof(UpdateInstallerClass))]
  public interface UpdateInstaller : IUpdateInstaller2 {  }



}   
