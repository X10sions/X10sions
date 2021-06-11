//using LinqToDB.Data;
//using LinqToDB.Mapping;
//using LinqToDB.SqlProvider;
//using LinqToDB.SqlQuery;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using LinqToDB;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
//  public class xMultipleRowsHelper_TB<T> {
//    public xMultipleRowsHelper_TB(DataConnection dataConnection__1, BulkCopyOptions options__2, bool enforceKeepIdentity) {
//      DataConnection = dataConnection__1;
//      Options = options__2;
//      SqlBuilder = dataConnection__1.DataProvider.CreateSqlBuilder();
//      ValueConverter = dataConnection__1.MappingSchema.ValueToSqlConverter;
//      Descriptor = dataConnection__1.MappingSchema.GetEntityDescriptor(typeof(T));
//      Columns = Descriptor.Columns.Where((c) => !c.SkipOnInsert || enforceKeepIdentity && options__2.KeepIdentity == true && c.IsIdentity).ToArray();
//      ColumnTypes = Columns.Select((c) => {
//        //Return New SqlDataType(c.DataType, c.MemberType, c.Length, c.Precision, c.Scale)
//        if (c.Precision.HasValue || c.Scale.HasValue) {
//          return new SqlDataType(c.DataType, c.MemberType, c.Precision.Value, c.Scale.Value);
//        } else {
//          return new SqlDataType(c.DataType, c.MemberType, c.Length.Value);
//        }
//      }).ToArray();

//      ParameterName = SqlBuilder.Convert("p", ConvertType.NameToQueryParameter).ToString();
//      TableName = xBasicBulkCopy.GetTableName(SqlBuilder, options__2, Descriptor);
//      BatchSize = Math.Max(10, (Options.MaxBatchSize ?? 1000));
//    }

//    public readonly ISqlBuilder SqlBuilder;
//    public readonly DataConnection DataConnection;
//    public readonly BulkCopyOptions Options;
//    public readonly ValueToSqlConverter ValueConverter;
//    public readonly EntityDescriptor Descriptor;
//    public readonly ColumnDescriptor[] Columns;
//    public readonly SqlDataType[] ColumnTypes;
//    public readonly string TableName;
//    public readonly string ParameterName;
//    public readonly List<DataParameter> Parameters = new List<DataParameter>();
//    public readonly StringBuilder StringBuilder = new StringBuilder();
//    public readonly xBulkCopyRowsCopied RowsCopied = new xBulkCopyRowsCopied();
//    public int CurrentCount;
//    public int ParameterIndex;
//    public int HeaderSize;
//    public int BatchSize;

//    public void SetHeader() => HeaderSize = StringBuilder.Length;

//    public void BuildColumns(object item) {
//      for (var i = 0; i < Columns.Length; i++) {
//        var column = Columns[i];
//        var value = column.GetValue(DataConnection.MappingSchema, item);
//        if (!ValueConverter.TryConvert(StringBuilder, ColumnTypes[i], value)) {
//          object name = (ParameterName == "?") ? ParameterName : ParameterName + System.Threading.Interlocked.Increment(ref ParameterIndex);
//          StringBuilder.Append(name);
//          if (value is DataParameter) {
//            value = ((DataParameter)value).Value;
//          }
//          Parameters.Add(new DataParameter(((ParameterName == "?") ? ParameterName : "p" + ParameterIndex), value, column.DataType));
//        }
//        StringBuilder.Append(",");
//      }
//      StringBuilder.Length -= 1;
//    }

//    public bool Execute() {
//      DataConnection.Execute(StringBuilder.AppendLine().ToString(), Parameters.ToArray());
//      if (Options.RowsCopiedCallback != null) {
//        Options.RowsCopiedCallback(RowsCopied);
//        if (RowsCopied.Abort) {
//          return false;
//        }
//      }
//      Parameters.Clear();
//      ParameterIndex = 0;
//      CurrentCount = 0;
//      StringBuilder.Length = HeaderSize;
//      return true;
//    }

//  }
//}