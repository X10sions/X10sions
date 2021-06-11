using LinqToDB.Mapping;
using System;

namespace LinqToDB.Tests.Model {
  [Table("LinqDataTypes")]
  public class LinqDataTypes2 : IEquatable<LinqDataTypes2>, IComparable {
    [PrimaryKey] public int ID;
    [Column] public decimal MoneyValue;
    // type it explicitly for sql server, because SQL Server 2005+ provider maps DateTime .Net type to DataType.DateTime2 by default
    [Column(DataType = DataType.DateTime, Configuration = ProviderName.SqlServer)]
    [Column(DataType = DataType.DateTime2, Configuration = ProviderName.Oracle)]
    [Column] public DateTime? DateTimeValue;
    [Column] public DateTime? DateTimeValue2;
    [Column(DataType = DataType.Int16, Configuration = ProviderName.Oracle)]
    [Column] public bool? BoolValue;
    [Column] public Guid? GuidValue;
    [Column] public short? SmallIntValue;
    [Column] public int? IntValue;
    [Column] public long? BigIntValue;
    [Column] public string? StringValue;

    public override bool Equals(object? obj) => Equals(obj as LinqDataTypes2);

    public bool Equals(LinqDataTypes2? other) {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return other.ID == ID &&
        other.MoneyValue == MoneyValue &&
        other.BoolValue == BoolValue &&
        other.GuidValue == GuidValue &&
        other.StringValue == StringValue &&
        other.DateTimeValue.HasValue == DateTimeValue.HasValue &&
        (other.DateTimeValue == null || (
          other.DateTimeValue.Value.Date == DateTimeValue!.Value.Date &&
          other.DateTimeValue.Value.Hour == DateTimeValue.Value.Hour &&
          other.DateTimeValue.Value.Minute == DateTimeValue.Value.Minute &&
          other.DateTimeValue.Value.Second == DateTimeValue.Value.Second
        ));
    }

    public override int GetHashCode() => ID;

    public int CompareTo(object? obj) {
      if (obj == null)
        return 1;
      return ID - ((LinqDataTypes2)obj).ID;
    }

    public static bool operator ==(LinqDataTypes2 left, LinqDataTypes2 right) => Equals(left, right);

    public static bool operator !=(LinqDataTypes2 left, LinqDataTypes2 right) => !Equals(left, right);

    public override string ToString() => string.Format("{{{0,2}, {1,7}, {2:O}, {3,5}, {4}, {5}, '{6}'}}", ID, MoneyValue, DateTimeValue, BoolValue, GuidValue, SmallIntValue, StringValue);
  }
}
