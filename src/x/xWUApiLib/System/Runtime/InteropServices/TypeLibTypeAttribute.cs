namespace System.Runtime.InteropServices {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
  [ComVisible(true)]
  public sealed class TypeLibTypeAttribute : Attribute {
    internal TypeLibTypeFlags _val;

    /// <summary>Gets the <see cref="T:System.Runtime.InteropServices.TypeLibTypeFlags" /> value for this type.</summary>
    /// <returns>The <see cref="T:System.Runtime.InteropServices.TypeLibTypeFlags" /> value for this type.</returns>
    public TypeLibTypeFlags Value => _val;

    /// <summary>Initializes a new instance of the TypeLibTypeAttribute class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibTypeFlags" /> value.</summary>
    /// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibTypeFlags" /> value for the attributed type as found in the type library it was imported from. </param>
    public TypeLibTypeAttribute(TypeLibTypeFlags flags) {
      _val = flags;
    }

    /// <summary>Initializes a new instance of the TypeLibTypeAttribute class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibTypeFlags" /> value.</summary>
    /// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibTypeFlags" /> value for the attributed type as found in the type library it was imported from. </param>
    public TypeLibTypeAttribute(short flags) {
      _val = (TypeLibTypeFlags)flags;
    }
  }
}