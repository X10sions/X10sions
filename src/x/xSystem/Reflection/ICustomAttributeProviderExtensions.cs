using System.Linq;

namespace System.Reflection {
  public static class ICustomAttributeProviderExtensions {
    public static bool HasCustomAttribute<T>(this ICustomAttributeProvider cap, bool inherit = false) where T : Attribute => cap.HasCustomAttribute(typeof(T), inherit);
    public static bool HasCustomAttribute(this ICustomAttributeProvider cap, Type attributeType, bool inherit = false) => cap.GetCustomAttributes(attributeType, inherit).Any();
  }
}