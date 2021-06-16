namespace System.Security.Principal {
  public static class IIdentityExtensions {

    public static bool IsWindowsIdentity(this IIdentity identity) => identity is WindowsIdentity;
    public static WindowsIdentity AsWindowsIdentity(this IIdentity identity) => identity.As<WindowsIdentity>();

  }
}