using LinqToDB.Data;

namespace LinqToDB;

public static class DataTypeExtensions {

  public static DataParameter GetDataParameter<T>(this DataType dataParameterType, T value) => value switch {
    null => new DataParameter { DataType = dataParameterType, Value = DBNull.Value },
    _ => new DataParameter { Value = value == null }
  };

}