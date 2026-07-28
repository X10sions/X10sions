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

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class EnumValueAttribute : Attribute {
  public EnumValueAttribute(object? value, bool isDefault) : this(value) {
    IsDefault = isDefault;
  }
  public EnumValueAttribute(object? value) {
    Value = value;
  }
  public object? Value { get; set; }
  public bool IsDefault { get; set; }
  //public bool IsNull => Value == null;
  //public bool IsNullOrEmpty => string.IsNullOrEmpty(Value);
  //public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(Value );
}

public static class EnumValueAttributeExtensions {
  public static string StringValue<T>(this T value) where T : Enum => value.GetCustomAttribute<EnumValueAttribute<string>>().Value;
}



