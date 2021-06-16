using Microsoft.Internal.WindowsBase;
using System;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Internal.Permissions {
  [Serializable]
  [FriendAccessAllowed]
  public abstract class InternalParameterlessPermissionBase : CodeAccessPermission, IUnrestrictedPermission {
    protected InternalParameterlessPermissionBase(PermissionState state) {
      switch (state) {
        case PermissionState.Unrestricted:
          break;
        default:
          throw new ArgumentException(SR.Get("InvalidPermissionStateValue", state), nameof(state));
      }
    }

    public bool IsUnrestricted() {
      return true;
    }

    public override SecurityElement ToXml() {
      var securityElement = new SecurityElement("IPermission");
      var type = GetType();
      var stringBuilder = new StringBuilder(type.Assembly.ToString());
      stringBuilder.Replace('"', '\'');
      securityElement.AddAttribute("class", type.FullName + ", " + stringBuilder);
      securityElement.AddAttribute("version", "1");
      return securityElement;
    }

    public override void FromXml(SecurityElement elem) {
    }

    public override IPermission Intersect(IPermission target) {
      if (target == null) {
        return null;
      }
      if (target.GetType() != GetType()) {
        throw new ArgumentException(SR.Get("InvalidPermissionType", GetType().FullName), nameof(target));
      }
      return Copy();
    }

    public override bool IsSubsetOf(IPermission target) {
      if (target == null) {
        return false;
      }
      if (target.GetType() != GetType()) {
        throw new ArgumentException(SR.Get("InvalidPermissionType", GetType().FullName), "target");
      }
      return true;
    }

    public override IPermission Union(IPermission target) {
      if (target == null) {
        return null;
      }
      if (target.GetType() != GetType()) {
        throw new ArgumentException(SR.Get("InvalidPermissionType", GetType().FullName), "target");
      }
      return Copy();
    }
  }
}
