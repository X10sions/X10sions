using IQToolkit.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IQToolkit.Data.SqlServerCe {
  /// <summary>
  /// Formats a query expression into SQL Server Compact Edition language syntax
  /// </summary>
  public class SqlCeFormatter : SqlFormatter {
    public SqlCeFormatter(QueryLanguage language)
        : base(language) {
    }

    protected override void WriteAggregateName(string aggregateName) {
      if (aggregateName == "LongCount") {
        Write("COUNT");
      } else {
        base.WriteAggregateName(aggregateName);
      }
    }

    protected override Expression VisitSelect(SelectExpression select) {
      if (select.Skip != null) // this should have been rewritten
        throw new InvalidOperationException("SQLCE can only support Skip when it is immediately followed by Take");
      return base.VisitSelect(select);
    }

    protected override Expression VisitMemberAccess(MemberExpression m) {
      if (m.Member.DeclaringType == typeof(string)) {
        switch (m.Member.Name) {
          case "Length":
            Write("LEN(");
            Visit(m.Expression);
            Write(")");
            return m;
        }
      } else if (m.Member.DeclaringType == typeof(DateTime) || m.Member.DeclaringType == typeof(DateTimeOffset)) {
        switch (m.Member.Name) {
          case "Day":
            Write("DATEPART(day, ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Month":
            Write("DATEPART(month, ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Year":
            Write("DATEPART(year, ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Hour":
            Write("DATEPART(hour, ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Minute":
            Write("DATEPART(minute, ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Second":
            Write("DATEPART(second, ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Millisecond":
            Write("DATEPART(millisecond, ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "DayOfWeek":
            Write("(DATEPART(weekday, ");
            Visit(m.Expression);
            Write(") - 1)");
            return m;
          case "DayOfYear":
            Write("(DATEPART(dayofyear, ");
            Visit(m.Expression);
            Write(") - 1)");
            return m;
        }
      }
      return base.VisitMemberAccess(m);
    }

    protected override Expression VisitMethodCall(MethodCallExpression m) {
      if (m.Method.DeclaringType == typeof(string)) {
        switch (m.Method.Name) {
          case "StartsWith":
            Write("(");
            Visit(m.Object);
            Write(" LIKE ");
            Visit(m.Arguments[0]);
            Write(" + '%')");
            return m;
          case "EndsWith":
            Write("(");
            Visit(m.Object);
            Write(" LIKE '%' + ");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Contains":
            Write("(");
            Visit(m.Object);
            Write(" LIKE '%' + ");
            Visit(m.Arguments[0]);
            Write(" + '%')");
            return m;
          case "Concat":
            IList<Expression> args = m.Arguments;
            if (args.Count == 1 && args[0].NodeType == ExpressionType.NewArrayInit) {
              args = ((NewArrayExpression)args[0]).Expressions;
            }
            for (int i = 0, n = args.Count; i < n; i++) {
              if (i > 0)
                Write(" + ");
              Visit(args[i]);
            }
            return m;
          case "IsNullOrEmpty":
            Write("(");
            Visit(m.Arguments[0]);
            Write(" IS NULL OR ");
            Visit(m.Arguments[0]);
            Write(" = '')");
            return m;
          case "ToUpper":
            Write("UPPER(");
            Visit(m.Object);
            Write(")");
            return m;
          case "ToLower":
            Write("LOWER(");
            Visit(m.Object);
            Write(")");
            return m;
          case "Replace":
            Write("REPLACE(");
            Visit(m.Object);
            Write(", ");
            Visit(m.Arguments[0]);
            Write(", ");
            Visit(m.Arguments[1]);
            Write(")");
            return m;
          case "Substring":
            Write("SUBSTRING(");
            Visit(m.Object);
            Write(", ");
            Visit(m.Arguments[0]);
            Write(" + 1, ");
            if (m.Arguments.Count == 2) {
              Visit(m.Arguments[1]);
            } else {
              Write("8000");
            }
            Write(")");
            return m;
          case "Remove":
            Write("STUFF(");
            Visit(m.Object);
            Write(", ");
            Visit(m.Arguments[0]);
            Write(" + 1, ");
            if (m.Arguments.Count == 2) {
              Visit(m.Arguments[1]);
            } else {
              Write("8000");
            }
            Write(", '')");
            return m;
          case "IndexOf":
            Write("(CHARINDEX(");
            Visit(m.Arguments[0]);
            Write(", ");
            Visit(m.Object);
            if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int)) {
              Write(", ");
              Visit(m.Arguments[1]);
              Write(" + 1");
            }
            Write(") - 1)");
            return m;
          case "Trim":
            Write("RTRIM(LTRIM(");
            Visit(m.Object);
            Write("))");
            return m;
        }
      } else if (m.Method.DeclaringType == typeof(DateTime)) {
        switch (m.Method.Name) {
          case "op_Subtract":
            if (m.Arguments[1].Type == typeof(DateTime)) {
              Write("DATEDIFF(");
              Visit(m.Arguments[0]);
              Write(", ");
              Visit(m.Arguments[1]);
              Write(")");
              return m;
            }
            break;
          case "AddYears":
            Write("DATEADD(YYYY,");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddMonths":
            Write("DATEADD(MM,");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddDays":
            Write("DATEADD(DAY,");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddHours":
            Write("DATEADD(HH,");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddMinutes":
            Write("DATEADD(MI,");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddSeconds":
            Write("DATEADD(SS,");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddMilliseconds":
            Write("DATEADD(MS,");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
        }
      } else if (m.Method.DeclaringType == typeof(decimal)) {
        switch (m.Method.Name) {
          case "Add":
          case "Subtract":
          case "Multiply":
          case "Divide":
          case "Remainder":
            Write("(");
            VisitValue(m.Arguments[0]);
            Write(" ");
            Write(GetOperator(m.Method.Name));
            Write(" ");
            VisitValue(m.Arguments[1]);
            Write(")");
            return m;
          case "Negate":
            Write("-");
            Visit(m.Arguments[0]);
            Write("");
            return m;
          case "Ceiling":
          case "Floor":
            Write(m.Method.Name.ToUpper());
            Write("(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Round":
            if (m.Arguments.Count == 1) {
              Write("ROUND(");
              Visit(m.Arguments[0]);
              Write(", 0)");
              return m;
            } else if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int)) {
              Write("ROUND(");
              Visit(m.Arguments[0]);
              Write(", ");
              Visit(m.Arguments[1]);
              Write(")");
              return m;
            }
            break;
          case "Truncate":
            Write("ROUND(");
            Visit(m.Arguments[0]);
            Write(", 0, 1)");
            return m;
        }
      } else if (m.Method.DeclaringType == typeof(Math)) {
        switch (m.Method.Name) {
          case "Abs":
          case "Acos":
          case "Asin":
          case "Atan":
          case "Cos":
          case "Exp":
          case "Log10":
          case "Sin":
          case "Tan":
          case "Sqrt":
          case "Sign":
          case "Ceiling":
          case "Floor":
            Write(m.Method.Name.ToUpper());
            Write("(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Atan2":
            Write("ATN2(");
            Visit(m.Arguments[0]);
            Write(", ");
            Visit(m.Arguments[1]);
            Write(")");
            return m;
          case "Log":
            if (m.Arguments.Count == 1) {
              goto case "Log10";
            }
            break;
          case "Pow":
            Write("POWER(");
            Visit(m.Arguments[0]);
            Write(", ");
            Visit(m.Arguments[1]);
            Write(")");
            return m;
          case "Round":
            if (m.Arguments.Count == 1) {
              Write("ROUND(");
              Visit(m.Arguments[0]);
              Write(", 0)");
              return m;
            } else if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int)) {
              Write("ROUND(");
              Visit(m.Arguments[0]);
              Write(", ");
              Visit(m.Arguments[1]);
              Write(")");
              return m;
            }
            break;
          case "Truncate":
            Write("ROUND(");
            Visit(m.Arguments[0]);
            Write(", 0, 1)");
            return m;
        }
      }
      if (m.Method.Name == "ToString") {
        if (m.Object.Type != typeof(string)) {
          Write("CONVERT(NVARCHAR, ");
          Visit(m.Object);
          Write(")");
        } else {
          Visit(m.Object);
        }
        return m;
      } else if (!m.Method.IsStatic && m.Method.Name == "CompareTo" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 1) {
        Write("(CASE WHEN ");
        Visit(m.Object);
        Write(" = ");
        Visit(m.Arguments[0]);
        Write(" THEN 0 WHEN ");
        Visit(m.Object);
        Write(" < ");
        Visit(m.Arguments[0]);
        Write(" THEN -1 ELSE 1 END)");
        return m;
      } else if (m.Method.IsStatic && m.Method.Name == "Compare" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 2) {
        Write("(CASE WHEN ");
        Visit(m.Arguments[0]);
        Write(" = ");
        Visit(m.Arguments[1]);
        Write(" THEN 0 WHEN ");
        Visit(m.Arguments[0]);
        Write(" < ");
        Visit(m.Arguments[1]);
        Write(" THEN -1 ELSE 1 END)");
        return m;
      }
      return base.VisitMethodCall(m);
    }

    protected override NewExpression VisitNew(NewExpression nex) {
      if (nex.Constructor.DeclaringType == typeof(DateTime)) {
        if (nex.Arguments.Count == 3) {
          Write("Convert(DateTime, ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[0]);
          Write(") + '/' + ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[1]);
          Write(") + '/' + ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[2]);
          Write("))");
          return nex;
        } else if (nex.Arguments.Count == 6) {
          Write("Convert(DateTime, ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[0]);
          Write(") + '/' + ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[1]);
          Write(") + '/' + ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[2]);
          Write(") + ' ' + ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[3]);
          Write(") + ':' + ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[4]);
          Write(") + ':' + ");
          Write("Convert(nvarchar, ");
          Visit(nex.Arguments[5]);
          Write("))");
          return nex;
        }
      }
      return base.VisitNew(nex);
    }

    protected override Expression VisitBinary(BinaryExpression b) {
      if (b.NodeType == ExpressionType.Power) {
        Write("POWER(");
        VisitValue(b.Left);
        Write(", ");
        VisitValue(b.Right);
        Write(")");
        return b;
      } else if (b.NodeType == ExpressionType.Coalesce) {
        Write("COALESCE(");
        VisitValue(b.Left);
        Write(", ");
        var right = b.Right;
        while (right.NodeType == ExpressionType.Coalesce) {
          var rb = (BinaryExpression)right;
          VisitValue(rb.Left);
          Write(", ");
          right = rb.Right;
        }
        VisitValue(right);
        Write(")");
        return b;
      } else if (b.NodeType == ExpressionType.LeftShift) {
        Write("(");
        VisitValue(b.Left);
        Write(" * POWER(2, ");
        VisitValue(b.Right);
        Write("))");
        return b;
      } else if (b.NodeType == ExpressionType.RightShift) {
        Write("(");
        VisitValue(b.Left);
        Write(" / POWER(2, ");
        VisitValue(b.Right);
        Write("))");
        return b;
      }
      return base.VisitBinary(b);
    }

    protected override Expression VisitValue(Expression expr) {
      if (IsPredicate(expr)) {
        Write("CASE WHEN (");
        Visit(expr);
        Write(") THEN 1 ELSE 0 END");
        return expr;
      }
      return base.VisitValue(expr);
    }

    protected override Expression VisitConditional(ConditionalExpression c) {
      if (IsPredicate(c.Test)) {
        Write("(CASE WHEN ");
        VisitPredicate(c.Test);
        Write(" THEN ");
        VisitValue(c.IfTrue);
        var ifFalse = c.IfFalse;
        while (ifFalse != null && ifFalse.NodeType == ExpressionType.Conditional) {
          var fc = (ConditionalExpression)ifFalse;
          Write(" WHEN ");
          VisitPredicate(fc.Test);
          Write(" THEN ");
          VisitValue(fc.IfTrue);
          ifFalse = fc.IfFalse;
        }
        if (ifFalse != null) {
          Write(" ELSE ");
          VisitValue(ifFalse);
        }
        Write(" END)");
      } else {
        Write("(CASE ");
        VisitValue(c.Test);
        Write(" WHEN 0 THEN ");
        VisitValue(c.IfFalse);
        Write(" ELSE ");
        VisitValue(c.IfTrue);
        Write(" END)");
      }
      return c;
    }
  }
}
