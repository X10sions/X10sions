using Microsoft.Internal;
using Microsoft.Internal.WindowsBase;
using System.ComponentModel;
using System.Windows.Converters;
using System.Windows.Markup;

namespace System.Windows {

  [Serializable]
  [TypeConverter(typeof(Int32RectConverter))]
  [ValueSerializer(typeof(Int32RectValueSerializer))]
  public struct Int32Rect : IFormattable {
    internal int _x;

    internal int _y;

    internal int _width;

    internal int _height;

    private static readonly Int32Rect s_empty = new Int32Rect(0, 0, 0, 0);

    /// <summary>Gets or sets the x-coordinate of the top-left corner of the rectangle.</summary>
    /// <returns>The x-coordinate of the top-left corner of the rectangle. The default value is 0.</returns>
    public int X {
      get {
        return _x;
      }
      set {
        _x = value;
      }
    }

    /// <summary>Gets or sets the y-coordinate of the top-left corner of the rectangle.</summary>
    /// <returns>The y-coordinate of the top-left corner of the rectangle. The default value is 0.</returns>
    public int Y {
      get {
        return _y;
      }
      set {
        _y = value;
      }
    }

    /// <summary>Gets or sets the width of the rectangle.</summary>
    /// <returns>The width of the rectangle. The default value is 0.</returns>
    public int Width {
      get {
        return _width;
      }
      set {
        _width = value;
      }
    }

    /// <summary>Gets or sets the height of the rectangle.</summary>
    /// <returns>The height of the rectangle. The default value is 0.</returns>
    public int Height {
      get {
        return _height;
      }
      set {
        _height = value;
      }
    }

    /// <summary>Gets the empty rectangle, a special value that represents a rectangle with no position or area. </summary>
    /// <returns>An empty rectangle with no position or area.</returns>
    public static Int32Rect Empty => s_empty;

    /// <summary>Gets a value indicating whether the rectangle is empty.</summary>
    /// <returns>true if the rectangle is empty; otherwise, false. The default value is true.</returns>
    public bool IsEmpty {
      get {
        if (_x == 0 && _y == 0 && _width == 0) {
          return _height == 0;
        }
        return false;
      }
    }

    public bool HasArea {
      get {
        if (_width > 0) {
          return _height > 0;
        }
        return false;
      }
    }

    /// <summary>Compares two rectangles for exact equality.</summary>
    /// <returns>true if int32Rect1 and int32Rect2 have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" />; otherwise, false.</returns>
    /// <param name="int32Rect1">The first rectangle to compare.</param>
    /// <param name="int32Rect2">The second rectangle to compare.</param>
    public static bool operator ==(Int32Rect int32Rect1, Int32Rect int32Rect2) {
      if (int32Rect1.X == int32Rect2.X && int32Rect1.Y == int32Rect2.Y && int32Rect1.Width == int32Rect2.Width) {
        return int32Rect1.Height == int32Rect2.Height;
      }
      return false;
    }

    /// <summary>Compares two rectangles for inequality.</summary>
    /// <returns>false if int32Rect1 and int32Rect2 have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" />; otherwise, if all of these values are the same, then true.</returns>
    /// <param name="int32Rect1">The first rectangle to compare.</param>
    /// <param name="int32Rect2">The second rectangle to compare.</param>
    public static bool operator !=(Int32Rect int32Rect1, Int32Rect int32Rect2) {
      return !(int32Rect1 == int32Rect2);
    }

    /// <summary>Determines whether the specified rectangles are equal.</summary>
    /// <returns>true if int32Rect1 and int32Rect2 have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" />; otherwise, false.</returns>
    /// <param name="int32Rect1">The first rectangle to compare.</param>
    /// <param name="int32Rect2">The second rectangle to compare.</param>
    public static bool Equals(Int32Rect int32Rect1, Int32Rect int32Rect2) {
      if (int32Rect1.IsEmpty) {
        return int32Rect2.IsEmpty;
      }
      int num = int32Rect1.X;
      if (num.Equals(int32Rect2.X)) {
        num = int32Rect1.Y;
        if (num.Equals(int32Rect2.Y)) {
          num = int32Rect1.Width;
          if (num.Equals(int32Rect2.Width)) {
            num = int32Rect1.Height;
            return num.Equals(int32Rect2.Height);
          }
        }
      }
      return false;
    }

    /// <summary>Determines whether the specified rectangle is equal to this rectangle.</summary>
    /// <returns>true if o is an <see cref="T:System.Windows.Int32Rect" /> and the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> as this rectangle; otherwise, false.</returns>
    /// <param name="o">The object to compare to the current rectangle.</param>
    public override bool Equals(object o) {
      if (o == null || !(o is Int32Rect)) {
        return false;
      }
      Int32Rect int32Rect = (Int32Rect)o;
      return Equals(this, int32Rect);
    }

    /// <summary>Determines whether the specified rectangle is equal to this rectangle.</summary>
    /// <returns>true if both rectangles have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> as this rectangle; otherwise, false.</returns>
    /// <param name="value">The rectangle to compare to the current rectangle.</param>
    public bool Equals(Int32Rect value) {
      return Equals(this, value);
    }

    /// <summary>Creates a hash code from this rectangle's <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> values.</summary>
    /// <returns>This rectangle's hash code.</returns>
    public override int GetHashCode() {
      if (IsEmpty) {
        return 0;
      }
      int num = X;
      int hashCode = num.GetHashCode();
      num = Y;
      int num2 = hashCode ^ num.GetHashCode();
      num = Width;
      int num3 = num2 ^ num.GetHashCode();
      num = Height;
      return num3 ^ num.GetHashCode();
    }

    /// <summary>Creates an <see cref="T:System.Windows.Int32Rect" /> structure from the specified <see cref="T:System.String" /> representation.</summary>
    /// <returns>The equivalent <see cref="T:System.Windows.Int32Rect" /> structure.</returns>
    /// <param name="source">A string representation of an <see cref="T:System.Windows.Int32Rect" />.</param>
    public static Int32Rect Parse(string source) {
      IFormatProvider invariantEnglishUS = TypeConverterHelper.InvariantEnglishUS;
      TokenizerHelper tokenizerHelper = new TokenizerHelper(source, invariantEnglishUS);
      string text = tokenizerHelper.NextTokenRequired();
      Int32Rect result = (text == "Empty") ? Empty : new Int32Rect(Convert.ToInt32(text, invariantEnglishUS), Convert.ToInt32(tokenizerHelper.NextTokenRequired(), invariantEnglishUS), Convert.ToInt32(tokenizerHelper.NextTokenRequired(), invariantEnglishUS), Convert.ToInt32(tokenizerHelper.NextTokenRequired(), invariantEnglishUS));
      tokenizerHelper.LastTokenRequired();
      return result;
    }

    /// <summary>Creates a string representation of this <see cref="T:System.Windows.Int32Rect" />.</summary>
    /// <returns>A string containing the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> values of this <see cref="T:System.Windows.Int32Rect" /> structure.</returns>
    public override string ToString() {
      return ConvertToString(null, null);
    }

    /// <summary>Creates a string representation of this <see cref="T:System.Windows.Int32Rect" /> based on the supplied <see cref="T:System.IFormatProvider" />.</summary>
    /// <returns>A string representation of this instance of <see cref="T:System.Windows.Int32Rect" />.</returns>
    /// <param name="provider">The format provider to use. If provider is null, the current culture is used.</param>
    public string ToString(IFormatProvider provider) {
      return ConvertToString(null, provider);
    }

    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <returns>The value of the current instance in the specified format.</returns>
    /// <param name="format">The format to use.</param>
    /// <param name="provider">The provider to use to format the value</param>
    string IFormattable.ToString(string format, IFormatProvider provider) {
      return ConvertToString(format, provider);
    }

    internal string ConvertToString(string format, IFormatProvider provider) {
      if (IsEmpty) {
        return "Empty";
      }
      char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
      return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", numericListSeparator, _x, _y, _width, _height);
    }

    /// <summary>Initializes a new instance of an <see cref="T:System.Windows.Int32Rect" /> with the specified <see cref="P:System.Windows.Int32Rect.X" /> and <see cref="P:System.Windows.Int32Rect.Y" /> coordinates and the specified <see cref="P:System.Windows.Int32Rect.Width" /> and <see cref="P:System.Windows.Int32Rect.Height" />. </summary>
    /// <param name="x">The <see cref="P:System.Windows.Int32Rect.X" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the x-coordinate of the top-left corner of the rectangle.</param>
    /// <param name="y">The <see cref="P:System.Windows.Int32Rect.Y" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the y-coordinate of the top-left corner of the rectangle.</param>
    /// <param name="width">The <see cref="P:System.Windows.Int32Rect.Width" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the width of the rectangle.</param>
    /// <param name="height">The <see cref="P:System.Windows.Int32Rect.Height" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the height of the rectangle.</param>
    public Int32Rect(int x, int y, int width, int height) {
      _x = x;
      _y = y;
      _width = width;
      _height = height;
    }

    internal void ValidateForDirtyRect(string paramName, int width, int height) {
      if (_x < 0) {
        throw new ArgumentOutOfRangeException(paramName, SR.Get("ParameterCannotBeNegative"));
      }
      if (_y < 0) {
        throw new ArgumentOutOfRangeException(paramName, SR.Get("ParameterCannotBeNegative"));
      }
      if (_width < 0 || _width > width) {
        throw new ArgumentOutOfRangeException(paramName, SR.Get("ParameterMustBeBetween", 0, width));
      }
      if (_height < 0 || _height > height) {
        throw new ArgumentOutOfRangeException(paramName, SR.Get("ParameterMustBeBetween", 0, height));
      }
    }
  }

}
