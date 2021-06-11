using System.Collections.Generic;
using System.Linq;

namespace LinqToDB.SqlQuery {
  public static class SelectQueryExtensions {

    public static ISqlExpression ConvertToSubquery(this SelectQuery sql, ISqlExpression element, HashSet<SqlTable> tableSet,
              List<SqlTable> tables,
              SqlTable firstTable,
              SqlTable secondTable) {
      // for table field references from association tables we must rewrite them with subquery
      if (element.ElementType == QueryElementType.SqlField) {
        var fld = (SqlField)element;
        var tbl = (SqlTable)fld.Table;
        // table is an association table, used in FROM clause - generate subquery
        if (tbl != firstTable && (secondTable == null || tbl != secondTable) && tableSet.Contains(tbl)) {
          var tempCopy = sql.Clone();
          var tempTables = new List<SqlTableSource>();
          // create copy of tables from main FROM clause for subquery clause
          new QueryVisitor().Visit(tempCopy.From, ee => {
            if (ee.ElementType == QueryElementType.TableSource)
              tempTables.Add((SqlTableSource)ee);
          });
          // main table reference in subquery
          var tt = tempTables[tables.IndexOf(tbl)];
          tempCopy.Select.Columns.Clear();
          tempCopy.Select.Add(((SqlTable)tt.Source).Fields.FirstOrDefault(x => x.Name == fld.Name));
          // create new WHERE for subquery
          tempCopy.Where.SearchCondition.Conditions.Clear();
          var firstTableKeys = tempCopy.From.Tables[0].Source.GetKeys(true);
          foreach (SqlField key in firstTableKeys)
            tempCopy.Where.Field(key).Equal.Field(firstTable.Fields.FirstOrDefault(x => x.Name == key.Name));
          if (secondTable != null) {
            var secondTableKeys = tempCopy.From.Tables[0].Joins[0].Table.Source.GetKeys(true);
            foreach (SqlField key in secondTableKeys)
              tempCopy.Where.Field(key).Equal.Field(secondTable.Fields.FirstOrDefault(x => x.Name == key.Name));
          }
          // set main query as parent
          tempCopy.ParentSelect = sql;

          return tempCopy;
        }
      }

      return element;
    }

  }
}
