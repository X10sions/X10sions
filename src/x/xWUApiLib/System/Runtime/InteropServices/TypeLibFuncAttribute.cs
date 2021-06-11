namespace System.Runtime.InteropServices {
  [AttributeUsage(AttributeTargets.Method, Inherited = false)]
  [ComVisible(true)]
  public sealed class TypeLibFuncAttribute : Attribute {
    internal TypeLibFuncFlags _val;

    /// <summary>Gets the <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for this method.</summary>
    /// <returns>The <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for this method.</returns>
    public TypeLibFuncFlags Value => _val;

    /// <summary>Initializes a new instance of the TypeLibFuncAttribute class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value.</summary>
    /// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for the attributed method as found in the type library it was imported from. </param>
    public TypeLibFuncAttribute(TypeLibFuncFlags flags) {
      _val = flags;
    }

    /// <summary>Initializes a new instance of the TypeLibFuncAttribute class with the specified <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value.</summary>
    /// <param name="flags">The <see cref="T:System.Runtime.InteropServices.TypeLibFuncFlags" /> value for the attributed method as found in the type library it was imported from. </param>
    public TypeLibFuncAttribute(short flags) {
      _val = (TypeLibFuncFlags)flags;
    }
  }


}