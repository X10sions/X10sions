using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [ClassInterface(ClassInterfaceType.None)]
  [Guid("B699E5E8-67FF-4177-88B0-3684A3388BFB")]
  [TypeLibType(2)]
  public class UpdateSearcherClass : UpdateSearcher {
    [DispId(1610743809)]
    public extern virtual bool CanAutomaticallyUpgradeService {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [param: In]
      set;
    }

    [DispId(1610743811)]
    public extern virtual string ClientApplicationID {
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
    public extern virtual bool IncludePotentiallySupersededUpdates {
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
    public extern virtual ServerSelection ServerSelection {
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
    public extern virtual bool Online {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      [param: In]
      set;
    }

    [DispId(1610743823)]
    public extern virtual string ServiceID {
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
    public extern virtual bool IgnoreDownloadPriority {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [param: In]
      set;
    }

    [DispId(1610874881)]
    [ComAliasName("WUApiLib.SearchScope")]
    public extern virtual SearchScope SearchScope {
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
    public virtual extern ISearchJob BeginSearch([In] [MarshalAs(UnmanagedType.BStr)] string criteria, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743817)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern ISearchResult EndSearch([In] [MarshalAs(UnmanagedType.Interface)] ISearchJob searchJob);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743818)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string EscapeString([In] [MarshalAs(UnmanagedType.BStr)] string unescaped);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743819)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern IUpdateHistoryEntryCollection QueryHistory([In] int startIndex, [In] int Count);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743820)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern ISearchResult Search([In] [MarshalAs(UnmanagedType.BStr)] string criteria);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743822)]
    public virtual extern int GetTotalHistoryCount();

  }
}