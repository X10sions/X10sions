using IQToolkit.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IQToolkit.Data.Access {

  /// <summary>
  /// Formats a query expression into MS Access query syntax
  /// </summary>
  public class AccessFormatter : SqlFormatter {
    private AccessFormatter(QueryLanguage language)
        : base(language) {
    }

    public static new string Format(Expression expression) {
      var formatter = new AccessFormatter(new AccessLanguage());
      formatter.FormatWithParameters(expression);
      return formatter.ToString();
    }

    protected virtual void FormatWithParameters(Expression expression) {
      var names = NamedValueGatherer.Gather(expression);
      if (names.Count > 0) {
        Write("PARAMETERS ");

        for (int i = 0, n = names.Count; i < n; i++) {
          if (i > 0)
            Write(", ");

          WriteParameterName(names[i].Name);
          Write(" ");
          Write(Language.TypeSystem.Format(names[i].QueryType, true));
        }

        Write(";");
        WriteLine(Indentation.Same);
      }
      Visit(expression);
    }

    protected override void WriteParameterName(string name) => Write(name);

    protected override Expression VisitSelect(SelectExpression select) {
      if (select.Skip != null) {
        if (select.OrderBy == null && select.OrderBy.Count == 0) {
          throw new NotSupportedException("Access cannot support the 'skip' operation without explicit ordering");
        } else if (select.Take == null) {
          throw new NotSupportedException("Access cannot support the 'skip' operation without the 'take' operation");
        } else {
          throw new NotSupportedException("Access cannot support the 'skip' operation in this query");
        }
      }
      return base.VisitSelect(select);
    }

    protected override void WriteTopClause(Expression expression) {
      Write("TOP ");
      Write(expression);
      Write(" ");
    }

    protected override Expression VisitJoin(JoinExpression join) {
      if (join.Join == JoinType.CrossJoin) {
        VisitJoinLeft(join.Left);
        Write(", ");
        VisitJoinRight(join.Right);
        return join;
      }
      return base.VisitJoin(join);
    }

    protected override Expression VisitJoinLeft(Expression source) {
      if (source is JoinExpression) {
        Write("(");
        VisitSource(source);
        Write(")");
      } else {
        VisitSource(source);
      }
      return source;
    }

    protected override Expression VisitDeclaration(DeclarationCommand decl) {
      if (decl.Source != null) {
        Visit(decl.Source);
        return decl;
      }
      return base.VisitDeclaration(decl);
    }

    protected override void WriteColumns(System.Collections.ObjectModel.ReadOnlyCollection<ColumnDeclaration> columns) {
      if (columns.Count == 0) {
        Write("0");
      } else {
        base.WriteColumns(columns);
      }
    }

    protected override Expression VisitMemberAccess(MemberExpression m) {
      if (m.Member.DeclaringType == typeof(string)) {
        switch (m.Member.Name) {
          case "Length":
            Write("Len(");
            Visit(m.Expression);
            Write(")");
            return m;
        }
      } else if (m.Member.DeclaringType == typeof(DateTime) || m.Member.DeclaringType == typeof(DateTimeOffset)) {
        switch (m.Member.Name) {
          case "Day":
            Write("Day(");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Month":
            Write("Month(");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Year":
            Write("Year(");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Hour":
            Write("Hour( ");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Minute":
            Write("Minute(");
            Visit(m.Expression);
            Write(")");
            return m;
          case "Second":
            Write("Second(");
            Visit(m.Expression);
            Write(")");
            return m;
          case "DayOfWeek":
            Write("(Weekday(");
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
              if (i > 0) Write(" + ");
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
            Write("UCase(");
            Visit(m.Object);
            Write(")");
            return m;
          case "ToLower":
            Write("LCase(");
            Visit(m.Object);
            Write(")");
            return m;
          case "Substring":
            Write("Mid(");
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
          case "Replace":
            Write("Replace(");
            Visit(m.Object);
            Write(", ");
            Visit(m.Arguments[0]);
            Write(", ");
            Visit(m.Arguments[1]);
            Write(")");
            return m;
          case "IndexOf":
            Write("(InStr(");
            if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int)) {
              Visit(m.Arguments[1]);
              Write(" + 1, ");
            }
            Visit(m.Object);
            Write(", ");
            Visit(m.Arguments[0]);
            Write(") - 1)");
            return m;
          case "Trim":
            Write("Trim(");
            Visit(m.Object);
            Write(")");
            return m;

        }
      } else if (m.Method.DeclaringType == typeof(DateTime)) {
        switch (m.Method.Name) {
          case "op_Subtract":
            if (m.Arguments[1].Type == typeof(DateTime)) {
              Write("DateDiff(\"d\",");
              Visit(m.Arguments[0]);
              Write(",");
              Visit(m.Arguments[1]);
              Write(")");
              return m;
            }
            break;
          case "AddYears":
            Write("DateAdd(\"yyyy\",");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddMonths":
            Write("DateAdd(\"m\",");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddDays":
            Write("DateAdd(\"d\",");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddHours":
            Write("DateAdd(\"h\",");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddMinutes":
            Write("DateAdd(\"n\",");
            Visit(m.Arguments[0]);
            Write(",");
            Visit(m.Object);
            Write(")");
            return m;
          case "AddSeconds":
            Write("DateAdd(\"s\",");
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
          case "Truncate":
            Write("Fix");
            Write("(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Round":
            if (m.Arguments.Count == 1) {
              Write("Round(");
              Visit(m.Arguments[0]);
              Write(")");
              return m;
            }
            break;
        }
      } else if (m.Method.DeclaringType == typeof(Math)) {
        switch (m.Method.Name) {
          case "Abs":
          case "Cos":
          case "Exp":
          case "Sin":
          case "Tan":
            Write(m.Method.Name.ToUpper());
            Write("(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Sqrt":
            Write("Sqr(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Sign":
            Write("Sgn(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Atan":
            Write("Atn(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
          case "Log":
            if (m.Arguments.Count == 1) {
              Write("Log(");
              Visit(m.Arguments[0]);
              Write(")");
              return m;
            }
            break;
          case "Pow":
            Visit(m.Arguments[0]);
            Write("^");
            Visit(m.Arguments[1]);
            return m;
          case "Round":
            if (m.Arguments.Count == 1) {
              Write("Round(");
              Visit(m.Arguments[0]);
              Write(")");
              return m;
            }
            break;
          case "Truncate":
            Write("Fix(");
            Visit(m.Arguments[0]);
            Write(")");
            return m;
        }
      }
      if (m.Method.Name == "ToString") {
        if (m.Object.Type != typeof(string)) {
          Write("CStr(");
          Visit(m.Object);
          Write(")");
        } else {
          Visit(m.Object);
        }
        return m;
      } else if (!m.Method.IsStatic && m.Method.Name == "CompareTo" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 1) {
        Write("IIF(");
        Visit(m.Object);
        Write(" = ");
        Visit(m.Arguments[0]);
        Write(", 0, IIF(");
        Visit(m.Object);
        Write(" < ");
        Visit(m.Arguments[0]);
        Write(", -1, 1))");
        return m;
      } else if (m.Method.IsStatic && m.Method.Name == "Compare" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 2) {
        Write("IIF(");
        Visit(m.Arguments[0]);
        Write(" = ");
        Visit(m.Arguments[1]);
        Write(", 0, IIF(");
        Visit(m.Arguments[0]);
        Write(" < ");
        Visit(m.Arguments[1]);
        Write(", -1, 1))");
        return m;
      }
      return base.VisitMethodCall(m);
    }

    protected override NewExpression VisitNew(NewExpression nex) {
      if (nex.Constructor.DeclaringType == typeof(DateTime)) {
        if (nex.Arguments.Count == 3) {
          Write("CDate(");
          Visit(nex.Arguments[0]);
          Write(" & '/' & ");
          Visit(nex.Arguments[1]);
          Write(" & '/' & ");
          Visit(nex.Arguments[2]);
          Write(")");
          return nex;
        } else if (nex.Arguments.Count == 6) {
          Write("CDate(");
          Visit(nex.Arguments[0]);
          Write(" & '/' & ");
          Visit(nex.Arguments[1]);
          Write(" & '/' & ");
          Visit(nex.Arguments[2]);
          Write(" & ' ' & ");
          Visit(nex.Arguments[3]);
          Write(" & ':' & ");
          Visit(nex.Arguments[4]);
          Write(" & + ':' & ");
          Visit(nex.Arguments[5]);
          Write(")");
          return nex;
        }
      }
      return base.VisitNew(nex);
    }

    protected override Expression VisitBinary(BinaryExpression b) {
      if (b.NodeType == ExpressionType.Power) {
        Write("(");
        VisitValue(b.Left);
        Write("^");
        VisitValue(b.Right);
        Write(")");
        return b;
      } else if (b.NodeType == ExpressionType.Coalesce) {
        Write("IIF(");
        VisitValue(b.Left);
        Write(" IS NOT NULL, ");
        VisitValue(b.Left);
        Write(", ");
        VisitValue(b.Right);
        Write(")");
        return b;
      } else if (b.NodeType == ExpressionType.LeftShift) {
        Write("(");
        VisitValue(b.Left);
        Write(" * (2^");
        VisitValue(b.Right);
        Write("))");
        return b;
      } else if (b.NodeType == ExpressionType.RightShift) {
        Write("(");
        VisitValue(b.Left);
        Write(@" \ (2^");
        VisitValue(b.Right);
        Write("))");
        return b;
      }
      return base.VisitBinary(b);
    }

    protected override Expression VisitConditional(ConditionalExpression c) {
      Write("IIF(");
      VisitPredicate(c.Test);
      Write(", ");
      VisitValue(c.IfTrue);
      Write(", ");
      VisitValue(c.IfFalse);
      Write(")");
      return c;
    }

    protected override string GetOperator(BinaryExpression b) {
      switch (b.NodeType) {
        case ExpressionType.And:
          if (b.Type == typeof(bool) || b.Type == typeof(bool?))
            return "AND";
          return "BAND";
        case ExpressionType.AndAlso:
          return "AND";
        case ExpressionType.Or:
          if (b.Type == typeof(bool) || b.Type == typeof(bool?))
            return "OR";
          return "BOR";
        case ExpressionType.OrElse:
          return "OR";
        case ExpressionType.Modulo:
          return "MOD";
        case ExpressionType.ExclusiveOr:
          return "XOR";
        case ExpressionType.Divide:
          if (IsInteger(b.Type))
            return "\\"; // integer divide
          goto default;
        default:
          return base.GetOperator(b);
      }
    }

    protected override string GetOperator(UnaryExpression u) {
      switch (u.NodeType) {
        case ExpressionType.Not:
          return "NOT";
        default:
          return base.GetOperator(u);
      }
    }

    protected override string GetOperator(string methodName) {
      if (methodName == "Remainder") {
        return "MOD";
      } else {
        return base.GetOperator(methodName);
      }
    }

    protected override void WriteValue(object value) {
      if (value != null && value.GetType() == typeof(bool)) {
        Write(((bool)value) ? -1 : 0);
      } else {
        base.WriteValue(value);
      }
    }
  }
}
