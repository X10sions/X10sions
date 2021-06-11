using LinqToDB.Data;

namespace LinqToDB {
  public static class DataTypeExtensions {

    public static DataParameter NewDataParameter(this DataType dataType) => new DataParameter { DataType = dataType };
    public static string GetDbType(this DataType dataType) => dataType.NewDataParameter().DbType;

  }
}
