namespace System {
  public static class EnumExtensions {

    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute {
      var type = enumValue.GetType();
      var name = Enum.GetName(type, enumValue);
      return type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault();
    }
    
    public static IEnumerable<T> GetValuesCast<T>() where T : struct, IConvertible, IComparable, IFormattable => Enum.GetValues(typeof(T)).Cast<T>();
    public static IEnumerable<T> GetValuesOfType<T>() where T : Enum => Enum.GetValues(typeof(T)).OfType<T>();

    public static IEnumerable<T> GetValuesCast<T>(this T enumObj) where T : struct, IConvertible, IComparable, IFormattable => Enum.GetValues(enumObj.GetType()).Cast<T>();
    public static IEnumerable<T> GetValuesOfType<T>(this T enumObj) where T : Enum => Enum.GetValues(enumObj.GetType()).OfType<T>();

    public static string GetRandomName<T>() where T : struct, Enum => GetNames<T>().GetRandom();
    public static T GetRandomValue<T>() where T : struct, Enum => GetValues<T>().GetRandom();

    [Obsolete("Not introdcues until .NET 6")] public static string[] GetNames<TEnum>() where TEnum : struct, Enum =>  Enum.GetNames(typeof(TEnum));
    [Obsolete("Not introdcues until .NET 6")] public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum => (TEnum[])Enum.GetValues(typeof(TEnum));
  }
}