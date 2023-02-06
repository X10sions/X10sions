using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using System.Data.Common;
using xFreeSql.v3_2_687.DB2iSeries.Curd;

namespace FreeSql.v3_2_687.DB2i;

public class DB2iProvider<TMark> : BaseDbProvider, IFreeSql<TMark> {
  protected DataType _dataType;
  protected string? _masterConnectionString;
  protected string[]? _slaveConnectionString;
  protected int[]? _slaveWeights;
  protected Func<DbConnection>? _connectionFactory;
  protected bool _isAutoSyncStructure = false;
  protected bool _isConfigEntityFromDbFirst = false;
  protected bool _isNoneCommandParameter = false;
  protected bool _isGenerateCommandParameterWithLambda = false;
  protected bool _isLazyLoading = false;
  protected bool _isExitAutoDisposePool = true;
  protected bool _isQuoteSqlName = true;
  protected MappingPriorityType[]? _mappingPriorityTypes;
  protected NameConvertType _nameConvertType = NameConvertType.None;
  protected Action<DbCommand>? _aopCommandExecuting = null;
  protected Action<DbCommand, string>? _aopCommandExecuted = null;

  public DB2iProvider() {
    //CodeFirst.IsAutoSyncStructure = _isAutoSyncStructure;
    //CodeFirst.IsConfigEntityFromDbFirst = _isConfigEntityFromDbFirst;
    //CodeFirst.IsNoneCommandParameter = _isNoneCommandParameter;
    //CodeFirst.IsGenerateCommandParameterWithLambda = _isGenerateCommandParameterWithLambda;
    //CodeFirst.IsLazyLoading = _isLazyLoading;

    //var select0Provider = (Select0Provider)Select<object>();
    //if (select0Provider is not null) {
    //  select0Provider._commonUtils._mappingPriorityTypes = _mappingPriorityTypes;
    //  select0Provider._commonUtils.IsQuoteSqlName = _isQuoteSqlName;
    //}

    //Aop.CommandBefore += new EventHandler<Aop.CommandBeforeEventArgs>((s, e) => _aopCommandExecuting?.Invoke(e.Command));
    //Aop.CommandAfter += new EventHandler<Aop.CommandAfterEventArgs>((s, e) => _aopCommandExecuted?.Invoke(e.Command, e.Log));

    //Aop.ConfigEntity += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToLower();
    //Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToLower();
    //CodeFirst.IsSyncStructureToLower = true;

    //Aop.ConfigEntityProperty += new EventHandler<Aop.ConfigEntityPropertyEventArgs>((s, e) => {
    //  object[]? attrs = null;
    //  try {
    //    attrs = e.Property.GetCustomAttributes(false).ToArray();
    //  } catch { }
    //  var dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.Name == "MaxLengthAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    var lenProp = dyattr.GetType().GetProperties().Where(a => a.PropertyType.IsNumberType()).FirstOrDefault();
    //    if (lenProp != null && int.TryParse(string.Concat(lenProp.GetValue(dyattr, null)), out var tryval) && tryval != 0) {
    //      e.ModifyResult.StringLength = tryval;
    //    }
    //  }
    //  dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.RequiredAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    e.ModifyResult.IsNullable = false;
    //  }
    //  dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    e.ModifyResult.IsIgnore = true;
    //  }
    //  dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.ColumnAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    var name = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "Name").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();
    //    short.TryParse(string.Concat(dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(int) && a.Name == "Order").FirstOrDefault()?.GetValue(dyattr, null)), out var order);
    //    var typeName = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "TypeName").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();

    //    if (string.IsNullOrEmpty(name) == false)
    //      e.ModifyResult.Name = name;
    //    if (order != 0)
    //      e.ModifyResult.Position = order;
    //    if (string.IsNullOrEmpty(typeName) == false)
    //      e.ModifyResult.DbType = typeName;
    //  }
    //  dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.KeyAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    e.ModifyResult.IsPrimary = true;
    //  }
    //  dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.StringLengthAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    var lenProps = dyattr.GetType().GetProperties().Where(a => a.PropertyType.IsNumberType()).ToArray();
    //    var lenProp = lenProps.Length == 1 ? lenProps.FirstOrDefault() : lenProps.Where(a => a.Name == "MaximumLength").FirstOrDefault();
    //    if (lenProp != null && int.TryParse(string.Concat(lenProp.GetValue(dyattr, null)), out var tryval) && tryval != 0) {
    //      e.ModifyResult.StringLength = tryval;
    //    }
    //  }
    //  //https://github.com/dotnetcore/FreeSql/issues/378
    //  dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    switch (string.Concat(dyattr.GetType().GetProperty("DatabaseGeneratedOption")?.GetValue(dyattr, null))) {
    //      case "Identity":
    //      case "1":
    //        e.ModifyResult.IsIdentity = true;
    //        break;
    //      default:
    //        e.ModifyResult.CanInsert = false;
    //        e.ModifyResult.CanUpdate = false;
    //        break;
    //    }
    //  }
    //});
    ////EFCore 
    //Aop.ConfigEntity += new EventHandler<Aop.ConfigEntityEventArgs>((s, e) => {
    //  object[]? attrs = null;
    //  try {
    //    attrs = e.EntityType.GetCustomAttributes(false).ToArray(); //.net core 反射存在版本冲突问题，导致该方法异常
    //  } catch { }

    //  var tableAttr = e.EntityType.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();

    //  var dyattr = attrs?.Where(a => {
    //    return ((a as Attribute)?.TypeId as Type)?.FullName == "System.ComponentModel.DataAnnotations.Schema.TableAttribute";
    //  }).FirstOrDefault();
    //  if (dyattr != null) {
    //    var name = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "Name").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();
    //    var schema = dyattr.GetType().GetProperties().Where(a => a.PropertyType == typeof(string) && a.Name == "Schema").FirstOrDefault()?.GetValue(dyattr, null)?.ToString();
    //    if (string.IsNullOrEmpty(name) == false && string.IsNullOrEmpty(schema) == false)
    //      e.ModifyResult.Name = $"{schema}.{name}";
    //    else if (string.IsNullOrEmpty(name) == false)
    //      e.ModifyResult.Name = name;
    //    else if (string.IsNullOrEmpty(schema) == false)
    //      e.ModifyResult.Name = $"{schema}.{e.ModifyResult.Name}";
    //  }
    //});
    //Ado.MasterPool.Policy.IsAutoDisposeWithSystem = _isExitAutoDisposePool;
    //Ado.SlavePools.ForEach(a => a.Policy.IsAutoDisposeWithSystem = _isExitAutoDisposePool);
    //if (_slaveWeights != null)
    //  for (var x = 0; x < _slaveWeights.Length; x++)
    //    Ado.SlavePools[x].Policy.Weight = _slaveWeights[x];
  }

  public override ISelect<T1> CreateSelectProvider<T1>(object dywhere) => new DB2iSelect<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsert<T1> CreateInsertProvider<T1>() => new DB2iInsert<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);
  public override IUpdate<T1> CreateUpdateProvider<T1>(object dywhere) => new DB2iUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IDelete<T1> CreateDeleteProvider<T1>(object dywhere) => new DB2iDelete<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsertOrUpdate<T1> CreateInsertOrUpdateProvider<T1>() => new DB2iInsertOrUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);

  public override void Dispose() {
    throw new NotImplementedException();
  }
}

public class DB2iOdbcProvider<TMark> : DB2iProvider<TMark> {
  public DB2iOdbcProvider() { }
}

public class DB2iOleDbProvider<TMark> : DB2iProvider<TMark> {
  public DB2iOleDbProvider() { }
}
