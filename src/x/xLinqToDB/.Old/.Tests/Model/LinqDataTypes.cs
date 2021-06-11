using LinqToDB.Mapping;
using System;
using System.Data.Linq;

namespace LinqToDB.Tests.Model {
  public class LinqDataTypes : IEquatable<LinqDataTypes>, IComparable {
    public int ID;
    public decimal MoneyValue;
    public DateTime DateTimeValue;
    [Column(DataType = DataType.Int16, Configuration = ProviderName.Oracle)] public bool BoolValue;
    public Guid GuidValue;
    public Binary? BinaryValue;
    public short SmallIntValue;
    public string? StringValue;

    public override bool Equals(object? obj) => Equals(obj as LinqDataTypes);

    public bool Equals(LinqDataTypes? other) {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return
        other.ID == ID &&
        other.MoneyValue == MoneyValue &&
        other.BoolValue == BoolValue &&
        other.GuidValue == GuidValue &&
        other.SmallIntValue == SmallIntValue &&
        other.DateTimeValue.Date == DateTimeValue.Date &&
        other.DateTimeValue.Hour == DateTimeValue.Hour &&
        other.DateTimeValue.Minute == DateTimeValue.Minute &&
        other.DateTimeValue.Second == DateTimeValue.Second &&
        (
          other.StringValue == StringValue ||
          string.IsNullOrWhiteSpace(other.StringValue) == string.IsNullOrWhiteSpace(StringValue)
        )
        ;
    }

    public override int GetHashCode() => ID;

    public int CompareTo(object? obj) {
      if (obj == null)
        return 1;

      return ID - ((LinqDataTypes)obj).ID;
    }

    public static bool operator ==(LinqDataTypes left, LinqDataTypes right) => Equals(left, right);

    public static bool operator !=(LinqDataTypes left, LinqDataTypes right) => !Equals(left, right);

    public override string ToString() => string.Format("{{{0,2}, {1,7}, {2:O}, {3,5}, {4}, {5}, '{6}'}}", ID, MoneyValue, DateTimeValue, BoolValue, GuidValue, SmallIntValue, StringValue);
  }
}
