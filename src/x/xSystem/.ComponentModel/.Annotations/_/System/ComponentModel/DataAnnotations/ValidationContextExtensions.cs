using System.Reflection;

namespace System.ComponentModel.DataAnnotations {
  public static class ValidationContextExtensions {

    public static int CompareToProperty(this ValidationContext validationContext, object value, string compareToPropertyName) {
      var currentValue = value.As<object, IComparable>();
      var comparisonValue = validationContext.GetPropertyValue(compareToPropertyName).As<object, IComparable>();
      if (!ReferenceEquals(value.GetType(), comparisonValue.GetType())) {
        throw new ArgumentException("The properties types must be the same");
      }
      return currentValue.CompareTo(comparisonValue);
    }

    public static bool IsCompareToType(this ValidationContext validationContext, object value, string compareToPropertyName, IComparableExtensions.CompareToType compareToType) {
      var thisValue = value.As<object, IComparable>();
      var otherValue = validationContext.GetPropertyValue(compareToPropertyName).As<object, IComparable>();
      return thisValue.IsCompareToType(otherValue, compareToType);
    }

    public static PropertyInfo GetPropertyInfo(this ValidationContext validationContext, string propertyName) => validationContext.ObjectType.GetProperty(propertyName);

    public static object GetPropertyValue(this ValidationContext validationContext, string propertyName) {
      var property = validationContext.GetPropertyInfo(propertyName);
      if (property == null) {
        throw new ArgumentException("Comparison property with this name not found");
      }
      return property.GetValue(validationContext.ObjectInstance);
    }

  }
}