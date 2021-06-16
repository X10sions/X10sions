using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows {
  /// <summary>Represents a property that can be set through methods such as, styling, data binding, animation, and inheritance.</summary>
  [TypeConverter("System.Windows.Markup.DependencyPropertyConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
  [ValueSerializer(typeof(DependencyPropertyValueSerializer))]
  public sealed class DependencyProperty {

    private class FromNameKey {
      private string _name;
      private Type _ownerType;
      private int _hashCode;

      public FromNameKey(string name, Type ownerType) {
        _name = name;
        _ownerType = ownerType;
        _hashCode = (_name.GetHashCode() ^ _ownerType.GetHashCode());
      }

      public void UpdateNameKey(Type ownerType) {
        _ownerType = ownerType;
        _hashCode = (_name.GetHashCode() ^ _ownerType.GetHashCode());
      }

      public override int GetHashCode() {
        return _hashCode;
      }

      public override bool Equals(object o) {
        if (o != null && o is FromNameKey) {
          return Equals((FromNameKey)o);
        }
        return false;
      }

      public bool Equals(FromNameKey key) {
        if (_name.Equals(key._name)) {
          return _ownerType == key._ownerType;
        }
        return false;
      }
    }

    [Flags]
    private enum Flags {
      GlobalIndexMask = 0xFFFF,
      IsValueType = 0x10000,
      IsFreezableType = 0x20000,
      IsStringType = 0x40000,
      IsPotentiallyInherited = 0x80000,
      IsDefaultValueChanged = 0x100000,
      IsPotentiallyUsingDefaultValueFactory = 0x200000,
      IsObjectType = 0x400000
    }

    /// <summary>Specifies a static value that is used by the WPF property system rather than null to indicate that the property exists, but does not have its value set by the property system.</summary>
    /// <returns>An unset value. This is effectively the result of a call to the <see cref="T:System.Object" /> constructor.</returns>
    public static readonly object UnsetValue = new NamedObject("DependencyProperty.UnsetValue");

    private string _name;

    private Type _propertyType;

    private Type _ownerType;

    private PropertyMetadata _defaultMetadata;

    private ValidateValueCallback _validateValueCallback;

    private DependencyPropertyKey _readOnlyKey;

    private Flags _packedData;

    internal InsertionSortMap _metadataMap;

    private CoerceValueCallback _designerCoerceValueCallback;

    internal static ItemStructList<DependencyProperty> RegisteredPropertyList = new ItemStructList<DependencyProperty>(768);

    private static Hashtable PropertyFromName = new Hashtable();

    private static int GlobalIndexCount;

    internal static object Synchronized = new object();

    private static Type NullableType = typeof(Nullable<>);

    /// <summary>Gets the name of the dependency property. </summary>
    /// <returns>The name of the property.</returns>
    public string Name => _name;

    /// <summary>Gets the type that the dependency property uses for its value. </summary>
    /// <returns>The <see cref="T:System.Type" /> of the property value.</returns>
    public Type PropertyType => _propertyType;

    /// <summary>Gets the type of the object that registered the dependency property with the property system, or added itself as owner of the property. </summary>
    /// <returns>The type of the object that registered the property or added itself as owner of the property.</returns>
    public Type OwnerType => _ownerType;

    /// <summary>Gets the default metadata of the dependency property. </summary>
    /// <returns>The default metadata of the dependency property.</returns>
    public PropertyMetadata DefaultMetadata => _defaultMetadata;

    /// <summary>Gets the value validation callback for the dependency property. </summary>
    /// <returns>The value validation callback for this dependency property, as provided for the <paramref name="validateValueCallback" /> parameter in the original dependency property registration.</returns>
    public ValidateValueCallback ValidateValueCallback => _validateValueCallback;

    /// <summary>Gets an internally generated value that uniquely identifies the dependency property.</summary>
    /// <returns>A unique numeric identifier.</returns>
    public int GlobalIndex => (int)(_packedData & Flags.GlobalIndexMask);

    internal bool IsObjectType => (_packedData & Flags.IsObjectType) != 0;

    internal bool IsValueType => (_packedData & Flags.IsValueType) != 0;

    internal bool IsFreezableType => (_packedData & Flags.IsFreezableType) != 0;

    internal bool IsStringType => (_packedData & Flags.IsStringType) != 0;

    internal bool IsPotentiallyInherited => (_packedData & Flags.IsPotentiallyInherited) != 0;

    internal bool IsDefaultValueChanged => (_packedData & Flags.IsDefaultValueChanged) != 0;

    internal bool IsPotentiallyUsingDefaultValueFactory => (_packedData & Flags.IsPotentiallyUsingDefaultValueFactory) != 0;

    /// <summary>Gets a value that indicates whether the dependency property identified by this <see cref="T:System.Windows.DependencyProperty" /> instance is a read-only dependency property.</summary>
    /// <returns>true if the dependency property is read-only; otherwise, false.</returns>
    public bool ReadOnly => _readOnlyKey != null;

    internal DependencyPropertyKey DependencyPropertyKey => _readOnlyKey;

    internal CoerceValueCallback DesignerCoerceValueCallback {
      get {
        return _designerCoerceValueCallback;
      }
      set {
        if (ReadOnly) {
          throw new InvalidOperationException(SR.Get("ReadOnlyDesignerCoersionNotAllowed", Name));
        }
        _designerCoerceValueCallback = value;
      }
    }

    internal static int RegisteredPropertyCount => RegisteredPropertyList.Count;

    internal static IEnumerable RegisteredProperties {
      get {
        DependencyProperty[] list = RegisteredPropertyList.List;
        foreach (DependencyProperty dependencyProperty in list) {
          if (dependencyProperty != null) {
            yield return dependencyProperty;
          }
        }
      }
    }

    /// <summary>Registers a dependency property with the specified property name, property type, and owner type. </summary>
    /// <returns>A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
    /// <param name="name">The name of the dependency property to register. The name must be unique within the registration namespace of the owner type.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    public static DependencyProperty Register(string name, Type propertyType, Type ownerType) {
      return Register(name, propertyType, ownerType, null, null);
    }

    /// <summary>Registers a dependency property with the specified property name, property type, owner type, and property metadata. </summary>
    /// <returns>A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="typeMetadata">Property metadata for the dependency property.</param>
    public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata) {
      return Register(name, propertyType, ownerType, typeMetadata, null);
    }

    /// <summary>Registers a dependency property with the specified property name, property type, owner type, property metadata, and a value validation callback for the property. </summary>
    /// <returns>A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="typeMetadata">Property metadata for the dependency property.</param>
    /// <param name="validateValueCallback">A reference to a callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
    public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback) {
      RegisterParameterValidation(name, propertyType, ownerType);
      PropertyMetadata defaultMetadata = null;
      if (typeMetadata != null && typeMetadata.DefaultValueWasSet()) {
        defaultMetadata = new PropertyMetadata(typeMetadata.DefaultValue);
      }
      DependencyProperty dependencyProperty = RegisterCommon(name, propertyType, ownerType, defaultMetadata, validateValueCallback);
      if (typeMetadata != null) {
        dependencyProperty.OverrideMetadata(ownerType, typeMetadata);
      }
      return dependencyProperty;
    }

    /// <summary> Registers a read-only dependency property, with the specified property type, owner type, and property metadata. </summary>
    /// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="typeMetadata">Property metadata for the dependency property.</param>
    public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata) {
      return RegisterReadOnly(name, propertyType, ownerType, typeMetadata, null);
    }

    /// <summary>Registers a read-only dependency property, with the specified property type, owner type, property metadata, and a validation callback. </summary>
    /// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property later.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="typeMetadata">Property metadata for the dependency property.</param>
    /// <param name="validateValueCallback">A reference to a user-created callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
    public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback) {
      RegisterParameterValidation(name, propertyType, ownerType);
      PropertyMetadata propertyMetadata = null;
      propertyMetadata = ((typeMetadata == null || !typeMetadata.DefaultValueWasSet()) ? AutoGeneratePropertyMetadata(propertyType, validateValueCallback, name, ownerType) : new PropertyMetadata(typeMetadata.DefaultValue));
      DependencyPropertyKey dependencyPropertyKey = new DependencyPropertyKey(null);
      DependencyProperty dependencyProperty = RegisterCommon(name, propertyType, ownerType, propertyMetadata, validateValueCallback);
      dependencyProperty._readOnlyKey = dependencyPropertyKey;
      dependencyPropertyKey.SetDependencyProperty(dependencyProperty);
      if (typeMetadata == null) {
        typeMetadata = AutoGeneratePropertyMetadata(propertyType, validateValueCallback, name, ownerType);
      }
      dependencyProperty.OverrideMetadata(ownerType, typeMetadata, dependencyPropertyKey);
      return dependencyPropertyKey;
    }

    /// <summary>Registers a read-only attached property, with the specified property type, owner type, and property metadata. </summary>
    /// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property later.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="defaultMetadata">Property metadata for the dependency property.</param>
    public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata) {
      return RegisterAttachedReadOnly(name, propertyType, ownerType, defaultMetadata, null);
    }

    /// <summary>Registers a read-only attached property, with the specified property type, owner type, property metadata, and a validation callback. </summary>
    /// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="defaultMetadata">Property metadata for the dependency property.</param>
    /// <param name="validateValueCallback">A reference to a user-created callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
    public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata, ValidateValueCallback validateValueCallback) {
      RegisterParameterValidation(name, propertyType, ownerType);
      if (defaultMetadata == null) {
        defaultMetadata = AutoGeneratePropertyMetadata(propertyType, validateValueCallback, name, ownerType);
      }
      DependencyPropertyKey dependencyPropertyKey = new DependencyPropertyKey(null);
      DependencyProperty dependencyProperty = RegisterCommon(name, propertyType, ownerType, defaultMetadata, validateValueCallback);
      dependencyProperty._readOnlyKey = dependencyPropertyKey;
      dependencyPropertyKey.SetDependencyProperty(dependencyProperty);
      return dependencyPropertyKey;
    }

    /// <summary>Registers an attached property with the specified property name, property type, and owner type. </summary>
    /// <returns>A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType) {
      return RegisterAttached(name, propertyType, ownerType, null, null);
    }

    /// <summary>Registers an attached property with the specified property name, property type, owner type, and property metadata. </summary>
    /// <returns>A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="defaultMetadata">Property metadata for the dependency property. This can include the default value as well as other characteristics.</param>
    public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata) {
      return RegisterAttached(name, propertyType, ownerType, defaultMetadata, null);
    }

    /// <summary>Registers an attached property with the specified property type, owner type, property metadata, and value validation callback for the property. </summary>
    /// <returns>A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
    /// <param name="name">The name of the dependency property to register.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="ownerType">The owner type that is registering the dependency property.</param>
    /// <param name="defaultMetadata">Property metadata for the dependency property. This can include the default value as well as other characteristics.</param>
    /// <param name="validateValueCallback">A reference to a callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
    public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata, ValidateValueCallback validateValueCallback) {
      RegisterParameterValidation(name, propertyType, ownerType);
      return RegisterCommon(name, propertyType, ownerType, defaultMetadata, validateValueCallback);
    }

    private static void RegisterParameterValidation(string name, Type propertyType, Type ownerType) {
      if (name == null) {
        throw new ArgumentNullException("name");
      }
      if (name.Length == 0) {
        throw new ArgumentException(SR.Get("StringEmpty"), "name");
      }
      if (ownerType == null) {
        throw new ArgumentNullException("ownerType");
      }
      if (propertyType == null) {
        throw new ArgumentNullException("propertyType");
      }
    }

    private static DependencyProperty RegisterCommon(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata, ValidateValueCallback validateValueCallback) {
      FromNameKey key = new FromNameKey(name, ownerType);
      lock (Synchronized) {
        if (PropertyFromName.Contains(key)) {
          throw new ArgumentException(SR.Get("PropertyAlreadyRegistered", name, ownerType.Name));
        }
      }
      if (defaultMetadata == null) {
        defaultMetadata = AutoGeneratePropertyMetadata(propertyType, validateValueCallback, name, ownerType);
      } else {
        if (!defaultMetadata.DefaultValueWasSet()) {
          defaultMetadata.DefaultValue = AutoGenerateDefaultValue(propertyType);
        }
        ValidateMetadataDefaultValue(defaultMetadata, propertyType, name, validateValueCallback);
      }
      DependencyProperty dependencyProperty = new DependencyProperty(name, propertyType, ownerType, defaultMetadata, validateValueCallback);
      defaultMetadata.Seal(dependencyProperty, null);
      if (defaultMetadata.IsInherited) {
        dependencyProperty._packedData |= Flags.IsPotentiallyInherited;
      }
      if (defaultMetadata.UsingDefaultValueFactory) {
        dependencyProperty._packedData |= Flags.IsPotentiallyUsingDefaultValueFactory;
      }
      lock (Synchronized) {
        PropertyFromName[key] = dependencyProperty;
      }
      if (TraceDependencyProperty.IsEnabled) {
        TraceDependencyProperty.TraceActivityItem(TraceDependencyProperty.Register, dependencyProperty, dependencyProperty.OwnerType);
      }
      return dependencyProperty;
    }

    private static object AutoGenerateDefaultValue(Type propertyType) {
      object result = null;
      if (propertyType.IsValueType) {
        result = Activator.CreateInstance(propertyType);
      }
      return result;
    }

    private static PropertyMetadata AutoGeneratePropertyMetadata(Type propertyType, ValidateValueCallback validateValueCallback, string name, Type ownerType) {
      object obj = AutoGenerateDefaultValue(propertyType);
      if (validateValueCallback != null && !validateValueCallback(obj)) {
        throw new ArgumentException(SR.Get("DefaultValueAutoAssignFailed", name, ownerType.Name));
      }
      return new PropertyMetadata(obj);
    }

    private static void ValidateMetadataDefaultValue(PropertyMetadata defaultMetadata, Type propertyType, string propertyName, ValidateValueCallback validateValueCallback) {
      if (!defaultMetadata.UsingDefaultValueFactory) {
        ValidateDefaultValueCommon(defaultMetadata.DefaultValue, propertyType, propertyName, validateValueCallback, true);
      }
    }

    internal void ValidateFactoryDefaultValue(object defaultValue) {
      ValidateDefaultValueCommon(defaultValue, PropertyType, Name, ValidateValueCallback, false);
    }

    private static void ValidateDefaultValueCommon(object defaultValue, Type propertyType, string propertyName, ValidateValueCallback validateValueCallback, bool checkThreadAffinity) {
      if (!IsValidType(defaultValue, propertyType)) {
        throw new ArgumentException(SR.Get("DefaultValuePropertyTypeMismatch", propertyName));
      }
      if (defaultValue is Expression) {
        throw new ArgumentException(SR.Get("DefaultValueMayNotBeExpression"));
      }
      if (checkThreadAffinity) {
        DispatcherObject dispatcherObject = defaultValue as DispatcherObject;
        if (dispatcherObject != null && dispatcherObject.Dispatcher != null) {
          ISealable sealable = dispatcherObject as ISealable;
          if (sealable == null || !sealable.CanSeal) {
            throw new ArgumentException(SR.Get("DefaultValueMustBeFreeThreaded", propertyName));
          }
          Invariant.Assert(!sealable.IsSealed, "A Sealed ISealable must not have dispatcher affinity");
          sealable.Seal();
          Invariant.Assert(dispatcherObject.Dispatcher == null, "ISealable.Seal() failed after ISealable.CanSeal returned true");
        }
      }
      if (validateValueCallback != null && !validateValueCallback(defaultValue)) {
        throw new ArgumentException(SR.Get("DefaultValueInvalid", propertyName));
      }
    }

    private void SetupOverrideMetadata(Type forType, PropertyMetadata typeMetadata, out DependencyObjectType dType, out PropertyMetadata baseMetadata) {
      if (forType == null) {
        throw new ArgumentNullException("forType");
      }
      if (typeMetadata == null) {
        throw new ArgumentNullException("typeMetadata");
      }
      if (typeMetadata.Sealed) {
        throw new ArgumentException(SR.Get("TypeMetadataAlreadyInUse"));
      }
      if (!typeof(DependencyObject).IsAssignableFrom(forType)) {
        throw new ArgumentException(SR.Get("TypeMustBeDependencyObjectDerived", forType.Name));
      }
      if (typeMetadata.IsDefaultValueModified) {
        ValidateMetadataDefaultValue(typeMetadata, PropertyType, Name, ValidateValueCallback);
      }
      dType = DependencyObjectType.FromSystemType(forType);
      baseMetadata = GetMetadata(dType.BaseType);
      if (!baseMetadata.GetType().IsAssignableFrom(typeMetadata.GetType())) {
        throw new ArgumentException(SR.Get("OverridingMetadataDoesNotMatchBaseMetadataType"));
      }
    }

    /// <summary>Specifies alternate metadata for this dependency property when it is present on instances of a specified type, overriding the metadata that existed for the dependency property as it was inherited from base types.</summary>
    /// <param name="forType">The type where this dependency property is inherited and where the provided alternate metadata will be applied.</param>
    /// <param name="typeMetadata">The metadata to apply to the dependency property on the overriding type.</param>
    /// <exception cref="T:System.InvalidOperationException">An attempt was made to override metadata on a read-only dependency property (that operation cannot be done using this signature).</exception>
    /// <exception cref="T:System.ArgumentException">Metadata was already established for the dependency property as it exists on the provided type.</exception>
    public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata) {
      SetupOverrideMetadata(forType, typeMetadata, out DependencyObjectType dType, out PropertyMetadata baseMetadata);
      if (ReadOnly) {
        throw new InvalidOperationException(SR.Get("ReadOnlyOverrideNotAllowed", Name));
      }
      ProcessOverrideMetadata(forType, typeMetadata, dType, baseMetadata);
    }

    /// <summary>Supplies alternate metadata for a read-only dependency property when it is present on instances of a specified type, overriding the metadata that was provided in the initial dependency property registration. You must pass the <see cref="T:System.Windows.DependencyPropertyKey" /> for the read-only dependency property to avoid raising an exception.</summary>
    /// <param name="forType">The type where this dependency property is inherited and where the provided alternate metadata will be applied.</param>
    /// <param name="typeMetadata">The metadata to apply to the dependency property on the overriding type.</param>
    /// <param name="key">The access key for a read-only dependency property. </param>
    public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata, DependencyPropertyKey key) {
      SetupOverrideMetadata(forType, typeMetadata, out DependencyObjectType dType, out PropertyMetadata baseMetadata);
      if (key == null) {
        throw new ArgumentNullException("key");
      }
      if (!ReadOnly) {
        throw new InvalidOperationException(SR.Get("PropertyNotReadOnly"));
      }
      if (key.DependencyProperty != this) {
        throw new ArgumentException(SR.Get("ReadOnlyOverrideKeyNotAuthorized", Name));
      }
      VerifyReadOnlyKey(key);
      ProcessOverrideMetadata(forType, typeMetadata, dType, baseMetadata);
    }

    private void ProcessOverrideMetadata(Type forType, PropertyMetadata typeMetadata, DependencyObjectType dType, PropertyMetadata baseMetadata) {
      lock (Synchronized) {
        if (UnsetValue != _metadataMap[dType.Id]) {
          throw new ArgumentException(SR.Get("TypeMetadataAlreadyRegistered", forType.Name));
        }
        _metadataMap[dType.Id] = typeMetadata;
      }
      typeMetadata.InvokeMerge(baseMetadata, this);
      typeMetadata.Seal(this, forType);
      if (typeMetadata.IsInherited) {
        _packedData |= Flags.IsPotentiallyInherited;
      }
      if (typeMetadata.DefaultValueWasSet() && typeMetadata.DefaultValue != DefaultMetadata.DefaultValue) {
        _packedData |= Flags.IsDefaultValueChanged;
      }
      if (typeMetadata.UsingDefaultValueFactory) {
        _packedData |= Flags.IsPotentiallyUsingDefaultValueFactory;
      }
    }

    [MS.Internal.WindowsBase.FriendAccessAllowed]
    internal object GetDefaultValue(DependencyObjectType dependencyObjectType) {
      if (!IsDefaultValueChanged) {
        return DefaultMetadata.DefaultValue;
      }
      return GetMetadata(dependencyObjectType).DefaultValue;
    }

    [MS.Internal.WindowsBase.FriendAccessAllowed]
    internal object GetDefaultValue(Type forType) {
      if (!IsDefaultValueChanged) {
        return DefaultMetadata.DefaultValue;
      }
      return GetMetadata(DependencyObjectType.FromSystemTypeInternal(forType)).DefaultValue;
    }

    /// <summary>Returns the metadata for this dependency property as it exists on a specified existing type. </summary>
    /// <returns>A property metadata object.</returns>
    /// <param name="forType">The specific type from which to retrieve the dependency property metadata.</param>
    public PropertyMetadata GetMetadata(Type forType) {
      if (forType != null) {
        return GetMetadata(DependencyObjectType.FromSystemType(forType));
      }
      throw new ArgumentNullException("forType");
    }

    /// <summary>Returns the metadata for this dependency property as it exists on the specified object instance. </summary>
    /// <returns>A property metadata object.</returns>
    /// <param name="dependencyObject">A dependency object that is checked for type, to determine which type-specific version of the dependency property the metadata should come from.</param>
    public PropertyMetadata GetMetadata(DependencyObject dependencyObject) {
      if (dependencyObject != null) {
        return GetMetadata(dependencyObject.DependencyObjectType);
      }
      throw new ArgumentNullException("dependencyObject");
    }

    /// <summary> Returns the metadata for this dependency property as it exists on a specified type. </summary>
    /// <returns>A property metadata object.</returns>
    /// <param name="dependencyObjectType">A specific object that records the dependency object type from which the dependency property metadata is desired.</param>
    public PropertyMetadata GetMetadata(DependencyObjectType dependencyObjectType) {
      if (dependencyObjectType != null) {
        int num = _metadataMap.Count - 1;
        if (num < 0) {
          return _defaultMetadata;
        }
        int key;
        object value;
        if (num == 0) {
          _metadataMap.GetKeyValuePair(num, out key, out value);
          while (dependencyObjectType.Id > key) {
            dependencyObjectType = dependencyObjectType.BaseType;
          }
          if (key == dependencyObjectType.Id) {
            return (PropertyMetadata)value;
          }
        } else if (dependencyObjectType.Id != 0) {
          do {
            _metadataMap.GetKeyValuePair(num, out key, out value);
            num--;
            while (dependencyObjectType.Id < key && num >= 0) {
              _metadataMap.GetKeyValuePair(num, out key, out value);
              num--;
            }
            while (dependencyObjectType.Id > key) {
              dependencyObjectType = dependencyObjectType.BaseType;
            }
            if (key == dependencyObjectType.Id) {
              return (PropertyMetadata)value;
            }
          }
          while (num >= 0);
        }
      }
      return _defaultMetadata;
    }

    /// <summary>Adds another type as an owner of a dependency property that has already been registered.</summary>
    /// <returns>A reference to the original <see cref="T:System.Windows.DependencyProperty" /> identifier that identifies the dependency property. This identifier should be exposed by the adding class as a public static readonly field.</returns>
    /// <param name="ownerType">The type to add as an owner of this dependency property.</param>
    public DependencyProperty AddOwner(Type ownerType) {
      return AddOwner(ownerType, null);
    }

    /// <summary>Adds another type as an owner of a dependency property that has already been registered, providing dependency property metadata for the dependency property as it will exist on the provided owner type. </summary>
    /// <returns>A reference to the original <see cref="T:System.Windows.DependencyProperty" /> identifier that identifies the dependency property. This identifier should be exposed by the adding class as a public static readonly field.</returns>
    /// <param name="ownerType">The type to add as owner of this dependency property.</param>
    /// <param name="typeMetadata">The metadata that qualifies the dependency property as it exists on the provided type.</param>
    public DependencyProperty AddOwner(Type ownerType, PropertyMetadata typeMetadata) {
      if (ownerType == null) {
        throw new ArgumentNullException("ownerType");
      }
      FromNameKey key = new FromNameKey(Name, ownerType);
      lock (Synchronized) {
        if (PropertyFromName.Contains(key)) {
          throw new ArgumentException(SR.Get("PropertyAlreadyRegistered", Name, ownerType.Name));
        }
      }
      if (typeMetadata != null) {
        OverrideMetadata(ownerType, typeMetadata);
      }
      lock (Synchronized) {
        PropertyFromName[key] = this;
        return this;
      }
    }

    /// <summary>Returns a hash code for this <see cref="T:System.Windows.DependencyProperty" />.</summary>
    /// <returns>The hash code for this <see cref="T:System.Windows.DependencyProperty" />.</returns>
    public override int GetHashCode() {
      return GlobalIndex;
    }

    /// <summary>Determines whether a specified value is acceptable for this dependency property's type, as checked against the property type provided in the original dependency property registration. </summary>
    /// <returns>true if the specified value is the registered property type or an acceptable derived type; otherwise, false.</returns>
    /// <param name="value">The value to check.</param>
    public bool IsValidType(object value) {
      return IsValidType(value, PropertyType);
    }

    /// <summary>Determines whether the provided value is accepted for the type of property through basic type checking, and also potentially if it is within the allowed range of values for that type. </summary>
    /// <returns>true if the value is acceptable and is of the correct type or a derived type; otherwise, false.</returns>
    /// <param name="value">The value to check.</param>
    public bool IsValidValue(object value) {
      if (!IsValidType(value, PropertyType)) {
        return false;
      }
      if (ValidateValueCallback != null) {
        return ValidateValueCallback(value);
      }
      return true;
    }

    internal void VerifyReadOnlyKey(DependencyPropertyKey candidateKey) {
      if (_readOnlyKey != candidateKey) {
        throw new ArgumentException(SR.Get("ReadOnlyKeyNotAuthorized"));
      }
    }

    internal bool IsValidValueInternal(object value) {
      if (ValidateValueCallback != null) {
        return ValidateValueCallback(value);
      }
      return true;
    }

    [MS.Internal.WindowsBase.FriendAccessAllowed]
    internal static DependencyProperty FromName(string name, Type ownerType) {
      DependencyProperty dependencyProperty = null;
      if (name == null) {
        throw new ArgumentNullException("name");
      }
      if (!(ownerType != null)) {
        throw new ArgumentNullException("ownerType");
      }
      FromNameKey fromNameKey = new FromNameKey(name, ownerType);
      while (dependencyProperty == null && ownerType != null) {
        SecurityHelper.RunClassConstructor(ownerType);
        fromNameKey.UpdateNameKey(ownerType);
        lock (Synchronized) {
          dependencyProperty = (DependencyProperty)PropertyFromName[fromNameKey];
        }
        ownerType = ownerType.BaseType;
      }
      return dependencyProperty;
    }

    /// <summary> Returns the string representation of the dependency property. </summary>
    /// <returns>The string representation of the dependency property.</returns>
    public override string ToString() {
      return _name;
    }

    internal static bool IsValidType(object value, Type propertyType) {
      if (value == null) {
        if (propertyType.IsValueType && (!propertyType.IsGenericType || !(propertyType.GetGenericTypeDefinition() == NullableType))) {
          return false;
        }
      } else if (!propertyType.IsInstanceOfType(value)) {
        return false;
      }
      return true;
    }

    private DependencyProperty(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata, ValidateValueCallback validateValueCallback) {
      _name = name;
      _propertyType = propertyType;
      _ownerType = ownerType;
      _defaultMetadata = defaultMetadata;
      _validateValueCallback = validateValueCallback;
      Flags flags = default(Flags);
      lock (Synchronized) {
        flags = (Flags)GetUniqueGlobalIndex(ownerType, name);
        RegisteredPropertyList.Add(this);
      }
      if (propertyType.IsValueType) {
        flags |= Flags.IsValueType;
      }
      if (propertyType == typeof(object)) {
        flags |= Flags.IsObjectType;
      }
      if (typeof(Freezable).IsAssignableFrom(propertyType)) {
        flags |= Flags.IsFreezableType;
      }
      if (propertyType == typeof(string)) {
        flags |= Flags.IsStringType;
      }
      _packedData = flags;
    }

    internal static int GetUniqueGlobalIndex(Type ownerType, string name) {
      if (GlobalIndexCount >= 65535) {
        if (ownerType != null) {
          throw new InvalidOperationException(SR.Get("TooManyDependencyProperties", ownerType.Name + "." + name));
        }
        throw new InvalidOperationException(SR.Get("TooManyDependencyProperties", "ConstantProperty"));
      }
      return GlobalIndexCount++;
    }
  }

}