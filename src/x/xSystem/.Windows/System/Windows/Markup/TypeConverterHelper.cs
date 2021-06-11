using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Markup {

  static class TypeConverterHelper {
    private static CultureInfo invariantEnglishUS = CultureInfo.InvariantCulture;

    internal static CultureInfo InvariantEnglishUS => invariantEnglishUS;

    internal static MemberInfo GetMemberInfoForPropertyConverter(object dpOrPiOrMi) {
      MemberInfo memberInfo = dpOrPiOrMi as PropertyInfo;
      if (memberInfo == null) {
        DependencyProperty dependencyProperty = dpOrPiOrMi as DependencyProperty;
        MethodInfo methodInfo;
        if (dependencyProperty != null) {
          memberInfo = dependencyProperty.OwnerType.GetProperty(dependencyProperty.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          if (memberInfo == null) {
            memberInfo = dependencyProperty.OwnerType.GetMethod("Get" + dependencyProperty.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          }
        } else if ((methodInfo = (dpOrPiOrMi as MethodInfo)) != null) {
          memberInfo = ((methodInfo.GetParameters().Length != 1) ? methodInfo.DeclaringType.GetMethod("Get" + methodInfo.Name.Substring("Set".Length), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy) : methodInfo);
        }
      }
      return memberInfo;
    }

    internal static Type GetConverterType(MemberInfo memberInfo) {
      string typeConverterAttributeData = ReflectionHelper.GetTypeConverterAttributeData(memberInfo, out var converterType);
      if (converterType == null) {
        converterType = GetConverterTypeFromName(typeConverterAttributeData);
      }
      return converterType;
    }

    internal static Type GetConverterType(Type type) {
      string typeConverterAttributeData = ReflectionHelper.GetTypeConverterAttributeData(type, out var converterType);
      if (converterType == null) {
        converterType = GetConverterTypeFromName(typeConverterAttributeData);
      }
      return converterType;
    }

    private static Type GetConverterTypeFromName(string converterName) {
      Type type = null;
      if (!string.IsNullOrEmpty(converterName)) {
        type = ReflectionHelper.GetQualifiedType(converterName);
        if (type != null && !ReflectionHelper.IsPublicType(type)) {
          type = null;
        }
      }
      return type;
    }

    internal static Type GetCoreConverterTypeFromCustomType(Type type) {
      Type result = null;
      if (type.IsEnum) {
        result = typeof(EnumConverter);
      } else if (typeof(int).IsAssignableFrom(type)) {
        result = typeof(Int32Converter);
      } else if (typeof(short).IsAssignableFrom(type)) {
        result = typeof(Int16Converter);
      } else if (typeof(long).IsAssignableFrom(type)) {
        result = typeof(Int64Converter);
      } else if (typeof(uint).IsAssignableFrom(type)) {
        result = typeof(UInt32Converter);
      } else if (typeof(ushort).IsAssignableFrom(type)) {
        result = typeof(UInt16Converter);
      } else if (typeof(ulong).IsAssignableFrom(type)) {
        result = typeof(UInt64Converter);
      } else if (typeof(bool).IsAssignableFrom(type)) {
        result = typeof(BooleanConverter);
      } else if (typeof(double).IsAssignableFrom(type)) {
        result = typeof(DoubleConverter);
      } else if (typeof(float).IsAssignableFrom(type)) {
        result = typeof(SingleConverter);
      } else if (typeof(byte).IsAssignableFrom(type)) {
        result = typeof(ByteConverter);
      } else if (typeof(sbyte).IsAssignableFrom(type)) {
        result = typeof(SByteConverter);
      } else if (typeof(char).IsAssignableFrom(type)) {
        result = typeof(CharConverter);
      } else if (typeof(decimal).IsAssignableFrom(type)) {
        result = typeof(DecimalConverter);
      } else if (typeof(TimeSpan).IsAssignableFrom(type)) {
        result = typeof(TimeSpanConverter);
      } else if (typeof(Guid).IsAssignableFrom(type)) {
        result = typeof(GuidConverter);
      } else if (typeof(string).IsAssignableFrom(type)) {
        result = typeof(StringConverter);
      } else if (typeof(CultureInfo).IsAssignableFrom(type)) {
        result = typeof(CultureInfoConverter);
      } else if (typeof(Type).IsAssignableFrom(type)) {
        result = typeof(TypeTypeConverter);
      } else if (typeof(DateTime).IsAssignableFrom(type)) {
        result = typeof(DateTimeConverter2);
      }
      return result;
    }

    private static TypeConverter GetCoreConverterFromCoreType(Type type) {
      TypeConverter result = null;
      if (type == typeof(int)) {
        result = new Int32Converter();
      } else if (type == typeof(short)) {
        result = new Int16Converter();
      } else if (type == typeof(long)) {
        result = new Int64Converter();
      } else if (type == typeof(uint)) {
        result = new UInt32Converter();
      } else if (type == typeof(ushort)) {
        result = new UInt16Converter();
      } else if (type == typeof(ulong)) {
        result = new UInt64Converter();
      } else if (type == typeof(bool)) {
        result = new BooleanConverter();
      } else if (type == typeof(double)) {
        result = new DoubleConverter();
      } else if (type == typeof(float)) {
        result = new SingleConverter();
      } else if (type == typeof(byte)) {
        result = new ByteConverter();
      } else if (type == typeof(sbyte)) {
        result = new SByteConverter();
      } else if (type == typeof(char)) {
        result = new CharConverter();
      } else if (type == typeof(decimal)) {
        result = new DecimalConverter();
      } else if (type == typeof(TimeSpan)) {
        result = new TimeSpanConverter();
      } else if (type == typeof(Guid)) {
        result = new GuidConverter();
      } else if (type == typeof(string)) {
        result = new StringConverter();
      } else if (type == typeof(CultureInfo)) {
        result = new CultureInfoConverter();
      } else if (type == typeof(Type)) {
        result = new TypeTypeConverter();
      } else if (type == typeof(DateTime)) {
        result = new DateTimeConverter2();
      } else if (ReflectionHelper.IsNullableType(type)) {
        result = new NullableConverter(type);
      }
      return result;
    }

    internal static TypeConverter GetCoreConverterFromCustomType(Type type) {
      TypeConverter result = null;
      if (type.IsEnum) {
        result = new EnumConverter(type);
      } else if (typeof(int).IsAssignableFrom(type)) {
        result = new Int32Converter();
      } else if (typeof(short).IsAssignableFrom(type)) {
        result = new Int16Converter();
      } else if (typeof(long).IsAssignableFrom(type)) {
        result = new Int64Converter();
      } else if (typeof(uint).IsAssignableFrom(type)) {
        result = new UInt32Converter();
      } else if (typeof(ushort).IsAssignableFrom(type)) {
        result = new UInt16Converter();
      } else if (typeof(ulong).IsAssignableFrom(type)) {
        result = new UInt64Converter();
      } else if (typeof(bool).IsAssignableFrom(type)) {
        result = new BooleanConverter();
      } else if (typeof(double).IsAssignableFrom(type)) {
        result = new DoubleConverter();
      } else if (typeof(float).IsAssignableFrom(type)) {
        result = new SingleConverter();
      } else if (typeof(byte).IsAssignableFrom(type)) {
        result = new ByteConverter();
      } else if (typeof(sbyte).IsAssignableFrom(type)) {
        result = new SByteConverter();
      } else if (typeof(char).IsAssignableFrom(type)) {
        result = new CharConverter();
      } else if (typeof(decimal).IsAssignableFrom(type)) {
        result = new DecimalConverter();
      } else if (typeof(TimeSpan).IsAssignableFrom(type)) {
        result = new TimeSpanConverter();
      } else if (typeof(Guid).IsAssignableFrom(type)) {
        result = new GuidConverter();
      } else if (typeof(string).IsAssignableFrom(type)) {
        result = new StringConverter();
      } else if (typeof(CultureInfo).IsAssignableFrom(type)) {
        result = new CultureInfoConverter();
      } else if (typeof(Type).IsAssignableFrom(type)) {
        result = new TypeTypeConverter();
      } else if (typeof(DateTime).IsAssignableFrom(type)) {
        result = new DateTimeConverter2();
      }
      return result;
    }

    internal static TypeConverter GetTypeConverter(Type type) {
      if (type == null) {
        throw new ArgumentNullException("type");
      }
      TypeConverter typeConverter = GetCoreConverterFromCoreType(type);
      if (typeConverter == null) {
        Type converterType = GetConverterType(type);
        typeConverter = ((!(converterType != null)) ? GetCoreConverterFromCustomType(type) : (Activator.CreateInstance(converterType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, InvariantEnglishUS) as TypeConverter));
        if (typeConverter == null) {
          typeConverter = new TypeConverter();
        }
      }
      return typeConverter;
    }
  }
}