using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(2)]
  [ClassInterface(ClassInterfaceType.None)]
  [Guid("D2E0FE7F-D23E-48E1-93C0-6FA8CC346474")]
  public class UpdateInstallerClass : IUpdateInstaller2, UpdateInstaller, IUpdateInstaller4 {
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
    public extern virtual bool IsForced {
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
    public extern virtual IntPtr ParentHwnd {
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
    public extern virtual object parentWindow {
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
    public extern virtual UpdateCollection Updates {
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
    public extern virtual bool IsBusy {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743820)]
      get;
    }

    [DispId(1610743822)]
    public extern virtual bool AllowSourcePrompts {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      [param: In]
      set;
    }

    [DispId(1610743823)]
    public extern virtual bool RebootRequiredBeforeInstallation {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743823)]
      get;
    }

    [DispId(1610809345)]
    public extern virtual bool ForceQuiet {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610809345)]
      [param: In]
      set;
    }

    public extern virtual string IUpdateInstaller4_ClientApplicationID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [param: In]
      [param: MarshalAs(UnmanagedType.BStr)]
      set;
    }

    public extern virtual bool IUpdateInstaller4_IsForced {
      [MethodImpl(MethodImplOptions.InternalCall)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [param: In]
      set;
    }

    [ComAliasName("WUApiLib.wireHWND")]
    public extern virtual IntPtr IUpdateInstaller4_ParentHwnd {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [TypeLibFunc(1)]
      [return: ComAliasName("WUApiLib.wireHWND")]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [TypeLibFunc(1)]
      [param: In]
      [param: ComAliasName("WUApiLib.wireHWND")]
      set;
    }

    public extern virtual object IUpdateInstaller4_parentWindow {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [return: MarshalAs(UnmanagedType.IUnknown)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [param: In]
      [param: MarshalAs(UnmanagedType.IUnknown)]
      set;
    }

    public extern virtual UpdateCollection IUpdateInstaller4_Updates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [param: In]
      [param: MarshalAs(UnmanagedType.Interface)]
      set;
    }

    public extern virtual bool IUpdateInstaller4_IsBusy {
      [MethodImpl(MethodImplOptions.InternalCall)]
      get;
    }

    public extern virtual bool IUpdateInstaller4_AllowSourcePrompts {
      [MethodImpl(MethodImplOptions.InternalCall)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [param: In]
      set;
    }

    public extern virtual bool IUpdateInstaller4_RebootRequiredBeforeInstallation {
      [MethodImpl(MethodImplOptions.InternalCall)]
      get;
    }

    public extern virtual bool IUpdateInstaller4_ForceQuiet {
      [MethodImpl(MethodImplOptions.InternalCall)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [param: In]
      set;
    }

    public extern virtual bool AttemptCloseAppsIfNecessary {
      [MethodImpl(MethodImplOptions.InternalCall)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [param: In]
      set;
    }


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743814)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationJob BeginInstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743815)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationJob BeginUninstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);


    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743816)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult EndInstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743817)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult EndUninstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743818)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult Install();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743819)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult RunWizard([In] [MarshalAs(UnmanagedType.BStr)] string dialogTitle = "");

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743821)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult Uninstall();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationJob IUpdateInstaller4_BeginInstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationJob IUpdateInstaller4_BeginUninstall([In] [MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In] [MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In] [MarshalAs(UnmanagedType.Struct)] object state);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult IUpdateInstaller4_EndInstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);


    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult IUpdateInstaller4_EndUninstall([In] [MarshalAs(UnmanagedType.Interface)] IInstallationJob value);


    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult IUpdateInstaller4_Install();


    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult IUpdateInstaller4_RunWizard([In] [MarshalAs(UnmanagedType.BStr)] string dialogTitle = "");



    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public extern virtual IInstallationResult IUpdateInstaller4_Uninstall();



    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern virtual void Commit([In] uint dwFlags);

  }
}
