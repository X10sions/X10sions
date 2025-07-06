using System.Data;

namespace Common.Models.MinMaxRange;
public class DateRange : RangeBase<DateTime> {
  public DateRange(DateTime? min, DateTime? max) : base(min, max) { }

  public override string SqlExpression(DateTime? value) => value.SqlLiteralDate();
  public override DbType DbType => DbType.Date;
}
