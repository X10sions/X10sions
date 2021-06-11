//using LinqToDB;
//using LinqToDB.Extensions;
//using LinqToDB.SqlQuery;
//using System.Reflection;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
//  class xDatePartExpressionAttribute_TB : Sql.ExpressionAttribute {
//    public xDatePartExpressionAttribute_TB(string sqlProvider, string expression, int datePartIndex, params int[] argIndices)
//      : this(sqlProvider, expression, 100, isExpression: false, null, datePartIndex, argIndices) {
//    }

//    public xDatePartExpressionAttribute_TB(string sqlProvider, string expression, bool isExpression, int datePartIndex, params int[] argIndices)
//      : this(sqlProvider, expression, 100, isExpression, null, datePartIndex, argIndices) {
//    }

//    public xDatePartExpressionAttribute_TB(string sqlProvider, string expression, bool isExpression, string[] partMapping, int datePartIndex, params int[] argIndices)
//      : this(sqlProvider, expression, 100, isExpression, partMapping, datePartIndex, argIndices) {
//    }

//    public xDatePartExpressionAttribute_TB(string sqlProvider, string expression, int precedence__1, bool isExpression, string[] partMapping, int datePartIndex, params int[] argIndices)
//      : base(sqlProvider, expression, argIndices) {
//      _isExpression = isExpression;
//      _partMapping = partMapping;
//      _datePartIndex = datePartIndex;
//      base.Precedence = precedence__1;
//    }

//    private readonly bool _isExpression;
//    private readonly string[] _partMapping;
//    private readonly int _datePartIndex;

//      public override ISqlExpression GetExpression(MemberInfo member, params ISqlExpression[] args) {
//        var part = (Sql.DateParts)((SqlValue)args[_datePartIndex]).Value;
//        var pstr = (_partMapping != null) ? _partMapping[(int)part] : part.ToString();
//        string str = base.Expression.Args(pstr ?? part.ToString());
//        var type = member.GetMemberType();
//        if (_isExpression) {
//          return new SqlExpression(type, str, base.Precedence, ConvertArgs(member, args));
//        }
//        return new SqlFunction(type, str, ConvertArgs(member, args));
//      }
//  }

//}
