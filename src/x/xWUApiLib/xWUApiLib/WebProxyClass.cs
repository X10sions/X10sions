using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(2)]
  [ClassInterface(ClassInterfaceType.None)]
  [Guid("650503CF-9108-4DDC-A2CE-6C2341E1C582")]
  public class WebProxyClass : IWebProxy, WebProxy {
    [DispId(1610743809)]
    public extern virtual string Address {
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
    public extern virtual StringCollection BypassList {
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
    public extern virtual bool BypassProxyOnLocal {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [param: In]
      set;
    }

    [DispId(1610743812)]
    public extern virtual bool ReadOnly {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
    }

    [DispId(1610743813)]
    public extern virtual string UserName {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      //[return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    [DispId(1610743817)]
    public extern virtual bool AutoDetect {
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
    public virtual extern void SetPassword([In] [MarshalAs(UnmanagedType.BStr)] string value);


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    public virtual extern void PromptForCredentials([In] [MarshalAs(UnmanagedType.IUnknown)] object parentWindow, [In] [MarshalAs(UnmanagedType.BStr)] string Title);


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743816)]
    [TypeLibFunc(1)]
    public virtual extern void PromptForCredentialsFromHwnd([In] [ComAliasName("WUApiLib.wireHWND")] ref _RemotableHandle parentWindow, [In] [MarshalAs(UnmanagedType.BStr)] string Title);

  }
}
