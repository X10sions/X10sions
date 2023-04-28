namespace Common;

public static class GenericClassExensions {
  public static GenericClass<T1> AsGenericClass<T1>(this T1 v1) => new GenericClass<T1>(v1);
  public static GenericClass<T1, T2> AsGenericClass<T1, T2>(this T1 v1, T2 v2) => new GenericClass<T1, T2>(v1, v2);
  public static GenericClass<T1, T2, T3> AsGenericClass<T1, T2, T3>(this T1 v1, T2 v2, T3 v3) => new GenericClass<T1, T2, T3>(v1, v2, v3);
  public static GenericClass<T1, T2, T3, T4> AsGenericClass<T1, T2, T3, T4>(this T1 v1, T2 v2, T3 v3, T4 v4) => new GenericClass<T1, T2, T3, T4>(v1, v2, v3, v4);
}

public class GenericClass<T1> {
  public static GenericClass<T1> Insance(T1 v1) => new GenericClass<T1>(v1);
  public GenericClass(T1 value1) { Value1 = value1; }
  public T1 Value1 { get; set; }
}

public class GenericClass<T1, T2> : GenericClass<T1> {
  public static GenericClass<T1, T2> Insance(T1 v1, T2 v2) => new GenericClass<T1, T2>(v1, v2);
  public GenericClass(T1 value1, T2 value2) : base(value1) { Value2 = value2; }
  public T2 Value2 { get; set; }
}

public class GenericClass<T1, T2, T3> : GenericClass<T1, T2> {
  public static GenericClass<T1, T2, T3> Insance(T1 v1, T2 v2, T3 v3) => new GenericClass<T1, T2, T3>(v1, v2, v3);
  public GenericClass(T1 value1, T2 value2, T3 value3) : base(value1, value2) { Value3 = value3; }
  public T3 Value3 { get; set; }
}

public class GenericClass<T1, T2, T3, T4> : GenericClass<T1, T2, T3> {
  public static GenericClass<T1, T2, T3, T4> Insance(T1 v1, T2 v2, T3 v3, T4 v4) => new GenericClass<T1, T2, T3, T4>(v1, v2, v3, v4);
  public GenericClass(T1 value1, T2 value2, T3 value3, T4 value4) : base(value1, value2, value3) { Value4 = value4; }
  public T4 Value4 { get; set; }
}
