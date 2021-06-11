//using LinqToDB.Common;
//using LinqToDB.Configuration;
using LinqToDB.Data;
//using LinqToDB.Extensions;
using System;
using System.Collections.Concurrent;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.RoyChase{

  public static class DB2iSeriesTools {

    //static readonly DB2iSeriesDataProvider_V2_9 _db2iDataProvider = new DB2iSeriesDataProvider_V2_9(DB2iSeriesProviderName.DB2, DB2iSeriesLevels.Any, false);
    //static readonly DB2iSeriesDataProvider_V2_9 _db2iDataProvider_gas = new DB2iSeriesDataProvider_V2_9(DB2iSeriesProviderName.DB2_GAS, DB2iSeriesLevels.Any, true);
    //static readonly DB2iSeriesDataProvider_V2_9 _db2iDataProvider_73 = new DB2iSeriesDataProvider_V2_9(DB2iSeriesProviderName.DB2_73, DB2iSeriesVersion.v7r3, false);
    //static readonly DB2iSeriesDataProvider_V2_9 _db2iDataProvider_73_gas = new DB2iSeriesDataProvider_V2_9(DB2iSeriesProviderName.DB2_73_GAS, DB2iSeriesLevels.V7_1_38, true);


    public static bool AutoDetectProvider { get; set; }

    static DB2iSeriesTools() {
      //AutoDetectProvider = true;

      //DataConnection.AddDataProvider(new DB2iSeriesProviderName.DB2, _db2iDataProvider);
      //DataConnection.AddDataProvider(DB2iSeriesProviderName.DB2_GAS, _db2iDataProvider_gas);
      //DataConnection.AddDataProvider(DB2iSeriesProviderName.DB2_73, _db2iDataProvider_73);
      //DataConnection.AddDataProvider(DB2iSeriesProviderName.DB2_73_GAS, _db2iDataProvider_73_gas);
      //DataConnection.AddProviderDetector(ProviderDetector);
    }

    //private static IDataProvider ProviderDetector(IConnectionStringSettings css, string connectionString) {
    //  if (css.IsGlobal) {
    //    return null;
    //  }
    //  if (DB2iSeriesProviderName.AllNames.Contains(css.Name) || new[] { DB2iSeriesProviderName.DB2, AssemblyName }.Contains(css.ProviderName)) {
    //    switch (css.Name) {
    //      case DB2iSeriesProviderName.DB2_73:
    //        return _db2iDataProvider_73;
    //      //case DB2iSeriesProviderName.DB2_GAS:
    //      //  return _db2iDataProvider_gas;
    //      //case DB2iSeriesProviderName.DB2_73_GAS:
    //      //  return _db2iDataProvider_73_gas;
    //    }
    //    if (AutoDetectProvider) {
    //      try {
    //        var connectionType = Type.GetType(ConnectionTypeName, true);
    //        var serverVersionProp = connectionType.GetPropertiesEx(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name == "ServerVersion");
    //        if (serverVersionProp != null) {
    //          var connectionCreator = DynamicDataProviderBase.CreateConnectionExpression(connectionType).Compile();
    //          var cs = string.IsNullOrWhiteSpace(connectionString) ? css.ConnectionString : connectionString;
    //          using (var conn = connectionCreator(cs)) {
    //            conn.Open();
    //            var version = Expression.Lambda<Func<object>>(Expression.Convert(Expression.MakeMemberAccess(Expression.Constant(conn), serverVersionProp), typeof(object))).Compile()();
    //            var serverVersion = version.ToString().Substring(0, 5);
    //            string ptf;
    //            int desiredLevel;
    //            switch (serverVersion) {
    //              case "07.03":
    //                return _db2iDataProvider_73;
    //              case "07.02":
    //                ptf = "SF99702";
    //                desiredLevel = 9;
    //                break;
    //              case "07.01":
    //                ptf = "SF99701";
    //                desiredLevel = 38;
    //                break;
    //              default:
    //                return _db2iDataProvider;
    //            }
    //            using (var cmd = conn.CreateCommand()) {
    //              cmd.CommandText = "SELECT MAX(PTF_GROUP_LEVEL) FROM QSYS2.GROUP_PTF_INFO WHERE PTF_GROUP_NAME = @p1 AND PTF_GROUP_STATUS = 'INSTALLED'";
    //              var param = cmd.CreateParameter();
    //              param.ParameterName = "p1";
    //              param.Value = ptf;
    //              cmd.Parameters.Add(param);
    //              var level = Converter.ChangeTypeTo<int>(cmd.ExecuteScalar());
    //              return level < desiredLevel ? _db2iDataProvider : _db2iDataProvider_73;
    //            }
    //          }
    //        }
    //      } catch (Exception) {
    //        throw;
    //      }
    //    }
    //  }
    //  return null;
    //}

    #region OnInitialized

    private static bool _isInitialized;
    private static readonly object _syncAfterInitialized = new object();
    private static ConcurrentBag<Action> _afterInitializedActions = new ConcurrentBag<Action>();

    internal static void Initialized() {
      if (!_isInitialized) {
        lock (_syncAfterInitialized) {
          if (!_isInitialized) {
            _isInitialized = true;
            foreach (var action in _afterInitializedActions) {
              action();
            }
            _afterInitializedActions = null;
          }
        }
      }
    }

    public static void AfterInitialized(Action action) {
      if (_isInitialized) {
        action();
      } else {
        lock (_syncAfterInitialized) {
          if (_isInitialized) {
            action();
          } else {
            _afterInitializedActions.Add(action);
          }
        }
      }
    }

    #endregion

    #region BulkCopy

    public static BulkCopyType DefaultBulkCopyType = BulkCopyType.MultipleRows;

    #endregion

  }
}