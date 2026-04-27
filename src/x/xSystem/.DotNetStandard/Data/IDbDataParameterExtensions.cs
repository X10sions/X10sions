namespace System.Data;

public static class IDbDataParameterExtensions {

  public static IDbDataParameter DbType(this IDbDataParameter parameter, DbType value) => parameter.Tap(parameter => parameter.DbType = value);
  public static IDbDataParameter Direction(this IDbDataParameter parameter, ParameterDirection value) => parameter.Tap(parameter => parameter.Direction = value);
  public static IDbDataParameter Name(this IDbDataParameter parameter, string value) => parameter.Tap(parameter => parameter.ParameterName = value);
  public static IDbDataParameter Value(this IDbDataParameter parameter, object value) => parameter.Tap(parameter => parameter.Value = value);


}
