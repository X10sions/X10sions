using LinqToDB.Data;
using System.Linq.Expressions;

namespace LinqToDB.Mapping;

public static class MappingSchemaExtensions {

  public static FluentMappingBuilder GetFluentMappingBuilder(this MappingSchema mappingSchema) => new FluentMappingBuilder(mappingSchema);

  public static MappingSchema SetConverters<T, T2>(this MappingSchema mappingSchema, Expression<Func<T, T2>> expr) => mappingSchema.SetConverter(expr.Compile()).SetConvertExpression(expr);

  public static MappingSchema SetConverters<T, T2>(this MappingSchema mappingSchema, DataType dataParameterType, Expression<Func<T, T2>> expr) {
    //mappingSchema.SetDataType(typeof(T), dataParameterType);
    //mappingSchema.SetDataType<T>( dataParameterType);
    return mappingSchema
      .SetDataType<T>(dataParameterType)
      .SetConverters(expr)
      .SetConverter<T, DataParameter>(o => dataParameterType.GetDataParameter(o));
  }

  public static MappingSchema SetConverters<T, T2>(this MappingSchema mappingSchema, DataType dataParameterType, Expression<Func<T, T2>> expr1, Expression<Func<T2, T>> expr2)
  => mappingSchema.SetConverters(dataParameterType, expr1).SetConverters(expr2);

  public static MappingSchema SetConverters<T, T2, T3>(this MappingSchema mappingSchema, DataType dataParameterType, Expression<Func<T, T2>> expr1, Expression<Func<T, T3>> expr3, Expression<Func<T2, T>> expr2)
    => mappingSchema.SetConverters(dataParameterType, expr1, expr2).SetConverters(expr3);

  /// <summary>
  /// Sets converter for SystemToDatabaseType, DatabaseToSystemType & SystemTypeToDataParameter
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="TDB"></typeparam>
  /// <param name="mappingSchema"></param>
  /// <param name="fromSystemType"></param>
  /// <param name="fromDatabaseType"></param>
  /// <returns>MappingSchema</returns>
  public static MappingSchema SetConvertersFor<T, TDB>(this MappingSchema mappingSchema, Func<T, TDB> fromSystemType, Func<TDB, T> fromDatabaseType) {
    mappingSchema.SetConverter<T, DataParameter>(o => new DataParameter { Value = fromSystemType(o) });
    mappingSchema.SetConverter(fromSystemType);
    mappingSchema.SetConverter(fromDatabaseType);
    return mappingSchema;
  }

  public static MappingSchema SetConvertersForEnumToString<TEnum>(this MappingSchema mappingSchema, Func<TEnum, string> fromSystemType) => mappingSchema.SetConvertersFor(fromSystemType, s => (TEnum)Enum.Parse(typeof(TEnum), s, true));

  public static MappingSchema SetConvertExpressionsFor<T, TDB>(this MappingSchema mappingSchema, Expression<Func<T, TDB>> fromSystemType, Expression<Func<TDB, T>> fromDatabaseType, bool addNullcheck = true) {
    //      mappingSchema.SetConvertExpression<T, DataParameter>(o => new DataParameter { Value = fromSystemType(o) });
    mappingSchema.SetConvertExpression(fromSystemType, addNullcheck);
    mappingSchema.SetConvertExpression(fromDatabaseType, addNullcheck);
    return mappingSchema;
  }

  public static MappingSchema SetDataType<T>(this MappingSchema mappingSchema, DataType dataParameterType) {
    mappingSchema.SetDataType(typeof(T), dataParameterType);
    return mappingSchema;
  }

}