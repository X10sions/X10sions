namespace System.Data;
public static class IDataParameterCollectionExtensions {

  public static IDataParameterCollection AddRange(this IDataParameterCollection parameterCollection, params IDbDataParameter[] parameters) {
    if (parameters != null) {
      foreach (var p in parameters) {
        parameterCollection.Add(p);
      }
    }
    return parameterCollection;
  }

  public static IDataParameterCollection AddRange(this IDataParameterCollection parameterCollection, IEnumerable<IDbDataParameter> parameters)
    => parameterCollection.AddRange(parameters.ToArray());

  public static IDataParameterCollection AddRange(this IDataParameterCollection parameterCollection, IList<IDbDataParameter> parameters)
    => parameterCollection.AddRange(parameters.ToArray());

  public static IDataParameterCollection ReplaceAll(this IDataParameterCollection parameterCollection, params IDbDataParameter[] parameters) {
    parameterCollection.Clear();
    return parameterCollection.AddRange(parameters);
  }

  public static IDataParameterCollection ReplaceAll(this IDataParameterCollection parameterCollection, IEnumerable<IDbDataParameter> parameters)
      => parameterCollection.ReplaceAll(parameters.ToArray());

  public static IDataParameterCollection ReplaceAll(this IDataParameterCollection parameterCollection, IList<IDbDataParameter> parameters)
    => parameterCollection.ReplaceAll(parameters.ToArray());

}
