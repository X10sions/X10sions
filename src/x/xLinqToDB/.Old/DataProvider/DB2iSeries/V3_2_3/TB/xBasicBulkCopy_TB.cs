//using LinqToDB.Custom.Data;
//using LinqToDB.Data;
//using LinqToDB.Expressions;
//using LinqToDB.Mapping;
//using LinqToDB.SqlProvider;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Linq.Expressions;
//using System.Text;

// namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
//  public class xBasicBulkCopy_TB {

//    public virtual xBulkCopyRowsCopied BulkCopy<T>(BulkCopyType bulkCopyType__1, DataConnection dataConnection, BulkCopyOptions options, IEnumerable<T> source) {
//      switch (bulkCopyType__1) {
//        case BulkCopyType.MultipleRows: return MultipleRowsCopy(dataConnection, options, source);
//        case BulkCopyType.RowByRow: return RowByRowCopy(dataConnection, options, source);
//        default: return ProviderSpecificCopy(dataConnection, options, source);
//      }
//    }

//    protected virtual xBulkCopyRowsCopied ProviderSpecificCopy<T>(DataConnection dataConnection, BulkCopyOptions options, IEnumerable<T> source) => MultipleRowsCopy(dataConnection, options, source);

//    protected virtual xBulkCopyRowsCopied MultipleRowsCopy<T>(DataConnection dataConnection, BulkCopyOptions options, IEnumerable<T> source) => RowByRowCopy(dataConnection, options, source);

//    protected virtual xBulkCopyRowsCopied RowByRowCopy<T>(DataConnection dataConnection, BulkCopyOptions options, IEnumerable<T> source) {
//      var rowsCopied = new xBulkCopyRowsCopied();
//      foreach (var item in source) {
//        dataConnection.Insert(item, options.TableName, options.DatabaseName, options.SchemaName);
//        rowsCopied.RowsCopied += 1;
//        if (options.NotifyAfter != 0 && options.RowsCopiedCallback != null && rowsCopied.RowsCopied % options.NotifyAfter == 0) {
//          options.RowsCopiedCallback(rowsCopied);
//          if (rowsCopied.Abort)
//            break;
//        }
//      }
//      return rowsCopied;
//    }

//    protected internal static string GetTableName(ISqlBuilder sqlBuilder, BulkCopyOptions options, EntityDescriptor descriptor) {
//      var databaseName = options.DatabaseName ?? descriptor.DatabaseName;
//      var schemaName = options.SchemaName ?? descriptor.SchemaName;
//      var tableName = options.TableName ?? descriptor.TableName;
//      return sqlBuilder.BuildTableName(new StringBuilder(), databaseName == null ? null : sqlBuilder.Convert(databaseName, ConvertType.NameToDatabase).ToString(), schemaName == null ? null : sqlBuilder.Convert(schemaName, ConvertType.NameToOwner).ToString(), tableName == null ? null : sqlBuilder.Convert(tableName, ConvertType.NameToQueryTable).ToString()).ToString();
//    }


//    protected Func<IDbConnection, int, IDisposable> CreateBulkCopyCreator(Type connectionType, Type bulkCopyType, Type bulkCopyOptionType) {
//      var p1 = Expression.Parameter(typeof(IDbConnection), "pc");
//      var p2 = Expression.Parameter(typeof(int), "po");
//      var l = Expression.Lambda<Func<IDbConnection, int, IDisposable>>(Expression.Convert(Expression.New(bulkCopyType.GetConstructor(new[] { connectionType, bulkCopyOptionType }), Expression.Convert(p1, connectionType), Expression.Convert(p2, bulkCopyOptionType)), typeof(IDisposable)), p1, p2);
//      return l.Compile();
//    }

//    protected Func<int, string, object> CreateColumnMappingCreator(Type columnMappingType) {
//      var p1 = Expression.Parameter(typeof(int), "p1");
//      var p2 = Expression.Parameter(typeof(string), "p2");
//      var l = Expression.Lambda<Func<int, string, object>>(Expression.Convert(Expression.New(columnMappingType.GetConstructor(new[] { typeof(int), typeof(string) }), new[] { p1, p2 }), typeof(object)), p1, p2);
//      return l.Compile();
//    }

//    protected Action<object, Action<object>> CreateBulkCopySubscriber(object bulkCopy, string eventName) {
//      var eventInfo = bulkCopy.GetType().GetEvent(eventName);
//      var handlerType = eventInfo.EventHandlerType;
//      var eventParams = handlerType.GetMethod("Invoke").GetParameters();
//      // Expression<Func<Action<object>,Delegate>> lambda =
//      // actionParameter => Delegate.CreateDelegate(
//      // typeof(int),
//      // (Action<object,DB2RowsCopiedEventArgs>)((o,e) => actionParameter(e)),
//      // "Invoke",
//      // false);
//      var actionParameter = Expression.Parameter(typeof(Action<object>), "p1");
//      var senderParameter = Expression.Parameter(eventParams[0].ParameterType, eventParams[0].Name);
//      var argsParameter = Expression.Parameter(eventParams[1].ParameterType, eventParams[1].Name);
//      // Expression.Convert(
//      // typeof(Action<object, EventArgs>)),
//      var lambda = Expression.Lambda<Func<Action<object>, Delegate>>(
//        Expression.Call(
//          null        /* TODO Change to default(_) if this is not a reference type */,
//          MemberHelper.MethodOf(() => Delegate.CreateDelegate(typeof(string), (object)null, "", false)),
//          new Expression[] {
//            Expression.Constant(handlerType, typeof(Type)),
//            Expression.Lambda(Expression.Invoke(actionParameter, new Expression[] { argsParameter }),
//            new[] { senderParameter, argsParameter }),
//            Expression.Constant("Invoke", typeof(string)),
//            Expression.Constant(false, typeof(bool))
//          }),
//        new[] { actionParameter }
//            );
//      var dgt = lambda.Compile();
//      return (obj, action) => eventInfo.AddEventHandler(obj, dgt(action));
//    }

//    protected void TraceAction(DataConnection dataConnection__1, string commandText, Func<int> action) {
//      if (DataConnection.TraceSwitch.TraceInfo && dataConnection__1.OnTraceConnection != null)
//        dataConnection__1.OnTraceConnection(new TraceInfo(TraceInfoStep.BeforeExecute) {
//          TraceLevel = TraceLevel.Info,
//          DataConnection = dataConnection__1,
//          CommandText = commandText
//        });
//      var now = DateTime.Now;
//      try {
//        var count = action();
//        if (DataConnection.TraceSwitch.TraceInfo && dataConnection__1.OnTraceConnection != null)
//          dataConnection__1.OnTraceConnection(new TraceInfo(TraceInfoStep.AfterExecute) {
//            TraceLevel = TraceLevel.Info,
//            DataConnection = dataConnection__1,
//            CommandText = commandText,
//            ExecutionTime = DateTime.Now - now,
//            RecordsAffected = count
//          });
//      } catch (Exception ex) {
//        if (DataConnection.TraceSwitch.TraceError && dataConnection__1.OnTraceConnection != null)
//          dataConnection__1.OnTraceConnection(new TraceInfo(TraceInfoStep.Error) {
//            TraceLevel = TraceLevel.Error,
//            DataConnection = dataConnection__1,
//            CommandText = commandText,
//            ExecutionTime = DateTime.Now - now,
//            Exception = ex
//          });
//        throw;
//      }
//    }



//    protected xBulkCopyRowsCopied MultipleRowsCopy1<T>(DataConnection dataConnection, BulkCopyOptions options, bool enforceKeepIdentity, IEnumerable<T> source) => MultipleRowsCopy1(new xMultipleRowsHelper<T>(dataConnection, options, enforceKeepIdentity), dataConnection, options, source);

//    protected xBulkCopyRowsCopied MultipleRowsCopy1<T>(xMultipleRowsHelper<T> helper, DataConnection dataConnection, BulkCopyOptions options, IEnumerable<T> source) {
//      helper.StringBuilder.AppendFormat("INSERT INTO {0}", helper.TableName).AppendLine().Append("(");
//      foreach (var column in helper.Columns)
//        helper.StringBuilder.AppendLine().Append("  ").Append(helper.SqlBuilder.Convert(column.ColumnName, ConvertType.NameToQueryField)).Append(",");
//      helper.StringBuilder.Length -= 1;
//      helper.StringBuilder.AppendLine().Append(")");
//      helper.StringBuilder.AppendLine().Append("VALUES");
//      helper.SetHeader();
//      foreach (var item in source) {
//        helper.StringBuilder.AppendLine().Append("(");
//        helper.BuildColumns(item);
//        helper.StringBuilder.Append("),");
//        helper.RowsCopied.RowsCopied += 1;
//        helper.CurrentCount += 1;
//        if (helper.CurrentCount >= helper.BatchSize || helper.Parameters.Count > 10000 || helper.StringBuilder.Length > 100000) {
//          helper.StringBuilder.Length -= 1;
//          if (!helper.Execute())
//            return helper.RowsCopied;
//        }
//      }
//      if (helper.CurrentCount > 0) {
//        helper.StringBuilder.Length -= 1;
//        helper.Execute();
//      }
//      return helper.RowsCopied;
//    }

//    protected xBulkCopyRowsCopied MultipleRowsCopy2<T>(DataConnection dataConnection, BulkCopyOptions options, bool enforceKeepIdentity, IEnumerable<T> source, string from) => MultipleRowsCopy2<T>(new xMultipleRowsHelper<T>(dataConnection, options, enforceKeepIdentity), dataConnection, options, source, from);

//    protected xBulkCopyRowsCopied MultipleRowsCopy2<T>(xMultipleRowsHelper<T> helper, DataConnection dataConnection, BulkCopyOptions options, IEnumerable<T> source, string from) {
//      helper.StringBuilder.AppendFormat("INSERT INTO {0}", helper.TableName).AppendLine().Append("(");
//      foreach (var column in helper.Columns)
//        helper.StringBuilder.AppendLine().Append("  ").Append(helper.SqlBuilder.Convert(column.ColumnName, ConvertType.NameToQueryField)).Append(",");
//      helper.StringBuilder.Length -= 1;
//      helper.StringBuilder.AppendLine().Append(")");
//      helper.SetHeader();
//      foreach (var item in source) {
//        helper.StringBuilder.AppendLine().Append("SELECT ");
//        helper.BuildColumns(item);
//        helper.StringBuilder.Append(from);
//        helper.StringBuilder.Append(" UNION ALL");
//        helper.RowsCopied.RowsCopied += 1;
//        helper.CurrentCount += 1;
//        if (helper.CurrentCount >= helper.BatchSize || helper.Parameters.Count > 10000 || helper.StringBuilder.Length > 100000) {
//          helper.StringBuilder.Length -= " UNION ALL".Length;
//          if (!helper.Execute())
//            return helper.RowsCopied;
//        }
//      }
//      if (helper.CurrentCount > 0) {
//        helper.StringBuilder.Length -= " UNION ALL".Length;
//        helper.Execute();
//      }
//      return helper.RowsCopied;
//    }

//    protected xBulkCopyRowsCopied MultipleRowsCopy3<T>(DataConnection dataConnection, BulkCopyOptions options, IEnumerable<T> source, string from) {
//      var helper = new xMultipleRowsHelper<T>(dataConnection, options, false);
//      helper.StringBuilder.AppendFormat("INSERT INTO {0}", helper.TableName).AppendLine().Append("(");
//      foreach (var column in helper.Columns)
//        helper.StringBuilder.AppendLine().Append("  ").Append(helper.SqlBuilder.Convert(column.ColumnName, ConvertType.NameToQueryField)).Append(",");
//      helper.StringBuilder.Length -= 1;
//      helper.StringBuilder.AppendLine().AppendLine(")").AppendLine("SELECT * FROM").Append("(");
//      helper.SetHeader();
//      foreach (var item in source) {
//        helper.StringBuilder.AppendLine().Append("  " + "SELECT ");
//        helper.BuildColumns(item);
//        helper.StringBuilder.Append(from);
//        helper.StringBuilder.Append(" UNION ALL");
//        helper.RowsCopied.RowsCopied += 1;
//        helper.CurrentCount += 1;
//        if (helper.CurrentCount >= helper.BatchSize || helper.Parameters.Count > 10000 || helper.StringBuilder.Length > 100000) {
//          helper.StringBuilder.Length -= " UNION ALL".Length;
//          helper.StringBuilder.AppendLine().Append(")");
//          if (!helper.Execute())
//            return helper.RowsCopied;
//        }
//      }
//      if (helper.CurrentCount > 0) {
//        helper.StringBuilder.Length -= " UNION ALL".Length;
//        helper.StringBuilder.AppendLine().Append(")");
//        helper.Execute();
//      }
//      return helper.RowsCopied;
//    }
//  }
//}
