using LinqToDB.DataProvider;
using System;

namespace xLinqToDB.DataProvider.DB2iSeries {
  public enum DB2iSeriesConnectionType {
    iDB2,
    Odbc,
    OleDb,
  }

  public static class DB2iSeriesConnectionTypeExtensions {

    //public static DB2iSeriesConnectionType GetDB2iSeriesConnectionType<T>() where T : IDbConnection => typeof(T).GetDB2iSeriesConnectionType();

    public static DB2iSeriesConnectionType GetDB2iSeriesConnectionType(this Type type) => type switch {
      var x when x == V3_1_6_1.TB.DB2iSeriesProviderAdapter.GetInstance(type.GetType().Name).ConnectionType => DB2iSeriesConnectionType.iDB2,
      var x when x == OdbcProviderAdapter.GetInstance().ConnectionType => DB2iSeriesConnectionType.Odbc,
      var x when x == OleDbProviderAdapter.GetInstance().ConnectionType => DB2iSeriesConnectionType.OleDb,
      _ => throw new NotImplementedException()
    };

    //typeof(iDB2Connection).Namespace
    public static string Namespace(this DB2iSeriesConnectionType connectionType) => connectionType switch {
      DB2iSeriesConnectionType.iDB2 => DB2iSeriesConstants.ClientNamespace,
      DB2iSeriesConnectionType.Odbc => OdbcProviderAdapter.ClientNamespace,
      DB2iSeriesConnectionType.OleDb => OleDbProviderAdapter.ClientNamespace,
      _ => throw new NotImplementedException()
    };

    //typeof(iDB2DataReader)
    public static Type DataReaderType(this DB2iSeriesConnectionType connectionType) => connectionType switch {
      DB2iSeriesConnectionType.iDB2 => V3_1_6_1.TB.DB2iSeriesProviderAdapter.GetInstance(connectionType).DataReaderType,
      DB2iSeriesConnectionType.Odbc => OdbcProviderAdapter.GetInstance().DataReaderType,
      DB2iSeriesConnectionType.OleDb => OleDbProviderAdapter.GetInstance().DataReaderType,
      _ => throw new NotImplementedException()
    };

    public static bool SupportsNamedParameters(this DB2iSeriesConnectionType connectionType) => connectionType == DB2iSeriesConnectionType.iDB2;

    public static bool SupportsNamedParameters(this Type connectionType) => connectionType.Name == DB2iSeriesConstants.ConnectionTypeName;

  }
}