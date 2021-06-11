using Microsoft.Internal.WindowsBase;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Internal {
  [FriendAccessAllowed]
  internal static class Invariant {
    [SecurityCritical]
    [SecurityTreatAsSafe]
    private static bool _strict;

    private const bool _strictDefaultValue = false;

    internal static bool Strict {
      get => _strict;
      set => _strict = value;
    }

    private static bool IsDialogOverrideEnabled {
      [SecurityCritical]
      [SecurityTreatAsSafe]
      get {
        bool flag = false;
        PermissionSet permissionSet = new PermissionSet(PermissionState.None);
        RegistryPermission perm = new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NetFramework");
        permissionSet.AddPermission(perm);
        permissionSet.Assert();
        RegistryKey registryKey = default(RegistryKey);
        object value = default(object);
        string text = default(string);
        try {
          registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework");
          value = registryKey.GetValue("DbgJITDebugLaunchSetting");
          text = (registryKey.GetValue("DbgManagedDebugger") as string);
        } finally {
          PermissionSet.RevertAssert();
        }
        if (registryKey != null) {
          flag = (value is int && ((int)value & 2) != 0);
          if (flag) {
            flag = (text != null && text.Length > 0);
          }
        }
        return flag;
      }
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    static Invariant() {
      _strict = false;
    }

    internal static void Assert(bool condition) {
      if (!condition) {
        FailFast(null, null);
      }
    }

    internal static void Assert(bool condition, string invariantMessage) {
      if (!condition) {
        FailFast(invariantMessage, null);
      }
    }

    internal static void Assert(bool condition, string invariantMessage, string detailMessage) {
      if (!condition) {
        FailFast(invariantMessage, detailMessage);
      }
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    private static void FailFast(string message, string detailMessage) {
      if (IsDialogOverrideEnabled) {
        Debugger.Break();
      }
      Environment.FailFast(SR.Get("InvariantFailure"));
    }
  }
}
