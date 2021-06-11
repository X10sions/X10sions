using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [Guid("3442D4FE-224D-4CEE-98CF-30E0C4D229E6")]
  [TypeLibType(4304)]
  public interface IUpdateInstaller2 : IUpdateInstaller {
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
      [TypeLibFunc(1)]
      [DispId(1610743811)]
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
    bool ForceQuiet {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
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
  }



}   
