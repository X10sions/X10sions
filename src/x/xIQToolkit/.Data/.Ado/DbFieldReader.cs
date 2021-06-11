using IQToolkit.Data.Common;
using System;
using System.Data.Common;

namespace IQToolkit.Data.Ado {

  /// <summary>
  /// A <see cref="FieldReader"/> implemented over a <see cref="DbDataReader"/>.
  /// </summary>
  public class DbFieldReader : FieldReader {
    private readonly QueryExecutor executor;
    private readonly DbDataReader reader;

    public DbFieldReader(QueryExecutor executor, DbDataReader reader) {
      this.executor = executor;
      this.reader = reader;
      Init();
    }

    protected override int FieldCount => reader.FieldCount;

    protected override Type GetFieldType(int ordinal) => reader.GetFieldType(ordinal);

    protected override bool IsDBNull(int ordinal) => reader.IsDBNull(ordinal);

    protected override T GetValue<T>(int ordinal) => (T)executor.Convert(reader.GetValue(ordinal), typeof(T));

    protected override byte GetByte(int ordinal) => reader.GetByte(ordinal);

    protected override char GetChar(int ordinal) => reader.GetChar(ordinal);

    protected override DateTime GetDateTime(int ordinal) => reader.GetDateTime(ordinal);

    protected override decimal GetDecimal(int ordinal) => reader.GetDecimal(ordinal);

    protected override double GetDouble(int ordinal) => reader.GetDouble(ordinal);

    protected override float GetSingle(int ordinal) => reader.GetFloat(ordinal);

    protected override Guid GetGuid(int ordinal) => reader.GetGuid(ordinal);

    protected override short GetInt16(int ordinal) => reader.GetInt16(ordinal);

    protected override int GetInt32(int ordinal) => reader.GetInt32(ordinal);

    protected override long GetInt64(int ordinal) => reader.GetInt64(ordinal);

    protected override string GetString(int ordinal) => reader.GetString(ordinal);
  }
}