using System;
using System.Data;

namespace Common.Models.MinMaxRange {
  public class TimeRange : _BaseRange<TimeSpan> {
    public TimeRange(TimeSpan? min, TimeSpan? max) : base(min, max) { }

    public override DbType DbType => DbType.Time;

    public override string SqlExpression(TimeSpan? value) => value.SqlLiteralTime();
  }
}
