using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [Guid("6A92B07A-D821-4682-B423-5C805022CC4D")]
  [DefaultMember("Title")]
  public interface IUpdate {
    [DispId(0)]
    string Title {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743809)]
    bool AutoSelectOnWebSites {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [DispId(1610743810)]
    UpdateCollection BundledUpdates {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743811)]
    bool CanRequireSource {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      get;
    }

    [DispId(1610743812)]
    ICategoryCollection Categories {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743812)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743813)]
    object Deadline {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743813)]
      [return: MarshalAs(UnmanagedType.Struct)]
      get;
    }

    [DispId(1610743814)]
    bool DeltaCompressedContentAvailable {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743814)]
      get;
    }

    [DispId(1610743815)]
    bool DeltaCompressedContentPreferred {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743815)]
      get;
    }

    [DispId(1610743816)]
    string Description {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743816)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743817)]
    bool EulaAccepted {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743817)]
      get;
    }

    [DispId(1610743818)]
    string EulaText {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743818)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743819)]
    string HandlerID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743819)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743820)]
    IUpdateIdentity Identity {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743820)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743821)]
    IImageInformation Image {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743821)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743822)]
    IInstallationBehavior InstallationBehavior {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743822)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743823)]
    bool IsBeta {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743823)]
      get;
    }

    [DispId(1610743824)]
    bool IsDownloaded {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743824)]
      get;
    }

    [DispId(1610743825)]
    bool IsHidden {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743825)]
      get;
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743825)]
      [param: In]
      set;
    }

    [DispId(1610743826)]
    bool IsInstalled {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743826)]
      get;
    }

    [DispId(1610743827)]
    bool IsMandatory {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743827)]
      get;
    }

    [DispId(1610743828)]
    bool IsUninstallable {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743828)]
      get;
    }

    [DispId(1610743829)]
    StringCollection Languages {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743829)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743830)]
    DateTime LastDeploymentChangeTime {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743830)]
      get;
    }

    [DispId(1610743831)]
    decimal MaxDownloadSize {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743831)]
      get;
    }

    [DispId(1610743832)]
    decimal MinDownloadSize {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743832)]
      get;
    }

    [DispId(1610743833)]
    StringCollection MoreInfoUrls {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743833)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743834)]
    string MsrcSeverity {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743834)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743835)]
    int RecommendedCpuSpeed {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743835)]
      get;
    }

    [DispId(1610743836)]
    int RecommendedHardDiskSpace {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743836)]
      get;
    }

    [DispId(1610743837)]
    int RecommendedMemory {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743837)]
      get;
    }

    [DispId(1610743838)]
    string ReleaseNotes {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743838)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743839)]
    StringCollection SecurityBulletinIDs {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743839)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743841)]
    StringCollection SupersededUpdateIDs {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743841)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743842)]
    string SupportUrl {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743842)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [ComAliasName("WUApiLib.UpdateType")]
    [DispId(1610743843)]
    UpdateType Type {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743843)]
      [return: ComAliasName("WUApiLib.UpdateType")]
      get;
    }

    [DispId(1610743844)]
    string UninstallationNotes {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743844)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743845)]
    IInstallationBehavior UninstallationBehavior {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743845)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743846)]
    StringCollection UninstallationSteps {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743846)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [DispId(1610743848)]
    StringCollection KBArticleIDs {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743848)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [ComAliasName("WUApiLib.DeploymentAction")]
    [DispId(1610743849)]
    DeploymentAction DeploymentAction {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743849)]
      [return: ComAliasName("WUApiLib.DeploymentAction")]
      get;
    }

    [DispId(1610743851)]
    [ComAliasName("WUApiLib.DownloadPriority")]
    DownloadPriority DownloadPriority {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743851)]
      [return: ComAliasName("WUApiLib.DownloadPriority")]
      get;
    }

    [DispId(1610743852)]
    IUpdateDownloadContentCollection DownloadContents {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743852)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743847)]
    void AcceptEula();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [DispId(1610743850)]
    void CopyFromCache([In] [MarshalAs(UnmanagedType.BStr)] string path, [In] bool toExtractCabFiles);
  }

}
