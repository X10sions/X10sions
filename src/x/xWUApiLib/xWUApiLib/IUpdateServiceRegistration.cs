using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xWUApiLib {
  [ComImport]
  [TypeLibType(4288)]
  [DefaultMember("RegistrationState")]
  [Guid("DDE02280-12B3-4E0B-937B-6747F6ACB286")]
  public interface IUpdateServiceRegistration {
    [ComAliasName("WUApiLib.UpdateServiceRegistrationState")]
    [DispId(0)]
    UpdateServiceRegistrationState RegistrationState {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(0)]
      [return: ComAliasName("WUApiLib.UpdateServiceRegistrationState")]
      get;
    }

    [DispId(1610743809)]
    string ServiceID {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743809)]
      [return: MarshalAs(UnmanagedType.BStr)]
      get;
    }

    [DispId(1610743810)]
    bool IsPendingRegistrationWithAU {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743810)]
      get;
    }

    [DispId(1610743811)]
    IUpdateService2 Service {
      [MethodImpl(MethodImplOptions.InternalCall)]
      [DispId(1610743811)]
      [return: MarshalAs(UnmanagedType.Interface)]
      get;
    }
  }


}
