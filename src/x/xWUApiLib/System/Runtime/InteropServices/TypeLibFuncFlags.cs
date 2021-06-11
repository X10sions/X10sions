namespace System.Runtime.InteropServices {
  [Serializable]
  [Flags]
  [ComVisible(true)]
  public enum TypeLibFuncFlags {
    /// <summary>This flag is intended for system-level functions or functions that type browsers should not display.</summary>
    FRestricted = 0x1,
    /// <summary>The function returns an object that is a source of events.</summary>
    FSource = 0x2,
    /// <summary>The function that supports data binding.</summary>
    FBindable = 0x4,
    /// <summary>When set, any call to a method that sets the property results first in a call to IPropertyNotifySink::OnRequestEdit.</summary>
    FRequestEdit = 0x8,
    /// <summary>The function that is displayed to the user as bindable. <see cref="F:System.Runtime.InteropServices.TypeLibFuncFlags.FBindable" /> must also be set.</summary>
    FDisplayBind = 0x10,
    /// <summary>The function that best represents the object. Only one function in a type information can have this attribute.</summary>
    FDefaultBind = 0x20,
    /// <summary>The function should not be displayed to the user, although it exists and is bindable.</summary>
    FHidden = 0x40,
    /// <summary>The function supports GetLastError.</summary>
    FUsesGetLastError = 0x80,
    /// <summary>Permits an optimization in which the compiler looks for a member named "xyz" on the type "abc". If such a member is found and is flagged as an accessor function for an element of the default collection, then a call is generated to that member function.</summary>
    FDefaultCollelem = 0x100,
    /// <summary>The type information member is the default member for display in the user interface.</summary>
    FUiDefault = 0x200,
    /// <summary>The property appears in an object browser, but not in a properties browser.</summary>
    FNonBrowsable = 0x400,
    /// <summary>Tags the interface as having default behaviors.</summary>
    FReplaceable = 0x800,
    /// <summary>The function is mapped as individual bindable properties.</summary>
    FImmediateBind = 0x1000
  }
}