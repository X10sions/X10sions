using System.Windows.Markup;

namespace System.Windows.Converters {
  public class SizeValueSerializer : ValueSerializer {
    /// <summary>Determines whether the specified <see cref="T:System.String" /> can be converted to an instance of <see cref="T:System.Windows.Size" />.</summary>
    /// <returns>Always returns true.</returns>
    /// <param name="value">String to evaluate for conversion.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    public override bool CanConvertFromString(string value, IValueSerializerContext context) => true;

    /// <summary>Determines whether the specified <see cref="T:System.Windows.Size" /> can be converted to a <see cref="T:System.String" />.</summary>
    /// <returns>true if <paramref name="value" /> can be converted into a <see cref="T:System.String" />; otherwise, false.</returns>
    /// <param name="value">The object to evaluate for conversion.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    public override bool CanConvertToString(object value, IValueSerializerContext context) => value is Size;

    /// <summary>Converts a <see cref="T:System.String" /> into a <see cref="T:System.Windows.Size" />.</summary>
    /// <returns>A new instance of <see cref="T:System.Windows.Size" /> based on the supplied <paramref name="value" />.</returns>
    /// <param name="value">The string to convert.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    public override object ConvertFromString(string value, IValueSerializerContext context) => value != null ? Size.Parse(value) : base.ConvertFromString(value, context);

    /// <summary>Converts an instance of <see cref="T:System.Windows.Size" /> to a <see cref="T:System.String" />.</summary>
    /// <returns>A string representation of the specified <see cref="T:System.Windows.Size" />.</returns>
    /// <param name="value">The object to convert into a string.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    public override string ConvertToString(object value, IValueSerializerContext context) => value is Size
        ? ((Size)value).ConvertToString(null, TypeConverterHelper.InvariantEnglishUS)
        : base.ConvertToString(value, context);
  }

}
