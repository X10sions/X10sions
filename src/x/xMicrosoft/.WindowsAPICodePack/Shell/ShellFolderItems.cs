using Microsoft.WindowsAPICodePack.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell {
  public class ShellFolderItems : IEnumerator<ShellObject> {
    #region Private Fields

    private IEnumIDList nativeEnumIdList;
    private ShellObject currentItem;
    ShellContainer nativeShellFolder;

    #endregion

    #region Internal Constructor

    internal ShellFolderItems(ShellContainer nativeShellFolder) {
      this.nativeShellFolder = nativeShellFolder;

      HResult hr = nativeShellFolder.NativeShellFolder.EnumObjects(
          IntPtr.Zero,
          ShellNativeMethods.ShellFolderEnumerationOptions.Folders | ShellNativeMethods.ShellFolderEnumerationOptions.NonFolders,
          out nativeEnumIdList);


      if (!CoreErrorHelper.Succeeded(hr)) {
        if (hr == HResult.Canceled) {
          throw new System.IO.FileNotFoundException();
        } else {
          throw new ShellException(hr);
        }
      }


    }

    #endregion

    #region IEnumerator<ShellObject> Members

    public ShellObject Current {
      get {
        return currentItem;
      }
    }

    #endregion

    #region IDisposable Members

    public void Dispose() {
      if (nativeEnumIdList != null) {
        Marshal.ReleaseComObject(nativeEnumIdList);
        nativeEnumIdList = null;
      }
    }

    #endregion

    #region IEnumerator Members

    object IEnumerator.Current {
      get { return currentItem; }

    }

      public bool MoveNext() {
      if (nativeEnumIdList == null) { return false; }
      uint itemsRequested = 1;
      HResult hr = nativeEnumIdList.Next(itemsRequested, out var item, out var numItemsReturned);

      if (numItemsReturned < itemsRequested || hr != HResult.Ok) { return false; }

      currentItem = ShellObjectFactory.Create(item, nativeShellFolder);

      return true;
    }

     public void Reset() {
      if (nativeEnumIdList != null) {
        nativeEnumIdList.Reset();
      }
    }


    #endregion
  }
}
