using Common;

namespace System.Reflection {
  public class TypeInfoDebugObject : IDebugObject<TypeInfo> {
    public TypeInfoDebugObject(TypeInfo typeInfo) {
      this.typeInfo = typeInfo;
    }

    readonly TypeInfo typeInfo;

    public string Name => typeInfo.Name;
    public string NameSpace => typeInfo.Namespace;

  }

  public static class TypeInfoExtensions {
    public static TypeInfoDebugObject GetDebugObject(this TypeInfo typeinfo) => new TypeInfoDebugObject(typeinfo);
  }

}