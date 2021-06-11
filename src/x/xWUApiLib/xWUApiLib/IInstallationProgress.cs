using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("345C8244-43A3-4E32-A368-65F073B76F36")]
  [TypeLibType(4288)]
  public interface IInstallationProgress {
    [DispId(1610743809)]
    int CurrentUpdateIndex {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [DispId(1610743810)]
    int CurrentUpdatePercentComplete {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    int PercentComplete {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateInstallationResult GetUpdateResult([In] int updateIndex);
  }


}
