#nullable enable
using System;

namespace Common.Attributes {
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
}