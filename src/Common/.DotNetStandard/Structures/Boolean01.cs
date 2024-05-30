using System.Runtime.Serialization;

namespace Common.Structures;

public struct Boolean01 :
  IComparable,
  IComparable<Boolean01>,
  IComparable<bool>,
  IComparable<decimal>,
  IComparable<double>,
  IComparable<int>,
  IComparable<string>,
  IConvertible,
  IEquatable<Boolean01>,
  IEquatable<bool>,
  IEquatable<decimal>,
  IEquatable<double>,
  IEquatable<int>,
  IEquatable<string>,
  IFormattable,
  ISerializable {
  public Boolean01(bool value) : this(GetStringValue(value)) { }
  public Boolean01(decimal value) : this(value.ToString()) { }
  public Boolean01(double value) : this(value.ToString()) { }
  public Boolean01(int value) : this(value.ToString()) { }
  public Boolean01(string value) : this() { StringValue = value; }

  public const int TrueInt = 1;
  public const string TrueString = "1";
  public const int FalseInt = 0;
  public const string FalseString = "0";

  public bool BoolValue { get => !string.IsNullOrWhiteSpace(StringValue) && StringValue != FalseString; set => StringValue = GetStringValue(value); }
  public decimal DecimalValue { get => decimal.Parse(StringValue); set => StringValue = value.ToString(); }
  public double DoubleValue { get => double.Parse(StringValue); set => StringValue = value.ToString(); }
  public int IntValue { get => int.Parse(StringValue); set => StringValue = value.ToString(); }
  public string StringValue { get; set; }

  #region IComparable
  public int CompareTo(object obj) {
    switch (obj) {
      //case null: return FalseInt;
      case int i: return i.CompareTo(IntValue);
      case bool b: return b.CompareTo(BoolValue);
      case decimal dec: return dec.CompareTo(IntValue);
      case double dbl: return dbl.CompareTo(IntValue);
    }
    throw new NotImplementedException();
  }
  public int CompareTo(Boolean01 other) => other.BoolValue.CompareTo(BoolValue);
  public int CompareTo(bool other) => other.CompareTo(BoolValue);
  public int CompareTo(decimal other) => other.CompareTo(DecimalValue);
  public int CompareTo(double other) => other.CompareTo(DoubleValue);
  public int CompareTo(int other) => other.CompareTo(IntValue);
  public int CompareTo(string other) => string.Compare(other, ToString(), StringComparison.Ordinal);
  #endregion

  #region IEquatable
  public bool Equals(Boolean01 other) => other.BoolValue.Equals(BoolValue);
  public bool Equals(bool other) => other.Equals(BoolValue);
  public bool Equals(decimal other) => other.Equals(DecimalValue);
  public bool Equals(double other) => other.Equals(DoubleValue);
  public bool Equals(int other) => other.Equals(IntValue);
  public bool Equals(string other) => other.Equals(ToString());
  #endregion

  #region IConvertible
  public bool ToBoolean(IFormatProvider provider) => Convert.ToBoolean(BoolValue, provider);
  public byte ToByte(IFormatProvider provider) => Convert.ToByte(StringValue, provider);
  public char ToChar(IFormatProvider provider) => Convert.ToChar(BoolValue ? TrueString : FalseString, provider);
  public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(StringValue, provider);
  public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(DecimalValue, provider);
  public double ToDouble(IFormatProvider provider) => Convert.ToDouble(DoubleValue, provider);
  public float ToSingle(IFormatProvider provider) => Convert.ToSingle(StringValue, provider);
  public int ToInt32(IFormatProvider provider) => Convert.ToInt32(IntValue, provider);
  public long ToInt64(IFormatProvider provider) => Convert.ToInt64(StringValue, provider);
  public object ToType(Type conversionType, IFormatProvider provider) {
    switch (conversionType) {
      case Type _ when conversionType == typeof(bool):
        return Convert.ChangeType(BoolValue, conversionType, provider);
      default:
        return Convert.ChangeType(StringValue, conversionType, provider);
    }
  }
  public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(StringValue, provider);
  public short ToInt16(IFormatProvider provider) => Convert.ToInt16(StringValue, provider);
  public string ToString(IFormatProvider provider) => Convert.ToString(BoolValue ? TrueString : FalseString, provider);
  public TypeCode GetTypeCode() => TypeCode.Boolean;
  public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(StringValue, provider);
  public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(StringValue, provider);
  public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(StringValue, provider);
  #endregion

  #region IFormattable
  public override string ToString() => StringValue;
  public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);

  #endregion

  #region ISerializable
  public void GetObjectData(SerializationInfo info, StreamingContext context) {
    if (info == null) {
      throw new ArgumentNullException(nameof(info));
    }
    // Serialize both the old and the new format
    info.AddValue(nameof(BoolValue), BoolValue);
    info.AddValue(nameof(DecimalValue), DecimalValue);
    info.AddValue(nameof(DoubleValue), DoubleValue);
    info.AddValue(nameof(IntValue), IntValue);
    info.AddValue(nameof(StringValue), StringValue);
  }
  #endregion

  public static string GetStringValue(bool value) => value ? TrueString : FalseString;
  public static int ToInt(bool value, int trueValue = TrueInt) => value ? trueValue : FalseInt;
  public static int? ToInt(bool? value, int trueValue = TrueInt) => value.HasValue ? ToInt(value.Value, trueValue) : null;
  public static bool ToBoolean(int value) => value == TrueInt;
  public static bool? ToBoolean(int? value) => value.HasValue ? ToBoolean(value.Value) : null;

  public static implicit operator Boolean01(bool value) => new Boolean01(value);
  public static implicit operator Boolean01(decimal value) => new Boolean01(value);
  public static implicit operator Boolean01(double value) => new Boolean01(value);
  public static implicit operator Boolean01(int value) => new Boolean01(value);
  public static implicit operator Boolean01(string value) => new Boolean01(value);

  public static implicit operator bool(Boolean01 value) => value.BoolValue;
  public static implicit operator decimal(Boolean01 value) => value.DecimalValue;
  public static implicit operator double(Boolean01 value) => value.DoubleValue;
  public static implicit operator int(Boolean01 value) => value.IntValue;
  public static implicit operator string(Boolean01 value) => value.StringValue;

}
