//using LinqToDB.Data;
//using LinqToDB.Mapping;
//using LinqToDB.SqlProvider;
//using LinqToDB.SqlQuery;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using LinqToDB.Linq;
//using System.Data;
//using System.Linq.Expressions;
//using System.Threading;
//using System.Reflection;
//using LinqToDB;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
//  public class xBasicMerge_TB {

//    protected class ColumnInfo {
//      public string Name;
//      public ColumnDescriptor Column;
//    }

//    public class PreparedQuery {
//      public string[] Commands;

//      public List<SqlParameter> SqlParameters;
//      public IDbDataParameter[] Parameters;
//      public SelectQuery SelectQuery;
//      public ISqlBuilder SqlProvider;
//      public List<string> QueryHints;
//    }

//    private class QueryContext : IQueryContext {
//      public SqlParameter[] SqlParameters;
//      public SelectQuery SelectQuery { get; set; }
//      public object Context { get; set; }
//      public List<string> QueryHints { get; set; }

//      public SqlParameter[] GetParameters() => SqlParameters;

//    }

//    protected string ByTargetText;
//    protected StringBuilder StringBuilder;
//    protected List<DataParameter> Parameters;
//    protected List<ColumnInfo> Columns;

//    protected virtual bool IsIdentitySupported { get; }

//    public xBasicMerge() {
//      StringBuilder = new StringBuilder();
//      Parameters = new List<DataParameter>();
//      IsIdentitySupported = false;
//    }

//    public virtual int Merge<T>(DataConnection dataConnection, Expression<Func<T, bool>> predicate, bool delete, IEnumerable<T> source, string tableName, string databaseName, string schemaName) where T : class {
//      if (!BuildCommand(dataConnection, predicate, delete, source, tableName, databaseName, schemaName)) {
//        return 0;
//      }
//      return Execute(dataConnection);
//    }

//    protected virtual bool BuildCommand<T>(DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source, string tableName, string databaseName, string schemaName) where T : class => throw new NotImplementedException("TODO");

//    protected virtual bool BuildUsing<T>(DataConnection dataConnection, IEnumerable<T> source) {
//      var table = dataConnection.MappingSchema.GetEntityDescriptor(typeof(T));
//      var pname = dataConnection.DataProvider.CreateSqlBuilder().Convert("p", ConvertType.NameToQueryParameter).ToString();
//      var valueConverter = dataConnection.MappingSchema.ValueToSqlConverter;
//      StringBuilder.AppendLine("USING").AppendLine("(").AppendLine("\tVALUES");
//      var pidx = 0;
//      var hasData = false;
//      var columnTypes = table.Columns.Select((c) => {
//        if (c.Precision.HasValue || c.Scale.HasValue) {
//          return new SqlDataType(c.DataType, c.MemberType, c.Precision.Value, c.Scale.Value);
//        } else {
//          return new SqlDataType(c.DataType, c.MemberType, c.Length.Value);
//        }
//        //Return New SqlDataType(c.DataType, c.MemberType, c.Length, c.Precision, c.Scale)
//      }).ToArray();


//      checked {
//        foreach (var item in source) {
//          hasData = true;
//          StringBuilder.Append("\t(");
//          var num = table.Columns.Count - 1;
//          for (var i = 0; i <= num; i++) {
//            var column2 = table.Columns[i];
//            var value = (column2.GetValue(dataConnection.MappingSchema, item));
//            if (!valueConverter.TryConvert(StringBuilder, columnTypes[i], (value))) {
//              var name = ((pname == "?") ? pname : ((object)(Convert.ToDouble(pname) + Interlocked.Increment(ref pidx))));
//              StringBuilder.Append((name));
//              Parameters.Add(new DataParameter(Convert.ToString((pname == "?") ? pname : ((object)(Convert.ToDouble("p") + pidx))), (value), column2.DataType));
//            }
//            StringBuilder.Append(",");
//          }
//          StringBuilder.Length--;
//          StringBuilder.AppendLine("),");
//        }
//        if (hasData) {
//          var idx = StringBuilder.Length;
//          while (StringBuilder[Interlocked.Decrement(ref idx)] != ',') {
//          }
//          StringBuilder.Remove(idx, 1);
//          StringBuilder.AppendLine(")").AppendLine("AS Source").AppendLine("(");
//          foreach (var column in Columns) {
//            StringBuilder.AppendFormat("\t{0},", column.Name).AppendLine();
//          }
//          StringBuilder.Length -= 1 + Environment.NewLine.Length;
//          StringBuilder.AppendLine().AppendLine(")");
//        }
//        return hasData;
//      }
//    }

//    protected bool BuildUsing2<T>(DataConnection dataConnection, IEnumerable<T> source, string top, string fromDummyTable) {
//      var entityDescriptor = dataConnection.MappingSchema.GetEntityDescriptor(typeof(T));
//      var pname = dataConnection.DataProvider.CreateSqlBuilder().Convert("p", ConvertType.NameToQueryParameter).ToString();
//      var valueConverter = dataConnection.MappingSchema.ValueToSqlConverter;
//      StringBuilder.AppendLine("USING").AppendLine("(");
//      var pidx = 0;
//      var hasData = false;
//      var columnTypes = entityDescriptor.Columns.Select((c) => {
//        if (c.Precision.HasValue || c.Scale.HasValue) {
//          return new SqlDataType(c.DataType, c.MemberType, c.Precision.Value, c.Scale.Value);
//        } else {
//          return new SqlDataType(c.DataType, c.MemberType, c.Length.Value);
//        }
//        //Return New SqlDataType(c.DataType, c.MemberType, c.Length, c.Precision, c.Scale)
//      }).ToArray();
//      checked {
//        foreach (var item in source) {
//          if (hasData) {
//            StringBuilder.Append(" UNION ALL").AppendLine();
//          }
//          StringBuilder.Append("\tSELECT ");
//          if (top != null) {
//            StringBuilder.Append(top);
//          }
//          var num = Columns.Count - 1;
//          for (var i = 0; i <= num; i++) {
//            var column = Columns[i];
//            var value = (column.Column.GetValue(dataConnection.MappingSchema, item));
//            if (!valueConverter.TryConvert(StringBuilder, columnTypes[i], (value))) {
//              var name = (((pname == "?")) ? pname : ((object)(Convert.ToDouble(pname) + Interlocked.Increment(ref pidx))));
//              StringBuilder.Append((name));
//              Parameters.Add(new DataParameter(Convert.ToString(pname == "?" ? pname : ((object)(Convert.ToDouble("p") + pidx))), (value), column.Column.DataType));
//            }
//            if (!hasData) {
//              StringBuilder.Append(" as ").Append(column.Name);
//            }
//            StringBuilder.Append(",");
//          }
//          StringBuilder.Length--;
//          StringBuilder.Append(' ').Append(fromDummyTable);
//          hasData = true;
//        }
//        if (hasData) {
//          StringBuilder.AppendLine();
//          StringBuilder.AppendLine(")").AppendLine("Source");
//        }
//        return hasData;
//      }
//    }

//    protected virtual int Execute(DataConnection dataConnection) {
//      var cmd = StringBuilder.AppendLine().ToString();
//      return dataConnection.Execute(cmd, Parameters.ToArray());
//    }
//  }
//}