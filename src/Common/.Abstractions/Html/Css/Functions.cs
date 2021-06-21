using System.Globalization;

namespace Common.Html.Css {
  public static class Functions {
    // https://www.w3schools.com/cssref/css_functions.asp

    public static string CubicBezier(double x1, double y1, double x2, double y2) => $"cubic-bezier({x1},{y1},{x2},{y2})";
    public static string HSL(int hue, decimal saturation, decimal lightness) => $"hsl({hue}, {saturation}, {lightness})";
    public static string HSLA(int hue, decimal saturation, decimal lightness, decimal alpha) => $"hsla({hue}, {saturation}, {lightness}, {alpha})";
    public static string RGB(int red, int green, int blue) => $"rgb({red}, {green}, {blue})";
    public static string RGBA(int red, int green, int blue, decimal alpha) => $"rgba({red}, {green}, {blue}, {alpha.ToString(CultureInfo.InvariantCulture)})";

  }
}