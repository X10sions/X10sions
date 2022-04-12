using System.Diagnostics;
using System.Globalization;

namespace System {

  [DebuggerStepThrough]
  public static class Check {

    public static string ArgumentPropertyNull(string property, string argument) => string.Format(CultureInfo.CurrentCulture, "The property '{0}' of the argument '{1}' cannot be null.", property, argument);
    public static string ArgumentIsEmpty(string argumentName) => string.Format(CultureInfo.CurrentCulture, "The string argument '{0}' cannot be empty.", argumentName);
    public static string InvalidEntityType(Type type, string argumentName) => string.Format(CultureInfo.CurrentCulture, "The entity type '{0}' provided for the argument '{1}' must be a reference type.", type, argumentName);
    public static string InvalidEnumValue(string argumentName, object enumType) => string.Format(CultureInfo.CurrentCulture, "The value provided for argument '{0}' must be a valid value of enum type '{1}'.", argumentName, enumType);
    public static string CollectionArgumentIsEmpty(string argumentName) => string.Format(CultureInfo.CurrentCulture, "The collection argument '{0}' must contain at least one element.", argumentName);

    public static T Condition<T>(T value, Predicate<T> condition, string parameterName) {
      NotNull(condition, nameof(condition));
      NotNull(value, nameof(value));
      if (!condition(value)) {
        NotEmpty(parameterName, nameof(parameterName));
        throw new ArgumentOutOfRangeException(parameterName);
      }
      return value;
    }

    //[ContractAnnotation("value:null => halt")]
    public static T NotNull<T>(T value, string parameterName) {
      if (ReferenceEquals(value, null)) {
        //Expression<Func<object>> expression = () => value;
        //parameterName = (expression.Body as MemberExpression).Member.Name;
        //Console.WriteLine(parameterName);
        NotEmpty(parameterName, nameof(parameterName));
        throw new ArgumentNullException(parameterName);
      }
      return value;
    }

    //[ContractAnnotation("value:null => halt")]
    public static T NotNull<T>(T value, string parameterName, string propertyName) {
      if (ReferenceEquals(value, null)) {
        NotEmpty(parameterName, nameof(parameterName));
        NotEmpty(propertyName, nameof(propertyName));
        throw new ArgumentException(ArgumentPropertyNull(propertyName, parameterName));
      }
      return value;
    }

    // [ContractAnnotation("value:null => halt")]
    public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, string parameterName) {
      NotNull(value, parameterName);
      if (value.Count == 0) {
        NotEmpty(parameterName, nameof(parameterName));
        throw new ArgumentException(CollectionArgumentIsEmpty(parameterName));
      }
      return value;
    }

    // [ContractAnnotation("value:null => halt")]
    public static string NotEmpty(string? value, string parameterName) {
      if (value is null) {
        throw new ArgumentNullException(parameterName);
      } else if (value.Trim().Length == 0) {
        throw new ArgumentException(ArgumentIsEmpty(parameterName));
      }
      NotEmpty(parameterName, nameof(parameterName));
      return value;
    }

    public static IReadOnlyList<T> HasNoNulls<T>(IReadOnlyList<T> value, string parameterName) where T : class {
      NotNull(value, parameterName);
      if (value.Any(e => e == null)) {
        NotEmpty(parameterName, nameof(parameterName));
        throw new ArgumentException(parameterName);
      }
      return value;
    }

    public static T IsDefined<T>(T value, string parameterName) where T : struct {
      if (!Enum.IsDefined(typeof(T), value)) {
        NotEmpty(parameterName, nameof(parameterName));
        throw new ArgumentException(InvalidEnumValue(parameterName, typeof(T)));
      }
      return value;
    }

    public static Type ValidEntityType(Type value, string parameterName) {
      if (!value.IsClass) {
        NotEmpty(parameterName, nameof(parameterName));
        throw new ArgumentException(InvalidEntityType(value, parameterName));
      }
      return value;
    }

  }
}