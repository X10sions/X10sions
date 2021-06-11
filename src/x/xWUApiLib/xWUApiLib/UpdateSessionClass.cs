using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [ClassInterface(ClassInterfaceType.None)]
  [TypeLibType(2)]
  [Guid("4CB43D7F-7EEE-4906-8698-60DA1C38F2FE")]
  public class UpdateSessionClass : IUpdateSession3, UpdateSession {
    [DispId(1610743809)]
    public extern virtual string ClientApplicationID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    [DispId(1610743810)]
    public extern virtual bool ReadOnly {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    public extern virtual WebProxy WebProxy {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [param: In]
      [param: MarshalAs(UnmanagedType.Interface)]
      set;
    }

    [DispId(1610809345)]
    public extern virtual uint UserLocale {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [param: In]
      set;
    }

  
    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743812)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern IUpdateSearcher CreateUpdateSearcher();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743813)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern UpdateDownloader CreateUpdateDownloader();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern IUpdateInstaller CreateUpdateInstaller();


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610874881)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern UpdateServiceManager CreateUpdateServiceManager();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610874882)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern IUpdateHistoryEntryCollection QueryHistory([In] [MarshalAs(UnmanagedType.BStr)] string criteria, [In] int startIndex, [In] int Count);

  }
}