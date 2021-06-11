using Microsoft.WindowsAPICodePack.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem {
  #region Property System COM Interfaces

  /// <summary>
  /// An in-memory property store cache
  /// </summary>
  [ComImport]
  [Guid(ShellIIDGuid.IPropertyStoreCache)]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IPropertyStoreCache {
    /// <summary>
    /// Gets the state of a property stored in the cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult GetState(ref PropertyKey key, [Out] out PropertyStoreCacheState state);

    /// <summary>
    /// Gets the valeu and state of a property in the cache
    /// </summary>
    /// <param name="propKey"></param>
    /// <param name="pv"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult GetValueAndState(ref PropertyKey propKey, [Out] PropVariant pv, [Out] out PropertyStoreCacheState state);

    /// <summary>
    /// Sets the state of a property in the cache.
    /// </summary>
    /// <param name="propKey"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult SetState(ref PropertyKey propKey, PropertyStoreCacheState state);

    /// <summary>
    /// Sets the value and state in the cache.
    /// </summary>
    /// <param name="propKey"></param>
    /// <param name="pv"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult SetValueAndState(ref PropertyKey propKey, [In] PropVariant pv, PropertyStoreCacheState state);
  }

  #endregion
}