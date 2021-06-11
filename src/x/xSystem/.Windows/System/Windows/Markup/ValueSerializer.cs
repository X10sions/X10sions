using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Markup {
  /// <summary>Abstract class that defines conversion behavior for serialization from an object representation.</summary>
  [TypeForwardedFrom("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  public abstract class ValueSerializer {
    private static List<Type> Empty;

    private static object _valueSerializersLock;

    private static Hashtable _valueSerializers;

    /// <summary>When overridden in a derived class, determines whether the specified object can be converted into a <see cref="T:System.String" />.</summary>
    /// <returns>true if the <paramref name="value" /> can be converted into a <see cref="T:System.String" />; otherwise, false.</returns>
    /// <param name="value">The object to evaluate for conversion.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    public virtual bool CanConvertToString(object value, IValueSerializerContext context) {
      return false;
    }

    /// <summary>When overridden in a derived class, determines whether the specified <see cref="T:System.String" /> can be converted to an instance of the type that the implementation of <see cref="T:System.Windows.Markup.ValueSerializer" /> supports.</summary>
    /// <returns>true if the value can be converted; otherwise, false.</returns>
    /// <param name="value">The string to evaluate for conversion.</param>
    /// <param name="context">Context information that is used for conversion. </param>
    public virtual bool CanConvertFromString(string value, IValueSerializerContext context) {
      return false;
    }

    /// <summary>When overridden in a derived class, converts the specified object to a <see cref="T:System.String" />.</summary>
    /// <returns>A string representation of the specified object.</returns>
    /// <param name="value">The object to convert into a string.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    /// <exception cref="T:System.NotSupportedException">
    ///   <paramref name="value" /> cannot be converted.</exception>
    public virtual string ConvertToString(object value, IValueSerializerContext context) {
      throw GetConvertToException(value, typeof(string));
    }

    /// <summary>When overridden in a derived class, converts a <see cref="T:System.String" /> to an instance of the type that the implementation of <see cref="T:System.Windows.Markup.ValueSerializer" /> supports.</summary>
    /// <returns>A new instance of the type that the implementation of <see cref="T:System.Windows.Markup.ValueSerializer" /> supports based on the supplied <paramref name="value" />.</returns>
    /// <param name="value">The string to convert.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    /// <exception cref="T:System.NotSupportedException">
    ///   <paramref name="value" /> cannot be converted.</exception>
    public virtual object ConvertFromString(string value, IValueSerializerContext context) {
      throw GetConvertFromException(value);
    }

    /// <summary>Gets an enumeration of the types referenced by the <see cref="T:System.Windows.Markup.ValueSerializer" />.</summary>
    /// <returns>The types converted by this serializer.</returns>
    /// <param name="value">The value being serialized.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    public virtual IEnumerable<Type> TypeReferences(object value, IValueSerializerContext context) {
      return Empty;
    }

    /// <summary>Gets the <see cref="T:System.Windows.Markup.ValueSerializer" /> declared for the specified type.</summary>
    /// <returns>The serializer associated with the specified type. May return null.</returns>
    /// <param name="type">The type to get the <see cref="T:System.Windows.Markup.ValueSerializer" /> for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="type" /> is null.</exception>
    public static ValueSerializer GetSerializerFor(Type type) {
      if (type == null) {
        throw new ArgumentNullException("type");
      }
      object obj = _valueSerializers[type];
      if (obj != null) {
        if (obj != _valueSerializersLock) {
          return obj as ValueSerializer;
        }
        return null;
      }
      AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
      ValueSerializerAttribute valueSerializerAttribute = attributes[typeof(ValueSerializerAttribute)] as ValueSerializerAttribute;
      ValueSerializer valueSerializer = null;
      if (valueSerializerAttribute != null) {
        valueSerializer = (ValueSerializer)Activator.CreateInstance(valueSerializerAttribute.ValueSerializerType);
      }
      if (valueSerializer == null) {
        if (type == typeof(string)) {
          valueSerializer = new StringValueSerializer();
        } else {
          TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(type);
          if (typeConverter.GetType() == typeof(DateTimeConverter2)) {
            valueSerializer = new DateTimeValueSerializer();
          } else if (typeConverter.CanConvertTo(typeof(string)) && typeConverter.CanConvertFrom(typeof(string)) && !(typeConverter is ReferenceConverter)) {
            valueSerializer = new TypeConverterValueSerializer(typeConverter);
          }
        }
      }
      lock (_valueSerializersLock) {
        _valueSerializers[type] = ((valueSerializer == null) ? _valueSerializersLock : valueSerializer);
        return valueSerializer;
      }
    }

    /// <summary>Gets the <see cref="T:System.Windows.Markup.ValueSerializer" /> declared for a property, by passing a CLR property descriptor for the property.</summary>
    /// <returns>The serializer associated with the specified property. May return null.</returns>
    /// <param name="descriptor">The CLR property descriptor for the property to be serialized.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="descriptor" /> is null.</exception>
    public static ValueSerializer GetSerializerFor(PropertyDescriptor descriptor) {
      if (descriptor == null) {
        throw new ArgumentNullException("descriptor");
      }
      ValueSerializerAttribute valueSerializerAttribute = descriptor.Attributes[typeof(ValueSerializerAttribute)] as ValueSerializerAttribute;
      ValueSerializer valueSerializer;
      if (valueSerializerAttribute != null) {
        valueSerializer = (ValueSerializer)Activator.CreateInstance(valueSerializerAttribute.ValueSerializerType);
      } else {
        valueSerializer = GetSerializerFor(descriptor.PropertyType);
        if (valueSerializer == null || valueSerializer is TypeConverterValueSerializer) {
          TypeConverter converter = descriptor.Converter;
          if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)) && !(converter is ReferenceConverter)) {
            valueSerializer = new TypeConverterValueSerializer(converter);
          }
        }
      }
      return valueSerializer;
    }

    /// <summary>Gets the <see cref="T:System.Windows.Markup.ValueSerializer" /> declared for the specified type, using the specified context.</summary>
    /// <returns>The serializer associated with the specified type.</returns>
    /// <param name="type">The type to get the <see cref="T:System.Windows.Markup.ValueSerializer" /> for.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="type" /> is null.</exception>
    public static ValueSerializer GetSerializerFor(Type type, IValueSerializerContext context) {
      if (context != null) {
        ValueSerializer valueSerializerFor = context.GetValueSerializerFor(type);
        if (valueSerializerFor != null) {
          return valueSerializerFor;
        }
      }
      return GetSerializerFor(type);
    }

    /// <summary>Gets the <see cref="T:System.Windows.Markup.ValueSerializer" /> declared for the specified property, using the specified context.</summary>
    /// <returns>The serializer associated with the specified property.</returns>
    /// <param name="descriptor">Descriptor for the property to be serialized.</param>
    /// <param name="context">Context information that is used for conversion.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="descriptor" /> is null.</exception>
    public static ValueSerializer GetSerializerFor(PropertyDescriptor descriptor, IValueSerializerContext context) {
      if (context != null) {
        ValueSerializer valueSerializerFor = context.GetValueSerializerFor(descriptor);
        if (valueSerializerFor != null) {
          return valueSerializerFor;
        }
      }
      return GetSerializerFor(descriptor);
    }

    /// <summary>Returns an exception to throw when a conversion cannot be performed.</summary>
    /// <returns>An <see cref="T:System.Exception" /> object for the exception to throw when a ConvertTo conversion cannot be performed.</returns>
    /// <param name="value">The object that could not be converted.</param>
    /// <param name="destinationType">A type that represents the type the conversion was trying to convert to.</param>
    protected Exception GetConvertToException(object value, Type destinationType) {
      string text = (value != null) ? value.GetType().FullName : SR.Get("ToStringNull");
      return new NotSupportedException(SR.Get("ConvertToException", GetType().Name, text, destinationType.FullName));
    }

    /// <summary>Returns an exception to throw when a conversion cannot be performed.</summary>
    /// <returns>An <see cref="T:System.Exception" /> object for the exception to throw when a ConvertFrom conversion cannot be performed.</returns>
    /// <param name="value">The object that could not be converted.</param>
    protected Exception GetConvertFromException(object value) {
      string text = (value != null) ? value.GetType().FullName : SR.Get("ToStringNull");
      return new NotSupportedException(SR.Get("ConvertFromException", GetType().Name, text));
    }

    private static void TypeDescriptorRefreshed(RefreshEventArgs args) {
      _valueSerializers = new Hashtable();
    }

    static ValueSerializer() {
      Empty = new List<Type>();
      _valueSerializersLock = new object();
      _valueSerializers = new Hashtable();
      TypeDescriptor.Refreshed += TypeDescriptorRefreshed;
    }
  }
}