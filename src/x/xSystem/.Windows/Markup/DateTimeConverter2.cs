using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup {
  internal class DateTimeConverter2 : TypeConverter {
    private DateTimeValueSerializer _dateTimeValueSerializer = new DateTimeValueSerializer();
    private IValueSerializerContext _valueSerializerContext = new DateTimeValueSerializerContext();

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
      if (sourceType == typeof(string)) {
        return true;
      }
      return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
      if (destinationType == typeof(string)) {
        return true;
      }
      return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
      return _dateTimeValueSerializer.ConvertFromString(value as string, _valueSerializerContext);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
      if (destinationType != null && value is DateTime) {
        _dateTimeValueSerializer.ConvertToString(value as string, _valueSerializerContext);
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}