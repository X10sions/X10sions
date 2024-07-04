using Humanizer;

namespace System {
  public static class StringExtensions {

    public static string ToTitleCase(this string value) => value.Titleize();
    public static string ToTrainCase(this string value) => value.Dasherize();

  }
}