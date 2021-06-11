using IQToolkit.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IQToolkit.Data.SQLite {

  public class SQLiteFormatter : SqlFormatter {
    private SQLiteFormatter(QueryLanguage language)
        : base(language) {
    }

    public static new string Format(Expression expression) => Format(expression, new SQLiteLanguage());

    public static string Format(Expression expression, QueryLanguage language) {
      var formatter = new SQLiteFormatter(language);
      formatter.Visit(expression);
      return formatter.ToString();
    }

    protected override Expression VisitSelect(SelectExpression select) {
      AddAliases(select.From);
      Write("SELECT ");
      if (select.IsDistinct) {
        Write("DISTINCT ");
      }
      WriteColumns(select.Columns);
      if (select.From != null) {
        WriteLine(Indentation.Same);
        Write("FROM ");
        VisitSource(select.From);
      }
      if (select.Where != null) {
        WriteLine(Indentation.Same);
        Write("WHERE ");
        VisitPredicate(select.Where);
      }
      if (select.GroupBy != null && select.GroupBy.Count > 0) {
        WriteLine(Indentation.Same);
        Write("GROUP BY ");
        for (int i = 0, n = select.GroupBy.Count; i < n; i++) {
          if (i > 0) {
            Write(", ");
          }
          VisitValue(select.GroupBy[i]);
        }
      }
      if (select.OrderBy != null && select.OrderBy.Count > 0) {
        WriteLine(Indentation.Same);
        Write("ORDER BY ");
        for (int i = 0, n = select.OrderBy.Count; i < n; i++) {
          var exp = select.OrderBy[i];
          if (i > 0) {
            Write(", ");
          }
          VisitValue(exp.Expression);
          if (exp.OrderType != OrderType.Ascending) {
            Write(" DESC");
          }
        }
      }
      if (select.Take != null) {
        WriteLine(Indentation.Same);
        Write("LIMIT ");
        if (select.Skip == null) {
          Write("0");
        } else {
          Write(select.Skip);
        }
        Write(", ");
        Visit(select.Take);
      }
      return select;
    }

    protected override void WriteColumns(System.Collections.ObjectModel.ReadOnlyCollection<ColumnDeclaration> columns) {
      if (columns.Count == 0) {
        Write("0");
        if (IsNested) {
          Write(" AS ");
          WriteColumnName("tmp");
          Write(" ");
        }
      } else {
        base.WriteColumns(columns);
      }
    }

    protected override Expression VisitMemberAccess(MemberExpression m) {
      if (m.Member.DeclaringType == typeof(string)) {
        switch (m.Member.Name) {
          case "Length":
            Write("LENGTH(");
            Visit(m.Expression);
            Write(")");
            return m;
        }
      } else if (m.Member.DeclaringType == typeof(DateTime) || m.Member.DeclaringType == typeof(DateTimeOffset)) {
        switch (m.Member.Name) {
          case "Day":
            Write("STRFTIME('%d', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Month":
            Write("STRFTIME('%m', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Year":
            Write("STRFTIME('%Y', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Hour":
            Write("STRFTIME('%H', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Minute":
            Write("STRFTIME('%M', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Second":
            Write("STRFTIME('%S', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Millisecond":
            Write("STRFTIME('%f', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "DayOfWeek":
            Write("STRFTIME('%w', ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "DayOfYear":
            Write("(STRFTIME('%j', ");
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
            Write("Like(");
            Visit(m.Arguments[0]);
            Write(" || '%', ");
            Visit(m.Object);
            Write(")");
            return m;
          case "EndsWith":
            Write("Like('%' || ");
            Visit(m.Arguments[0]);
            Write(", ");
            Visit(m.Object);
            Write(")");
            return m;
          case "Contains":
            Write("Like('%' || ");
            Visit(m.Arguments[0]);
            Write(" || '%', ");
            Visit(m.Object);
            Write(")");
            return m;
          case "Concat":
            IList<Expression> args = m.Arguments;
            if (args.Count == 1 && args[0].NodeType == ExpressionType.NewArrayInit) {
              args = ((NewArrayExpression)args[0]).Expressions;
            }
            for (int i = 0, n = args.Count; i < n; i++) {
              if (i > 0) Write(" || ");
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
            Write("SUBSTR(");
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
            if (m.Arguments.Count == 1) {
              Write("SUBSTR(");
              Visit(m.Object);
              Write(", 1, ");
              Visit(m.Arguments[0]);
              Write(")");
            } else {
              Write("SUBSTR(");
              Visit(m.Object);
              Write(", 1, ");
              Visit(m.Arguments[0]);
              Write(") + SUBSTR(");
              Visit(m.Object);
              Write(", ");
              Visit(m.Arguments[0]);
              Write(" + ");
              Visit(m.Arguments[1]);
              Write(")");
            }
            return m;
          case "Trim":
            Write("TRIM(");
            Visit(m.Object);
            Write(")");
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
        }
      }
      if (m.Method.Name == "ToString") {
        // no-op
        Visit(m.Object);
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
      } else if (b.NodeType == ExpressionType.ExclusiveOr) {
        // SQLite does not have XOR (^).. Use translation:  ((A & ~B) | (~A & B))
        Write("((");
        VisitValue(b.Left);
        Write(" & ~");
        VisitValue(b.Right);
        Write(") | (~");
        VisitValue(b.Left);
        Write(" & ");
        VisitValue(b.Right);
        Write("))");
        return b;
      }
      return base.VisitBinary(b);
    }

    protected override string GetOperator(BinaryExpression b) {
      if (b.NodeType == ExpressionType.Add && b.Type == typeof(string)) {
        return "||";
      } else {
        return base.GetOperator(b);
      }
    }

    protected override NewExpression VisitNew(NewExpression nex) {
      if (nex.Constructor.DeclaringType == typeof(DateTime)) {
        if (nex.Arguments.Count == 3) {
          Write("(");
          Visit(nex.Arguments[0]);
          Write(" || '-' || (CASE WHEN ");
          Visit(nex.Arguments[1]);
          Write(" < 10 THEN '0' || ");
          Visit(nex.Arguments[1]);
          Write(" ELSE ");
          Visit(nex.Arguments[1]);
          Write(" END)");
          Write(" || '-' || (CASE WHEN ");
          Visit(nex.Arguments[2]);
          Write(" < 10 THEN '0' || ");
          Visit(nex.Arguments[2]);
          Write(" ELSE ");
          Visit(nex.Arguments[2]);
          Write(" END)");
          Write(")");
          return nex;
        } else if (nex.Arguments.Count == 6) {
          Write("(");
          Visit(nex.Arguments[0]);
          Write(" || '-' || ");
          Visit(nex.Arguments[1]);
          Write(" || '-' || ");
          Visit(nex.Arguments[2]);
          Write(" || ' ' || ");
          Visit(nex.Arguments[3]);
          Write(" || ':' || ");
          Visit(nex.Arguments[4]);
          Write(" || ':' || ");
          Visit(nex.Arguments[5]);
          Write(")");
          return nex;
        }
      }
      return base.VisitNew(nex);
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
