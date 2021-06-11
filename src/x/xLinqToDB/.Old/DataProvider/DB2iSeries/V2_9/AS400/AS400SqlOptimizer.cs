using Common;
using LinqToDB;
using LinqToDB.DataProvider;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  [IsCustom(IsCustomReason.ThirdPartyExtension)]
  public class AS400SqlOptimizer : BasicSqlOptimizer {

    public AS400SqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) {
    }

    static void SetQueryParameter(IQueryElement element) {
      if (element.ElementType == QueryElementType.SqlParameter) {
        var sqlParameter = (SqlParameter)element;
        if (!(sqlParameter.Type.SystemType == null) && !sqlParameter.Type.SystemType.IsScalar(false)) {
          return;
        }
        sqlParameter.IsQueryParameter = false;
      }
    }

    public override SqlStatement Finalize(SqlStatement statement) {
      statement.WalkQueries(delegate (SelectQuery selectQuery) {
        new QueryVisitor().Visit(selectQuery, SetQueryParameter);
        return selectQuery;
      });
      statement = base.Finalize(statement);
      switch (statement.QueryType) {
        case QueryType.Delete:
          return base.GetAlternativeDelete((SqlDeleteStatement)statement);
        case QueryType.Update:
          return base.GetAlternativeUpdate((SqlUpdateStatement)statement);
        default:
          return statement;
      }
    }
    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expr, ConvertVisitor visitor, EvaluationContext context) {
      expr = base.ConvertExpressionImpl(expr, visitor, context);
      if (expr is SqlBinaryExpression) {
        var sqlBinaryExpression = (SqlBinaryExpression)expr;
        switch (sqlBinaryExpression.Operation) {
          case "%": {
              object sqlExpression;
              if (sqlBinaryExpression.Expr1.SystemType.IsInteger()) {
                sqlExpression = sqlBinaryExpression.Expr1;
              } else {
                sqlExpression = new SqlFunction(typeof(int), "Int", sqlBinaryExpression.Expr1);
              }
              var sqlExpression3 = (ISqlExpression)sqlExpression;
              return new SqlFunction(sqlBinaryExpression.SystemType, "Mod", sqlExpression3, sqlBinaryExpression.Expr2);
            }
          case "&":
            return new SqlFunction(sqlBinaryExpression.SystemType, "BitAnd", sqlBinaryExpression.Expr1, sqlBinaryExpression.Expr2);
          case "|":
            return new SqlFunction(sqlBinaryExpression.SystemType, "BitOr", sqlBinaryExpression.Expr1, sqlBinaryExpression.Expr2);
          case "^":
            return new SqlFunction(sqlBinaryExpression.SystemType, "BitXor", sqlBinaryExpression.Expr1, sqlBinaryExpression.Expr2);
          case "+":
            if (!(sqlBinaryExpression.SystemType == typeof(string))) {
              return expr;
            }
            return new SqlBinaryExpression(sqlBinaryExpression.SystemType, sqlBinaryExpression.Expr1, "||", sqlBinaryExpression.Expr2, sqlBinaryExpression.Precedence);
        }
      } else if (expr is SqlFunction) {
        var sqlFunction = (SqlFunction)expr;
        switch (sqlFunction.Name) {
          case "Convert": {
              if (sqlFunction.SystemType.ToUnderlying() == typeof(bool)) {
                var sqlExpression4 = base.AlternativeConvertToBoolean(sqlFunction, 1);
                if (sqlExpression4 != null) {
                  return sqlExpression4;
                }
              }
              if (sqlFunction.Parameters[0] is SqlDataType) {
                var sqlDataType = (SqlDataType)sqlFunction.Parameters[0];
                if (sqlDataType.Type.SystemType == typeof(string) && sqlFunction.Parameters[1].SystemType != typeof(string)) {
                  return new SqlFunction(sqlFunction.SystemType, "RTrim", new SqlFunction(typeof(string), "Char", sqlFunction.Parameters[1]));
                }
                DataType dataType;
                if (sqlDataType.Type.Length > 0) {
                  var systemType = sqlFunction.SystemType;
                  dataType = sqlDataType.Type.DataType;
                  return new SqlFunction(systemType, dataType.ToString(), sqlFunction.Parameters[1], new SqlValue(sqlDataType.Type.Length));
                }
                if (sqlDataType.Type.Precision > 0) {
                  var systemType2 = sqlFunction.SystemType;
                  dataType = sqlDataType.Type.DataType;
                  return new SqlFunction(systemType2, dataType.ToString(), sqlFunction.Parameters[1], new SqlValue(sqlDataType.Type.Precision), new SqlValue(sqlDataType.Type.Scale));
                }
                var systemType3 = sqlFunction.SystemType;
                dataType = sqlDataType.Type.DataType;
                return new SqlFunction(systemType3, dataType.ToString(), sqlFunction.Parameters[1]);
              }
              if (sqlFunction.Parameters[0] is SqlFunction) {
                var sqlFunction2 = (SqlFunction)sqlFunction.Parameters[0];
                if (!(sqlFunction2.Name == "Char")) {
                  if (sqlFunction2.Parameters.Length != 1) {
                    return new SqlFunction(sqlFunction.SystemType, sqlFunction2.Name, sqlFunction.Parameters[1], sqlFunction2.Parameters[0], sqlFunction2.Parameters[1]);
                  }
                  return new SqlFunction(sqlFunction.SystemType, sqlFunction2.Name, sqlFunction.Parameters[1], sqlFunction2.Parameters[0]);
                }
                return new SqlFunction(sqlFunction.SystemType, sqlFunction2.Name, sqlFunction.Parameters[1]);
              }
              var sqlExpression5 = (SqlExpression)sqlFunction.Parameters[0];
              return new SqlFunction(sqlFunction.SystemType, sqlExpression5.Expr, sqlFunction.Parameters[1]);
            }
          case "Millisecond":
            return base.Div(new SqlFunction(sqlFunction.SystemType, "Microsecond", sqlFunction.Parameters), 1000);
          case "SmallDateTime":
          case "DateTime":
          case "DateTime2":
            return new SqlFunction(sqlFunction.SystemType, "TimeStamp", sqlFunction.Parameters);
          case "UInt16":
            return new SqlFunction(sqlFunction.SystemType, "Int", sqlFunction.Parameters);
          case "UInt32":
            return new SqlFunction(sqlFunction.SystemType, "BigInt", sqlFunction.Parameters);
          case "UInt64":
            return new SqlFunction(sqlFunction.SystemType, "Decimal", sqlFunction.Parameters);
          case "Byte":
          case "SByte":
          case "Int16":
            return new SqlFunction(sqlFunction.SystemType, "SmallInt", sqlFunction.Parameters);
          case "Int32":
            return new SqlFunction(sqlFunction.SystemType, "Int", sqlFunction.Parameters);
          case "Int64":
            return new SqlFunction(sqlFunction.SystemType, "BigInt", sqlFunction.Parameters);
          case "Double":
            return new SqlFunction(sqlFunction.SystemType, "Float", sqlFunction.Parameters);
          case "Single":
            return new SqlFunction(sqlFunction.SystemType, "Real", sqlFunction.Parameters);
          case "Money":
            return new SqlFunction(sqlFunction.SystemType, "Decimal", sqlFunction.Parameters[0], new SqlValue(19), new SqlValue(4));
          case "SmallMoney":
            return new SqlFunction(sqlFunction.SystemType, "Decimal", sqlFunction.Parameters[0], new SqlValue(10), new SqlValue(4));
          case "VarChar":
            if (!(sqlFunction.Parameters[0].SystemType.ToUnderlying() == typeof(decimal))) {
              break;
            }
            return new SqlFunction(sqlFunction.SystemType, "Char", sqlFunction.Parameters[0]);
          case "NChar":
          case "NVarChar":
            return new SqlFunction(sqlFunction.SystemType, "Char", sqlFunction.Parameters);
          case "DateDiff":
            switch ((Sql.DateParts)((SqlValue)sqlFunction.Parameters[0]).Value) {
              case Sql.DateParts.Day:
                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 86400", 80, sqlFunction.Parameters[2], sqlFunction.Parameters[1]);
              case Sql.DateParts.Hour:
                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 3600", 80, sqlFunction.Parameters[2], sqlFunction.Parameters[1]);
              case Sql.DateParts.Minute:
                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 60", 80, sqlFunction.Parameters[2], sqlFunction.Parameters[1]);
              case Sql.DateParts.Second:
                return new SqlExpression(typeof(int), "(Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))", 60, sqlFunction.Parameters[2], sqlFunction.Parameters[1]);
              case Sql.DateParts.Millisecond:
                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) * 1000 + (MICROSECOND({0}) - MICROSECOND({1})) / 1000", 60, sqlFunction.Parameters[2], sqlFunction.Parameters[1]);
            }
            break;
        }
      }
      return expr;
    }
  }
}