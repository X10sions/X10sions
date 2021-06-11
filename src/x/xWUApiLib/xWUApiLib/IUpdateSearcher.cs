using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [Guid("8F45ABF1-F9AE-4B95-A933-F0F66E5056EA")]
  public interface IUpdateSearcher {
    [DispId(1610743809)]
    bool CanAutomaticallyUpgradeService {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [param: In]
      set;
    }

    [DispId(1610743811)]
    string ClientApplicationID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    [DispId(1610743812)]
    bool IncludePotentiallySupersededUpdates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [param: In]
      set;
    }

    [ComAliasName("WUApiLib.ServerSelection")]
    [DispId(1610743815)]
    ServerSelection ServerSelection {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743815)]
      [return: ComAliasName("WUApiLib.ServerSelection")]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743815)]
      [param: In]
      [param: ComAliasName("WUApiLib.ServerSelection")]
      set;
    }

    [DispId(1610743821)]
    bool Online {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      [param: In]
      set;
    }

    [DispId(1610743823)]
    string ServiceID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743823)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743823)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743816)]
    [return: MarshalAs(UnmanagedType.Interface)]
    ISearchJob BeginSearch([In] [MarshalAs(UnmanagedType.BStr)] string criteria, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743817)]
    [return: MarshalAs(UnmanagedType.Interface)]
    ISearchResult EndSearch([In] [MarshalAs(UnmanagedType.Interface)] ISearchJob searchJob);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743818)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string EscapeString([In] [MarshalAs(UnmanagedType.BStr)] string unescaped);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743819)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IUpdateHistoryEntryCollection QueryHistory([In] int startIndex, [In] int Count);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743820)]
    [return: MarshalAs(UnmanagedType.Interface)]
    ISearchResult Search([In] [MarshalAs(UnmanagedType.BStr)] string criteria);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743822)]
    int GetTotalHistoryCount();
  }
}