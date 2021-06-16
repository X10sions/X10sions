using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup {
  internal class TypeTypeConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
      return sourceType == typeof(string);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
      string text = value as string;
      if (context != null && text != null) {
        IXamlTypeResolver xamlTypeResolver = (IXamlTypeResolver)context.GetService(typeof(IXamlTypeResolver));
        if (xamlTypeResolver != null) {
          return xamlTypeResolver.Resolve(text);
        }
      }
      return base.ConvertFrom(context, culture, value);
    }
  }
}