namespace System.Management {
  public static class ManagementObjectExtensions {

    public static T GetPropertyValue<T>(this ManagementObject mo, string propertyName, T defaultValue = default) {
      try {
        return (T)mo.GetPropertyValue(propertyName);
      } catch {
        return defaultValue;
      }
    }
  }
}