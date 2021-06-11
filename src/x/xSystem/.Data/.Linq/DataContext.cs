using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq {
  public class DataContext : IDisposable {
    private CommonDataServices services;
    private IProvider provider;
    private Dictionary<MetaTable, ITable> tables;
    private bool objectTrackingEnabled = true;
    private bool deferredLoadingEnabled = true;
    private bool disposed;
    private bool isInSubmitChanges;
    private DataLoadOptions loadOptions;
    private ChangeConflictCollection conflicts;
    private static MethodInfo _miExecuteQuery;

    internal CommonDataServices Services => CheckDispose(services);

    public DbConnection Connection => CheckDispose(provider.Connection);

    public DbTransaction Transaction {
      get => CheckDispose(provider.Transaction);
      set {
        CheckDispose();
        provider.Transaction = value;
      }
    }

    public int CommandTimeout {
      get => CheckDispose(provider.CommandTimeout);
      set {
        CheckDispose();
        provider.CommandTimeout = value;
      }
    }

    public TextWriter Log {
      get => CheckDispose(provider.Log);
      set {
        CheckDispose();
        provider.Log = value;
      }
    }

    public bool ObjectTrackingEnabled {
      get => CheckDispose(objectTrackingEnabled);
      set {
        CheckDispose();
        if (Services.HasCachedObjects) {
          throw System.Data.Linq.Error.OptionsCannotBeModifiedAfterQuery();
        }
        objectTrackingEnabled = value;
        if (!objectTrackingEnabled) {
          deferredLoadingEnabled = false;
        }
        services.ResetServices();
      }
    }

    public bool DeferredLoadingEnabled {
      get => CheckDispose(deferredLoadingEnabled);
      set {
        CheckDispose();
        if (Services.HasCachedObjects) {
          throw System.Data.Linq.Error.OptionsCannotBeModifiedAfterQuery();
        }
        if (!ObjectTrackingEnabled & value) {
          throw System.Data.Linq.Error.DeferredLoadingRequiresObjectTracking();
        }
        deferredLoadingEnabled = value;
      }
    }

    public MetaModel Mapping => CheckDispose(services.Model);

    internal IProvider Provider => CheckDispose(provider);

    public DataLoadOptions LoadOptions {
      get => CheckDispose(loadOptions);
      set {
        CheckDispose();
        if (services.HasCachedObjects && value != loadOptions) {
          throw System.Data.Linq.Error.LoadOptionsChangeNotAllowedAfterQuery();
        }
        value?.Freeze();
        loadOptions = value;
      }
    }

    public ChangeConflictCollection ChangeConflicts => CheckDispose(conflicts);

    private DataContext() {
    }

    public DataContext(string fileOrServerOrConnection) {
      if (fileOrServerOrConnection == null) {
        throw System.Data.Linq.Error.ArgumentNull("fileOrServerOrConnection");
      }
      InitWithDefaultMapping(fileOrServerOrConnection);
    }

    public DataContext(string fileOrServerOrConnection, MappingSource mapping) {
      if (fileOrServerOrConnection == null) {
        throw System.Data.Linq.Error.ArgumentNull("fileOrServerOrConnection");
      }
      if (mapping == null) {
        throw System.Data.Linq.Error.ArgumentNull("mapping");
      }
      Init(fileOrServerOrConnection, mapping);
    }

    public DataContext(IDbConnection connection) {
      if (connection == null) {
        throw System.Data.Linq.Error.ArgumentNull("connection");
      }
      InitWithDefaultMapping(connection);
    }

    public DataContext(IDbConnection connection, MappingSource mapping) {
      if (connection == null) {
        throw System.Data.Linq.Error.ArgumentNull("connection");
      }
      if (mapping == null) {
        throw System.Data.Linq.Error.ArgumentNull("mapping");
      }
      Init(connection, mapping);
    }

    internal DataContext(DataContext context) {
      if (context == null) {
        throw System.Data.Linq.Error.ArgumentNull("context");
      }
      Init(context.Connection, context.Mapping.MappingSource);
      LoadOptions = context.LoadOptions;
      Transaction = context.Transaction;
      Log = context.Log;
      CommandTimeout = context.CommandTimeout;
    }

    public void Dispose() {
      disposed = true;
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        if (provider != null) {
          provider.Dispose();
          provider = null;
        }
        services = null;
        tables = null;
        loadOptions = null;
      }
    }

    internal void CheckDispose() {
      if (disposed) {
        throw System.Data.Linq.Error.DataContextCannotBeUsedAfterDispose();
      }
    }

    internal T CheckDispose<T>(T value) {
      if (disposed) {
        throw System.Data.Linq.Error.DataContextCannotBeUsedAfterDispose();
      }
      return value;
    }

    private void InitWithDefaultMapping(object connection) => Init(connection, new AttributeMappingSource());

    internal object Clone() {
      CheckDispose();
      return Activator.CreateInstance(GetType(), Connection, Mapping.MappingSource);
    }

    private void Init(object connection, MappingSource mapping) {
      var model = mapping.GetModel(GetType());
      services = new CommonDataServices(this, model);
      conflicts = new ChangeConflictCollection();
      if (!(model.ProviderType != null)) {
        throw System.Data.Linq.Error.ProviderTypeNull();
      }
      var providerType = model.ProviderType;
      if (!typeof(IProvider).IsAssignableFrom(providerType)) {
        throw System.Data.Linq.Error.ProviderDoesNotImplementRequiredInterface(providerType, typeof(IProvider));
      }
      provider = (IProvider)Activator.CreateInstance(providerType);
      provider.Initialize(services, connection);
      tables = new Dictionary<MetaTable, ITable>();
      InitTables(this);
    }

    internal void ClearCache() {
      CheckDispose();
      services.ResetServices();
    }

    internal void VerifyTrackingEnabled() {
      CheckDispose();
      if (!ObjectTrackingEnabled) {
        throw System.Data.Linq.Error.ObjectTrackingRequired();
      }
    }

    internal void CheckNotInSubmitChanges() {
      CheckDispose();
      if (isInSubmitChanges) {
        throw System.Data.Linq.Error.CannotPerformOperationDuringSubmitChanges();
      }
    }

    internal void CheckInSubmitChanges() {
      CheckDispose();
      if (!isInSubmitChanges) {
        throw System.Data.Linq.Error.CannotPerformOperationOutsideSubmitChanges();
      }
    }

    public Table<TEntity> GetTable<TEntity>() where TEntity : class {
      CheckDispose();
      var table = services.Model.GetTable(typeof(TEntity));
      if (table == null) {
        throw System.Data.Linq.Error.TypeIsNotMarkedAsTable(typeof(TEntity));
      }
      var table2 = GetTable(table);
      if (table2.ElementType != typeof(TEntity)) {
        throw System.Data.Linq.Error.CouldNotGetTableForSubtype(typeof(TEntity), table.RowType.Type);
      }
      return (Table<TEntity>)table2;
    }

    public ITable GetTable(Type type) {
      CheckDispose();
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      var table = services.Model.GetTable(type);
      if (table == null) {
        throw System.Data.Linq.Error.TypeIsNotMarkedAsTable(type);
      }
      if (table.RowType.Type != type) {
        throw System.Data.Linq.Error.CouldNotGetTableForSubtype(type, table.RowType.Type);
      }
      return GetTable(table);
    }

    private ITable GetTable(MetaTable metaTable) {
      if (!tables.TryGetValue(metaTable, out var value)) {
        ValidateTable(metaTable);
        var type = typeof(Table<>).MakeGenericType(metaTable.RowType.Type);
        value = (ITable)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[2]
        {
        this,
        metaTable
        }, null);
        tables.Add(metaTable, value);
      }
      return value;
    }

    private static void ValidateTable(MetaTable metaTable) {
      foreach (var association in metaTable.RowType.Associations) {
        if (!association.ThisMember.DeclaringType.IsEntity) {
          throw System.Data.Linq.Error.NonEntityAssociationMapping(association.ThisMember.DeclaringType.Type, association.ThisMember.Name, association.ThisMember.DeclaringType.Type);
        }
        if (!association.OtherType.IsEntity) {
          throw System.Data.Linq.Error.NonEntityAssociationMapping(association.ThisMember.DeclaringType.Type, association.ThisMember.Name, association.OtherType.Type);
        }
      }
    }

    private void InitTables(object schema) {
      var fields = schema.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
      var array = fields;
      foreach (var fieldInfo in array) {
        var fieldType = fieldInfo.FieldType;
        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Table<>)) {
          var table = (ITable)fieldInfo.GetValue(schema);
          if (table == null) {
            var type = fieldType.GetGenericArguments()[0];
            table = GetTable(type);
            fieldInfo.SetValue(schema, table);
          }
        }
      }
    }

    public bool DatabaseExists() {
      CheckDispose();
      return provider.DatabaseExists();
    }

    public void CreateDatabase() {
      CheckDispose();
      provider.CreateDatabase();
    }

    public void DeleteDatabase() {
      CheckDispose();
      provider.DeleteDatabase();
    }

    public void SubmitChanges() {
      CheckDispose();
      SubmitChanges(ConflictMode.FailOnFirstConflict);
    }

    public virtual void SubmitChanges(ConflictMode failureMode) {
      CheckDispose();
      CheckNotInSubmitChanges();
      VerifyTrackingEnabled();
      conflicts.Clear();
      try {
        isInSubmitChanges = true;
        if (System.Transactions.Transaction.Current == null && provider.Transaction == null) {
          var flag = false;
          DbTransaction dbTransaction = null;
          try {
            if (provider.Connection.State == ConnectionState.Open) {
              provider.ClearConnection();
            }
            if (provider.Connection.State == ConnectionState.Closed) {
              provider.Connection.Open();
              flag = true;
            }
            dbTransaction = provider.Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            provider.Transaction = dbTransaction;
            new ChangeProcessor(services, this).SubmitChanges(failureMode);
            AcceptChanges();
            provider.ClearConnection();
            dbTransaction.Commit();
          } catch {
            dbTransaction?.Rollback();
            throw;
          } finally {
            provider.Transaction = null;
            if (flag) {
              provider.Connection.Close();
            }
          }
        } else {
          new ChangeProcessor(services, this).SubmitChanges(failureMode);
          AcceptChanges();
        }
      } finally {
        isInSubmitChanges = false;
      }
    }

    public void Refresh(RefreshMode mode, object entity) {
      CheckDispose();
      CheckNotInSubmitChanges();
      VerifyTrackingEnabled();
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var array = Array.CreateInstance(entity.GetType(), 1);
      array.SetValue(entity, 0);
      Refresh(mode, array);
    }

    public void Refresh(RefreshMode mode, params object[] entities) {
      CheckDispose();
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      Refresh(mode, (IEnumerable)entities);
    }

    public void Refresh(RefreshMode mode, IEnumerable entities) {
      CheckDispose();
      CheckNotInSubmitChanges();
      VerifyTrackingEnabled();
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      var list = entities.Cast<object>().ToList();
      var dataContext = CreateRefreshContext();
      foreach (var item in list) {
        var inheritanceRoot = services.Model.GetMetaType(item.GetType()).InheritanceRoot;
        GetTable(inheritanceRoot.Type);
        var trackedObject = services.ChangeTracker.GetTrackedObject(item);
        if (trackedObject == null) {
          throw System.Data.Linq.Error.UnrecognizedRefreshObject();
        }
        if (trackedObject.IsNew) {
          throw System.Data.Linq.Error.RefreshOfNewObject();
        }
        var keyValues = CommonDataServices.GetKeyValues(trackedObject.Type, trackedObject.Original);
        var objectByKey = dataContext.Services.GetObjectByKey(trackedObject.Type, keyValues);
        if (objectByKey == null) {
          throw System.Data.Linq.Error.RefreshOfDeletedObject();
        }
        trackedObject.Refresh(mode, objectByKey);
      }
    }

    internal DataContext CreateRefreshContext() {
      CheckDispose();
      return new DataContext(this);
    }

    private void AcceptChanges() {
      CheckDispose();
      VerifyTrackingEnabled();
      services.ChangeTracker.AcceptChanges();
    }

    internal string GetQueryText(IQueryable query) {
      CheckDispose();
      if (query == null) {
        throw System.Data.Linq.Error.ArgumentNull("query");
      }
      return provider.GetQueryText(query.Expression);
    }

    public DbCommand GetCommand(IQueryable query) {
      CheckDispose();
      if (query == null) {
        throw System.Data.Linq.Error.ArgumentNull("query");
      }
      return provider.GetCommand(query.Expression);
    }

    internal string GetChangeText() {
      CheckDispose();
      VerifyTrackingEnabled();
      return new ChangeProcessor(services, this).GetChangeText();
    }

    public ChangeSet GetChangeSet() {
      CheckDispose();
      return new ChangeProcessor(services, this).GetChangeSet();
    }

    public int ExecuteCommand(string command, params object[] parameters) {
      CheckDispose();
      if (command == null) {
        throw System.Data.Linq.Error.ArgumentNull("command");
      }
      if (parameters == null) {
        throw System.Data.Linq.Error.ArgumentNull("parameters");
      }
      return (int)ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), command, parameters).ReturnValue;
    }

    public IEnumerable<TResult> ExecuteQuery<TResult>(string query, params object[] parameters) {
      CheckDispose();
      if (query == null) {
        throw System.Data.Linq.Error.ArgumentNull("query");
      }
      if (parameters == null) {
        throw System.Data.Linq.Error.ArgumentNull("parameters");
      }
      return (IEnumerable<TResult>)ExecuteMethodCall(this, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)), query, parameters).ReturnValue;
    }

    public IEnumerable ExecuteQuery(Type elementType, string query, params object[] parameters) {
      CheckDispose();
      if (elementType == null) {
        throw System.Data.Linq.Error.ArgumentNull("elementType");
      }
      if (query == null) {
        throw System.Data.Linq.Error.ArgumentNull("query");
      }
      if (parameters == null) {
        throw System.Data.Linq.Error.ArgumentNull("parameters");
      }
      if (_miExecuteQuery == null) {
        _miExecuteQuery = typeof(DataContext).GetMethods().Single(delegate (MethodInfo m) {
          if (m.Name == "ExecuteQuery") {
            return m.GetParameters().Length == 2;
          }
          return false;
        });
      }
      return (IEnumerable)ExecuteMethodCall(this, _miExecuteQuery.MakeGenericMethod(elementType), query, parameters).ReturnValue;
    }

    protected internal IExecuteResult ExecuteMethodCall(object instance, MethodInfo methodInfo, params object[] parameters) {
      CheckDispose();
      if (instance == null) {
        throw System.Data.Linq.Error.ArgumentNull("instance");
      }
      if (methodInfo == null) {
        throw System.Data.Linq.Error.ArgumentNull("methodInfo");
      }
      if (parameters == null) {
        throw System.Data.Linq.Error.ArgumentNull("parameters");
      }
      return provider.Execute(GetMethodCall(instance, methodInfo, parameters));
    }

    protected internal IQueryable<TResult> CreateMethodCallQuery<TResult>(object instance, MethodInfo methodInfo, params object[] parameters) {
      CheckDispose();
      if (instance == null) {
        throw System.Data.Linq.Error.ArgumentNull("instance");
      }
      if (methodInfo == null) {
        throw System.Data.Linq.Error.ArgumentNull("methodInfo");
      }
      if (parameters == null) {
        throw System.Data.Linq.Error.ArgumentNull("parameters");
      }
      if (!typeof(IQueryable<TResult>).IsAssignableFrom(methodInfo.ReturnType)) {
        throw System.Data.Linq.Error.ExpectedQueryableArgument("methodInfo", typeof(IQueryable<TResult>));
      }
      return new DataQuery<TResult>(this, GetMethodCall(instance, methodInfo, parameters));
    }

    private Expression GetMethodCall(object instance, MethodInfo methodInfo, params object[] parameters) {
      CheckDispose();
      if (parameters.Length != 0) {
        var parameters2 = methodInfo.GetParameters();
        var list = new List<Expression>(parameters.Length);
        var i = 0;
        for (var num = parameters.Length; i < num; i++) {
          var type = parameters2[i].ParameterType;
          if (type.IsByRef) {
            type = type.GetElementType();
          }
          list.Add(Expression.Constant(parameters[i], type));
        }
        return Expression.Call(Expression.Constant(instance), methodInfo, list);
      }
      return Expression.Call(Expression.Constant(instance), methodInfo);
    }

    protected internal void ExecuteDynamicInsert(object entity) {
      CheckDispose();
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      CheckInSubmitChanges();
      var trackedObject = services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null) {
        throw System.Data.Linq.Error.CannotPerformOperationForUntrackedObject();
      }
      services.ChangeDirector.DynamicInsert(trackedObject);
    }

    protected internal void ExecuteDynamicUpdate(object entity) {
      CheckDispose();
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      CheckInSubmitChanges();
      var trackedObject = services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null) {
        throw System.Data.Linq.Error.CannotPerformOperationForUntrackedObject();
      }
      if (services.ChangeDirector.DynamicUpdate(trackedObject) == 0) {
        throw new ChangeConflictException();
      }
    }

    protected internal void ExecuteDynamicDelete(object entity) {
      CheckDispose();
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      CheckInSubmitChanges();
      var trackedObject = services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null) {
        throw System.Data.Linq.Error.CannotPerformOperationForUntrackedObject();
      }
      if (services.ChangeDirector.DynamicDelete(trackedObject) == 0) {
        throw new ChangeConflictException();
      }
    }

    public IEnumerable<TResult> Translate<TResult>(DbDataReader reader) {
      CheckDispose();
      return (IEnumerable<TResult>)Translate(typeof(TResult), reader);
    }

    public IEnumerable Translate(Type elementType, DbDataReader reader) {
      CheckDispose();
      if (elementType == null) {
        throw System.Data.Linq.Error.ArgumentNull("elementType");
      }
      if (reader == null) {
        throw System.Data.Linq.Error.ArgumentNull("reader");
      }
      return provider.Translate(elementType, reader);
    }

    public IMultipleResults Translate(DbDataReader reader) {
      CheckDispose();
      if (reader == null) {
        throw System.Data.Linq.Error.ArgumentNull("reader");
      }
      return provider.Translate(reader);
    }

    internal void ResetLoadOptions() {
      CheckDispose();
      loadOptions = null;
    }
  }

}