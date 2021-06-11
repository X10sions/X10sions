using LinqToDB;
using LinqToDB.SqlQuery;
using System.Reflection;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {

  class xConcatExpressionAttribute_TB : Sql.ExpressionAttribute {
    public xConcatExpressionAttribute_TB()
      : base(string.Empty) {
    }

    public override ISqlExpression GetExpression(MemberInfo member, params ISqlExpression[] args) {
      var arr = new ISqlExpression[args.Length];
      for (var i = 0; i <= args.Length; i++) {
        var arg = args[i];
        if (arg.SystemType == typeof(string)) {
          arr[i] = arg;
        } else {
          var len = arg.SystemType == null || arg.SystemType == typeof(object) ? 100 : SqlDataType.GetMaxDisplaySize(SqlDataType.GetDataType(arg.SystemType).Type.DataType);
          arr[i] = new SqlFunction(typeof(string), "Convert", new SqlDataType(DataType.VarChar, len), arg);
        }
      }
      if (arr.Length == 1) {
        return arr[0];
      }
      var expr = new SqlBinaryExpression(typeof(string), arr[0], "+", arr[1]);
      var num2 = arr.Length - 1;
      for (var i = 2; i <= num2; i++) {
        expr = new SqlBinaryExpression(typeof(string), expr, "+", arr[i]);
      }
      return expr;
    }
  }
}