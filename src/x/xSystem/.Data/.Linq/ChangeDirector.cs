using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Data.Linq {
  internal abstract class ChangeDirector {
    internal class StandardChangeDirector : ChangeDirector {
      private enum UpdateType {
        Insert,
        Update,
        Delete
      }

      private enum AutoSyncBehavior {
        ApplyNewAutoSync,
        RollbackSavedValues
      }

      private DataContext context;

      private List<KeyValuePair<TrackedObject, object[]>> syncRollbackItems;

      private List<KeyValuePair<TrackedObject, object[]>> SyncRollbackItems {
        get {
          if (syncRollbackItems == null) {
            syncRollbackItems = new List<KeyValuePair<TrackedObject, object[]>>();
          }
          return syncRollbackItems;
        }
      }

      internal StandardChangeDirector(DataContext context) {
        this.context = context;
      }

      internal override int Insert(TrackedObject item) {
        if (item.Type.Table.InsertMethod != null) {
          try {
            item.Type.Table.InsertMethod.Invoke(context, new object[1]
            {
            item.Current
            });
          } catch (TargetInvocationException ex) {
            if (ex.InnerException != null) {
              throw ex.InnerException;
            }
            throw;
          }
          return 1;
        }
        return DynamicInsert(item);
      }

      internal override int DynamicInsert(TrackedObject item) {
        var insertCommand = GetInsertCommand(item);
        if (insertCommand.Type == typeof(int)) {
          return (int)context.Provider.Execute(insertCommand).ReturnValue;
        }
        var source = (IEnumerable<object>)context.Provider.Execute(insertCommand).ReturnValue;
        var array = (object[])source.FirstOrDefault();
        if (array != null) {
          AutoSyncMembers(array, item, UpdateType.Insert, AutoSyncBehavior.ApplyNewAutoSync);
          return 1;
        }
        throw System.Data.Linq.Error.InsertAutoSyncFailure();
      }

      internal override void AppendInsertText(TrackedObject item, StringBuilder appendTo) {
        if (item.Type.Table.InsertMethod != null) {
          appendTo.Append(System.Data.Linq.Strings.InsertCallbackComment);
        } else {
          var insertCommand = GetInsertCommand(item);
          appendTo.Append(context.Provider.GetQueryText(insertCommand));
          appendTo.AppendLine();
        }
      }

      internal override int Update(TrackedObject item) {
        if (item.Type.Table.UpdateMethod != null) {
          try {
            item.Type.Table.UpdateMethod.Invoke(context, new object[1]
            {
            item.Current
            });
          } catch (TargetInvocationException ex) {
            if (ex.InnerException != null) {
              throw ex.InnerException;
            }
            throw;
          }
          return 1;
        }
        return DynamicUpdate(item);
      }

      internal override int DynamicUpdate(TrackedObject item) {
        var updateCommand = GetUpdateCommand(item);
        if (updateCommand.Type == typeof(int)) {
          return (int)context.Provider.Execute(updateCommand).ReturnValue;
        }
        var source = (IEnumerable<object>)context.Provider.Execute(updateCommand).ReturnValue;
        var array = (object[])source.FirstOrDefault();
        if (array != null) {
          AutoSyncMembers(array, item, UpdateType.Update, AutoSyncBehavior.ApplyNewAutoSync);
          return 1;
        }
        return 0;
      }

      internal override void AppendUpdateText(TrackedObject item, StringBuilder appendTo) {
        if (item.Type.Table.UpdateMethod != null) {
          appendTo.Append(System.Data.Linq.Strings.UpdateCallbackComment);
        } else {
          var updateCommand = GetUpdateCommand(item);
          appendTo.Append(context.Provider.GetQueryText(updateCommand));
          appendTo.AppendLine();
        }
      }

      internal override int Delete(TrackedObject item) {
        if (item.Type.Table.DeleteMethod != null) {
          try {
            item.Type.Table.DeleteMethod.Invoke(context, new object[1]
            {
            item.Current
            });
          } catch (TargetInvocationException ex) {
            if (ex.InnerException != null) {
              throw ex.InnerException;
            }
            throw;
          }
          return 1;
        }
        return DynamicDelete(item);
      }

      internal override int DynamicDelete(TrackedObject item) {
        var deleteCommand = GetDeleteCommand(item);
        var num = (int)context.Provider.Execute(deleteCommand).ReturnValue;
        if (num == 0) {
          deleteCommand = GetDeleteVerificationCommand(item);
          num = (((int?)context.Provider.Execute(deleteCommand).ReturnValue) ?? (-1));
        }
        return num;
      }

      internal override void AppendDeleteText(TrackedObject item, StringBuilder appendTo) {
        if (item.Type.Table.DeleteMethod != null) {
          appendTo.Append(System.Data.Linq.Strings.DeleteCallbackComment);
        } else {
          var deleteCommand = GetDeleteCommand(item);
          appendTo.Append(context.Provider.GetQueryText(deleteCommand));
          appendTo.AppendLine();
        }
      }

      internal override void RollbackAutoSync() {
        if (syncRollbackItems != null) {
          foreach (var syncRollbackItem in SyncRollbackItems) {
            var key = syncRollbackItem.Key;
            var value = syncRollbackItem.Value;
            AutoSyncMembers(value, key, (!key.IsNew) ? UpdateType.Update : UpdateType.Insert, AutoSyncBehavior.RollbackSavedValues);
          }
        }
      }

      internal override void ClearAutoSyncRollback() => syncRollbackItems = null;

      private Expression GetInsertCommand(TrackedObject item) {
        var type = item.Type;
        var autoSyncMembers = GetAutoSyncMembers(type, UpdateType.Insert);
        var parameterExpression = Expression.Parameter(item.Type.Table.RowType.Type, "p");
        if (autoSyncMembers.Count > 0) {
          var body = CreateAutoSync(autoSyncMembers, parameterExpression);
          var lambdaExpression = Expression.Lambda(body, parameterExpression);
          return Expression.Call(typeof(DataManipulation), "Insert", new Type[2]
          {
          item.Type.InheritanceRoot.Type,
          lambdaExpression.Body.Type
          }, Expression.Constant(item.Current), lambdaExpression);
        }
        return Expression.Call(typeof(DataManipulation), "Insert", new Type[1]
        {
        item.Type.InheritanceRoot.Type
        }, Expression.Constant(item.Current));
      }

      private Expression CreateAutoSync(List<MetaDataMember> membersToSync, Expression source) {
        var num = 0;
        var array = new Expression[membersToSync.Count];
        foreach (var item in membersToSync) {
          array[num++] = Expression.Convert(GetMemberExpression(source, item.Member), typeof(object));
        }
        return Expression.NewArrayInit(typeof(object), array);
      }

      private static List<MetaDataMember> GetAutoSyncMembers(MetaType metaType, UpdateType updateType) {
        var list = new List<MetaDataMember>();
        foreach (var item in from m in metaType.PersistentDataMembers
                             orderby m.Ordinal
                             select m) {
          if ((updateType == UpdateType.Insert && item.AutoSync == AutoSync.OnInsert) || (updateType == UpdateType.Update && item.AutoSync == AutoSync.OnUpdate) || item.AutoSync == AutoSync.Always) {
            list.Add(item);
          }
        }
        return list;
      }

      private void AutoSyncMembers(object[] syncResults, TrackedObject item, UpdateType updateType, AutoSyncBehavior autoSyncBehavior) {
        object[] array = null;
        if (syncResults != null) {
          var num = 0;
          var autoSyncMembers = GetAutoSyncMembers(item.Type, updateType);
          if (autoSyncBehavior == AutoSyncBehavior.ApplyNewAutoSync) {
            array = new object[syncResults.Length];
          }
          foreach (var item2 in autoSyncMembers) {
            var value = syncResults[num];
            var instance = item.Current;
            var metaAccessor = (item2.Member is PropertyInfo && ((PropertyInfo)item2.Member).CanWrite) ? item2.MemberAccessor : item2.StorageAccessor;
            if (array != null) {
              array[num] = metaAccessor.GetBoxedValue(instance);
            }
            metaAccessor.SetBoxedValue(ref instance, DBConvert.ChangeType(value, item2.Type));
            num++;
          }
        }
        if (array != null) {
          SyncRollbackItems.Add(new KeyValuePair<TrackedObject, object[]>(item, array));
        }
      }

      private Expression GetUpdateCommand(TrackedObject tracked) {
        var original = tracked.Original;
        var inheritanceType = tracked.Type.GetInheritanceType(original.GetType());
        var inheritanceRoot = inheritanceType.InheritanceRoot;
        var parameterExpression = Expression.Parameter(inheritanceRoot.Type, "p");
        Expression expression = parameterExpression;
        if (inheritanceType != inheritanceRoot) {
          expression = Expression.Convert(parameterExpression, inheritanceType.Type);
        }
        var expression2 = GetUpdateCheck(expression, tracked);
        if (expression2 != null) {
          expression2 = Expression.Lambda(expression2, parameterExpression);
        }
        var autoSyncMembers = GetAutoSyncMembers(inheritanceType, UpdateType.Update);
        if (autoSyncMembers.Count > 0) {
          var body = CreateAutoSync(autoSyncMembers, expression);
          var lambdaExpression = Expression.Lambda(body, parameterExpression);
          if (expression2 != null) {
            return Expression.Call(typeof(DataManipulation), "Update", new Type[2]
            {
            inheritanceRoot.Type,
            lambdaExpression.Body.Type
            }, Expression.Constant(tracked.Current), expression2, lambdaExpression);
          }
          return Expression.Call(typeof(DataManipulation), "Update", new Type[2]
          {
          inheritanceRoot.Type,
          lambdaExpression.Body.Type
          }, Expression.Constant(tracked.Current), lambdaExpression);
        }
        if (expression2 != null) {
          return Expression.Call(typeof(DataManipulation), "Update", new Type[1]
          {
          inheritanceRoot.Type
          }, Expression.Constant(tracked.Current), expression2);
        }
        return Expression.Call(typeof(DataManipulation), "Update", new Type[1]
        {
        inheritanceRoot.Type
        }, Expression.Constant(tracked.Current));
      }

      private Expression GetUpdateCheck(Expression serverItem, TrackedObject tracked) {
        var type = tracked.Type;
        if (type.VersionMember != null) {
          return Expression.Equal(GetMemberExpression(serverItem, type.VersionMember.Member), GetMemberExpression(Expression.Constant(tracked.Current), type.VersionMember.Member));
        }
        Expression expression = null;
        foreach (var persistentDataMember in type.PersistentDataMembers) {
          if (!persistentDataMember.IsPrimaryKey) {
            switch (persistentDataMember.UpdateCheck) {
              case UpdateCheck.WhenChanged:
                if (!tracked.HasChangedValue(persistentDataMember)) {
                  break;
                }
                goto case UpdateCheck.Always;
              case UpdateCheck.Always: {
                  var boxedValue = persistentDataMember.MemberAccessor.GetBoxedValue(tracked.Original);
                  Expression expression2 = Expression.Equal(GetMemberExpression(serverItem, persistentDataMember.Member), Expression.Constant(boxedValue, persistentDataMember.Type));
                  expression = ((expression != null) ? Expression.And(expression, expression2) : expression2);
                  break;
                }
            }
          }
        }
        return expression;
      }

      private Expression GetDeleteCommand(TrackedObject tracked) {
        var type = tracked.Type;
        var inheritanceRoot = type.InheritanceRoot;
        var parameterExpression = Expression.Parameter(inheritanceRoot.Type, "p");
        Expression serverItem = parameterExpression;
        if (type != inheritanceRoot) {
          serverItem = Expression.Convert(parameterExpression, type.Type);
        }
        var value = tracked.CreateDataCopy(tracked.Original);
        var updateCheck = GetUpdateCheck(serverItem, tracked);
        if (updateCheck != null) {
          updateCheck = Expression.Lambda(updateCheck, parameterExpression);
          return Expression.Call(typeof(DataManipulation), "Delete", new Type[1]
          {
          inheritanceRoot.Type
          }, Expression.Constant(value), updateCheck);
        }
        return Expression.Call(typeof(DataManipulation), "Delete", new Type[1]
        {
        inheritanceRoot.Type
        }, Expression.Constant(value));
      }

      private Expression GetDeleteVerificationCommand(TrackedObject tracked) {
        var table = context.GetTable(tracked.Type.InheritanceRoot.Type);
        var parameterExpression = Expression.Parameter(table.ElementType, "p");
        Expression expression = Expression.Lambda(Expression.Equal(parameterExpression, Expression.Constant(tracked.Current)), parameterExpression);
        Expression expression2 = Expression.Call(typeof(Queryable), "Where", new Type[1]
        {
        table.ElementType
        }, table.Expression, expression);
        Expression expression3 = Expression.Lambda(Expression.Constant(0, typeof(int?)), parameterExpression);
        Expression expression4 = Expression.Call(typeof(Queryable), "Select", new Type[2]
        {
        table.ElementType,
        typeof(int?)
        }, expression2, expression3);
        return Expression.Call(typeof(Queryable), "SingleOrDefault", new Type[1]
        {
        typeof(int?)
        }, expression4);
      }

      private Expression GetMemberExpression(Expression exp, MemberInfo mi) {
        var fieldInfo = mi as FieldInfo;
        if (fieldInfo != null) {
          return Expression.Field(exp, fieldInfo);
        }
        var property = (PropertyInfo)mi;
        return Expression.Property(exp, property);
      }
    }

    internal abstract int Insert(TrackedObject item);

    internal abstract int DynamicInsert(TrackedObject item);

    internal abstract void AppendInsertText(TrackedObject item, StringBuilder appendTo);

    internal abstract int Update(TrackedObject item);

    internal abstract int DynamicUpdate(TrackedObject item);

    internal abstract void AppendUpdateText(TrackedObject item, StringBuilder appendTo);

    internal abstract int Delete(TrackedObject item);

    internal abstract int DynamicDelete(TrackedObject item);

    internal abstract void AppendDeleteText(TrackedObject item, StringBuilder appendTo);

    internal abstract void RollbackAutoSync();

    internal abstract void ClearAutoSyncRollback();

    internal static ChangeDirector CreateChangeDirector(DataContext context) => new StandardChangeDirector(context);
  }

}