namespace System.Runtime.InteropServices {
  [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum TypeLibTypeFlags {
      /// <summary>A type description that describes an Application object.</summary>
      FAppObject = 0x1,
      /// <summary>Instances of the type can be created by ITypeInfo::CreateInstance.</summary>
      FCanCreate = 0x2,
      /// <summary>The type is licensed.</summary>
      FLicensed = 0x4,
      /// <summary>The type is predefined. The client application should automatically create a single instance of the object that has this attribute. The name of the variable that points to the object is the same as the class name of the object.</summary>
      FPreDeclId = 0x8,
      /// <summary>The type should not be displayed to browsers.</summary>
      FHidden = 0x10,
      /// <summary>The type is a control from which other types will be derived, and should not be displayed to users.</summary>
      FControl = 0x20,
      /// <summary>The interface supplies both IDispatch and V-table binding.</summary>
      FDual = 0x40,
      /// <summary>The interface cannot add members at run time.</summary>
      FNonExtensible = 0x80,
      /// <summary>The types used in the interface are fully compatible with Automation, including vtable binding support.</summary>
      FOleAutomation = 0x100,
      /// <summary>This flag is intended for system-level types or types that type browsers should not display.</summary>
      FRestricted = 0x200,
      /// <summary>The class supports aggregation.</summary>
      FAggregatable = 0x400,
      /// <summary>The object supports IConnectionPointWithDefault, and has default behaviors.</summary>
      FReplaceable = 0x800,
      /// <summary>Indicates that the interface derives from IDispatch, either directly or indirectly.</summary>
      FDispatchable = 0x1000,
      /// <summary>Indicates base interfaces should be checked for name resolution before checking child interfaces. This is the reverse of the default behavior.</summary>
      FReverseBind = 0x2000
    }
}
