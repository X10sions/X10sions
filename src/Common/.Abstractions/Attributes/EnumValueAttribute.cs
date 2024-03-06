namespace Common.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class EnumValueAttribute<T> : Attribute {
  public EnumValueAttribute(T value, bool isDefault = false) {
    IsDefault = isDefault;
    Value = value;
  }
  public T Value { get; set; }
  public bool IsDefault { get; set; }

  public static EnumValueAttribute<string> Create(string value, bool isDefault = false) => new(value, isDefault);
}

//[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
//public class EnumValueAttribute : EnumValueAttribute<object?> {
//  public EnumValueAttribute(object? value, bool isDefault = false) : base(value, isDefault) { }
//}

//[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
//public class EnumStringValueAttribute : EnumValueAttribute<string> {
//  public EnumStringValueAttribute(string value, bool isDefault = false) : base(value, isDefault) { }
//}

public static class EnumValueAttributeExtensions {
  public static string StringValue<T>(this T value) where T : Enum => value.GetCustomAttribute<EnumValueAttribute<string>>().Value;
  //public static IEnumerable<string> StringValues<T>(this T[] values) where T : Enum => values.Select(x => x.StringValue());

  //public static EnumValueAttribute GetEnumValueAttribute<T>(this T value) where T : Enum => value.GetCustomAttribute<EnumValueAttribute>();
  //public static EnumValueAttribute<TValue> GetEnumValueAttribute<T, TValue>(this T value) where T : Enum => value.GetCustomAttribute<EnumValueAttribute<TValue>>();
  //public static EnumStringValueAttribute GetEnumStringValueAttribute<T>(this T value) where T : Enum => value.GetCustomAttribute<EnumStringValueAttribute>();
}