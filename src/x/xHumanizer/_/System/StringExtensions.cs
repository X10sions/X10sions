using Humanizer;

namespace System {
  public static class StringExtensions {

    public static string ToCamelCase(this string value) => value.Camelize();
    public static string ToKebabCase(this string value) => value.Kebaberize();
    public static string ToPascalCase(this string value) => value.Pascalize();
    public static string ToSnakeCase(this string value) => value.Underscore();
    public static string ToTitleCase(this string value) => value.Titleize();
    public static string ToTrainCase(this string value) => value.Dasherize();

  }
}