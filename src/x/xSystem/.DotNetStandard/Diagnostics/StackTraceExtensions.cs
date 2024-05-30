using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Diagnostics {
  public static class StackFrameExtensions {
    [MethodImpl(MethodImplOptions.NoInlining)] public static Type GetCurrentMethodType(this StackFrame sf) => sf.GetMethod().ReflectedType;
    [MethodImpl(MethodImplOptions.NoInlining)] public static string GetCurrentMethodFullName(this StackFrame sf) => sf.GetCurrentMethodType().FullName;
    [MethodImpl(MethodImplOptions.NoInlining)] public static string GetCurrentMethodName(this StackFrame sf) => sf.GetMethod().Name;


  }

  public static class StackTraceExtensions {
    
    [MethodImpl(MethodImplOptions.NoInlining)] public static MemberInfo GetCurrentMethod() => new StackTrace().GetFrame(1).GetMethod();
    [MethodImpl(MethodImplOptions.NoInlining)] public static Type GetCurrentMethodType() => GetCurrentMethod().ReflectedType;
    [MethodImpl(MethodImplOptions.NoInlining)] public static string GetCurrentMethodFullName() => GetCurrentMethodType().FullName;
    [MethodImpl(MethodImplOptions.NoInlining)] public static string GetCurrentMethodName() => GetCurrentMethod().Name;


  }
}
