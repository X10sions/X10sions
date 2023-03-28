using System.Data;

namespace Common.Models.MinMaxRange;
public class DecimalRange : RangeBase<decimal> {
  public DecimalRange(decimal? min, decimal? max) : base(min, max) { }
  public override DbType DbType => DbType.Decimal;

  public override string SqlExpression(decimal? value) => value.SqlLiteral();
}

