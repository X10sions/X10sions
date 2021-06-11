using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Converters {
  public sealed class Int32RectConverter : TypeConverter {
    /// <summary>Determines whether an object can be converted from a given type to an instance of an <see cref="T:System.Windows.Int32Rect" />.  </summary>
    /// <returns>true if the type can be converted to an <see cref="T:System.Windows.Int32Rect" />; otherwise, false.</returns>
    /// <param name="context">Describes the context information of a type.</param>
    /// <param name="sourceType">The type of the source that is being evaluated for conversion.</param>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
      if (sourceType == typeof(string)) {
        return true;
      }
      return base.CanConvertFrom(context, sourceType);
    }

    /// <summary>Determines whether an instance of an <see cref="T:System.Windows.Int32Rect" /> can be converted to a different type.</summary>
    /// <returns>true if this <see cref="T:System.Windows.Int32Rect" /> can be converted to <paramref name="destinationType" />; otherwise, false.</returns>
    /// <param name="context">Describes the context information of a type.</param>
    /// <param name="destinationType">The desired type this <see cref="T:System.Windows.Int32Rect" /> is being evaluated for conversion.</param>
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
      if (destinationType == typeof(string)) {
        return true;
      }
      return base.CanConvertTo(context, destinationType);
    }

    /// <summary>Attempts to convert the specified type to an <see cref="T:System.Windows.Int32Rect" />.</summary>
    /// <returns>The <see cref="T:System.Windows.Int32Rect" /> created from converting <paramref name="value" />.</returns>
    /// <param name="context">Provides contextual information required for conversion.</param>
    /// <param name="culture">Cultural information to respect during conversion.</param>
    /// <param name="value">The object being converted.</param>
    /// <exception cref="T:System.NotSupportedException">Thrown if the specified object is NULL or is a type that cannot be converted to an <see cref="T:System.Windows.Int32Rect" />.</exception>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
      if (value == null) {
        throw GetConvertFromException(value);
      }
      string text = value as string;
      if (text != null) {
        return Int32Rect.Parse(text);
      }
      return base.ConvertFrom(context, culture, value);
    }

    /// <summary>Attempts to convert an <see cref="T:System.Windows.Int32Rect" /> to a specified type.</summary>
    /// <returns>The object created from converting this <see cref="T:System.Windows.Int32Rect" />.</returns>
    /// <param name="context">Provides contextual information required for conversion.</param>
    /// <param name="culture">Cultural information to respect during conversion.</param>
    /// <param name="value">The <see cref="T:System.Windows.Int32Rect" /> to convert.</param>
    /// <param name="destinationType">The type to convert this <see cref="T:System.Windows.Int32Rect" /> to.</param>
    /// <exception cref="T:System.NotSupportedException">Thrown if <paramref name="value" /> is null or is not an <see cref="T:System.Windows.Int32Rect" />, or if the <paramref name="destinationType" /> is not one of the valid types for conversion.</exception>
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
      if (destinationType != null && value is Int32Rect) {
        Int32Rect int32Rect = (Int32Rect)value;
        if (destinationType == typeof(string)) {
          return int32Rect.ConvertToString(null, culture);
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }

}
