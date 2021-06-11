using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("174C81FE-AECD-4DAE-B8A0-2C6318DD86A8")]
  [TypeLibType(4288)]
  public interface IWebProxy {
    [DispId(1610743809)]
    string Address {
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
    StringCollection BypassList {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [param: In]
      [param: MarshalAs(UnmanagedType.Interface)]
      set;
    }

    [DispId(1610743811)]
    bool BypassProxyOnLocal {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [param: In]
      set;
    }

    [DispId(1610743812)]
    bool ReadOnly {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
    }

    [DispId(1610743813)]
    string UserName {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    [DispId(1610743817)]
    bool AutoDetect {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743817)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743817)]
      [param: In]
      set;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    void SetPassword([In] [MarshalAs(UnmanagedType.BStr)] string value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    void PromptForCredentials([In] [MarshalAs(UnmanagedType.IUnknown)] object parentWindow, [In] [MarshalAs(UnmanagedType.BStr)] string Title);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743816)]
    [TypeLibFunc(1)]
    void PromptForCredentialsFromHwnd([In] [ComAliasName("WUApiLib.wireHWND")] ref _RemotableHandle parentWindow, [In] [MarshalAs(UnmanagedType.BStr)] string Title);
  }

}
