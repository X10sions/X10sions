namespace Common.ValueObjects;

public interface IMaxLength {
  int MaxLength { get; }
}

public interface IPadLeftValueObject<out T> : IValueObject<T>, IMaxLength {
  char PaddingChar { get; }
}

public interface IPadLeftStringValueObject : IValueObject<string>, IMaxLength {
  char PaddingChar { get; }
}

public interface IPadRightValueObject<out T> : IValueObject<T>, IMaxLength {
  char PaddingChar { get; }
}


public interface IPadRightStringValueObject : IValueObject<string>, IMaxLength {
  char PaddingChar { get; }
}

public static class IMaxLengthExtensions {
  public static string ToFixedLength<T>(this IPadLeftValueObject<T> valueObject) => valueObject.Value.ToString().PadLeft(valueObject.MaxLength, valueObject.PaddingChar);
  public static string ToFixedLength(this IPadLeftStringValueObject valueObject) => valueObject.Value.PadLeft(valueObject.MaxLength, valueObject.PaddingChar);
  public static string ToFixedLength<T>(this IPadRightValueObject<T> valueObject) => valueObject.Value.ToString().PadRight(valueObject.MaxLength, valueObject.PaddingChar);
  public static string ToFixedLength(this IPadRightStringValueObject valueObject) => valueObject.Value.PadRight(valueObject.MaxLength, valueObject.PaddingChar);
}