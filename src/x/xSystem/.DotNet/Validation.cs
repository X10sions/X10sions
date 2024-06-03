using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace System;

public static class Validation {
  //public static T AssertBetween<T>(this T value, T min, T max, bool isMinExclusive = false, bool isMaxExclusive = false, [CallerArgumentExpression("value")] string? argumentName = null) where T : IComparable, IComparable<T> => value.IsBetween(min, max, isMinExclusive, isMaxExclusive) ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static T AssertGreaterThan<T>(this T value, T min, [CallerArgumentExpression("value")] string? argumentName = null) where T : IComparable, IComparable<T> => value.IsGreaterThanOrEqualTo(min) ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static T AssertGreaterThanOrEqualTo<T>(this T value, T min, [CallerArgumentExpression("value")] string? argumentName = null) where T : IComparable, IComparable<T> => value.IsGreaterThanOrEqualTo(min) ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static T AssertLessThan<T>(this T value, T max, [CallerArgumentExpression("value")] string? argumentName = null) where T : IComparable, IComparable<T> => value.IsLessThan(max) ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static T AssertLessThanOrEqualTo<T>(this T value, T max, [CallerArgumentExpression("value")] string? argumentName = null) where T : IComparable, IComparable<T> => value.IsLessThan(max) ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static string AssertMatchesRegex([NotNull] this string? value, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, [CallerArgumentExpression("value")] string? argumentName = null) => Regex.IsMatch(value.AssertNotEmpty(), pattern) ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static int AssertNegative(this int value, [CallerArgumentExpression("value")] string? argumentName = null) => value < 0 ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static int AssertNegative([NotNull] this int? value, [CallerArgumentExpression("value")] string? argumentName = null) => value?.AssertNegative() ?? throw new ArgumentOutOfRangeException(argumentName);
  //public static int AssertPositive(this int value, [CallerArgumentExpression("value")] string? argumentName = null) => value > 0 ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static int AssertPositive([NotNull] this int? value, [CallerArgumentExpression("value")] string? argumentName = null) => value?.AssertPositive() ?? throw new ArgumentOutOfRangeException(argumentName);
  //public static Guid AssertNotEmpty([NotNull] this Guid? value, [CallerArgumentExpression("value")] string? argumentName = null) => (value != null && value.Value != Guid.Empty) ? value.Value : throw new ArgumentOutOfRangeException(argumentName);
  //public static string AssertNotEmpty([NotNull] this string? value, [CallerArgumentExpression("value")] string? argumentName = null) => !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentOutOfRangeException(argumentName);
  //public static string? AssertNullOrNotEmpty(this string? value, [CallerArgumentExpression("value")] string? argumentName = null) => value?.AssertNotEmpty(argumentName);

}