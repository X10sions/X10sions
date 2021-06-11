//using LinqToDB.DataProvider;
//using LinqToDB.Mapping;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Globalization;
//using System.Linq;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
//  public class xBulkCopyReader : IDataReader {
//    internal class Parameter : IDbDataParameter {

//      public DbType DbType { get; set; }
//      public ParameterDirection Direction { get; set; }
//      public bool IsNullable { get; }
//      public string ParameterName { get; set; }
//      public byte Precision { get; set; }
//      public string SourceColumn { get; set; }
//      public DataRowVersion SourceVersion { get; set; }
//      public byte Scale { get; set; }
//      public int Size { get; set; }
//      public object Value { get; set; }
//    }

//    public int Count;

//    private readonly DataType[] _columnTypes;
//    private readonly IDataProvider _dataProvider;
//    private readonly List<ColumnDescriptor> _columns;
//    private readonly IEnumerator _enumerator;
//    private readonly Parameter _valueConverter;
//    private readonly MappingSchema _mappingSchema;
//    public int FieldCount => _columns.Count;

//    public object this[int i] => throw new NotImplementedException();

//    public object this[string name] => throw new NotImplementedException();

//    public int Depth => throw new NotImplementedException();

//    public bool IsClosed => false;

//    public int RecordsAffected => throw new NotImplementedException();

//    public xBulkCopyReader(IDataProvider dataProvider, MappingSchema mappingSchema, List<ColumnDescriptor> columns, IEnumerable collection) {
//      _valueConverter = new Parameter();
//      _dataProvider = dataProvider;
//      _columns = columns;
//      _enumerator = collection.GetEnumerator();
//      _mappingSchema = mappingSchema;
//      _columnTypes = _columns.Select((ColumnDescriptor c) => (c.DataType != 0) ? c.DataType : dataProvider.MappingSchema.GetDataType(c.MemberType).DataType).ToArray();
//    }

//    public Type GetFieldType(int i) => _dataProvider.ConvertParameterType(_columns[i].MemberType, _columnTypes[i]);


//    public string GetName(int i) => _columns[i].ColumnName;


//    public object GetValue(int i) {
//      var value = (_columns[i].GetValue(_mappingSchema, (_enumerator.Current)));
//      _dataProvider.SetParameter(_valueConverter, string.Empty, _columnTypes[i], (value));
//      return _valueConverter.Value;
//    }

//    public int GetValues(object[] values) {
//      var count = _columns.Count;
//      var obj = (_enumerator.Current);
//      checked {
//        var num = count - 1;
//        for (var it = 0; it <= num; it++) {
//          var value = (_columns[it].GetValue(_mappingSchema, (obj)));
//          _dataProvider.SetParameter(_valueConverter, string.Empty, _columnTypes[it], (value));
//          values[it] = (_valueConverter.Value);
//        }
//        return count;
//      }
//    }

//    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
//    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
//    public string GetDataTypeName(int i) => throw new NotImplementedException();
//    public int GetOrdinal(string name) => throw new NotImplementedException();
//    public bool GetBoolean(int i) => throw new NotImplementedException();
//    public byte GetByte(int i) => throw new NotImplementedException();
//    public char GetChar(int i) => throw new NotImplementedException();
//    public Guid GetGuid(int i) => throw new NotImplementedException();
//    public short GetInt16(int i) => throw new NotImplementedException();
//    public int GetInt32(int i) => throw new NotImplementedException();
//    public long GetInt64(int i) => throw new NotImplementedException();
//    public float GetFloat(int i) => throw new NotImplementedException();
//    public double GetDouble(int i) => throw new NotImplementedException();
//    public string GetString(int i) => throw new NotImplementedException();
//    public decimal GetDecimal(int i) => throw new NotImplementedException();
//    public DateTime GetDateTime(int i) => throw new NotImplementedException();
//    public IDataReader GetData(int i) => throw new NotImplementedException();

//    public bool IsDBNull(int i) => GetValue(i) == null;

//    public void Close() {
//    }

//    public DataTable GetSchemaTable() {
//      var table = new DataTable("SchemaTable") {
//        Locale = CultureInfo.InvariantCulture
//      };
//      table.Columns.Add(new DataColumn(SchemaTableColumn.ColumnName, typeof(string)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.ColumnSize, typeof(int)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.NumericScale, typeof(short)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.DataType, typeof(Type)));
//      table.Columns.Add(new DataColumn(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.NonVersionedProviderType, typeof(int)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.ProviderType, typeof(int)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.IsLong, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.AllowDBNull, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsReadOnly, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsRowVersion, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.IsUnique, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.IsKey, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsHidden, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableOptionalColumn.BaseCatalogName, typeof(string)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(string)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.BaseTableName, typeof(string)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.BaseColumnName, typeof(string)));
//      table.Columns.Add(new DataColumn(SchemaTableOptionalColumn.BaseServerName, typeof(string)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.IsAliased, typeof(bool)));
//      table.Columns.Add(new DataColumn(SchemaTableColumn.IsExpression, typeof(bool)));
//      checked {
//        var num = _columns.Count - 1;
//        for (var i = 0; i <= num; i++) {
//          var columnDescriptor = _columns[i];
//          var row = table.NewRow();
//          row[SchemaTableColumn.ColumnName] = columnDescriptor.ColumnName;
//          row[SchemaTableColumn.DataType] = _dataProvider.ConvertParameterType(columnDescriptor.MemberType, _columnTypes[i]);
//          row[SchemaTableColumn.IsKey] = columnDescriptor.IsPrimaryKey;
//          row[SchemaTableOptionalColumn.IsAutoIncrement] = columnDescriptor.IsIdentity;
//          row[SchemaTableColumn.AllowDBNull] = columnDescriptor.CanBeNull;
//          row[SchemaTableColumn.ColumnSize] = ((columnDescriptor.Length.HasValue && columnDescriptor.Length.Value > 0) ? columnDescriptor.Length.Value : int.MaxValue);
//          if (columnDescriptor.Precision.HasValue) {
//            row[SchemaTableColumn.NumericPrecision] = (short)columnDescriptor.Precision.Value;
//          }
//          if (columnDescriptor.Scale.HasValue) {
//            row[SchemaTableColumn.NumericScale] = (short)columnDescriptor.Scale.Value;
//          }
//          table.Rows.Add(row);
//        }
//        return table;
//      }
//    }

//    public bool NextResult() => false;

//    public bool Read() {
//      var num = _enumerator.MoveNext();
//      checked {
//        if (num) {
//          Count++;
//        }
//        return num;
//      }
//    }

//    public void Dispose() {
//    }

//  }
//}
