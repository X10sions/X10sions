using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [ComConversionLoss]
  [Guid("7B929C68-CCDC-4226-96B1-8724600B54C2")]
  [TypeLibType(4288)]
  public interface IUpdateInstaller {
    [DispId(1610743809)]
    string ClientApplicationID {
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
    bool IsForced {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [param: In]
      set;
    }

    [DispId(1610743811)]
    [ComAliasName("WUApiLib.wireHWND")]
    IntPtr ParentHwnd {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [TypeLibFunc(1)]
      [DispId(1610743811)]
      [return: ComAliasName("WUApiLib.wireHWND")]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [TypeLibFunc(1)]
      [DispId(1610743811)]
      [param: In]
      [param: ComAliasName("WUApiLib.wireHWND")]
      set;
    }

    [DispId(1610743812)]
    object parentWindow {
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
    UpdateCollection Updates {
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
    bool IsBusy {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743820)]
      get;
    }

    [DispId(1610743822)]
    bool AllowSourcePrompts {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      [param: In]
      set;
    }

    [DispId(1610743823)]
    bool RebootRequiredBeforeInstallation {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743823)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IInstallationJob BeginInstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IInstallationJob BeginUninstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743816)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IInstallationResult EndInstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743817)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IInstallationResult EndUninstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743818)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IInstallationResult Install();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743819)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IInstallationResult RunWizard([In] [MarshalAs(UnmanagedType.BStr)] string dialogTitle = "");

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743821)]
    [return: MarshalAs(UnmanagedType.Interface)]
    IInstallationResult Uninstall();
  }

}
