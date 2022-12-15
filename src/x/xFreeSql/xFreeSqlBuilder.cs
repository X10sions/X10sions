using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using System.Data.Common;

namespace FreeSql;
public partial class xFreeSqlBuilder : FreeSqlBuilder {

  xDataType _dataType;
  string? _masterConnectionString;
  string[]? _slaveConnectionString;
  int[]? _slaveWeights;
  Func<DbConnection>? _connectionFactory;
  bool _isAutoSyncStructure = false;
  bool _isSyncStructureToLower = false;
  bool _isSyncStructureToUpper = false;
  bool _isConfigEntityFromDbFirst = false;
  bool _isNoneCommandParameter = false;
  bool _isGenerateCommandParameterWithLambda = false;
  bool _isLazyLoading = false;
  bool _isExitAutoDisposePool = true;
  MappingPriorityType[]? _mappingPriorityTypes;
  StringConvertType _entityPropertyConvertType = StringConvertType.None;
  NameConvertType _nameConvertType = NameConvertType.None;
  Action<DbCommand>? _aopCommandExecuting = null;
  Action<DbCommand, string>? _aopCommandExecuted = null;
  Type? _providerType = null;

  public xFreeSqlBuilder UseConnectionString(xDataType dataType, string connectionString, Type? providerType = null) {
    UseConnectionString(DataType.Custom, connectionString, providerType);
    _dataType = dataType;
    return this;
  }

  public xFreeSqlBuilder UseConnectionFactory(xDataType dataType, Func<DbConnection> connectionFactory, Type? providerType = null) {
    UseConnectionFactory(DataType.Custom, connectionFactory, providerType);
    _dataType = dataType;
    return this;
  }

  public IFreeSql Build() => Build<IFreeSql>();

  public IFreeSql<TMark> Build<TMark>() {
    if (string.IsNullOrEmpty(_masterConnectionString) && _connectionFactory == null) throw new Exception(CoreStrings.Check_UseConnectionString);
    IFreeSql<TMark> ret = null;
    var type = _providerType;
    if (type != null) {
      if (type.IsGenericTypeDefinition)
        type = type.MakeGenericType(typeof(TMark));
    } else {
      Action<string, string> throwNotFind = (dll, providerType) => throw new Exception(CoreStrings.Missing_FreeSqlProvider_Package_Reason(dll, providerType));
      switch (_dataType) {
        case xDataType.MySql:
          type = Type.GetType("FreeSql.MySql.MySqlProvider`1,FreeSql.Provider.MySql")?.MakeGenericType(typeof(TMark)); //MySql.Data.dll
          if (type == null) type = Type.GetType("FreeSql.MySql.MySqlProvider`1,FreeSql.Provider.MySqlConnector")?.MakeGenericType(typeof(TMark)); //MySqlConnector.dll
          if (type == null) throwNotFind("FreeSql.Provider.MySql.dll", "FreeSql.MySql.MySqlProvider<>");
          break;
        case xDataType.SqlServer:
          type = Type.GetType("FreeSql.SqlServer.SqlServerProvider`1,FreeSql.Provider.SqlServer")?.MakeGenericType(typeof(TMark)); //Microsoft.Data.SqliClient.dll
          if (type == null) type = Type.GetType("FreeSql.SqlServer.SqlServerProvider`1,FreeSql.Provider.SqlServerForSystem")?.MakeGenericType(typeof(TMark)); //System.Data.SqliClient.dll
          if (type == null) throwNotFind("FreeSql.Provider.SqlServer.dll", "FreeSql.SqlServer.SqlServerProvider<>");
          break;
        case xDataType.PostgreSQL:
          type = Type.GetType("FreeSql.PostgreSQL.PostgreSQLProvider`1,FreeSql.Provider.PostgreSQL")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.PostgreSQL.dll", "FreeSql.PostgreSQL.PostgreSQLProvider<>");
          break;
        case xDataType.Oracle:
          type = Type.GetType("FreeSql.Oracle.OracleProvider`1,FreeSql.Provider.Oracle")?.MakeGenericType(typeof(TMark));
          if (type == null) type = Type.GetType("FreeSql.Oracle.OracleProvider`1,FreeSql.Provider.OracleOledb")?.MakeGenericType(typeof(TMark)); //基于 oledb 实现，解决 US7ASCII 中文乱码问题
          if (type == null) throwNotFind("FreeSql.Provider.Oracle.dll", "FreeSql.Oracle.OracleProvider<>");
          break;
        case xDataType.Sqlite:
          type = Type.GetType("FreeSql.Sqlite.SqliteProvider`1,FreeSql.Provider.Sqlite")?.MakeGenericType(typeof(TMark));
          if (type == null) type = Type.GetType("FreeSql.Sqlite.SqliteProvider`1,FreeSql.Provider.SqliteCore")?.MakeGenericType(typeof(TMark)); //Microsoft.Data.Sqlite.Core.dll
          if (type == null) throwNotFind("FreeSql.Provider.Sqlite.dll", "FreeSql.Sqlite.SqliteProvider<>");
          break;

        case xDataType.OdbcOracle:
          type = Type.GetType("FreeSql.Odbc.Oracle.OdbcOracleProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Odbc.dll", "FreeSql.Odbc.Oracle.OdbcOracleProvider<>");
          break;
        case xDataType.OdbcDB2iSeries:
          type = Type.GetType("FreeSql.Odbc.DB2iSeries.OdbcDB2iSeriesProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("xFreeSql.Provider.Odbc.dll", "FreeSql.Odbc.DB2iSeries.OdbcDB2iSeriesProvider<>");
          break;
        case xDataType.OdbcSqlServer:
          type = Type.GetType("FreeSql.Odbc.SqlServer.OdbcSqlServerProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Odbc.dll", "FreeSql.Odbc.SqlServer.OdbcSqlServerProvider<>");
          break;
        case xDataType.OdbcMySql:
          type = Type.GetType("FreeSql.Odbc.MySql.OdbcMySqlProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Odbc.dll", "FreeSql.Odbc.MySql.OdbcMySqlProvider<>");
          break;
        case xDataType.OdbcPostgreSQL:
          type = Type.GetType("FreeSql.Odbc.PostgreSQL.OdbcPostgreSQLProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Odbc.dll", "FreeSql.Odbc.PostgreSQL.OdbcPostgreSQLProvider<>");
          break;
        case xDataType.Odbc:
          type = Type.GetType("FreeSql.Odbc.Default.OdbcProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Odbc.dll", "FreeSql.Odbc.Default.OdbcProvider<>");
          break;

        case xDataType.OdbcDameng:
          type = Type.GetType("FreeSql.Odbc.Dameng.OdbcDamengProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Odbc.dll", "FreeSql.Odbc.Dameng.OdbcDamengProvider<>");
          break;

        case xDataType.MsAccess:
          type = Type.GetType("FreeSql.MsAccess.MsAccessProvider`1,FreeSql.Provider.MsAccess")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.MsAccess.dll", "FreeSql.MsAccess.MsAccessProvider<>");
          break;

        case xDataType.Dameng:
          type = Type.GetType("FreeSql.Dameng.DamengProvider`1,FreeSql.Provider.Dameng")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Dameng.dll", "FreeSql.Dameng.DamengProvider<>");
          break;

        case xDataType.OdbcKingbaseES:
          type = Type.GetType("FreeSql.Odbc.KingbaseES.OdbcKingbaseESProvider`1,FreeSql.Provider.Odbc")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Odbc.dll", "FreeSql.Odbc.KingbaseES.OdbcKingbaseESProvider<>");
          break;

        case xDataType.ShenTong:
          type = Type.GetType("FreeSql.ShenTong.ShenTongProvider`1,FreeSql.Provider.ShenTong")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.ShenTong.dll", "FreeSql.ShenTong.ShenTongProvider<>");
          break;

        case xDataType.KingbaseES:
          type = Type.GetType("FreeSql.KingbaseES.KingbaseESProvider`1,FreeSql.Provider.KingbaseES")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.KingbaseES.dll", "FreeSql.KingbaseES.KingbaseESProvider<>");
          break;

        case xDataType.Firebird:
          type = Type.GetType("FreeSql.Firebird.FirebirdProvider`1,FreeSql.Provider.Firebird")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Firebird.dll", "FreeSql.Firebird.FirebirdProvider<>");
          break;

        case xDataType.Custom:
          type = Type.GetType("FreeSql.Custom.CustomProvider`1,FreeSql.Provider.Custom")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Custom.dll", "FreeSql.Custom.CustomProvider<>");
          break;

        case xDataType.ClickHouse:
          type = Type.GetType("FreeSql.ClickHouse.ClickHouseProvider`1,FreeSql.Provider.ClickHouse")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.ClickHouse.dll", "FreeSql.ClickHouse.ClickHouseProvider<>");
          break;

        case xDataType.GBase:
          type = Type.GetType("FreeSql.GBase.GBaseProvider`1,FreeSql.Provider.GBase")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.GBase.dll", "FreeSql.GBase.GBaseProvider<>");
          break;

        case xDataType.CustomOracle:
          type = Type.GetType("FreeSql.Custom.Oracle.CustomOracleProvider`1,FreeSql.Provider.Custom")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Custom.dll", "FreeSql.Custom.Oracle.CustomOracleProvider<>");
          break;

        case xDataType.CustomSqlServer:
          type = Type.GetType("FreeSql.Custom.SqlServer.CustomSqlServerProvider`1,FreeSql.Provider.Custom")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Custom.dll", "FreeSql.Custom.SqlServer.CustomSqlServerProvider<>");
          break;

        case xDataType.CustomMySql:
          type = Type.GetType("FreeSql.Custom.MySql.CustomMySqlProvider`1,FreeSql.Provider.Custom")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Custom.dll", "FreeSql.Custom.MySql.CustomMySqlProvider<>");
          break;

        case xDataType.CustomPostgreSQL:
          type = Type.GetType("FreeSql.Custom.PostgreSQL.CustomPostgreSQLProvider`1,FreeSql.Provider.Custom")?.MakeGenericType(typeof(TMark));
          if (type == null) throwNotFind("FreeSql.Provider.Custom.dll", "FreeSql.Custom.PostgreSQL.CustomPostgreSQLProvider<>");
          break;

        default: throw new Exception(CoreStrings.NotSpecified_UseConnectionString_UseConnectionFactory);
      }
    }
    ret = Activator.CreateInstance(type, new object[] { _masterConnectionString, _slaveConnectionString, _connectionFactory }) as IFreeSql<TMark>;
    if (ret != null) {
      ret.CodeFirst.IsAutoSyncStructure = _isAutoSyncStructure;

      ret.CodeFirst.IsSyncStructureToLower = _isSyncStructureToLower;
      ret.CodeFirst.IsSyncStructureToUpper = _isSyncStructureToUpper;
      ret.CodeFirst.IsConfigEntityFromDbFirst = _isConfigEntityFromDbFirst;
      ret.CodeFirst.IsNoneCommandParameter = _isNoneCommandParameter;
      ret.CodeFirst.IsGenerateCommandParameterWithLambda = _isGenerateCommandParameterWithLambda;
      ret.CodeFirst.IsLazyLoading = _isLazyLoading;

      if (_mappingPriorityTypes != null)
        (ret.Select<object>() as Select0Provider)._commonUtils._mappingPriorityTypes = _mappingPriorityTypes;

      if (_aopCommandExecuting != null)
        ret.Aop.CommandBefore += new EventHandler<Aop.CommandBeforeEventArgs>((s, e) => _aopCommandExecuting?.Invoke(e.Command));
      if (_aopCommandExecuted != null)
        ret.Aop.CommandAfter += new EventHandler<Aop.CommandAfterEventArgs>((s, e) => _aopCommandExecuted?.Invoke(e.Command, e.Log));

      this.EntityPropertyNameConvert(ret);
      //添加实体属性名全局AOP转换处理
      if (_nameConvertType != NameConvertType.None) {
        string PascalCaseToUnderScore(string str) => string.IsNullOrWhiteSpace(str) ? str : string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        //string UnderScorePascalCase(string str) => string.IsNullOrWhiteSpace(str) ? str : string.Join("", str.Split('_').Select(a => a.Length > 0 ? string.Concat(char.ToUpper(a[0]), a.Substring(1)) : ""));

        switch (_nameConvertType) {
          case NameConvertType.ToLower:
            ret.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToLower();
            ret.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToLower();
            ret.CodeFirst.IsSyncStructureToLower = true;
            break;
          case NameConvertType.ToUpper:
            ret.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToUpper();
            ret.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToUpper();
            ret.CodeFirst.IsSyncStructureToUpper = true;
            break;
          case NameConvertType.PascalCaseToUnderscore:
            ret.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name);
            ret.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name);
            break;
          case NameConvertType.PascalCaseToUnderscoreWithLower:
            ret.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToLower();
            ret.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToLower();
            break;
          case NameConvertType.PascalCaseToUnderscoreWithUpper:
            ret.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToUpper();
            ret.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToUpper();
            break;
          //case NameConvertType.UnderscoreToPascalCase:
          //    ret.Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = UnderScorePascalCase(e.ModifyResult.Name);
          //    ret.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = UnderScorePascalCase(e.ModifyResult.Name);
          //    break;
          default:
            break;
        }
      }
      //处理 MaxLength、EFCore 特性
      ret.Aop.ConfigEntityProperty += new EventHandler<Aop.ConfigEntityPropertyEventArgs>((s, e) => {
        object[] attrs = null;
        try {
          attrs = e.Property.GetCustomAttributes(false).ToArray(); //.net core 反射存在版本冲突问题，导致该方法异常
        } catch { }

        var dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.Name == "MaxLengthAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          var lenProp = dyattr.GetType().GetProperties().Where(a => a.PropertyType.IsNumberType()).FirstOrDefault();
          if (lenProp != null && int.TryParse(string.Concat(lenProp.GetValue(dyattr, null)), out var tryval) && tryval != 0) {
            e.ModifyResult.StringLength = tryval;
          }
        }

        dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.RequiredAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          e.ModifyResult.IsNullable = false;
        }

        dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          e.ModifyResult.IsIgnore = true;
        }

        dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.ColumnAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          var name = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "Name").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();
          short.TryParse(string.Concat(dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(int) && a.Name == "Order").FirstOrDefault()?.GetValue(dyattr, null)), out var order);
          var typeName = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "TypeName").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();

          if (string.IsNullOrEmpty(name) == false)
            e.ModifyResult.Name = name;
          if (order != 0)
            e.ModifyResult.Position = order;
          if (string.IsNullOrEmpty(typeName) == false)
            e.ModifyResult.DbType = typeName;
        }

        dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.KeyAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          e.ModifyResult.IsPrimary = true;
        }

        dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.StringLengthAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          var lenProps = dyattr.GetType().GetProperties().Where(a => a.PropertyType.IsNumberType()).ToArray();
          var lenProp = lenProps.Length == 1 ? lenProps.FirstOrDefault() : lenProps.Where(a => a.Name == "MaximumLength").FirstOrDefault();
          if (lenProp != null && int.TryParse(string.Concat(lenProp.GetValue(dyattr, null)), out var tryval) && tryval != 0) {
            e.ModifyResult.StringLength = tryval;
          }
        }

        //https://github.com/dotnetcore/FreeSql/issues/378
        dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          switch (string.Concat(dyattr.GetType().GetProperty("DatabaseGeneratedOption")?.GetValue(dyattr, null))) {
            case "Identity":
            case "1":
              e.ModifyResult.IsIdentity = true;
              break;
            default:
              e.ModifyResult.CanInsert = false;
              e.ModifyResult.CanUpdate = false;
              break;
          }
        }
      });
      //EFCore 特性
      ret.Aop.ConfigEntity += new EventHandler<Aop.ConfigEntityEventArgs>((s, e) => {
        object[] attrs = null;
        try {
          attrs = e.EntityType.GetCustomAttributes(false).ToArray(); //.net core 反射存在版本冲突问题，导致该方法异常
        } catch { }

        var dyattr = attrs?.Where(a => {
          return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.TableAttribute";
        }).FirstOrDefault();
        if (dyattr != null) {
          var name = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "Name").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();
          var schema = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "Schema").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();
          if (string.IsNullOrEmpty(name) == false && string.IsNullOrEmpty(schema) == false)
            e.ModifyResult.Name = $"{schema}.{name}";
          else if (string.IsNullOrEmpty(name) == false)
            e.ModifyResult.Name = name;
          else if (string.IsNullOrEmpty(schema) == false)
            e.ModifyResult.Name = $"{schema}.{e.ModifyResult.Name}";
        }
      });

      ret.Ado.MasterPool.Policy.IsAutoDisposeWithSystem = _isExitAutoDisposePool;
      ret.Ado.SlavePools.ForEach(a => a.Policy.IsAutoDisposeWithSystem = _isExitAutoDisposePool);
      if (_slaveWeights != null)
        for (var x = 0; x < _slaveWeights.Length; x++)
          ret.Ado.SlavePools[x].Policy.Weight = _slaveWeights[x];
    }

    return ret;
  }

  [Obsolete]
  void EntityPropertyNameConvert(IFreeSql fsql) {
    if (_entityPropertyConvertType != StringConvertType.None) {
      string PascalCaseToUnderScore(string str) => string.IsNullOrWhiteSpace(str) ? str : string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
      switch (_entityPropertyConvertType) {
        case StringConvertType.Lower:
          fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToLower();
          break;
        case StringConvertType.Upper:
          fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToUpper();
          break;
        case StringConvertType.PascalCaseToUnderscore:
          fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name);
          break;
        case StringConvertType.PascalCaseToUnderscoreWithLower:
          fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToLower();
          break;
        case StringConvertType.PascalCaseToUnderscoreWithUpper:
          fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToUpper();
          break;
        default:
          break;
      }
    }
  }

}
