using LinqToDB.DataProvider;
using System;
using System.Collections.Generic;

namespace LinqToDB.SqlQuery {
  public static class SqlStatementExtensions {

    public static Tuple<SqlTableSource, SqlTableSource> MoveJoinsToSubqueries(
      this SqlStatement statement,
      string firstTableAlias,
      string secondTableAlias,
      BasicMergeBuilder.QueryElement part) {
      var baseTablesCount = secondTableAlias == null ? 1 : 2;

      // collect tables, referenced in FROM clause
      var tableSet = new HashSet<SqlTable>();
      var tables = new List<SqlTable>();

      new QueryVisitor().Visit(statement.SelectQuery.From, e => {
        if (e.ElementType == QueryElementType.TableSource) {
          var et = (SqlTableSource)e;

          tableSet.Add((SqlTable)et.Source);
          tables.Add((SqlTable)et.Source);
        }
      });

      if (tables.Count > baseTablesCount) {
        var firstTable = (SqlTable)statement.SelectQuery.From.Tables[0].Source;
        var secondTable = baseTablesCount > 1
          ? (SqlTable)statement.SelectQuery.From.Tables[0].Joins[0].Table.Source
          : null;

        ISqlExpressionWalkable queryPart;
        switch (part) {
          case BasicMergeBuilder.QueryElement.Where:
            queryPart = statement.SelectQuery.Where;
            break;
          case BasicMergeBuilder.QueryElement.InsertSetter:
            queryPart = statement.GetInsertClause();
            break;
          case BasicMergeBuilder.QueryElement.UpdateSetter:
            queryPart = statement.GetUpdateClause();
            break;
          default:
            throw new InvalidOperationException();
        }
        queryPart.Walk(new WalkOptions(true), element => statement.SelectQuery.ConvertToSubquery(element, tableSet, tables, firstTable, secondTable));
      }

      var table1 = statement.SelectQuery.From.Tables[0];
      table1.Alias = firstTableAlias;

      SqlTableSource table2 = null;

      if (secondTableAlias != null) {
        if (tables.Count > baseTablesCount)
          table2 = statement.SelectQuery.From.Tables[0].Joins[0].Table;
        else
          table2 = statement.SelectQuery.From.Tables[1];

        table2.Alias = secondTableAlias;
      }

      return Tuple.Create(table1, table2);
    }

  }
}
