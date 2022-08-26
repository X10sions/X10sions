using LinqToDB.Configuration;
using LinqToDB.Data;
using System.Data;
using System.Data.Common;

namespace LinqToDB.DataProvider.DB2iSeries;
public static class xDB2iSeriesTools {

  public static bool AutoDetectProvider { get; set; } = true;

  //internal static IDataProvider? ProviderDetector(IConnectionStringSettings css, string connectionString) {
  //  // DB2 ODS provider could be used by informix
  //  if (css.Name.Contains("Informix"))
  //    return null;
  //  switch (css.ProviderName) {
  //    case ProviderName.DB2LUW: return _db2DataProviderLUW.Value;
  //    case ProviderName.DB2zOS: return _db2DataProviderzOS.Value;
  //    case "":
  //    case null:
  //      if (css.Name == "DB2")
  //        goto case ProviderName.DB2;
  //      break;
  //    case ProviderName.DB2:
  //    case DB2ProviderAdapter.NetFxClientNamespace:
  //    case DB2ProviderAdapter.CoreClientNamespace:
  //      if (css.Name.Contains("LUW"))
  //        return _db2DataProviderLUW.Value;
  //      if (css.Name.Contains("z/OS") || css.Name.Contains("zOS"))
  //        return _db2DataProviderzOS.Value;
  //      if (AutoDetectProvider) {
  //        try {
  //          var cs = string.IsNullOrWhiteSpace(connectionString) ? css.ConnectionString : connectionString;

  //          using (var conn = DB2ProviderAdapter.Instance.CreateConnection(cs)) {
  //            conn.Open();

  //            var iszOS = conn.eServerType == DB2ProviderAdapter.DB2ServerTypes.DB2_390;

  //            return iszOS ? _db2DataProviderzOS.Value : _db2DataProviderLUW.Value;
  //          }
  //        } catch {
  //        }
  //      }
  //      return GetDataProvider();
  //  }
  //  return null;
  //}

  public static IDataProvider GetDataProvider<TConnection, TDataReader>(string connectionString) where TConnection : DbConnection, new() where TDataReader : IDataReader => DB2iSeriesDataProvider<TConnection>.GetInstance<TDataReader>(connectionString);

  //  public static void ResolveDB2iSeries(string path) {
  //    new AssemblyResolver(path, DB2iSeriesProviderAdapter.AssemblyName);
  //    if (DB2iSeriesProviderAdapter.AssemblyNameOld != null)
  //#pragma warning disable CS0162 // Unreachable code detected
  //      new AssemblyResolver(path, DB2iSeriesProviderAdapter.AssemblyNameOld);
  //#pragma warning restore CS0162 // Unreachable code detected
  //  }

  //  public static void ResolveDB2iSeries(Assembly assembly) =>    new AssemblyResolver(assembly, assembly.GetName().Name!);

  #region CreateDataConnection

  /// <summary>
  /// Creates <see cref="DataConnection"/> object using provided DB2iSeries connection string.
  /// </summary>
  /// <param name="connectionString">Connection string.</param>
  /// <param name="version">DB2iSeries version.</param>
  /// <returns><see cref="DataConnection"/> instance.</returns>
  public static DataConnection CreateDataConnection<TConnection, TDataReader>(string connectionString)
    where TConnection : DbConnection, new() where TDataReader : IDataReader
    => new DataConnection(GetDataProvider<TConnection, TDataReader>(connectionString), connectionString);

  /// <summary>
  /// Creates <see cref="DataConnection"/> object using provided connection object.
  /// </summary>
  /// <param name="connection">Connection instance.</param>
  /// <param name="version">DB2iSeries version.</param>
  /// <returns><see cref="DataConnection"/> instance.</returns>
  public static DataConnection CreateDataConnection<TConnection, TDataReader>(DbConnection connection)
    where TConnection : DbConnection, new() where TDataReader : IDataReader
    => new DataConnection(GetDataProvider<TConnection, TDataReader>(connection.ConnectionString), connection);

  /// <summary>
  /// Creates <see cref="DataConnection"/> object using provided transaction object.
  /// </summary>
  /// <param name="transaction">Transaction instance.</param>
  /// <param name="version">DB2iSeries version.</param>
  /// <returns><see cref="DataConnection"/> instance.</returns>
  public static DataConnection CreateDataConnection<TConnection, TDataReader>(DbTransaction transaction)
    where TConnection : DbConnection, new() where TDataReader : IDataReader
    => new DataConnection(GetDataProvider<TConnection, TDataReader>(transaction.Connection.ConnectionString), transaction);

  #endregion

  #region BulkCopy

  /// <summary>
  /// Default bulk copy mode, used for DB2 by <see cref="DataConnectionExtensions.BulkCopy{T}(DataConnection, IEnumerable{T})"/>
  /// methods, if mode is not specified explicitly.
  /// Default value: <see cref="BulkCopyType.MultipleRows"/>.
  /// </summary>
  public static BulkCopyType DefaultBulkCopyType { get; set; } = BulkCopyType.MultipleRows;

  #endregion
}
