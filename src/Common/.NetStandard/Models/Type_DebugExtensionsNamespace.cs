using System;
using System.Runtime.CompilerServices;

namespace Common.Models {
  public class Type_DebugExtensionsNamespace : _BaseExtensionsNamespace<Type> {
    public Type_DebugExtensionsNamespace(Type value) : base(value) { }
  }

  public static class Type_DebugExtensions {

    #region "Extensions Namespace "
    public static Type_DebugExtensionsNamespace Debug(this Type type) => new Type_DebugExtensionsNamespace(type);
    #endregion

    public static DebugInfo GetDebugInfo(this Type_DebugExtensionsNamespace type,
      [CallerMemberName] string memberName = "",
      [CallerFilePath] string filePath = "",
      [CallerLineNumber] int lineNumber = 0) {
      var callerType = type.value;
      return new DebugInfo {
        FilePath = filePath,
        LineNumber = lineNumber,
        MemberName = memberName,
        Name = callerType.Name,
        Namespace = callerType.Namespace
      };
    }


  }

}
