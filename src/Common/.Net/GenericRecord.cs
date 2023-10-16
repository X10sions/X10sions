namespace Common;

public static class GenericRecordExensions {
  public static GenericRecord<T1> AsGenericRecord<T1>(this T1 v1) => new GenericRecord<T1>(v1);
  public static GenericRecord<T1, T2> AsGenericRecord<T1, T2>(this T1 v1, T2 v2) => new GenericRecord<T1, T2>(v1, v2);
  public static GenericRecord<T1, T2, T3> AsGenericRecord<T1, T2, T3>(this T1 v1, T2 v2, T3 v3) => new GenericRecord<T1, T2, T3>(v1, v2, v3);
  public static GenericRecord<T1, T2, T3, T4> AsGenericRecord<T1, T2, T3, T4>(this T1 v1, T2 v2, T3 v3, T4 v4) => new GenericRecord<T1, T2, T3, T4>(v1, v2, v3, v4);
}

public record GenericRecord<T1>(T1 Value1);
public record GenericRecord<T1, T2>(T1 Value1, T2 Value2) : GenericRecord<T1>(Value1);
public record GenericRecord<T1, T2, T3>(T1 Value1, T2 Value2, T3 Value3) : GenericRecord<T1, T2>(Value1, Value2);
public record GenericRecord<T1, T2, T3, T4>(T1 Value1, T2 Value2, T3 Value3, T4 Value4) : GenericRecord<T1, T2, T3>(Value1, Value2, Value3);
