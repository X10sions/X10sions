using LinqToDB.Common;
using LinqToDB.DataProvider;
using System;
using System.Collections.Concurrent;
using System.Data;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  //public class IbmDataDB2iSeriesProviderAdapter : IDynamicProviderAdapter {
  //  public IbmDataDB2iSeriesProviderAdapter() {
  //      var assembly = Tools.TryLoadAssembly(DB2iSeriesConstants.AssemblyName, DB2iSeriesConstants.ProviderFactoryName);
  //      if (assembly == null) throw new InvalidOperationException($"Cannot load assembly {DB2iSeriesConstants.AssemblyName}");
  //      CommandType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.CommandTypeName}", true);
  //      ConnectionType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.ConnectionTypeName}", true);
  //      DataReaderType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.DataReaderTypeName}", true);
  //      ParameterType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.ParameterTypeName}", true);
  //      TransactionType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.TransactionTypeName}", true);
  //  }
  //  public Type ConnectionType { get; }
  //  public Type DataReaderType { get; }
  //  public Type ParameterType { get; }
  //  public Type CommandType { get; }
  //  public Type TransactionType { get; }
  //}

  public class DB2iSeriesProviderAdapter : IDynamicProviderAdapter {
    DB2iSeriesProviderAdapter(string assemblyName, string clientNamespace, string prefix) {
      var assembly = Tools.TryLoadAssembly(assemblyName, null);
      if (assembly == null) throw new InvalidOperationException($"Cannot load assembly {assemblyName}");
      ConnectionType = assembly.GetType($"{clientNamespace}.{prefix}Connection", true);
      DataReaderType = assembly.GetType($"{clientNamespace}.{prefix}DataReader", true);
      ParameterType = assembly.GetType($"{clientNamespace}.{prefix}Parameter", true);
      CommandType = assembly.GetType($"{clientNamespace}.{prefix}Command", true);
      TransactionType = assembly.GetType($"{clientNamespace}.{prefix}Transaction", true);
    }

    //DB2iSeriesProviderAdapter(string assemblyName, string clientNamespace, string prefix) {
    //  var assembly = Tools.TryLoadAssembly(assemblyName, null);
    //  if (assembly == null) throw new InvalidOperationException($"Cannot load assembly {assemblyName}");
    //  ConnectionType = assembly.GetType($"{clientNamespace}.{prefix}Connection", true);
    //  DataReaderType = assembly.GetType($"{clientNamespace}.{prefix}DataReader", true);
    //  ParameterType = assembly.GetType($"{clientNamespace}.{prefix}Parameter", true);
    //  CommandType = assembly.GetType($"{clientNamespace}.{prefix}Command", true);
    //  TransactionType = assembly.GetType($"{clientNamespace}.{prefix}Transaction", true);
    //}

    //DB2iSeriesProviderAdapter(Type connectionType, Type dataReaderType, Type parameterType, Type commandType, Type transactionType) {
    //  ConnectionType = connectionType;
    //  DataReaderType = dataReaderType;
    //  ParameterType = parameterType;
    //  CommandType = commandType;
    //  TransactionType = transactionType;
    //}

    //static DB2iSeriesProviderAdapter CreateInstance(string assemblyName, string clientNamespace, string prefix) {
    //  var assembly = Tools.TryLoadAssembly(assemblyName, null);
    //  if (assembly == null) throw new InvalidOperationException($"Cannot load assembly {assemblyName}");
    //  return new DB2iSeriesProviderAdapter(
    //    assembly.GetType($"{clientNamespace}.{prefix}Connection", true),
    //    assembly.GetType($"{clientNamespace}.{prefix}DataReader", true),
    //    assembly.GetType($"{clientNamespace}.{prefix}Parameter", true),
    //    assembly.GetType($"{clientNamespace}.{prefix}Command", true),
    //    assembly.GetType($"{clientNamespace}.{prefix}Transaction", true)
    //    );
    //}

    //DB2iSeriesProviderAdapter() {
    //  var assembly = Tools.TryLoadAssembly(DB2iSeriesConstants.AssemblyName, DB2iSeriesConstants.ProviderFactoryName);
    //  if (assembly == null) throw new InvalidOperationException($"Cannot load assembly {DB2iSeriesConstants.AssemblyName}");
    //  CommandType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.CommandTypeName}", true);
    //  ConnectionType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.ConnectionTypeName}", true);
    //  DataReaderType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.DataReaderTypeName}", true);
    //  ParameterType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.ParameterTypeName}", true);
    //  TransactionType = assembly.GetType($"{DB2iSeriesConstants.ClientNamespace}.{DB2iSeriesConstants.TransactionTypeName}", true);
    //}

    //static (Type connectionType, Type dataReaderType, Type parameterType, Type commandType, Type transactionType) GetTypes(string assemblyName, string clientNamespace, string prefix)
    //  => GetTypes(assemblyName, clientNamespace, $"{prefix}Connection", $"{prefix}DataReader", $"{prefix}Parameter", $"{prefix}Command", $"{prefix}Transaction");

    //static (Type connectionType, Type dataReaderType, Type parameterType, Type commandType, Type transactionType)
    //  GetTypes(string assemblyName, string clientNamespace, string connectionTypeName, string dataReaderTypeName, string parameterTypeName, string commandTypeName, string transactionTypeName) {
    //  var assembly = Tools.TryLoadAssembly(assemblyName, null);
    //  if (assembly == null) throw new InvalidOperationException($"Cannot load assembly {assemblyName}");
    //  return (
    //    assembly.GetType($"{clientNamespace}.{connectionTypeName}", true),
    //    assembly.GetType($"{clientNamespace}.{dataReaderTypeName}", true),
    //    assembly.GetType($"{clientNamespace}.{parameterTypeName}", true),
    //    assembly.GetType($"{clientNamespace}.{commandTypeName}", true),
    //    assembly.GetType($"{clientNamespace}.{transactionTypeName}", true)
    //    );
    //}

    public Type ConnectionType { get; }
    public Type DataReaderType { get; }
    public Type ParameterType { get; }
    public Type CommandType { get; }
    public Type TransactionType { get; }

    static readonly object _syncRoot = new object();
    static readonly ConcurrentDictionary<string, DB2iSeriesProviderAdapter> _instances = new ConcurrentDictionary<string, DB2iSeriesProviderAdapter>();

    static DB2iSeriesProviderAdapter GetOrAddInstance(string key, string assemblyName, string clientNamespace, string prefix) {
      if (!_instances.TryGetValue(key, out var instance)) {
        lock (_syncRoot) {
          instance = new DB2iSeriesProviderAdapter(assemblyName, clientNamespace, prefix);
          //instance = CreateInstance (assemblyName, clientNamespace, prefix);
          _instances.TryAdd(key, instance);
        }
      }
      return instance;
    }

    public static DB2iSeriesProviderAdapter GetInstance<TConnection>() where TConnection : IDbConnection => GetInstance(typeof(TConnection));
    public static DB2iSeriesProviderAdapter GetInstance(Type connectionType) => GetInstance(connectionType.Name);
    public static DB2iSeriesProviderAdapter GetInstance(IDbConnection connection) => GetInstance(connection.GetType());

    public static DB2iSeriesProviderAdapter GetInstance(string connectionTypeName) => connectionTypeName switch {
      DB2iSeriesConstants.ConnectionTypeName => GetOrAddInstance(connectionTypeName, DB2iSeriesConstants.AssemblyName, DB2iSeriesConstants.ClientNamespace, "iDB2"),
      "OdbcConnection" => GetOrAddInstance(connectionTypeName, OdbcProviderAdapter.AssemblyName, OdbcProviderAdapter.ClientNamespace, "Odbc"),
      "OleDbConnection" => GetOrAddInstance(connectionTypeName, OleDbProviderAdapter.AssemblyName, OleDbProviderAdapter.ClientNamespace, "OleDb"),
      _ => throw new NotImplementedException(connectionTypeName),
    };

    public static DB2iSeriesProviderAdapter GetInstance(DB2iSeriesConnectionType connectionType) => connectionType switch {
      DB2iSeriesConnectionType.iDB2 => GetInstance(DB2iSeriesConstants.ConnectionTypeName),
      DB2iSeriesConnectionType.Odbc  => GetInstance("OdbcConnection"),
      DB2iSeriesConnectionType.OleDb => GetInstance("OleDbConnection"),
      _ => throw new NotImplementedException(connectionType.ToString()),
    };

  }
}