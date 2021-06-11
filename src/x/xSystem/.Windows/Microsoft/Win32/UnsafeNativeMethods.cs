using Microsoft.Internal.WindowsBase;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32 {
  [FriendAccessAllowed]
  internal sealed partial class UnsafeNativeMethods {
    private struct POINTSTRUCT {
      public int x;
      public int y;
      public POINTSTRUCT(int x, int y) {
        this.x = x;
        this.y = y;
      }
    }

    public enum MonitorOpts {
      MONITOR_DEFAULTTONULL,
      MONITOR_DEFAULTTOPRIMARY,
      MONITOR_DEFAULTTONEAREST
    }

    public enum MonitorDpiType {
      MDT_Effective_DPI,
      MDT_Angular_DPI,
      MDT_Raw_DPI
    }

    public enum ProcessDpiAwareness {
      Process_DPI_Unaware,
      Process_System_DPI_Aware,
      Process_Per_Monitor_DPI_Aware
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class ShellExecuteInfo {
      public int cbSize;
      public ShellExecuteFlags fMask;
      public IntPtr hwnd;
      public string lpVerb;
      public string lpFile;
      public string lpParameters;
      public string lpDirectory;
      public int nShow;
      public IntPtr hInstApp;
      public IntPtr lpIDList;
      public string lpClass;
      public IntPtr hkeyClass;
      public int dwHotKey;
      public IntPtr hIcon;
      public IntPtr hProcess;
    }

    [Flags]
    internal enum ShellExecuteFlags {
      SEE_MASK_CLASSNAME = 0x1,
      SEE_MASK_CLASSKEY = 0x3,
      SEE_MASK_NOCLOSEPROCESS = 0x40,
      SEE_MASK_FLAG_DDEWAIT = 0x100,
      SEE_MASK_DOENVSUBST = 0x200,
      SEE_MASK_FLAG_NO_UI = 0x400,
      SEE_MASK_UNICODE = 0x4000,
      SEE_MASK_NO_CONSOLE = 0x8000,
      SEE_MASK_ASYNCOK = 0x100000,
      SEE_MASK_HMONITOR = 0x200000,
      SEE_MASK_NOZONECHECKS = 0x800000,
      SEE_MASK_NOQUERYCLASSSTORE = 0x1000000,
      SEE_MASK_WAITFORINPUTIDLE = 0x2000000
    }

    [Flags]
    internal enum LoadLibraryFlags : uint {
      None = 0x0,
      DONT_RESOLVE_DLL_REFERENCES = 0x1,
      LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x10,
      LOAD_LIBRARY_AS_DATAFILE = 0x2,
      LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x40,
      LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x20,
      LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x200,
      LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x1000,
      LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x100,
      LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x800,
      LOAD_LIBRARY_SEARCH_USER_DIRS = 0x400,
      LOAD_WITH_ALTERED_SEARCH_PATH = 0x8
    }

    [Flags]
    internal enum GetModuleHandleFlags : uint {
      None = 0x0,
      GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS = 0x4,
      GET_MODULE_HANDLE_EX_FLAG_PIN = 0x1,
      GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT = 0x2
    }

    [SecurityCritical]
    public delegate bool EnumChildrenCallback(IntPtr hwnd, IntPtr lParam);

    public enum EXTENDED_NAME_FORMAT {
      NameUnknown = 0,
      NameFullyQualifiedDN = 1,
      NameSamCompatible = 2,
      NameDisplay = 3,
      NameUniqueId = 6,
      NameCanonical = 7,
      NameUserPrincipal = 8,
      NameCanonicalEx = 9,
      NameServicePrincipal = 10
    }

    [ComImport]
    [SecurityCritical(SecurityCriticalScope.Everything)]
    [SuppressUnmanagedCodeSecurity]
    [Guid("00000122-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleDropTarget {
      [PreserveSig] int OleDragEnter([In] [MarshalAs(UnmanagedType.Interface)] object pDataObj, [In] [MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] [MarshalAs(UnmanagedType.U8)] long pt, [In] [Out] ref int pdwEffect);
      [PreserveSig] int OleDragOver([In] [MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] [MarshalAs(UnmanagedType.U8)] long pt, [In] [Out] ref int pdwEffect);
      [PreserveSig] int OleDragLeave();
      [PreserveSig] int OleDrop([In] [MarshalAs(UnmanagedType.Interface)] object pDataObj, [In] [MarshalAs(UnmanagedType.U4)] int grfKeyState, [In] [MarshalAs(UnmanagedType.U8)] long pt, [In] [Out] ref int pdwEffect);
    }

  }
}