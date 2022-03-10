using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System.Text;

namespace LinqToDB.DataProvider.DB2iSeries.MTGFS01_V2_9_8 {

  [Obsolete]
  public interface Obsolete_IValueContainer {
    object Value { get; }
  }

  [Obsolete]
  public static class Obsolete_IValueContainerExtensions {

    public static ISqlExpression GetDateParmeter(this Obsolete_IValueContainer parameter) {
      if (parameter != null && parameter is SqlParameter) {
        SqlParameter obj = (SqlParameter)parameter;
        obj.Type = obj.Type.WithDataType(DataType.Date);
        return obj;
      }
      return null;
    }

    public static ISqlExpression GetParmeter(this Obsolete_IValueContainer parameter, Type type) {
      if (type != null && parameter != null) {
        if (parameter is SqlValue) {
          if (((SqlValue)parameter).ValueType.SystemType == null) {
            return new SqlValue(type, parameter.Value);
          }
        } else if (parameter is SqlParameter) {
          SqlParameter obj = (SqlParameter)parameter;
          obj.Type = obj.Type.WithSystemType(obj.Type.SystemType ?? type);
          return obj;
        }
      }
      return null;
    }
  }

  public abstract partial class DB2iSeriesSqlBuilder_Base : BasicSqlBuilder {

    [Obsolete]
    protected void Obsolete_AlternativeBuildSql(bool implementOrderBy, Action buildSql, string emptyOrderByValue) {
      SelectQuery selectQuery = Statement.SelectQuery;
      if (selectQuery != null && Obsolete_NeedSkip(selectQuery)) {
        SkipAlias = false;
        string[] tempAliases = GetTempAliases(2, "t");
        string text = GetTempAliases(1, "rn")[0];
        AppendIndent().Append("SELECT *").AppendLine();
        AppendIndent().Append("FROM").AppendLine();
        AppendIndent().Append("(").AppendLine();
        Indent++;
        AppendIndent().Append("SELECT").AppendLine();
        Indent++;
        AppendIndent().AppendFormat("{0}.*,", tempAliases[0]).AppendLine();
        AppendIndent().Append("ROW_NUMBER() OVER");
        if (!selectQuery.OrderBy.IsEmpty && !implementOrderBy) {
          StringBuilder.Append("()");
        } else {
          StringBuilder.AppendLine();
          AppendIndent().Append("(").AppendLine();
          Indent++;
          if (selectQuery.OrderBy.IsEmpty) {
            AppendIndent().Append("ORDER BY").AppendLine();
            if (selectQuery.Select.Columns.Count > 0) {
              Obsolete_BuildAliases(tempAliases[0], selectQuery.Select.Columns.Take(1).ToList(), null);
            } else {
              AppendIndent().Append(emptyOrderByValue).AppendLine();
            }
          } else {
            Obsolete_BuildAlternativeOrderBy(ascending: true);
          }
          Indent--;
          AppendIndent().Append(")");
        }
        StringBuilder.Append(" as ").Append(text).AppendLine();
        Indent--;
        AppendIndent().Append("FROM").AppendLine();
        AppendIndent().Append("(").AppendLine();
        Indent++;
        buildSql();
        Indent--;
        AppendIndent().AppendFormat(") {0}", tempAliases[0]).AppendLine();
        Indent--;
        AppendIndent().AppendFormat(") {0}", tempAliases[1]).AppendLine();
        AppendIndent().Append("WHERE").AppendLine();
        Indent++;
        if (NeedTake(selectQuery)) {
          ISqlExpression sqlExpression = Obsolete_Add(selectQuery.Select.SkipValue, 1);
          ISqlExpression sqlExpression2 = Obsolete_Add<int>(selectQuery.Select.SkipValue, selectQuery.Select.TakeValue);
          SqlValue sqlValue;
          SqlValue sqlValue2;
          if ((sqlValue = (sqlExpression as SqlValue)) != null && (sqlValue2 = (sqlExpression2 as SqlValue)) != null && object.Equals(sqlValue.Value, sqlValue2.Value)) {
            AppendIndent().AppendFormat("{0}.{1} = ", tempAliases[1], text);
            BuildExpression(sqlExpression);
          } else {
            AppendIndent().AppendFormat("{0}.{1} BETWEEN ", tempAliases[1], text);
            BuildExpression(sqlExpression);
            StringBuilder.Append(" AND ");
            BuildExpression(sqlExpression2);
          }
        } else {
          AppendIndent().AppendFormat("{0}.{1} > ", tempAliases[1], text);
          BuildExpression(selectQuery.Select.SkipValue);
        }
        StringBuilder.AppendLine();
        Indent--;
      } else {
        buildSql();
      }
    }

    [Obsolete] private ISqlExpression Obsolete_Add(ISqlExpression expr1, ISqlExpression expr2, Type type) => new SqlBinaryExpression(type, expr1, "+", expr2, Precedence.Additive);
    //[Obsolete] private ISqlExpression Obsolete_Add(ISqlExpression expr1, ISqlExpression expr2, Type type) => SqlOptimizer.ConvertExpression(new SqlBinaryExpression(type, expr1, "+", expr2, 60));

    [Obsolete] private ISqlExpression Obsolete_Add(ISqlExpression expr1, int value) => Obsolete_Add<int>(expr1, new SqlValue(value));

    [Obsolete] protected ISqlExpression Obsolete_Add<T>(ISqlExpression expr1, ISqlExpression expr2) => Obsolete_Add(expr1, expr2, typeof(T));

    [Obsolete]
    private void Obsolete_BuildAliases(string table, List<SqlColumn> columns, string postfix) {
      Indent++;
      bool flag = true;
      foreach (SqlColumn column in columns) {
        if (!flag) {
          StringBuilder.Append(',').AppendLine();
        }
        flag = false;
        AppendIndent().AppendFormat("{0}.{1}", table, Convert(StringBuilder, column.Alias, ConvertType.NameToQueryFieldAlias));
        if (postfix != null) {
          StringBuilder.Append(postfix);
        }
      }
      Indent--;
      StringBuilder.AppendLine();
    }

    [Obsolete]
    private void Obsolete_BuildAlternativeOrderBy(bool ascending) {
      SelectQuery selectQuery = Statement.SelectQuery;
      if (selectQuery == null) {
        return;
      }
      SkipAlias = false;
      AppendIndent().Append("ORDER BY").AppendLine();
      string[] tempAliases = GetTempAliases(selectQuery.OrderBy.Items.Count, "oby");
      Indent++;
      for (int i = 0; i < tempAliases.Length; i++) {
        AppendIndent().Append(tempAliases[i]);
        if ((ascending && selectQuery.OrderBy.Items[i].IsDescending) || (!ascending && !selectQuery.OrderBy.Items[i].IsDescending)) {
          StringBuilder.Append(" DESC");
        }
        if (i + 1 < tempAliases.Length) {
          StringBuilder.Append(',');
        }
        StringBuilder.AppendLine();
      }
      Indent--;
    }

    [Obsolete]
    protected bool Obsolete_NeedSkip(SelectQuery selectQuery) {
      if (selectQuery.Select.SkipValue != null) {
        return SqlProviderFlags.GetIsSkipSupportedFlag(selectQuery.Select.TakeValue, selectQuery.Select.SkipValue);
      }
      return false;
    }

    //[Obsolete]
    //protected void Obsolete_ExtractMergeParametersIfCannotCombine(SqlInsertOrUpdateStatement insertOrUpdate, List<SqlSetExpression> keys) {
    //  if (SqlProviderFlags.CanCombineParameters) {
    //    return;
    //  }
    //  insertOrUpdate.Parameters.Clear();
    //  for (int i = 0; i < keys.Count; i++) {
    //    ExtractParameters(insertOrUpdate, keys[i].Expression);
    //  }
    //  foreach (SqlSetExpression item in insertOrUpdate.Update.Items) {
    //    ExtractParameters(insertOrUpdate, item.Expression);
    //  }
    //  foreach (SqlSetExpression item2 in insertOrUpdate.Insert.Items) {
    //    ExtractParameters(insertOrUpdate, item2.Expression);
    //  }
    //  if (insertOrUpdate.Parameters.Count > 0) {
    //    insertOrUpdate.IsParameterDependent = true;
    //  }
    //}

    //[Obsolete]
    //private void Obsolete_ExtractParameters(SqlStatement statement, ISqlExpression expression) {
    //  new QueryVisitor().Visit(expression, delegate (IQueryElement e)
    //  {
    //    QueryElementType elementType = e.ElementType;
    //    if (elementType == QueryElementType.SqlParameter) {
    //      SqlParameter sqlParameter = (SqlParameter)e;
    //      if (sqlParameter.IsQueryParameter) {
    //        statement.Parameters.Add(sqlParameter);
    //      }
    //    }
    //  });
    //}


  }

}