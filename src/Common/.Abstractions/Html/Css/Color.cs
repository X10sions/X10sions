//using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Common.Html.Css {
  //[JsonConverter(typeof(ToStringJsonConverter))]
  public class Color {
    //https://www.w3schools.com/colors/colors_converter.asp

    public Color() { }

    public Color(string color) {
      Value = color;
    }

    public Color(int hue, decimal saturation, decimal lightness, decimal alpha) {
      if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("The alpha value must be a value between 0.0 and 1.0", nameof(alpha));
      Hue = hue;
      Saturation = saturation;
      Lightness = lightness;
      Alpha = alpha;
    }

    public Color(byte red, byte green, byte blue, decimal alpha) {
      if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("The alpha value must be a value between 0.0 and 1.0", nameof(alpha));
      Red = red;
      Green = green;
      Blue = blue;
      Alpha = alpha;
    }

    public string? Value { get; set; }

    public int Hue { get; set; } = 0;
    public decimal Saturation { get; set; } = 0;
    public decimal Lightness { get; set; } = 0;

    public byte Red { get; set; } = 0;
    public byte Green { get; set; } = 0;
    public byte Blue { get; set; } = 0;
    public decimal Alpha { get; set; } = 0;

    public override string ToString() => Alpha > 0 ? RGBA : RGB;

    public string HSL => Functions.HSL(Hue, Saturation, Lightness);
    public string HSLA => Functions.HSLA(Hue, Saturation, Lightness, Alpha);
    public string RGB => Functions.RGB(Red, Green, Blue);
    public string RGBA => Functions.RGBA(Red, Green, Blue, Alpha);

    public static Color FromRgba(byte r, byte g, byte b, decimal a) => new Color(r, g, b, a);

    public static Color FromRgb(byte r, byte g, byte b) => new Color { Red = r, Green = g, Blue = b, Alpha = 1 };

    public static Color FromHexString(string hexString) {
      var color = new Color();
      try {
        hexString = hexString.Remove(0, 1);
        if (hexString.Length == 3) {
          color.Red = byte.Parse(hexString.Substring(0, 1) + hexString.Substring(0, 1), NumberStyles.HexNumber);
          color.Green = byte.Parse(hexString.Substring(1, 1) + hexString.Substring(1, 1), NumberStyles.HexNumber);
          color.Blue = byte.Parse(hexString.Substring(2, 1) + hexString.Substring(2, 1), NumberStyles.HexNumber);
          color.Alpha = 1;
          return color;
        }
        if (hexString.Length == 6) {
          color.Red = byte.Parse(hexString.Substring(0, 2), NumberStyles.HexNumber);
          color.Green = byte.Parse(hexString.Substring(2, 2), NumberStyles.HexNumber);
          color.Blue = byte.Parse(hexString.Substring(4, 2), NumberStyles.HexNumber);
          color.Alpha = 1;
          return color;
        }
      } catch (FormatException) {
        // Could not parse the hex-string.
      } catch (ArgumentOutOfRangeException) {
        // Could not parse the hex-string.
      } catch (ArgumentNullException) {
        // Could not parse the hex-string.
      }
      throw new FormatException("The hex string has an invalid format. It has to contain 3 or 4 numbers encoded in hex.");
    }

    public static Color RandomColor(bool randomAlpha) {
      var rand = new Random();
      return new Color((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256), randomAlpha ? (decimal)rand.NextDouble() : 1);
    }

    public static class Instances {
      public static Color Blue => FromRgba(54, 162, 235, 1);
      public static Color Green => FromRgba(75, 192, 192, 1);
      public static Color Grey => FromRgba(201, 203, 207, 1);
      public static Color Orange => FromRgba(255, 159, 64, 1);
      public static Color Purple => FromRgba(153, 102, 255, 1);
      public static Color Red => FromRgba(255, 99, 132, 1);
      public static Color Yellow => FromRgba(255, 205, 86, 1);
    }

  }
}