using LinqToDB.Data;
using System.Linq.Expressions;

namespace LinqToDB.Mapping {
  public static class MappingSchemaExtensions {

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

  }
}