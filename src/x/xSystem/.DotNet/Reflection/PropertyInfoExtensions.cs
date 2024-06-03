namespace System.Reflection;

public static class PropertyInfoExtensions {

  /// <summary> https://stackoverflow.com/questions/6026824/detecting-a-nullable-type-via-reflection </summary>
  public static bool IsNullable(this PropertyInfo property) => property.GetMethod is null ? false : new NullabilityInfoContext().Create(property.GetMethod.ReturnParameter).ReadState == NullabilityState.Nullable;

}