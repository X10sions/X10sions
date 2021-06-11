using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [DefaultMember("Message")]
  [TypeLibType(4288)]
  [Guid("A376DD5E-09D4-427F-AF7C-FED5B6E1C1D6")]
  public interface IUpdateException {
    [DispId(0)]
    string Message {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743809)]
    int HResult {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      get;
    }

    [ComAliasName("WUApiLib.UpdateExceptionContext")]
    [DispId(1610743810)]
    UpdateExceptionContext Context {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      [return: ComAliasName("WUApiLib.UpdateExceptionContext")]
      get;
    }
  }

}
