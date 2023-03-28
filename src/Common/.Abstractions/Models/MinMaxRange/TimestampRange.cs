using System.Data;

namespace Common.Models.MinMaxRange;
public class TimestampRange : RangeBase<DateTime> {
  public TimestampRange(DateTime? min, DateTime? max, int milliSecondsPrecision)
    : base(min, max) {
    MilliSecondsPrecision = milliSecondsPrecision;
  }

  public int MilliSecondsPrecision { get; set; }

  public override DbType DbType => DbType.DateTime2;

  public override string SqlExpression(DateTime? value) => value.SqlLiteralTimestamp(new SqlTimestampOptions { MilliSecondsPrecision = MilliSecondsPrecision });
}
