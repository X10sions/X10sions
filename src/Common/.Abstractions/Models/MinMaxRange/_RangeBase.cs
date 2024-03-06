using System.Data;

namespace Common.Models.MinMaxRange {
  public abstract class RangeBase<T> where T : struct, IComparable, IComparable<T> {
    public RangeBase(T? min, T? max) {
      Min = min;
      Max = max;
    }

    public T? Min { get; set; }
    public T? Max { get; set; }

    public override string ToString() => $"[{Min} - {Max}]";
    public bool IsValid => Min?.CompareTo(Max.Value) <= 0;
    public bool ContainsValue(T? value) => value.Value.IsBetween(Min.Value, Max.Value);
    public bool IsInsideRange(RangeBase<T> range) => IsValid && range.IsValid && range.ContainsValue(Min.Value) && range.ContainsValue(Max.Value);
    public bool ContainsRange(RangeBase<T> range) => IsValid && range.IsValid && ContainsValue(range.Min.Value) && ContainsValue(range.Max.Value);

    public abstract string SqlExpression(T? value);

    public string MinSqlExpression() => SqlExpression(Min);
    public string MaxSqlExpression() => SqlExpression(Max);

    public abstract DbType DbType { get; }
    public string MinSqlQualifiedValue => DbType.GetSqlQualifiedValue(Min, true);
    public string MaxSqlQualifiedValue => DbType.GetSqlQualifiedValue(Max, true);

    public string ToSql(string columnOrExpression) {
      if (Min.HasValue && Max.HasValue) {
        return Min.Value.Equals(Max.Value)
          ? $"{columnOrExpression} = {MinSqlQualifiedValue}"
          : $"{columnOrExpression} Between {MinSqlQualifiedValue} And {MaxSqlQualifiedValue}";
      }
      return Min.HasValue
        ? $"{columnOrExpression} >= {MinSqlQualifiedValue}"
        : Max.HasValue ? $"{columnOrExpression} <= {MaxSqlQualifiedValue}" : string.Empty;
    }

  }
}