using System;
using System.Data;

namespace Common.Models.MinMaxRange {
  public class IntegerRange : _BaseRange<int> {
    public IntegerRange(int? min, int? max) : base(min, max) { }

    public static readonly IntegerRange YearRange = new IntegerRange(1, 9999);
    public static readonly IntegerRange MonthRange = new IntegerRange(1, 12);
    public static readonly IntegerRange DayRange = new IntegerRange(1, 31);
    public static readonly IntegerRange HourRange = new IntegerRange(0, 23);
    public static readonly IntegerRange MinuteRange = new IntegerRange(0, 59);
    public static readonly IntegerRange SecondRange = new IntegerRange(0, 59);

    public override DbType DbType => DbType.Int32;
    public override string SqlExpression(int? value) => value.SqlLiteral();
  }
}
