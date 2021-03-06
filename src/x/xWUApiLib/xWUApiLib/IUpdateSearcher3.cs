﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4304)]
  [Guid("04C6895D-EAF2-4034-97F3-311DE9BE413A")]
  public interface IUpdateSearcher3 : IUpdateSearcher2 {
    [DispId(1610743809)]
    new bool CanAutomaticallyUpgradeService {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [param: In]
      set;
    }

    [DispId(1610743811)]
    new string ClientApplicationID {
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
    new bool IncludePotentiallySupersededUpdates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [param: In]
      set;
    }

    [DispId(1610743815)]
    [ComAliasName("WUApiLib.ServerSelection")]
    new ServerSelection ServerSelection {
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
    new bool Online {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      [param: In]
      set;
    }

    [DispId(1610743823)]
    new string ServiceID {
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

    [DispId(1610809345)]
    new bool IgnoreDownloadPriority {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [param: In]
      set;
    }

    [ComAliasName("WUApiLib.SearchScope")]
    [DispId(1610874881)]
    SearchScope SearchScope {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610874881)]
      [return: ComAliasName("WUApiLib.SearchScope")]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610874881)]
      [param: In]
      [param: ComAliasName("WUApiLib.SearchScope")]
      set;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743816)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new ISearchJob BeginSearch([In] [MarshalAs(UnmanagedType.BStr)] string criteria, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743817)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new ISearchResult EndSearch([In] [MarshalAs(UnmanagedType.Interface)] ISearchJob searchJob);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743818)]
    [return: MarshalAs(UnmanagedType.BStr)]
    new string EscapeString([In] [MarshalAs(UnmanagedType.BStr)] string unescaped);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743819)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IUpdateHistoryEntryCollection QueryHistory([In] int startIndex, [In] int Count);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743820)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new ISearchResult Search([In] [MarshalAs(UnmanagedType.BStr)] string criteria);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743822)]
    new int GetTotalHistoryCount();
  }


}
