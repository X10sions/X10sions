﻿using Microsoft.WindowsAPICodePack.Internal;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using System;
using System.Diagnostics;

namespace Microsoft.WindowsAPICodePack.Shell {
  public class ChangeNotifyLock {
    private uint _event = 0;

    internal ChangeNotifyLock(Message message) {
      IntPtr lockId = ShellNativeMethods.SHChangeNotification_Lock(
              message.WParam, (int)message.LParam, out var pidl, out _event);
      try {
        Trace.TraceInformation("Message: {0}", (ShellObjectChangeTypes)_event);

        var notifyStruct = pidl.MarshalAs<ShellNativeMethods.ShellNotifyStruct>();

        Guid guid = new Guid(ShellIIDGuid.IShellItem2);
        if (notifyStruct.item1 != IntPtr.Zero &&
            (((ShellObjectChangeTypes)_event) & ShellObjectChangeTypes.SystemImageUpdate) == ShellObjectChangeTypes.None) {
          if (CoreErrorHelper.Succeeded(ShellNativeMethods.SHCreateItemFromIDList(
              notifyStruct.item1, ref guid, out var nativeShellItem))) {
            nativeShellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath,
                out var name);
            ItemName = name;

            Trace.TraceInformation("Item1: {0}", ItemName);
          }
        } else {
          ImageIndex = notifyStruct.item1.ToInt32();
        }

        if (notifyStruct.item2 != IntPtr.Zero) {
          if (CoreErrorHelper.Succeeded(ShellNativeMethods.SHCreateItemFromIDList(
              notifyStruct.item2, ref guid, out var nativeShellItem))) {
            nativeShellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.FileSystemPath,
                out var name);
            ItemName2 = name;

            Trace.TraceInformation("Item2: {0}", ItemName2);
          }
        }
      } finally {
        if (lockId != IntPtr.Zero) {
          ShellNativeMethods.SHChangeNotification_Unlock(lockId);
        }
      }

    }

    public bool FromSystemInterrupt {
      get {
        return ((ShellObjectChangeTypes)_event & ShellObjectChangeTypes.FromInterrupt)
            != ShellObjectChangeTypes.None;
      }
    }

    public int ImageIndex { get; private set; }
    public string ItemName { get; private set; }
    public string ItemName2 { get; private set; }

    public ShellObjectChangeTypes ChangeType { get { return (ShellObjectChangeTypes)_event; } }


  }
}
