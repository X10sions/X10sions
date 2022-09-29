namespace System;

/// <summary>
/// https://github.com/aspnetcorehero/ThrowR/blob/master/AspNetCoreHero.ThrowR/Throw.cs
/// </summary>
public static class Throw {

  public static T IfNull<T>(T value, string message) => value.ThrowIfNull(message);
  public static T IfNull<T>(T value, string propertyName, string message) => IfNull(value, $"{propertyName} is NULL. {message}");
  public static T? IfNotNull<T>(T value, string message) =>  value.ThrowIfNotNull (message);
  public static string IfNullOrWhiteSpace(string value, string propertyName) {
    IfNull(value, propertyName);
    if (string.IsNullOrEmpty(value)) {
      throw new ArgumentException($"Paramater {propertyName} cannot be empty.");
    }
    return value;
  }

  public static void IfNotEqual<T>(T valueOne, T valueTwo, string property) where T: notnull => (valueOne.Equals(valueTwo)).ThrowIfTrue($"Supplied {property} Values are not equal.");
  public static void IfFalse(bool value, string message) => (value == false).ThrowIfTrue(message);
  public static void IfTrue(bool value, string message) => value.ThrowIfTrue(message);

  public static void IfZero(int value, string property) => (value == 0).ThrowIfTrue($"This Property {property} Cannot be Zero");

}