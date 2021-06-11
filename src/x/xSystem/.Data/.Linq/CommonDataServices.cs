using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Data.Linq {
  internal class CommonDataServices : IDataServices {
    private class DeferredSourceFactory<T> : IDeferredSourceFactory {
      private class DeferredSource : IEnumerable<T> {
        private DeferredSourceFactory<T> factory;

        private object instance;

        internal DeferredSource(DeferredSourceFactory<T> factory, object instance) {
          this.factory = factory;
          this.instance = instance;
        }

        public IEnumerator<T> GetEnumerator() {
          var array = instance as object[];
          if (array != null) {
            return factory.ExecuteKeys(array);
          }
          return factory.Execute(instance);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
      }

      private MetaDataMember member;
      private CommonDataServices services;
      private ICompiledQuery query;
      private bool refersToPrimaryKey;
      private T[] empty;

      internal DeferredSourceFactory(MetaDataMember member, CommonDataServices services) {
        this.member = member;
        this.services = services;
        refersToPrimaryKey = (this.member.IsAssociation && this.member.Association.OtherKeyIsPrimaryKey);
        empty = new T[0];
      }

      public IEnumerable CreateDeferredSource(object instance) {
        if (instance == null) {
          throw System.Data.Linq.Error.ArgumentNull("instance");
        }
        return new DeferredSource(this, instance);
      }

      public IEnumerable CreateDeferredSource(object[] keyValues) {
        if (keyValues == null) {
          throw System.Data.Linq.Error.ArgumentNull("keyValues");
        }
        return new DeferredSource(this, keyValues);
      }

      private IEnumerator<T> Execute(object instance) {
        ReadOnlyCollection<MetaDataMember> readOnlyCollection = null;
        readOnlyCollection = ((!member.IsAssociation) ? member.DeclaringType.IdentityMembers : member.Association.ThisKey);
        var array = new object[readOnlyCollection.Count];
        var i = 0;
        for (var count = readOnlyCollection.Count; i < count; i++) {
          var obj = array[i] = readOnlyCollection[i].StorageAccessor.GetBoxedValue(instance);
        }
        if (HasNullForeignKey(array)) {
          return ((IEnumerable<T>)empty).GetEnumerator();
        }
        if (TryGetCachedObject(array, out var cached)) {
          return ((IEnumerable<T>)new T[1]
          {
          cached
          }).GetEnumerator();
        }
        if (member.LoadMethod != null) {
          try {
            var obj2 = member.LoadMethod.Invoke(services.Context, new object[1]
            {
            instance
            });
            if (!typeof(T).IsAssignableFrom(member.LoadMethod.ReturnType)) {
              return ((IEnumerable<T>)obj2).GetEnumerator();
            }
            return ((IEnumerable<T>)new T[1]
            {
            (T)obj2
            }).GetEnumerator();
          } catch (TargetInvocationException ex) {
            if (ex.InnerException != null) {
              throw ex.InnerException;
            }
            throw;
          }
        }
        return ExecuteKeyQuery(array);
      }

      private IEnumerator<T> ExecuteKeys(object[] keyValues) {
        if (HasNullForeignKey(keyValues)) {
          return ((IEnumerable<T>)empty).GetEnumerator();
        }
        if (TryGetCachedObject(keyValues, out var cached)) {
          return ((IEnumerable<T>)new T[1]
          {
          cached
          }).GetEnumerator();
        }
        return ExecuteKeyQuery(keyValues);
      }

      private bool HasNullForeignKey(object[] keyValues) {
        if (refersToPrimaryKey) {
          var flag = false;
          var i = 0;
          for (var num = keyValues.Length; i < num; i++) {
            flag |= (keyValues[i] == null);
          }
          if (flag) {
            return true;
          }
        }
        return false;
      }

      private bool TryGetCachedObject(object[] keyValues, out T cached) {
        cached = default(T);
        if (refersToPrimaryKey) {
          var type = member.IsAssociation ? member.Association.OtherType : member.DeclaringType;
          var cachedObject = services.GetCachedObject(type, keyValues);
          if (cachedObject != null) {
            cached = (T)cachedObject;
            return true;
          }
        }
        return false;
      }

      private IEnumerator<T> ExecuteKeyQuery(object[] keyValues) {
        if (query == null) {
          var parameterExpression = Expression.Parameter(typeof(object[]), "keys");
          var array = new Expression[keyValues.Length];
          var readOnlyCollection = member.IsAssociation ? member.Association.OtherKey : member.DeclaringType.IdentityMembers;
          var i = 0;
          for (var num = keyValues.Length; i < num; i++) {
            var metaDataMember = readOnlyCollection[i];
            array[i] = Expression.Convert(Expression.ArrayIndex(parameterExpression, Expression.Constant(i)), metaDataMember.Type);
          }
          var dataMemberQuery = services.GetDataMemberQuery(member, array);
          var lambdaExpression = Expression.Lambda(dataMemberQuery, parameterExpression);
          query = services.Context.Provider.Compile(lambdaExpression);
        }
        return ((IEnumerable<T>)query.Execute(services.Context.Provider, new object[1]
        {
        keyValues
        }).ReturnValue).GetEnumerator();
      }
    }

    private IdentityManager identifier;
    private ChangeTracker tracker;
    private ChangeDirector director;
    private Dictionary<MetaDataMember, IDeferredSourceFactory> factoryMap;
    public DataContext Context { get; }
    public MetaModel Model { get; private set; }
    internal IdentityManager IdentityManager => identifier;
    internal ChangeTracker ChangeTracker => tracker;
    internal ChangeDirector ChangeDirector => director;
    internal bool HasCachedObjects { get; private set; }

    internal CommonDataServices(DataContext context, MetaModel model) {
      Context = context;
      Model = model;
      var asReadOnly = !context.ObjectTrackingEnabled;
      identifier = IdentityManager.CreateIdentityManager(asReadOnly);
      tracker = ChangeTracker.CreateChangeTracker(this, asReadOnly);
      director = ChangeDirector.CreateChangeDirector(context);
      factoryMap = new Dictionary<MetaDataMember, IDeferredSourceFactory>();
    }

    internal void SetModel(MetaModel model) => Model = model;

    internal IEnumerable<RelatedItem> GetParents(MetaType type, object item) => GetRelations(type, item, true);

    internal IEnumerable<RelatedItem> GetChildren(MetaType type, object item) => GetRelations(type, item, false);

    private IEnumerable<RelatedItem> GetRelations(MetaType type, object item, bool isForeignKey) {
      foreach (var persistentDataMember in type.PersistentDataMembers) {
        if (persistentDataMember.IsAssociation) {
          var otherType = persistentDataMember.Association.OtherType;
          if (persistentDataMember.Association.IsForeignKey == isForeignKey) {
            var value = (!persistentDataMember.IsDeferred) ? persistentDataMember.StorageAccessor.GetBoxedValue(item) : persistentDataMember.DeferredValueAccessor.GetBoxedValue(item);
            if (value != null) {
              if (persistentDataMember.Association.IsMany) {
                var enumerable = (IEnumerable)value;
                foreach (var item2 in enumerable) {
                  yield return new RelatedItem(otherType.GetInheritanceType(item2.GetType()), item2);
                }
              } else {
                yield return new RelatedItem(otherType.GetInheritanceType(value.GetType()), value);
              }
            }
          }
        }
      }
    }

    internal void ResetServices() {
      HasCachedObjects = false;
      var asReadOnly = !Context.ObjectTrackingEnabled;
      identifier = IdentityManager.CreateIdentityManager(asReadOnly);
      tracker = ChangeTracker.CreateChangeTracker(this, asReadOnly);
      factoryMap = new Dictionary<MetaDataMember, IDeferredSourceFactory>();
    }

    internal static object[] GetKeyValues(MetaType type, object instance) {
      var list = new List<object>();
      foreach (var identityMember in type.IdentityMembers) {
        list.Add(identityMember.MemberAccessor.GetBoxedValue(instance));
      }
      return list.ToArray();
    }

    internal static object[] GetForeignKeyValues(MetaAssociation association, object instance) {
      var list = new List<object>();
      foreach (var item in association.ThisKey) {
        list.Add(item.MemberAccessor.GetBoxedValue(instance));
      }
      return list.ToArray();
    }

    internal object GetCachedObject(MetaType type, object[] keyValues) {
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      if (!type.IsEntity) {
        return null;
      }
      return identifier.Find(type, keyValues);
    }

    internal object GetCachedObjectLike(MetaType type, object instance) {
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      if (!type.IsEntity) {
        return null;
      }
      return identifier.FindLike(type, instance);
    }

    public bool IsCachedObject(MetaType type, object instance) {
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      if (!type.IsEntity) {
        return false;
      }
      return identifier.FindLike(type, instance) == instance;
    }

    public object InsertLookupCachedObject(MetaType type, object instance) {
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      HasCachedObjects = true;
      if (!type.IsEntity) {
        return instance;
      }
      return identifier.InsertLookup(type, instance);
    }

    public bool RemoveCachedObjectLike(MetaType type, object instance) {
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      if (!type.IsEntity) {
        return false;
      }
      return identifier.RemoveLike(type, instance);
    }

    public void OnEntityMaterialized(MetaType type, object instance) {
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      tracker.FastTrack(instance);
      if (type.HasAnyLoadMethod) {
        SendOnLoaded(type, instance);
      }
    }

    private static void SendOnLoaded(MetaType type, object item) {
      if (type != null) {
        SendOnLoaded(type.InheritanceBase, item);
        if (type.OnLoadedMethod != null) {
          try {
            type.OnLoadedMethod.Invoke(item, new object[0]);
          } catch (TargetInvocationException ex) {
            if (ex.InnerException != null) {
              throw ex.InnerException;
            }
            throw;
          }
        }
      }
    }

    internal Expression GetObjectQuery(MetaType type, object[] keyValues) {
      if (type == null) {
        throw System.Data.Linq.Error.ArgumentNull("type");
      }
      if (keyValues == null) {
        throw System.Data.Linq.Error.ArgumentNull("keyValues");
      }
      return GetObjectQuery(type, BuildKeyExpressions(keyValues, type.IdentityMembers));
    }

    internal Expression GetObjectQuery(MetaType type, Expression[] keyValues) {
      var table = Context.GetTable(type.InheritanceRoot.Type);
      var parameterExpression = Expression.Parameter(table.ElementType, "p");
      Expression expression = null;
      var i = 0;
      for (var count = type.IdentityMembers.Count; i < count; i++) {
        var metaDataMember = type.IdentityMembers[i];
        Expression left = (metaDataMember.Member is FieldInfo) ? Expression.Field(parameterExpression, (FieldInfo)metaDataMember.Member) : Expression.Property(parameterExpression, (PropertyInfo)metaDataMember.Member);
        Expression expression2 = Expression.Equal(left, keyValues[i]);
        expression = ((expression != null) ? Expression.And(expression, expression2) : expression2);
      }
      return Expression.Call(typeof(Queryable), "Where", new Type[1]
      {
      table.ElementType
      }, table.Expression, Expression.Lambda(expression, parameterExpression));
    }

    internal Expression GetDataMemberQuery(MetaDataMember member, Expression[] keyValues) {
      if (member == null) {
        throw System.Data.Linq.Error.ArgumentNull("member");
      }
      if (keyValues == null) {
        throw System.Data.Linq.Error.ArgumentNull("keyValues");
      }
      if (member.IsAssociation) {
        var association = member.Association;
        var type = association.ThisMember.DeclaringType.InheritanceRoot.Type;
        Expression expression = Expression.Constant(Context.GetTable(type));
        if (type != association.ThisMember.DeclaringType.Type) {
          expression = Expression.Call(typeof(Enumerable), "Cast", new Type[1]
          {
          association.ThisMember.DeclaringType.Type
          }, expression);
        }
        Expression thisInstance = Expression.Call(typeof(Enumerable), "FirstOrDefault", new Type[1]
        {
        association.ThisMember.DeclaringType.Type
        }, Translator.WhereClauseFromSourceAndKeys(expression, association.ThisKey.ToArray(), keyValues));
        Expression expression2 = Expression.Constant(Context.GetTable(association.OtherType.InheritanceRoot.Type));
        if (association.OtherType.Type != association.OtherType.InheritanceRoot.Type) {
          expression2 = Expression.Call(typeof(Enumerable), "Cast", new Type[1]
          {
          association.OtherType.Type
          }, expression2);
        }
        return Translator.TranslateAssociation(Context, association, expression2, keyValues, thisInstance);
      }
      var objectQuery = GetObjectQuery(member.DeclaringType, keyValues);
      var elementType = TypeSystem.GetElementType(objectQuery.Type);
      var parameterExpression = Expression.Parameter(elementType, "p");
      Expression expression3 = parameterExpression;
      if (elementType != member.DeclaringType.Type) {
        expression3 = Expression.Convert(expression3, member.DeclaringType.Type);
      }
      Expression body = (member.Member is PropertyInfo) ? Expression.Property(expression3, (PropertyInfo)member.Member) : Expression.Field(expression3, (FieldInfo)member.Member);
      var lambdaExpression = Expression.Lambda(body, parameterExpression);
      return Expression.Call(typeof(Queryable), "Select", new Type[2]
      {
      elementType,
      lambdaExpression.Body.Type
      }, objectQuery, lambdaExpression);
    }

    private static Expression[] BuildKeyExpressions(object[] keyValues, ReadOnlyCollection<MetaDataMember> keyMembers) {
      var array = new Expression[keyValues.Length];
      var i = 0;
      for (var count = keyMembers.Count; i < count; i++) {
        var metaDataMember = keyMembers[i];
        var expression = array[i] = Expression.Constant(keyValues[i], metaDataMember.Type);
      }
      return array;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public IDeferredSourceFactory GetDeferredSourceFactory(MetaDataMember member) {
      if (member == null) {
        throw System.Data.Linq.Error.ArgumentNull("member");
      }
      if (factoryMap.TryGetValue(member, out var value)) {
        return value;
      }
      var type = (member.IsAssociation && member.Association.IsMany) ? TypeSystem.GetElementType(member.Type) : member.Type;
      value = (IDeferredSourceFactory)Activator.CreateInstance(typeof(DeferredSourceFactory<>).MakeGenericType(type), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[2]
      {
      member,
      this
      }, null);
      factoryMap.Add(member, value);
      return value;
    }

    public object GetCachedObject(Expression query) {
      if (query == null) {
        return null;
      }
      var methodCallExpression = query as MethodCallExpression;
      if (methodCallExpression == null || methodCallExpression.Arguments.Count < 1 || methodCallExpression.Arguments.Count > 2) {
        return null;
      }
      if (!(methodCallExpression.Method.DeclaringType != typeof(Queryable))) {
        switch (methodCallExpression.Method.Name) {
          default:
            return null;
          case "Where":
          case "First":
          case "FirstOrDefault":
          case "Single":
          case "SingleOrDefault": {
              if (methodCallExpression.Arguments.Count == 1) {
                return GetCachedObject(methodCallExpression.Arguments[0]);
              }
              var unaryExpression = methodCallExpression.Arguments[1] as UnaryExpression;
              if (unaryExpression == null || unaryExpression.NodeType != ExpressionType.Quote) {
                return null;
              }
              var lambdaExpression = unaryExpression.Operand as LambdaExpression;
              if (lambdaExpression == null) {
                return null;
              }
              var constantExpression = methodCallExpression.Arguments[0] as ConstantExpression;
              if (constantExpression == null) {
                return null;
              }
              var table = constantExpression.Value as ITable;
              if (table == null) {
                return null;
              }
              var elementType = TypeSystem.GetElementType(query.Type);
              if (elementType != table.ElementType) {
                return null;
              }
              var table2 = Model.GetTable(table.ElementType);
              var keyValues = GetKeyValues(table2.RowType, lambdaExpression);
              if (keyValues != null) {
                return GetCachedObject(table2.RowType, keyValues);
              }
              return null;
            }
        }
      }
      return null;
    }

    internal object[] GetKeyValues(MetaType type, LambdaExpression predicate) {
      if (predicate == null) {
        throw System.Data.Linq.Error.ArgumentNull("predicate");
      }
      if (predicate.Parameters.Count != 1) {
        return null;
      }
      var dictionary = new Dictionary<MetaDataMember, object>();
      if (GetKeysFromPredicate(type, dictionary, predicate.Body) && dictionary.Count == type.IdentityMembers.Count) {
        return (from kv in dictionary
                orderby kv.Key.Ordinal
                select kv.Value).ToArray();
      }
      return null;
    }

    private bool GetKeysFromPredicate(MetaType type, Dictionary<MetaDataMember, object> keys, Expression expr) {
      var binaryExpression = expr as BinaryExpression;
      if (binaryExpression == null) {
        var methodCallExpression = expr as MethodCallExpression;
        if (methodCallExpression == null || !(methodCallExpression.Method.Name == "op_Equality") || methodCallExpression.Arguments.Count != 2) {
          return false;
        }
        binaryExpression = Expression.Equal(methodCallExpression.Arguments[0], methodCallExpression.Arguments[1]);
      }
      switch (binaryExpression.NodeType) {
        case ExpressionType.And:
          if (GetKeysFromPredicate(type, keys, binaryExpression.Left)) {
            return GetKeysFromPredicate(type, keys, binaryExpression.Right);
          }
          return false;
        case ExpressionType.Equal:
          if (!GetKeyFromPredicate(type, keys, binaryExpression.Left, binaryExpression.Right)) {
            return GetKeyFromPredicate(type, keys, binaryExpression.Right, binaryExpression.Left);
          }
          return true;
        default:
          return false;
      }
    }

    private static bool GetKeyFromPredicate(MetaType type, Dictionary<MetaDataMember, object> keys, Expression mex, Expression vex) {
      var memberExpression = mex as MemberExpression;
      if (memberExpression == null || memberExpression.Expression == null || memberExpression.Expression.NodeType != ExpressionType.Parameter || memberExpression.Expression.Type != type.Type) {
        return false;
      }
      if (!type.Type.IsAssignableFrom(memberExpression.Member.ReflectedType) && !memberExpression.Member.ReflectedType.IsAssignableFrom(type.Type)) {
        return false;
      }
      var dataMember = type.GetDataMember(memberExpression.Member);
      if (!dataMember.IsPrimaryKey) {
        return false;
      }
      if (keys.ContainsKey(dataMember)) {
        return false;
      }
      var constantExpression = vex as ConstantExpression;
      if (constantExpression != null) {
        keys.Add(dataMember, constantExpression.Value);
        return true;
      }
      var invocationExpression = vex as InvocationExpression;
      if (invocationExpression != null && invocationExpression.Arguments != null && invocationExpression.Arguments.Count == 0) {
        var constantExpression2 = invocationExpression.Expression as ConstantExpression;
        if (constantExpression2 != null) {
          keys.Add(dataMember, ((Delegate)constantExpression2.Value).DynamicInvoke());
          return true;
        }
      }
      return false;
    }

    internal object GetObjectByKey(MetaType type, object[] keyValues) {
      var obj = GetCachedObject(type, keyValues);
      if (obj == null) {
        obj = ((IEnumerable)Context.Provider.Execute(GetObjectQuery(type, keyValues)).ReturnValue).OfType<object>().SingleOrDefault();
      }
      return obj;
    }
  }

}