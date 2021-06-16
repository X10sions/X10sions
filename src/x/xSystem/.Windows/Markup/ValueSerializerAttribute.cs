using System.Runtime.CompilerServices;

namespace System.Windows.Markup {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
  [TypeForwardedFrom("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  public sealed class ValueSerializerAttribute : Attribute {
    public ValueSerializerAttribute(Type type) {
      _valueSerializerType = type;
    }

    public ValueSerializerAttribute(string valueSerializerTypeName) {
      _valueSerializerTypeName = valueSerializerTypeName;
    }

    Type _valueSerializerType;

    string _valueSerializerTypeName;

    public Type ValueSerializerType {
      get {
        if (_valueSerializerType == null && _valueSerializerTypeName != null) {
          _valueSerializerType = Type.GetType(_valueSerializerTypeName);
        }
        return _valueSerializerType;
      }
    }

    public string ValueSerializerTypeName => _valueSerializerType != null ? _valueSerializerType.AssemblyQualifiedName : _valueSerializerTypeName;


  }
}