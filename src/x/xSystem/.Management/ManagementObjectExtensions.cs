namespace System.Management {
  public static class ManagementObjectExtensions {

    public static T GetPropertyValue<T>(this ManagementObject mo, string propertyName) => mo.GetPropertyValue<T>(propertyName);

  }
}
