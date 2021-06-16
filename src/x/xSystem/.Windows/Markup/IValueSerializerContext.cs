using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Markup {
  /// <summary>Defines a context that is provided to a <see cref="T:System.Windows.Markup.ValueSerializer" />. The context can be used to enable special cases of serialization or different modes of serialization.</summary>
  [TypeForwardedFrom("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  public interface IValueSerializerContext : ITypeDescriptorContext, IServiceProvider {
    /// <summary>Gets the <see cref="T:System.Windows.Markup.ValueSerializer" /> associated with the specified type.</summary>
    /// <returns>A <see cref="T:System.Windows.Markup.ValueSerializer" /> capable of serializing the specified type.</returns>
    /// <param name="type">The type of the value being converted.</param>
    ValueSerializer GetValueSerializerFor(Type type);

    /// <summary>Gets a <see cref="T:System.Windows.Markup.ValueSerializer" /> for the given property descriptor.</summary>
    /// <returns>A <see cref="T:System.Windows.Markup.ValueSerializer" /> capable of serializing the specified property.</returns>
    /// <param name="descriptor">The descriptor of the property being converted.</param>
    ValueSerializer GetValueSerializerFor(PropertyDescriptor descriptor);
  }
}