using Microsoft.WindowsAPICodePack.Internal;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Taskbar {
  public static class TaskbarNativeMethods {
    public static class TaskbarGuids {
      public static Guid IObjectArray = new Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9");
      public static Guid IUnknown = new Guid("00000000-0000-0000-C000-000000000046");
    }

    internal const int WmCommand = 0x0111;

    // Register Window Message used by Shell to notify that the corresponding taskbar button has been added to the taskbar.
    public static readonly uint WmTaskbarButtonCreated = RegisterWindowMessage("TaskbarButtonCreated");

    internal const uint WmDwmSendIconThumbnail = 0x0323;
    internal const uint WmDwmSendIconicLivePreviewBitmap = 0x0326;

    #region Methods

    [DllImport("shell32.dll")]
    public static extern void SetCurrentProcessExplicitAppUserModelID(
        [MarshalAs(UnmanagedType.LPWStr)] string AppID);

    [DllImport("shell32.dll")]
    public static extern void GetCurrentProcessExplicitAppUserModelID(
        [Out(), MarshalAs(UnmanagedType.LPWStr)] out string AppID);

    [DllImport("shell32.dll")]
    public static extern void SHAddToRecentDocs(
        ShellAddToRecentDocs flags,
        [MarshalAs(UnmanagedType.LPWStr)] string path);

    public static void SHAddToRecentDocs(string path) {
      SHAddToRecentDocs(ShellAddToRecentDocs.PathW, path);
    }

    [DllImport("user32.dll", EntryPoint = "RegisterWindowMessage", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);


    [DllImport("shell32.dll")]
    public static extern int SHGetPropertyStoreForWindow(
        IntPtr hwnd,
        ref Guid iid /*IID_IPropertyStore*/,
        [Out(), MarshalAs(UnmanagedType.Interface)]out IPropertyStore propertyStore);

    /// <summary>
    /// Sets the window's application id by its window handle.
    /// </summary>
    /// <param name="hwnd">The window handle.</param>
    /// <param name="appId">The application id.</param>
    public static void SetWindowAppId(IntPtr hwnd, string appId) {
      SetWindowProperty(hwnd, SystemProperties.System.AppUserModel.ID, appId);
    }

    public static void SetWindowProperty(IntPtr hwnd, PropertyKey propkey, string value) {
      // Get the IPropertyStore for the given window handle
      IPropertyStore propStore = GetWindowPropertyStore(hwnd);

      // Set the value
      using (PropVariant pv = new PropVariant(value)) {
        HResult result = propStore.SetValue(ref propkey, pv);
        if (!CoreErrorHelper.Succeeded(result)) {
          throw new ShellException(result);
        }
      }


      // Dispose the IPropertyStore and PropVariant
      Marshal.ReleaseComObject(propStore);
    }

    public static IPropertyStore GetWindowPropertyStore(IntPtr hwnd) {
      Guid guid = new Guid(ShellIIDGuid.IPropertyStore);
      int rc = SHGetPropertyStoreForWindow(
                hwnd,
                ref guid,
                out var propStore);
      if (rc != 0) {
        throw Marshal.GetExceptionForHR(rc);
      }
      return propStore;
    }

    #endregion
  }
}
