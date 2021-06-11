using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("EF8208EA-2304-492D-9109-23813B0958E1")]
  [TypeLibType(4304)]
  public interface IUpdateInstaller4 : IUpdateInstaller3 {
    [DispId(1610743809)]
    new string ClientApplicationID {
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
    new bool IsForced {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [param: In]
      set;
    }

    [ComAliasName("WUApiLib.wireHWND")]
    [DispId(1610743811)]
    new IntPtr ParentHwnd {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [TypeLibFunc(1)]
      [return: ComAliasName("WUApiLib.wireHWND")]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [TypeLibFunc(1)]
      [param: In]
      [param: ComAliasName("WUApiLib.wireHWND")]
      set;
    }

    [DispId(1610743812)]
    new object parentWindow {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [return: MarshalAs(UnmanagedType.IUnknown)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [param: In]
      [param: MarshalAs(UnmanagedType.IUnknown)]
      set;
    }

    [DispId(1610743813)]
    new UpdateCollection Updates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      [param: In]
      [param: MarshalAs(UnmanagedType.Interface)]
      set;
    }

    [DispId(1610743820)]
    new bool IsBusy {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743820)]
      get;
    }

    [DispId(1610743822)]
    new bool AllowSourcePrompts {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      [param: In]
      set;
    }

    [DispId(1610743823)]
    new bool RebootRequiredBeforeInstallation {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743823)]
      get;
    }

    [DispId(1610809345)]
    new bool ForceQuiet {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [param: In]
      set;
    }

    [DispId(1610874881)]
    new bool AttemptCloseAppsIfNecessary {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610874881)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610874881)]
      [param: In]
      set;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IInstallationJob BeginInstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IInstallationJob BeginUninstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743816)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IInstallationResult EndInstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743817)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IInstallationResult EndUninstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743818)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IInstallationResult Install();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743819)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IInstallationResult RunWizard([In] [MarshalAs(UnmanagedType.BStr)] string dialogTitle = "");

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743821)]
    [return: MarshalAs(UnmanagedType.Interface)]
    new IInstallationResult Uninstall();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610874882)]
    void Commit([In] uint dwFlags);
  }

}
