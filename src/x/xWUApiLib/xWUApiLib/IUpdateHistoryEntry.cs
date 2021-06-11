using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("BE56A644-AF0E-4E0E-A311-C1D8E695CBFF")]
  [TypeLibType(4288)]
  public interface IUpdateHistoryEntry {
    [DispId(1610743809)]
    [ComAliasName("WUApiLib.UpdateOperation")]
    UpdateOperation Operation {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: ComAliasName("WUApiLib.UpdateOperation")]
      get;
    }

    [DispId(1610743810)]
    [ComAliasName("WUApiLib.OperationResultCode")]
    OperationResultCode ResultCode {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: ComAliasName("WUApiLib.OperationResultCode")]
      get;
    }

    [DispId(1610743811)]
    int HResult {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
    }

    [DispId(1610743812)]
    DateTime Date {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      get;
    }

    [DispId(1610743813)]
    IUpdateIdentity UpdateIdentity {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743814)]
    string Title {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743814)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743815)]
    string Description {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743815)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743816)]
    int UnmappedResultCode {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743816)]
      get;
    }

    [DispId(1610743817)]
    string ClientApplicationID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743817)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [ComAliasName("WUApiLib.ServerSelection")]
    [DispId(1610743818)]
    ServerSelection ServerSelection {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743818)]
      [return: ComAliasName("WUApiLib.ServerSelection")]
      get;
    }

    [DispId(1610743819)]
    string ServiceID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743819)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743820)]
    StringCollection UninstallationSteps {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743820)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743821)]
    string UninstallationNotes {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743822)]
    string SupportUrl {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }
  }


}
