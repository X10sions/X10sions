using System;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Internal.Permissions {

  [Serializable]
  public class UserInitiatedRoutedEventPermission : InternalParameterlessPermissionBase {
    public UserInitiatedRoutedEventPermission()
      : this(PermissionState.Unrestricted) {
    }

    public UserInitiatedRoutedEventPermission(PermissionState state)
      : base(state) {
    }

    public override IPermission Copy() {
      return new UserInitiatedRoutedEventPermission();
    }
  }
}
