using System.Collections;
using System.Data.Common;

namespace Common.Data;

/// <summary>
/// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
/// ability to discover parameters for stored procedures at run-time.
/// </summary>
/// <see cref="///https://gist.github.com/imranbaloch/10895917"/>
public static class SqlClientHelperParameterCache {
  #region private methods, variables, and constructors

  //Since this class provides only static methods, make the default constructor private to prevent
  //instances from being created with "new SqlHelperParameterCache()"

  private static readonly Hashtable ParamCache = Hashtable.Synchronized(new Hashtable());


  /// <summary>
  /// Resolve at run time the appropriate set of DbParameters for a stored procedure
  /// </summary>
  /// <param name="connection">A valid DbConnection object</param>
  /// <param name="spName">The name of the stored procedure</param>
  /// <param name="includeReturnValueParameter">Whether or not to include their return value parameter</param>
  /// <returns>The parameter array discovered.</returns>
  private static DbParameter[] DiscoverSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter) {
    throw new NotImplementedException();
    //if (connection == null) throw new ArgumentNullException("connection");
    //if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");
    //var cmd = connection.CreateCommand(spName, null, CommandType.StoredProcedure);
    //connection.Open();
    //DbCommandBuilder.DeriveParameters(cmd);
    //connection.Close();
    //if (!includeReturnValueParameter) {
    //  cmd.Parameters.RemoveAt(0);
    //}
    //var discoveredParameters = new DbParameter[cmd.Parameters.Count];
    //cmd.Parameters.CopyTo(discoveredParameters, 0);
    //// Init the parameters with a DBNull value
    //foreach (var discoveredParameter in discoveredParameters) {
    //  discoveredParameter.Value = DBNull.Value;
    //}
    //return discoveredParameters;
  }

  /// <summary>
  /// Deep copy of cached DbParameter array
  /// </summary>
  /// <param name="originalParameters"></param>
  private static T[] CloneParameters<T>(IList<T> originalParameters) where T : DbParameter {
    var clonedParameters = new T[originalParameters.Count];
    for (int i = 0, j = originalParameters.Count; i < j; i++) {
      clonedParameters[i] = (T)((ICloneable)originalParameters[i]).Clone();
    }
    return clonedParameters;
  }

  #endregion private methods, variables, and constructors

  #region caching functions

  /// <summary>
  /// Add parameter array to the cache
  /// </summary>
  /// <param name="connectionString">A valid connection string for a DbConnection</param>
  /// <param name="commandText">The stored procedure name or T-SQL command</param>
  /// <param name="commandParameters">An array of SqlParamters to be cached</param>
  public static void CacheParameterSet(string connectionString, string commandText, params DbParameter[] commandParameters) {
    if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
    if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");

    var hashKey = connectionString + ":" + commandText;

    ParamCache[hashKey] = commandParameters;
  }

  /// <summary>
  /// Retrieve a parameter array from the cache
  /// </summary>
  /// <param name="connectionString">A valid connection string for a DbConnection</param>
  /// <param name="commandText">The stored procedure name or T-SQL command</param>
  /// <returns>An array of SqlParamters</returns>
  public static DbParameter[] GetCachedParameterSet(string connectionString, string commandText) {
    if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
    if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");

    var hashKey = connectionString + ":" + commandText;

    var cachedParameters = ParamCache[hashKey] as DbParameter[];
    if (cachedParameters == null) {
      return null;
    }
    return CloneParameters(cachedParameters);
  }

  #endregion caching functions

  #region Parameter Discovery Functions

  /// <summary>
  /// Retrieves the set of DbParameters appropriate for the stored procedure
  /// </summary>
  /// <remarks>
  /// This method will query the database for this information, and then store it in a cache for future requests.
  /// </remarks>
  /// <param name="connectionString">A valid connection string for a DbConnection</param>
  /// <param name="spName">The name of the stored procedure</param>
  /// <returns>An array of DbParameters</returns>
  public static DbParameter[] GetSpParameterSet<T>(string connectionString, string spName) where T : DbConnection, new() {
    return GetSpParameterSet<T>(connectionString, spName, false);
  }

  /// <summary>
  /// Retrieves the set of DbParameters appropriate for the stored procedure
  /// </summary>
  /// <remarks>
  /// This method will query the database for this information, and then store it in a cache for future requests.
  /// </remarks>
  /// <param name="connectionString">A valid connection string for a DbConnection</param>
  /// <param name="spName">The name of the stored procedure</param>
  /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
  /// <returns>An array of DbParameters</returns>
  public static DbParameter[] GetSpParameterSet<T>(string connectionString, string spName, bool includeReturnValueParameter) where T : DbConnection, new() {
    if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
    if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");
    using (var connection = new T { ConnectionString = connectionString }) {
      return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
    }
  }

  /// <summary>
  /// Retrieves the set of DbParameters appropriate for the stored procedure
  /// </summary>
  /// <remarks>
  /// This method will query the database for this information, and then store it in a cache for future requests.
  /// </remarks>
  /// <param name="connection">A valid DbConnection object</param>
  /// <param name="spName">The name of the stored procedure</param>
  /// <returns>An array of DbParameters</returns>
  internal static DbParameter[] GetSpParameterSet(DbConnection connection, string spName) {
    return GetSpParameterSet(connection, spName, false);
  }

  /// <summary>
  /// Retrieves the set of DbParameters appropriate for the stored procedure
  /// </summary>
  /// <remarks>
  /// This method will query the database for this information, and then store it in a cache for future requests.
  /// </remarks>
  /// <param name="connection">A valid DbConnection object</param>
  /// <param name="spName">The name of the stored procedure</param>
  /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
  /// <returns>An array of DbParameters</returns>
  internal static DbParameter[] GetSpParameterSet(DbConnection connection, string spName, bool includeReturnValueParameter) {
    if (connection == null) throw new ArgumentNullException("connection");
    using (var clonedConnection = (DbConnection)((ICloneable)connection).Clone()) {
      return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
    }
  }

  /// <summary>
  /// Retrieves the set of DbParameters appropriate for the stored procedure
  /// </summary>
  /// <param name="connection">A valid DbConnection object</param>
  /// <param name="spName">The name of the stored procedure</param>
  /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
  /// <returns>An array of DbParameters</returns>
  private static DbParameter[] GetSpParameterSetInternal(DbConnection connection, string spName, bool includeReturnValueParameter) {
    if (connection == null) throw new ArgumentNullException("connection");
    if (string.IsNullOrEmpty(spName)) throw new ArgumentNullException("spName");
    var hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
    var cachedParameters = ParamCache[hashKey] as DbParameter[];
    if (cachedParameters == null) {
      var spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
      ParamCache[hashKey] = spParameters;
      cachedParameters = spParameters;
    }
    return CloneParameters(cachedParameters);
  }

  #endregion Parameter Discovery Functions

}
